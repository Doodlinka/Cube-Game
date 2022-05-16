using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    public int health;
    public bool godmode = false;
    public Text text;
    public GameObject heal, exit;
    public bool dropsHealth = true;

    void Start() {
        if (tag == "Player") {
            text.text = "Health: " + health.ToString();
        }
    }

    public void TakeDamage(int dmg) {
        if (!godmode) {
            health -= dmg;
            if (tag == "Player") {
                text.text = "Health: " + health.ToString();
            }
        }
        if (health <= 0) {
            if (tag == "Player") {
                PlayerPrefs.DeleteKey("level");
                PlayerPrefs.DeleteKey("health");

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PlayerPrefs.Save();
                SceneManager.LoadScene("Game Over");
            }
            else {
                // TODO: oh dear god, GO names (store an onDeath function?)
                if (transform.name == "Boss1(Clone)") {
                    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 30 + PlayerPrefs.GetInt("golddrop"));
                    for (int i = 0; i < 10; i++) {
                        GameObject tmp = Instantiate(heal);
                        tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    }
                    GameObject e = Instantiate(exit);
                    e.transform.position = new Vector3(transform.position.x, 0.51f, transform.position.z);
                }
                else if (transform.name == "Boss2(Clone)") {
                    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 30 + PlayerPrefs.GetInt("golddrop"));
                    for (int i = 0; i < 10; i++) {
                        GameObject tmp = Instantiate(heal);
                        tmp.transform.position = transform.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-2, 3));
                    }
                    GameObject e = Instantiate(exit);
                    e.transform.position = new Vector3(transform.position.x, 0.51f, transform.position.z);
                }
                else {
                    if (Random.Range(1, 6 - PlayerPrefs.GetInt("hdrop") + 1) == 1 && dropsHealth) {
                        GameObject tmp =  Instantiate(heal);
                        tmp.transform.position = transform.position;
                    }
                    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 1 + PlayerPrefs.GetInt("golddrop"));
                }
            }
            Destroy(this.gameObject);
        }
    }
}
