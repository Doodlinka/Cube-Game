using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour
{
    public GameObject heal, enemy;
    public void Open() {
        int rand = Random.Range(1, 7);
        if (rand == 1) {
            GameObject tmp =  Instantiate(heal);
            tmp.transform.position = transform.position;
        }
        else if (rand == 2 || rand == 3) {
            GameObject tmp =  Instantiate(enemy);
            tmp.transform.position = transform.position;
        }
        else {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + Random.Range(4, 7) + PlayerPrefs.GetInt("golddrop"));
        }
        Destroy(gameObject);
    }
}
