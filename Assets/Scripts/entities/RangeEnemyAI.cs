using UnityEngine;

public class RangeEnemyAI : EnemyAI
{
    [SerializeField] private GameObject projectile;

    protected override void Attack() {
        // if no line of sight with player, follow and bite them like regular enemies
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity)) {
            if (hit.transform != player.transform) {
                base.Attack();
            }
        }
        if (cooldown <= 0) {
            cooldown = maxCooldown;
            ProjectileScript proj = Instantiate(projectile).GetComponent<ProjectileScript>();
            proj.transform.position = transform.position;
            proj.direction = (player.transform.position - transform.position).normalized;
            proj.speed = 6;
            proj.tag = tag;
            proj.damage = dmg;
        }
    }
}