using UnityEngine;

public static class BossRooms
{
    public static int[,] Room1(int roomSize) {
        int[,] data = new int[roomSize, roomSize];

        // set outline to 1
		for (int x = 0; x < 22; x++) {
			data[0, x] = 1;
			data[21, x] = 1;
		}
		for (int z = 0; z < 23; z++) {
			data[z, 0] = 1;
			data[z, 21] = 1;
		}

        // everything else
        data[7, 3] = 1; data[7, 12] = 1; data[16, 3] = 1; data[16, 12] = 1;

        return data;
    }

    public static int[,] Room2(int roomSize) {
        int[,] data = new int[roomSize, roomSize];

        // set outline to 1
		for (int x = 0; x < 22; x++) {
			data[0, x] = 1;
			data[21, x] = 1;
		}
		for (int z = 0; z < 23; z++) {
			data[z, 0] = 1;
			data[z, 21] = 1;
		}

        return data;
    }
}