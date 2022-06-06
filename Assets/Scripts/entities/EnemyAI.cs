using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(HealthScript), typeof(MeshRenderer))]
public class EnemyAI : MonoBehaviour
{
    protected static Material _hurtMaterial;
    protected const string _hurtMaterialPath = "glowing red";

    protected float cooldown;
    protected bool triggered;
    protected GameObject player;
    [SerializeField] protected int dmg, maxHealth;
    [SerializeField] protected float maxCooldown, speed;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioClip hitSound;
    protected Material _defaultMaterial;
    protected Rigidbody _rb;
    protected MeshRenderer _mr;
    protected HealthScript _healthScript;
    [SerializeField] protected GameObject heal;
    public bool dropsHealth = true;

    protected virtual void Start()
    {
        if (!_hurtMaterial) {
            _hurtMaterial = Utils.LoadObject<Material>(_hurtMaterialPath);
        }
        _rb = GetComponent<Rigidbody>();
        _mr = GetComponent<MeshRenderer>();
        _defaultMaterial = _mr.material;
        _healthScript = GetComponent<HealthScript>();
        _healthScript.OnHit += OnHit;
        _healthScript.OnDeath += OnDeath;
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

    protected virtual void OnHit() {
        _mr.material = _hurtMaterial;
        Invoke("SetDefaultMaterial", 0.15f);
    }

    protected void SetDefaultMaterial() {
        _mr.material = _defaultMaterial;
    }

    protected virtual void OnDeath() {
        if (Random.Range(1, 6 - PlayerPrefs.GetInt("hdrop") + 1) == 1 && dropsHealth) {
            GameObject tmp =  Instantiate(heal);
            tmp.transform.position = transform.position;
        }
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 2 + PlayerPrefs.GetInt("golddrop"));
    }

    ~EnemyAI() {
        _healthScript.OnHit -= OnHit;
        _healthScript.OnDeath -= OnDeath;
    }
}