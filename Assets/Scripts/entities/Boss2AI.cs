using UnityEngine;


public class Boss2AI : Boss1AI
{
    [SerializeField] private GameObject minion, projectile;

    protected override void Attack()
    {
        if (_healthScript.health < maxHealth / 2) {
            secondPhase = true;
        }

        if (cooldown > 0) { return; }

        if (attack < 1 || attack > 5) {
            if (secondPhase) {
                attack = Random.Range(1, 6);
            }
            else {
                attack = Random.Range(1, 3);
            }
        }
        if (attack == 2) {
            _rb.AddForce((player.transform.position - transform.position) * dashForce, ForceMode.Impulse);
            cooldown = maxCooldown * 0.75f;
            attack = 0;
        }
        else if (attack == 5) {
            for (int i = 0; i < Random.Range(1, 4); i++) {
                GameObject tmp = Instantiate(minion);
                tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                tmp.GetComponent<EnemyAI>().dropsHealth = false;
            }
            attack = 0;
            cooldown = maxCooldown*2;
        }
        else if (attack == 1) {
            cooldown = maxCooldown / 2;
            for (int i = 0; i < Random.Range(11, 14); i++) {
                ProjectileScript proj = Instantiate(projectile).GetComponent<ProjectileScript>();
                proj.transform.position = transform.position;
                proj.damage = dmg;
                Vector2 dir = Random.insideUnitCircle.normalized;
                proj.direction = new Vector3(dir.x, 0, dir.y);
                proj.speed = Random.Range(7f, 9f);
            }
        }
        else if (attack == 3) {
            cooldown = maxCooldown / 2;
            for (int i = 0; i < Random.Range(7, 10); i++) {
                ProjectileScript proj = Instantiate(projectile).GetComponent<ProjectileScript>();
                proj.transform.position = transform.position;
                proj.damage = dmg;
                proj.direction = (player.transform.position - transform.position).normalized;
                proj.speed = Random.Range(5f, 7f);
            }
        }
        else if (attack == 4) {
            cooldown = maxCooldown / 2;
            for (int i = 0; i < Random.Range(3, 6); i++) {
                ProjectileScript proj = Instantiate(projectile).GetComponent<ProjectileScript>();
                proj.transform.position = transform.position;
                proj.damage = dmg;
                proj.homingTarget = player.transform;
                proj.speed = Random.Range(3f, 5f);;
            }
        }
        attack = 0;
    }

    protected virtual void OnDeath() {
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 30 + PlayerPrefs.GetInt("golddrop"));
        for (int i = 0; i < 10; i++) {
            GameObject tmp = Instantiate(heal);
            tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
        }
        GameObject e = Instantiate(exit);
        e.transform.position = new Vector3(transform.position.x, 0.51f, transform.position.z);
    }
}