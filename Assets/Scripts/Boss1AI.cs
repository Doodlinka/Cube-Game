using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
    private float cooldown;
    private bool triggered;
    public GameObject player, mSmall, mMedium, mBig;
    public int dmg, maxH;
    public float maxcld, spd;
    public AudioSource source;
    public AudioClip hitSound;
    public Door door;
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
        door = FindObjectOfType<Door>();
        secondPhase = false;
        maxH = GetComponent<HealthScript>().health;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, maxcld);

        if (!triggered && door.Open) {
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
                    attack = Random.Range(1, 4);
                }
                if (attack == 1 || attack == 2) {
                    goal = player.transform.position;
                }
                cooldown = 0;
            }

            if (attack == 1 || attack == 2) {
                transform.position = Vector3.MoveTowards(transform.position, goal, spd * Time.deltaTime);
                if (Vector3.Distance(transform.position, goal) < 0.5) {
                    attack = 0;
                }
                if (Vector3.Distance(transform.position, player.transform.position) < 2 && player.GetComponent<HealthScript>()) {
                    player.GetComponent<HealthScript>().TakeDamage(dmg);
                    source.clip = hitSound;
                    source.Play();
                }
                cooldown = 0;
            }
            else if (attack == 5) {
                for (int i = 0; i < 6; i++) {
                    GameObject tmp = Instantiate(mSmall);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    tmp.GetComponent<HealthScript>().dropsHealth = false;
                }
                attack = 0;
                cooldown = 0;
            }
            else if (attack == 4) {
                for (int i = 0; i < 4; i++) {
                    GameObject tmp = Instantiate(mMedium);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                }
                attack = 0;
                cooldown = 0;
            }
            else if (attack == 3) {
                for (int i = 0; i < 2; i++) {
                    GameObject tmp = Instantiate(mBig);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                }
                attack = 0;
                cooldown = 0;
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Explosion") {
            GetComponent<HealthScript>().TakeDamage(50 + (PlayerPrefs.GetInt("damage") * 25));
        }
    }
}