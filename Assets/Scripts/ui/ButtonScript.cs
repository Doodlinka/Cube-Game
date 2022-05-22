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
        if (pauseScript) {
            pauseScript.Paused = false;
        }
        PlayerPrefs.Save();
        SceneManager.LoadScene("Menu");
    }

    public void UpgradeMenu() {
        SceneManager.LoadScene("UpgradeMenu");
    }

    public void Resume() {
        pauseScript.Paused = false;
    }
}