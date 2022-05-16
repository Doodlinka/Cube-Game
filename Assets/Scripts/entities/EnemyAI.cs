using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(HealthScript))]
public class EnemyAI : MonoBehaviour
{
    protected float cooldown;
    protected bool triggered;
    protected GameObject player;
    [SerializeField] protected int dmg, maxHealth;
    [SerializeField] protected float maxCooldown, speed;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioClip hitSound;
    protected Rigidbody _rb;
    protected HealthScript _healthScript;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _healthScript = GetComponent<HealthScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        maxHealth =_healthScript.health;
    }

    protected virtual void Update()
    {
        cooldown -= Time.deltaTime;
        if (!triggered &&_healthScript.health < maxHealth
          || Vector3.Distance(transform.position, player.transform.position) <= 20) {
            triggered = true;
        }

        if (triggered) {
            Attack();
        }
    }

    protected void OnCollisionStay(Collision other) {
        GameObject obj = other.collider.gameObject;
        if (obj.TryGetComponent<Door>(out Door door) && !door.Open && Random.Range(1, 101) == 100) {
            door.OnInteract();
        }
        // TODO: may still need the distance instead of collision check in some cases
        if (cooldown <= 0 && obj.TryGetComponent<PlayerScript>(out PlayerScript pl)) {
            player.GetComponent<HealthScript>().TakeDamage(dmg);
            source.clip = hitSound;
            source.Play();
            cooldown = maxCooldown;
        }
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<ExplosionScript>(out ExplosionScript e)) {
           _healthScript.TakeDamage(50);
           _rb.AddExplosionForce(e.size, e.transform.position, e.size);
        }
    }

    protected virtual void Attack() {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}