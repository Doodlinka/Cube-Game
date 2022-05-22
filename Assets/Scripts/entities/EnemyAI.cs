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
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        cooldown -= Time.deltaTime;
        if (!triggered && _healthScript.health < maxHealth
          || Vector3.Distance(transform.position, player.transform.position) <= 20) {
            triggered = true;
        }

        if (triggered) {
            Attack();
            TryHitMelee();
        }
    }

    protected void OnCollisionStay(Collision other) {
        GameObject obj = other.collider.gameObject;
        if (obj.TryGetComponent<Door>(out Door door) && !door.Open && Random.Range(1, 101) == 100) {
            door.Interact();
        }
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<ExplosionScript>(out ExplosionScript e)) {
           _healthScript.TakeDamage(50);
           _rb.AddExplosionForce(e.speed*20, e.transform.position, e.size);
        }
    }

    protected virtual void Attack() {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    protected virtual void TryHitMelee() {
        if (cooldown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= transform.localScale.magnitude) {
            player.GetComponent<HealthScript>().TakeDamage(dmg);
            source.clip = hitSound;
            source.Play();
            cooldown = maxCooldown;
        }
    }
}