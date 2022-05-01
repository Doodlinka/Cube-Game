using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public int damage;
    public bool isExplosive;
    public GameObject explosion;
    public AudioClip explosionSound;
    private float lifetime = 0;
    public float maxLifetime = 8;
    public Transform homingTarget;

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime) {
            Destroy(gameObject);
        }

        if (homingTarget != null) { 
            transform.position = Vector3.MoveTowards(transform.position, homingTarget.position, speed * Time.deltaTime);
        }
        else {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision hit) {
        GameObject hitGO = hit.collider.gameObject;

        if ((hitGO.tag == "Player" || hitGO.tag == "Enemy") && hitGO.GetComponent<HealthScript>()) {
            hitGO.GetComponent<HealthScript>().TakeDamage(damage);
        }

        if (hitGO.tag == "Barrel") {
            GameObject tmp = Instantiate(explosion);
            tmp.transform.position = transform.position;
            tmp.layer = 0;
            tmp.GetComponent<ExplosionScript>().size = 5;
            tmp.GetComponent<AudioSource>().clip = explosionSound;
            Destroy(hitGO);
        }

        if (isExplosive) {
            GameObject tmp = Instantiate(explosion);
            tmp.transform.position = transform.position;
        }
        Destroy(gameObject);
    }
}