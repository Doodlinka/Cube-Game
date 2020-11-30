using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI : MonoBehaviour
{
    private float cooldown;
    private bool triggered;
    public GameObject player, minion, projectile;
    public int dmg, maxH;
    public float maxcld, spd;
    public AudioSource source;
    public AudioClip hitSound;
    public DoorScript door;
    public int attack;
    public bool secondPhase;
    public Vector3 goal;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        attack = 0;
        triggered = false;
        player = GameObject.FindGameObjectWithTag("Player");
        door = FindObjectOfType<DoorScript>();
        secondPhase = false;
        maxH = GetComponent<HealthScript>().health;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, maxcld);

        if (!triggered && door.open) {
            triggered = true;
        }
        
        if (triggered) {
            if (GetComponent<HealthScript>().health < maxH / 2) {
                secondPhase = true;
            }

            if (cooldown >= maxcld && attack == 0) {
                if (secondPhase) {
                    attack = Random.Range(1, 6);
                }
                else {
                    attack = Random.Range(1, 3);
                }
                if (attack == 3) {
                    goal = player.transform.position;
                }
                cooldown = 0;
            }

            if (attack == 2) {
                transform.position = Vector3.MoveTowards(transform.position, goal, spd * Time.deltaTime);
                if (Vector3.Distance(transform.position, goal) < 1.5) {
                    attack = 0;
                }
                if (Vector3.Distance(transform.position, player.transform.position) < 2 && player.GetComponent<HealthScript>()) {
                    player.GetComponent<HealthScript>().TakeDamage(dmg / 2);
                    source.clip = hitSound;
                    source.Play();
                }
                cooldown = 0;
            }
            else if (attack == 5) {
                for (int i = 0; i < 8; i++) {
                    GameObject tmp = Instantiate(minion);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                }
                attack = 0;
                cooldown = 0;
            }
            else {
                ProjectileScript tmp = Instantiate(projectile).GetComponent<ProjectileScript>();
                tmp.transform.position = transform.position;
                tmp.damage = dmg;
                if (attack == 1) {
                    for (int i = 0; i < 12; i++) {
                        int x = 0, z = 1;
                        if (Random.value < 0.5) {
                            if (Random.value < 0.5) {
                                x = Random.value < 0.5 ? 1 : -1;
                            } else {
                                z = Random.value < 0.5 ? 1 : -1;
                            }
                        }
                        else {
                            x = Random.value < 0.5 ? 1 : -1;
                            z = Random.value < 0.5 ? 1 : -1;
                        }
                        tmp.direction = new Vector3(x, 0, z);
                        tmp.speed = 5;
                    }

                    attack = 0;
                    cooldown = 0;
                }
                else if (attack == 3) {
                    for (int i = 0; i < 8; i++) {
                        tmp.goal = transform.position + new Vector3(Random.value, 0, Random.value) * 5;
                        tmp.hasGoal = true;
                        tmp.speed = 3;
                    }
                }
                else if (attack == 4) {
                    for (int i = 0; i < 4; i++) {
                        tmp.target = player.transform;
                        tmp.isHoming = true;
                        tmp.speed = 2;
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Explosion") {
            GetComponent<HealthScript>().TakeDamage(50 + (PlayerPrefs.GetInt("damage") * 25));
        }
    }
}