using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float cooldown;
    private bool triggered;
    public GameObject player;
    public int dmg, maxH;
    public float maxcld, spd;
    public AudioSource source;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        triggered = false;
        player = GameObject.FindGameObjectWithTag("Player");
        maxH = GetComponent<HealthScript>().health;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, maxcld);
        if (!triggered && GetComponent<HealthScript>().health < maxH) {
            triggered = true;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= 15 || triggered) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, spd * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.transform.position) < 1.25 && cooldown >= maxcld && player.GetComponent<HealthScript>()) {
                player.GetComponent<HealthScript>().TakeDamage(dmg);
                source.clip = hitSound;
                source.Play();
                cooldown -= maxcld;
            }
        }
    }

    void OnCollisionStay(Collision other) {
        GameObject obj = other.collider.gameObject;
        if(obj.name == "Door(Clone)" && !obj.GetComponent<DoorScript>().open && Random.Range(1, 101) == 100) {
            obj.GetComponent<DoorScript>().ChangeState();
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Explosion") {
            GetComponent<HealthScript>().TakeDamage(50 + (PlayerPrefs.GetInt("damage") * 25));
        }
    }
}