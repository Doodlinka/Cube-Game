using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    private const string _HealPrefabPath = "Prefabs/Heal";
    private const string _EnemyPrefabPath = "Prefabs/Enemies/Enemy(small)";

    private GameObject _heal, _enemy;

    void Start() {
        _heal = Utils.LoadObject<GameObject>(_HealPrefabPath);
        _enemy = Utils.LoadObject<GameObject>(_EnemyPrefabPath);
    }

    public void OnInteract() {
        int rand = Random.Range(1, 7);
        if (rand == 1) {
            GameObject tmp = Instantiate(_heal);
            tmp.transform.position = transform.position;
        }
        else if (rand == 2 || rand == 3) {
            GameObject tmp = Instantiate(_enemy);
            tmp.transform.position = transform.position;
        }
        else {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + Random.Range(4, 7) + PlayerPrefs.GetInt("golddrop"));
        }
        Destroy(gameObject);
    }
}
