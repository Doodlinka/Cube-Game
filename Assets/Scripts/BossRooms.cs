using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRooms : MonoBehaviour
{
    public LevelGenerator generator;
    public Spawner spawner;
    public GameObject bosswall;

    public int[,] Room1() {
        int[,] data = new int[generator.mazeSize, generator.mazeSize];

        // set outline to 1
		for (int x = 0; x < 24; x++) {
			data[2, x] = 1;
			data[23, x] = 1;
		}
		for (int z = 0; z < 23; z++) {
			data[z, 0] = 1;
			data[z, 21] = 1;
		}

        // everything else
        data[0, 0] = 1; data[1, 0] = 1; data[0, 1] = 1; data[1, 1] = 0;
        data[0, 2] = 1; data[1, 2] = 1; data[2, 1] = 2;

        data[7, 9] = 1; data[7, 10] = 1; data[7, 11] = 1; data[7, 12] = 1;
        data[11, 5] = 1; data[12, 5] = 1; data[13, 5] = 1; data[14, 5] = 1;
        data[18, 9] = 1; data[18, 10] = 1; data[18, 11] = 1; data[18, 12] = 1;
        data[11, 16] = 1; data[12, 16] = 1; data[13, 16] = 1; data[14, 16] = 1;

        //data[5, 7] = 1; data[14, 7] = 1; data[5, 16] = 1; data[14, 16] = 1;

        Instantiate(bosswall);

        return data;
    }

    public int[,] Room2() {
        int[,] data = new int[generator.mazeSize, generator.mazeSize];

        // set outline to 1
		for (int x = 0; x < 24; x++) {
			data[2, x] = 1;
			data[23, x] = 1;
		}
		for (int z = 0; z < 23; z++) {
			data[z, 0] = 1;
			data[z, 21] = 1;
		}

        // everything else
        data[0, 0] = 1; data[1, 0] = 1; data[0, 1] = 1; data[1, 1] = 0;
        data[0, 2] = 1; data[1, 2] = 1; data[2, 1] = 2;

        data[7, 5] = 1; data[7, 14] = 1; data[16, 5] = 1; data[16, 14] = 1;

        Instantiate(bosswall);

        return data;
    }
}