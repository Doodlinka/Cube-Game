﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class RoomExit : MonoBehaviour, IInteractable
{
    public void OnInteract() {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        Debug.Log("exit");

        if (PlayerPrefs.GetInt("level") > 5) {
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
            Debug.Log("new level");
            SceneManager.LoadScene("Game");
        }
    }
}
