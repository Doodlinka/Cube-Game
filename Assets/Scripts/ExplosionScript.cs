using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float size = 3;

    // Update is called once per frame
    void Update()
    {
        float growth = Time.deltaTime * 15f;
        transform.localScale = transform.localScale + new Vector3(growth, growth, growth);
        if (transform.localScale.x > size) {
            Destroy(gameObject);
        }
    }
}