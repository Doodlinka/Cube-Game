using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAI : MonoBehaviour
{
    private float cooldown;
    private bool triggered;
    public GameObject player, projectile;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        triggered = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, 2.5f);

        if (!triggered && (GetComponent<HealthScript>().health < 150
        || Vector3.Distance(transform.position, player.transform.position) <= 20)) {
            triggered = true;
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= 5 && triggered) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 2f * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 10 && cooldown >= 2.5f && player.GetComponent<HealthScript>()) {
            cooldown -= 2.5f;

            GameObject tmp = Instantiate(projectile);
            tmp.transform.position = transform.position;
            tmp.GetComponent<ProjectileScript>().target = player.transform;
            tmp.GetComponent<ProjectileScript>().speed = 3;
            tmp.GetComponent<ProjectileScript>().tag = tag;
            tmp.GetComponent<ProjectileScript>().damage = 15;
            tmp.GetComponent<ProjectileScript>().isHoming = true;
        }
    }

    void OnCollisionStay(Collision other) {
        GameObject obj = other.collider.gameObject;
        if(obj.name == "Door(Clone)" && !obj.GetComponent<DoorScript>().open && Random.Range(1, 101) == 100) {
            obj.GetComponent<DoorScript>().ChangeState();
        }
    }
}