using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour {
	// TODO: all the comments below and add enum for tile ids
	public GameObject currentPrefab;
	// create myself
	public GameObject[] prefabs = new GameObject[4];
	// create myself
	public GameObject minimapPrefab;
	// create myself
	public GameObject characterController;
	// create myself
	public GameObject floorParent;
	// create myself
	public GameObject wallsParent;
	// create myself
	public GameObject exitPrefab, doorPrefab;

	// allows us to see the maze generation from the scene view
	public bool generateRoof = true;

	// number of times we want to "dig" in our maze
	private int tilesToPlace;

	// make private + readonly property
	public int mazeSize;

	// this will determine whether we've placed the character controller
	private bool characterPlaced = false;

	// 2D array representing the map
	// make private + readonly property
	public int[,] mapData;

	// make private + readonly property
	public int level;
	public Text text;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("level")) {
			level = (PlayerPrefs.GetInt("level") - 1);
		}
		else {
			level = 0;
			PlayerPrefs.SetInt("level", 1);
		}
		text.text = "Level: " + PlayerPrefs.GetInt("level").ToString();

		characterPlaced = false;

		if (level >= prefabs.Length) {
			currentPrefab = prefabs[prefabs.Length - 1];
		}
		else {
			currentPrefab = prefabs[level];
		}

		// randomize numbers
		tilesToPlace = Random.Range(150, 251);

		if (PlayerPrefs.GetInt("level") == 5) {
			if (Random.value < 0.5) {
				mapData = BossRooms.Room1(mazeSize);
			}
			else {
				mapData = BossRooms.Room2(mazeSize);
			}
		}
		else {
			// initialize map 2D array
			mapData = GenerateMazeData();
		}

		floorParent = GameObject.Find("Floor");
		wallsParent = GameObject.Find("Walls");
		characterController = GameObject.FindWithTag("Player");

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSize; z++) {
			for (int x = 0; x < mazeSize; x++) {
				if (mapData[z, x] == 1) {
					CreateChildPrefab(currentPrefab, wallsParent, x, 1, z);
					CreateChildPrefab(currentPrefab, wallsParent, x, 2, z);
					CreateChildPrefab(currentPrefab, wallsParent, x, 3, z);

					CreateChildPrefab(minimapPrefab, wallsParent, x, 5, z);
				}
				else if (mapData[z, x] == 2) {
					//create doors
					GameObject door = Instantiate(doorPrefab, new Vector3(x, 1.5f, z), Quaternion.identity);
					door.transform.parent = wallsParent.transform;
					if (mapData[z + 1, x] == 0 && mapData[z - 1, x] == 0) {
						door.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
					}
					CreateChildPrefab(currentPrefab, wallsParent, x, 3, z);
				}
				else if (!characterPlaced) {
					// place the character controller on the first empty wall we generate
					characterController.transform.position = new Vector3(x, 1, z);

					// flag as placed so we never consider placing again
					characterPlaced = true;
				}
			}
		}
		GameObject floor = Instantiate(currentPrefab);
		floor.transform.position = new Vector3(mazeSize/2, 0, mazeSize/2);
		floor.transform.localScale = new Vector3(mazeSize, 1, mazeSize);
		if (generateRoof) {
			Instantiate(floor).transform.position = new Vector3(mazeSize/2, 4, mazeSize/2);
		}

		if (PlayerPrefs.GetInt("level") != 5) {
			// create exit position
			int exitX, exitZ;
			do {
				exitX = Random.Range(0, mazeSize);
				exitZ = Random.Range(0, mazeSize);
			} while (mapData[exitZ, exitX] != 0);

			// create exit
			Instantiate(exitPrefab, new Vector3(exitX, 0.51f, exitZ), Quaternion.identity);
		}
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	int[,] GenerateMazeData() {
		int[,] data = new int[mazeSize, mazeSize];

		// set outline to 1
		for (int x = 0; x < mazeSize; x++) {
			data[0, x] = 1;
			data[mazeSize - 1, x] = 1;
		}
		for (int z = 0; z < mazeSize; z++) {
			data[z, 0] = 1;
			data[z, mazeSize - 1] = 1;
		}

		// counter to ensure we consume a minimum number of tiles
		int tilesConsumed = 0;

		// iterate our random crawler, creating walls and straying from edges
		while (tilesConsumed < tilesToPlace) {
			
			// directions we will be moving along each axis; one must always be 0
			// to avoid diagonal lines
			int x = 3, z = 3;
			int xDirection = 0, zDirection = 0;

			if (Random.value < 0.5) {
				float val = Random.value;
				xDirection = val < 0.5 ? 1 : -1;
				x = val < 0.5 ? 0 : 50;
				z = Random.Range(0, mazeSize);
			} else {
				float val = Random.value;
				zDirection = val < 0.5 ? 1 : -1;
				z = val < 0.5 ? 0 : 50;
				x = Random.Range(0, mazeSize);	
			}

			// move the number of spaces we just calculated, placing tiles along the way
			for (int i = 0; i < mazeSize; i++) {
				x = Mathf.Clamp(x + xDirection, 1, mazeSize - 2);
				z = Mathf.Clamp(z + zDirection, 1, mazeSize - 2);

				// there is a check to avoid placing walls too close
				if (xDirection != 0 && data[z, x] != 1 && data[z + 1, x] != 1 && data[z - 1, x] != 1 && data[z + 2, x] != 1 && data[z - 2, x] != 1) {
					data[z, x] = 1;
					tilesConsumed++;
				}
				if (zDirection != 0 && data[z, x] != 1 && data[z, x + 1] != 1 && data[z, x - 1] != 1 && data[z, x + 2] != 1 && data[z, x - 2] != 1) {
					data[z, x] = 1;
					tilesConsumed++;
				}
			}
		}

		// place doors
		for (int z = 1; z < mazeSize - 1; z++) {
			for (int x = 1; x < mazeSize - 1; x++) {
				// intersection check
				if ((data[z + 1, x] == 1 && data[z, x + 1] == 1)
				|| (data[z - 1, x] == 1 && data[z, x - 1] == 1)
				|| (data[z + 1, x] == 1 && data[z, x - 1] == 1)
				|| (data[z - 1, x] == 1 && data[z, x + 1] == 1)) {

					// check if there is a door in walls in all directions
					CheckForDoor(z, x, 1, 0, ref data);
					CheckForDoor(z, x, -1, 0, ref data);
					CheckForDoor(z, x, 0, 1, ref data);
					CheckForDoor(z, x, 0, -1, ref data);
				}
			}
		}

		// set outline to true again
		for (int x = 0; x < mazeSize; x++) {
			data[0, x] = 1;
			data[mazeSize - 1, x] = 1;
		}
		for (int z = 0; z < mazeSize; z++) {
			data[z, 0] = 1;
			data[z, mazeSize - 1] = 1;
		}

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	void CreateChildPrefab(GameObject prefab, GameObject parent, float x, float y, float z) {
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
	}

	void CheckForDoor(int z, int x, int xDirection, int zDirection, ref int[,] data) {
		int startZ = z;
		int startX = x;

		while (true) {
			x += xDirection;
			z += zDirection;

			if (x == 0 || z == 0 || x == mazeSize - 1 || z == mazeSize - 1) {
				data[Random.Range(startZ + zDirection, z), Random.Range(startX + xDirection, x)] = 2;
				return;
			}

			if (data[z, x] != 1) {
				return;
			}

			// intersection check
			if ((data[z + 1, x] == 1 && data[z, x + 1] == 1)
			|| (data[z - 1, x] == 1 && data[z, x - 1] == 1)
			|| (data[z + 1, x] == 1 && data[z, x - 1] == 1)
			|| (data[z - 1, x] == 1 && data[z, x + 1] == 1)) {

				// create door at random place at the wall
				data[Random.Range(startZ + zDirection, z), Random.Range(startX + xDirection, x)] = 2;
				return;
			}
		}
	}
}