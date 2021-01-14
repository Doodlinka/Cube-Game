using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
    private const string _EnemyPrefabFolder = "Enemies";
    // i don't get why arrays can't be const
    private string[] _EnemyPrefabNames = new string[3] {"Enemy(small)", "Enemy(medium)", "Enemy(big)"};

    public AudioSource source;
    public AudioClip hitSound;
    public Door door;
    private GameObject player;
    public Vector3 goal;

    public int damage, maxHealth;
    public float maxCooldown, speed;
    public int attackMode;
    public bool secondPhase;
    private float cooldown;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        attackMode = 0;
        triggered = false;
        player = GameObject.FindGameObjectWithTag("Player");
        door = FindObjectOfType<Door>();
        secondPhase = false;
        maxHealth = GetComponent<HealthScript>().health;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, maxCooldown);

        if (!triggered && door.Open) {
            triggered = true;
        }
        
        if (triggered) {
            if (GetComponent<HealthScript>().health < maxHealth / 2) {
                secondPhase = true;
            }

            if (cooldown >= maxCooldown && attackMode == 0) {
                if (secondPhase) {
                    attackMode = Random.Range(1, 6);
                }
                else {
                    attackMode = Random.Range(1, 4);
                }
                if (attackMode == 1 || attackMode == 2) {
                    goal = player.transform.position;
                }
                cooldown = 0;
            }

            if (attackMode == 1 || attackMode == 2) {
                transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, goal) < 0.5) {
                    attackMode = 0;
                }
                if (Vector3.Distance(transform.position, player.transform.position) < 2 && player.GetComponent<HealthScript>()) {
                    player.GetComponent<HealthScript>().TakeDamage(damage);
                    source.clip = hitSound;
                    source.Play();
                }
                cooldown = 0;
            }
            else if (attackMode == 5) {
                for (int i = 0; i < 6; i++) {
                    GameObject tmp = Instantiate(mSmall);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    tmp.GetComponent<HealthScript>().dropsHealth = false;
                }
                attackMode = 0;
                cooldown = 0;
            }
            else if (attackMode == 4) {
                for (int i = 0; i < 4; i++) {
                    GameObject tmp = Instantiate(mMedium);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                }
                attackMode = 0;
                cooldown = 0;
            }
            else if (attackMode == 3) {
                for (int i = 0; i < 2; i++) {
                    GameObject tmp = Instantiate(mBig);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                }
                attackMode = 0;
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