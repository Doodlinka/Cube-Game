using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(HealthScript))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject projectile;
    private float cooldown;
    [SerializeField] private float maxCooldown;

    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PauseScript pauseScript;

    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private int markerCount;
    [SerializeField] private Text text;
    
    [SerializeField] private Image dot;

    private HealthScript healthScript;
    private Rigidbody _rb;

    void Start()
    {
        cooldown = 0;
        healthScript = GetComponent<HealthScript>();
        _rb = GetComponent<Rigidbody>();
        if (PlayerPrefs.HasKey("health")) {
            healthScript.health = PlayerPrefs.GetInt("health");
        }
        else {
            healthScript.health = 100 + (PlayerPrefs.GetInt("hp") * 10);
        }
        // healthScript.OnHit += OnHit;
        healthScript.OnDeath += OnDeath;
    }

    void Update()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        cooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            PlayerPrefs.SetInt("health", healthScript.health);
            pauseScript.Paused = true;
        }

        if (Input.GetMouseButton(0) && cooldown <= 0) {
            cooldown = maxCooldown;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            GameObject tmp = Instantiate(projectile);
            tmp.transform.position = ray.origin;
            tmp.GetComponent<ProjectileScript>().direction = ray.direction;
            tmp.GetComponent<ProjectileScript>().speed = 25;

            tmp.GetComponent<ProjectileScript>().isExplosive = PlayerPrefs.GetInt("explosivebullets") != 0;
            
            if (healthScript.godmode) {
                tmp.GetComponent<ProjectileScript>().damage = 999;
            }
            else {
                tmp.GetComponent<ProjectileScript>().damage = 50 + (PlayerPrefs.GetInt("damage") * 25);
            }

            audioSource.clip = shootSounds[Random.Range(1, 4)];
            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 3f, ~LayerMask.GetMask("Player"));

            if (hit.collider) {
                GameObject tmp = hit.collider.gameObject;

                if(tmp.TryGetComponent<IInteractable>(out IInteractable interactable)) {
                    PlayerPrefs.SetInt("health", healthScript.health);
                    interactable.Interact();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 3f, ~LayerMask.GetMask("Player"));

            if (hit.collider && hit.collider.gameObject.tag == "Marker") {
                markerCount++;
                Destroy(hit.collider.gameObject);
                text.text = "Markers: " + markerCount.ToString();
            }
            else if (markerCount > 0) {
                markerCount--;
                GameObject mark = Instantiate(markerPrefab);
                mark.transform.position = transform.position;
                text.text = "Markers: " + markerCount.ToString();
            }      
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            cam.enabled = !cam.enabled;
            text.enabled = cam.enabled;
            dot.enabled = cam.enabled;
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            healthScript.godmode = !healthScript.godmode;
            healthScript.text.text = healthScript.godmode ? "Godmode" : "Health: " + healthScript.health.ToString();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Heal" && healthScript.health < 100 + (PlayerPrefs.GetInt("hp") * 10)) {
            healthScript.health = Mathf.Clamp(healthScript.health + 20, 0, 100 + (PlayerPrefs.GetInt("hp") * 10));
            healthScript.text.text = healthScript.godmode ? "Godmode" : "Health: " + healthScript.health.ToString();
            Destroy(other.gameObject);
        }
        if (other.TryGetComponent<ExplosionScript>(out ExplosionScript e)) {
            healthScript.TakeDamage(50);
            _rb.AddExplosionForce(e.speed*20, e.transform.position, e.size);
        }
    }

    // private void OnHit() {

    // }

    private void OnDeath() {
        PlayerPrefs.DeleteKey("level");
        PlayerPrefs.DeleteKey("health");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerPrefs.Save();
        SceneManager.LoadScene("Game Over");
    }

    ~PlayerScript() {
        // healthScript.OnHit -= OnHit;
        healthScript.OnDeath -= OnDeath;
    }
}