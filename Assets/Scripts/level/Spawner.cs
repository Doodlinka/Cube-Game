using UnityEngine;

public class Spawner : MonoBehaviour
{
    public LevelGenerator generator;
    public GameObject prefab, prefabB, prefabS, prefabR, crate, barrel;
    public int counter = 0;

    void Start()
    {
        if (PlayerPrefs.GetInt("level") == 5) { return; }

        GameObject parent = new GameObject();

        for (int z = 0; z < generator.mazeSize; z++) {
            for (int x = 0; x < generator.mazeSize; x++) {
                // spawn monster
                if (generator.mapData[z, x] == 0 && Random.Range(1, 31) == 30 && Vector3.Distance(new Vector3(x, 0, z), new Vector3(3, 0, 3)) >= 10) {
                    GameObject tmp;
                    int random = Random.Range(1, 41);
                    // big
                    if (random <= generator.level * 4) {
                        tmp = Instantiate(prefabB);
                    }
                    // small
                    else if (random > 40 - generator.level * 4) {
                        tmp = Instantiate(prefabS);
                    }
                    //range
                    else if (random > 4 * generator.level && random <= 8 * generator.level) {
                        tmp = Instantiate(prefabR);
                    }
                    // normal
                    else {
                        tmp = Instantiate(prefab);
                    }
                    tmp.transform.position = new Vector3(x, 1, z);
                    tmp.transform.parent = parent.transform;
                    counter++;
                }

                // spawn crate
                if (generator.mapData[z, x] == 0 && Random.Range(1, 201 - (generator.level * 10) - (PlayerPrefs.GetInt("cratechance") * 10)) == 1) {
                    GameObject tmp = Instantiate(crate);
                    tmp.transform.position = new Vector3(x, 1, z);
                    tmp.transform.parent = parent.transform;
                }
                if (generator.mapData[z, x] == 0 && Random.Range(1, 501) == 1) {
                    GameObject tmp = Instantiate(barrel);
                    tmp.transform.position = new Vector3(x, 1, z);
                    tmp.transform.parent = parent.transform;
                }
            }
        }
    }
}