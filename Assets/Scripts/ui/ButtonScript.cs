using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public PauseScript pauseScript;

    public void Play() {
        SceneManager.LoadScene("Game");
    }

    public void Exit() {
        Application.Quit();
        Debug.Log("Quit in editor");
    }
    
    public void GoToMenu() {
        SceneManager.LoadScene("Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerPrefs.Save();
    }

    public void UpgradeMenu() {
        SceneManager.LoadScene("UpgradeMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume() {
        pauseScript.Pause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}