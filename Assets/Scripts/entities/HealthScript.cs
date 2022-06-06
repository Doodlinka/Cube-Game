using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthScript : MonoBehaviour
{
    public event Action OnHit, OnDeath;

    public int health;
    public bool godmode = false;
    public Text text;

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
            OnHit?.Invoke();
        }
        if (health <= 0) {
            OnDeath?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
