using UnityEngine;


public class Boss2AI : Boss1AI
{
    [SerializeField] private GameObject minion, projectile;

    protected override void Attack()
    {
        if (_healthScript.health < maxHealth / 2) {
            secondPhase = true;
        }

        if (cooldown <= 0 && attack == 0) {
            if (secondPhase) {
                attack = Random.Range(1, 6);
            }
            else {
                attack = Random.Range(1, 3);
            }
            if (attack == 2) {
                _rb.AddForce((player.transform.position - transform.position) * dashForce, ForceMode.Impulse);
                cooldown = maxCooldown;
                attack = 0;
            }
        }

        else if (attack == 5) {
            for (int i = 0; i < 2; i++) {
                GameObject tmp = Instantiate(minion);
                tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                tmp.GetComponent<HealthScript>().dropsHealth = false;
            }
            attack = 0;
            cooldown = maxCooldown;
        }
        else if (attack != 0) {
            ProjectileScript proj = Instantiate(projectile).GetComponent<ProjectileScript>();
            proj.transform.position = transform.position;
            proj.damage = dmg;
            attack = 0;
            cooldown = maxCooldown / 2;
            if (attack == 1) {
                for (int i = 0; i < 12; i++) {
                    int x = 0, z = 0;
                    // cardinal
                    if (Random.value < 0.5) {
                        if (Random.value < 0.5) {
                            x = Random.value < 0.5 ? 1 : -1;
                        } else {
                            z = Random.value < 0.5 ? 1 : -1;
                        }
                    }
                    // diagonal
                    else {
                        x = Random.value < 0.5 ? 1 : -1;
                        z = Random.value < 0.5 ? 1 : -1;
                    }
                    proj.direction = new Vector3(x, 0, z);
                    proj.speed = Random.Range(7f, 9f);
                }
            }
            else if (attack == 3) {
                for (int i = 0; i < 8; i++) {
                    proj.direction = (player.transform.position - transform.position).normalized;
                    proj.speed = Random.Range(5f, 7f);
                }
            }
            else if (attack == 4) {
                for (int i = 0; i < 4; i++) {
                    proj.homingTarget = player.transform;
                    proj.speed = Random.Range(3f, 5f);;
                }
            }
        }
    }
}