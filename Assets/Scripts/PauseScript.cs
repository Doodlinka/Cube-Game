using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject player, pauseCanvas;

    private bool paused = false;
    public bool Paused {
        get => paused;
        set {
            paused = value;

            pauseCanvas.SetActive(paused);

            Time.timeScale = paused ? 0 : 1;

            if (paused) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}