using UnityEngine;

enum EnemyPrefabPathIDs {
    small = 0,
    medium = 1,
    large = 2
}


public class Boss1AI : EnemyAI
{
    private const string _EnemyPrefabFolder = "Prefabs/Enemies/";
    // i don't get why arrays can't be const
    private string[] _EnemyPrefabNames = new string[3] {"Enemy(small)", "Enemy", "Enemy(big)"};

    [SerializeField] protected float dashForce;
    [SerializeField] protected GameObject exit;
    protected int attack;
    protected bool secondPhase;
    protected float _meleeCooldown;

    private GameObject[] _minionPrefabs = new GameObject[3];

    protected override void Start()
    {
        base.Start();
        _healthScript.OnHit += OnHit;
        _healthScript.OnDeath += OnDeath;
        for (int i = 0; i < _EnemyPrefabNames.Length; i++) {
            _minionPrefabs[i] = Utils.LoadObject<GameObject>(_EnemyPrefabNames[i], _EnemyPrefabFolder);
        }
    }

    // override only to remove trigger on distance 
    protected override void Update()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        cooldown -= Time.deltaTime;
        _meleeCooldown -= Time.deltaTime;
        if (!triggered && _healthScript.health < maxHealth) {
            triggered = true;
        }

        if (triggered) {
            Attack();
            TryHitMelee();
        }
    }

    protected override void Attack() {
        if (_healthScript.health < maxHealth / 2) {
            secondPhase = true;
        }

        if (cooldown > 0) { return; }

        if (attack >= 1 && attack <= 6) {
            if (attack == 1 || attack == 2) {
                _rb.AddForce((player.transform.position - transform.position) * dashForce, ForceMode.Impulse);
                cooldown = maxCooldown * 0.75f;
            }
            // TODO: remove minion spawn repetition
            else if (attack == 5) {
                for (int i = 0; i < 3; i++) {
                    GameObject tmp = Instantiate(_minionPrefabs[(int)EnemyPrefabPathIDs.small]);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    tmp.GetComponent<EnemyAI>().dropsHealth = false;
                }
                cooldown = maxCooldown * 2;
            }
            else if (attack == 4) {
                for (int i = 0; i < 2; i++) {
                    GameObject tmp = Instantiate(_minionPrefabs[(int)EnemyPrefabPathIDs.medium]);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    tmp.GetComponent<EnemyAI>().dropsHealth = false;
                }
                cooldown = maxCooldown * 2;
            }
            else if (attack == 3) {
                for (int i = 0; i < 1; i++) {
                    GameObject tmp = Instantiate(_minionPrefabs[(int)EnemyPrefabPathIDs.large]);
                    tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    tmp.GetComponent<EnemyAI>().dropsHealth = false;
                }
                cooldown = maxCooldown * 2;
            }
            attack = 0; 
        }
        else {
            if (secondPhase) {
                attack = Random.Range(1, 6);
            }
            else {
                attack = Random.Range(1, 4);
            }
        }
    }

    protected override void TryHitMelee() {
        if (_meleeCooldown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= transform.localScale.magnitude) {
            player.GetComponent<HealthScript>().TakeDamage(dmg);
            source.clip = hitSound;
            source.Play();
            _meleeCooldown = maxCooldown;
        }
    }

    protected virtual void OnDeath() {
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 30 + PlayerPrefs.GetInt("golddrop"));
        for (int i = 0; i < 10; i++) {
            GameObject tmp = Instantiate(heal);
            tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
        }
        GameObject e = Instantiate(exit);
        e.transform.position = new Vector3(transform.position.x, 0.55f, transform.position.z);
    }
}