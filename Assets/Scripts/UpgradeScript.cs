using UnityEngine;
using UnityEngine.UI;

public class UpgradeScript : MonoBehaviour
{
    public Text goldText, healthText, damageText, healthChanceText, explosiveBulletsText, crateChanceText, goldDropText;

    void Start()
    {
        if (!PlayerPrefs.HasKey("golddrop")) {
            PlayerPrefs.SetInt("golddrop", 0);
        }
        if (!PlayerPrefs.HasKey("hp")) {
            PlayerPrefs.SetInt("hp", 0);
        }
        if (!PlayerPrefs.HasKey("hdrop")) {
            PlayerPrefs.SetInt("hdrop", 0);
        }
        if (!PlayerPrefs.HasKey("damage")) {
            PlayerPrefs.SetInt("damage", 0);
        }
        if (!PlayerPrefs.HasKey("cratechance")) {
            PlayerPrefs.SetInt("cratechance", 0);
        }
        if (!PlayerPrefs.HasKey("explosivebullets")) {
            PlayerPrefs.SetInt("explosivebullets", 0);
        }
        UpdateText();  
    }

    public void UpgradeHealth() {
        if (PlayerPrefs.GetInt("hp") < 5 && PlayerPrefs.GetInt("gold") >= (PlayerPrefs.GetInt("hp") + 1) * 20) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - ((PlayerPrefs.GetInt("hp") + 1) * 20));
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("health", PlayerPrefs.GetInt("health") + 10);
        }
        UpdateText();
    }

    public void UpgradeDamage() {
        if (PlayerPrefs.GetInt("damage") < 2 && PlayerPrefs.GetInt("gold") >= (PlayerPrefs.GetInt("damage") + 1) * 50) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - ((PlayerPrefs.GetInt("damage") + 1) * 50));
            PlayerPrefs.SetInt("damage", PlayerPrefs.GetInt("damage") + 1);
        }
        UpdateText();
    }

    public void UpgradeHealthDrop() {
        if (PlayerPrefs.GetInt("hdrop") < 2 && PlayerPrefs.GetInt("gold") >= (PlayerPrefs.GetInt("hdrop") + 1) * 50) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - ((PlayerPrefs.GetInt("hdrop") + 1) * 50));
            PlayerPrefs.SetInt("hdrop", PlayerPrefs.GetInt("hdrop") + 1);
        }
        UpdateText();
    }
    public void UpgradeGoldDrop() {
        if (PlayerPrefs.GetInt("golddrop") < 2 && PlayerPrefs.GetInt("gold") >= (PlayerPrefs.GetInt("golddrop") + 1) * 50) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - ((PlayerPrefs.GetInt("golddrop") + 1) * 50));
            PlayerPrefs.SetInt("golddrop", PlayerPrefs.GetInt("golddrop") + 1);
        }
        UpdateText();
    }
    public void UpgradeCrateChance() {
        if (PlayerPrefs.GetInt("cratechance") < 3 && PlayerPrefs.GetInt("gold") >= (PlayerPrefs.GetInt("cratechance") + 1) * 30) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - ((PlayerPrefs.GetInt("cratechance") + 1) * 30));
            PlayerPrefs.SetInt("cratechance", PlayerPrefs.GetInt("cratechance") + 1);
        }
        UpdateText();
    }

    public void UpgradeExplosiveBullets() {
        if (PlayerPrefs.GetInt("explosivebullets") < 1 && PlayerPrefs.GetInt("gold") >= (PlayerPrefs.GetInt("explosivebullets") + 1) * 100) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - ((PlayerPrefs.GetInt("explosivebullets") + 1) * 100));
            PlayerPrefs.SetInt("explosivebullets", PlayerPrefs.GetInt("explosivebullets") + 1);
        } 
        UpdateText();
    }

    private void UpdateText() {
        if (PlayerPrefs.GetInt("hp") >= 5) {
            healthText.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("damage") >= 2) {
            damageText.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("hdrop") >= 2) {
            healthChanceText.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("golddrop") >= 2) {
            goldDropText.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("cratechance") >= 3) {
            crateChanceText.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("explosivebullets") >= 1) {
            explosiveBulletsText.gameObject.SetActive(false);
        }
        healthText.text = "Health + 10: " + ((PlayerPrefs.GetInt("hp") + 1) * 20).ToString() + "g";
        damageText.text = "Damage + 25: " + ((PlayerPrefs.GetInt("damage") + 1) * 50).ToString() + "g";
        healthChanceText.text = "Heal drop +: " + ((PlayerPrefs.GetInt("hdrop") + 1) * 50).ToString() + "g";
        goldDropText.text = "Gold drop + 1: " + ((PlayerPrefs.GetInt("golddrop") + 1) * 50).ToString() + "g";
        crateChanceText.text = "Crate chance +: " + ((PlayerPrefs.GetInt("cratechance") + 1) * 30).ToString() + "g";
        explosiveBulletsText.text = "Explosive bullets: " + ((PlayerPrefs.GetInt("explosivebullets") + 1) * 100).ToString() + "g";
        goldText.text = "Gold: " + PlayerPrefs.GetInt("gold");
    }
}