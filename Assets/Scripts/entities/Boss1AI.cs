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
    protected int attack;
    protected bool secondPhase;

    private GameObject[] _minionPrefabs = new GameObject[3];

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < _EnemyPrefabNames.Length; i++) {
            _minionPrefabs[i] = Utils.LoadObject<GameObject>(_EnemyPrefabNames[i], _EnemyPrefabFolder);
        }
    }

    // override only to remove trigger on distance 
    protected override void Update()
    {
        cooldown -= Time.deltaTime;
        if (!triggered && _healthScript.health < maxHealth) {
            Debug.Log("triggered");
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

        if (cooldown <= 0) {   
            if (attack >= 1 && attack <= 6) {
                if (attack == 1 || attack == 2) {
                    _rb.AddForce((player.transform.position - transform.position) * dashForce, ForceMode.Impulse);
                }
                // TODO: remove minion spawn repetition
                else if (attack == 5) {
                    for (int i = 0; i < 6; i++) {
                        GameObject tmp = Instantiate(_minionPrefabs[(int)EnemyPrefabPathIDs.small]);
                        tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                        tmp.GetComponent<HealthScript>().dropsHealth = false;
                    }
                }
                else if (attack == 4) {
                    for (int i = 0; i < 4; i++) {
                        GameObject tmp = Instantiate(_minionPrefabs[(int)EnemyPrefabPathIDs.medium]);
                        tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                        tmp.GetComponent<HealthScript>().dropsHealth = false;
                    }
                }
                else if (attack == 3) {
                    for (int i = 0; i < 2; i++) {
                        GameObject tmp = Instantiate(_minionPrefabs[(int)EnemyPrefabPathIDs.large]);
                        tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                        tmp.GetComponent<HealthScript>().dropsHealth = false;
                    }
                }
                cooldown = maxCooldown;
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
    }
}