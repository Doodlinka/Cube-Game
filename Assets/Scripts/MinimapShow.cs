using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapShow : MonoBehaviour
{
    public GameObject player;
    public MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!mr.enabled && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
        new Vector3(player.transform.position.x, 0, player.transform.position.z)) <= 5) {
            mr.enabled = true;
        }
    }
}
