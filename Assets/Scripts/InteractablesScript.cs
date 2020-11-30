using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractablesScript : MonoBehaviour
{
    public void Action (GameObject obj) {
        if (obj.name == "Exit(Clone)") {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);

            if (PlayerPrefs.GetInt("level") > 8) {
                PlayerPrefs.DeleteKey("level");
                PlayerPrefs.DeleteKey("health");

                GameObject player = GameObject.Find("FPSController");
                player.GetComponent<FirstPersonController>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PlayerPrefs.Save();
                SceneManager.LoadScene("Win");
            }
            else {
                SceneManager.LoadScene("Game");
            }
        }

        if (obj.name == "Door(Clone)") {
            obj.GetComponent<DoorScript>().ChangeState();
        }

        if (obj.name == "Crate(Clone)") {
            obj.GetComponent<CrateScript>().Open();
        }
    }
}