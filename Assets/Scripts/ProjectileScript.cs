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
    public bool isHoming = false, hasGoal = false;
    public Transform target;
    private float lifetime;
    public Vector3 goal;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 7.5) {
            Destroy(gameObject);
        }

        if (isHoming) { 
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else if (hasGoal) {
            transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
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