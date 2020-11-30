using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Camera cam;
    public InteractablesScript interactables;
    public GameObject projectile;
    private float cooldown;

    public AudioClip[] shootSounds;
    public AudioSource audioSource;

    public PauseScript pauseScript;

    public GameObject markerPrefab;
    public int markerCount;
    public Text text;
    
    public Image dot;

    public HealthScript healthScript;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        markerCount = 5;
        healthScript = GetComponent<HealthScript>();
        if (PlayerPrefs.HasKey("health")) {
            healthScript.health = PlayerPrefs.GetInt("health");
        }
        else {
            healthScript.health = 100 + (PlayerPrefs.GetInt("hp") * 10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        cooldown = Mathf.Clamp(cooldown, 0, 0.75f);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            PlayerPrefs.SetInt("health", healthScript.health);
            pauseScript.Pause();
        }

        if (Input.GetMouseButton(0) && cooldown >= 0.75f) {
            cooldown -= 0.75f;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            GameObject tmp = Instantiate(projectile);
            tmp.transform.position = ray.origin;
            tmp.GetComponent<ProjectileScript>().direction = ray.direction;
            tmp.GetComponent<ProjectileScript>().speed = 25;

            if (PlayerPrefs.GetInt("explosivebullets") == 1) {
                tmp.GetComponent<ProjectileScript>().isExplosive = true;
            }
            else {
                tmp.GetComponent<ProjectileScript>().isExplosive = false;
            }
            
            if (healthScript.godmode) {
                tmp.GetComponent<ProjectileScript>().damage = 99999;
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
            Physics.Raycast(ray, out hit);

            if (hit.collider) {
                GameObject tmp = hit.collider.gameObject;

                if(tmp.tag == "Interactable" && Vector3.Distance(transform.position, tmp.transform.position) < 3) {
                    PlayerPrefs.SetInt("health", healthScript.health);
                    interactables.Action(tmp);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider) {
                GameObject tmp = hit.collider.gameObject;

                if (hit.collider.gameObject.name == "Marker(Clone)" &&
                Vector3.Distance(transform.position, tmp.transform.position) < 3) {
                    markerCount++;
                    Destroy(tmp);
                }
                else if (markerCount > 0) {
                    markerCount--;
                    GameObject mark = Instantiate(markerPrefab);
                    mark.transform.position = transform.position;
                }
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
            healthScript.text.text = "Godmode";
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Heal" && healthScript.health < 100 + (PlayerPrefs.GetInt("hp") * 10)) {
            healthScript.health = Mathf.Clamp(healthScript.health + 20, 0, 100 + (PlayerPrefs.GetInt("hp") * 10));
            Destroy(other.gameObject);
        }
        if (other.tag == "Explosion") {
            healthScript.TakeDamage(50);
            other.gameObject.layer = 8;
        }
    }
}