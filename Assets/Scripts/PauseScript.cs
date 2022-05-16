using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseScript : MonoBehaviour
{
    public bool paused;
    public GameObject player;
    public Canvas pauseCanvas;
    public Image pauseBackground;

    public void Pause() {
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        HealthScript[] healthScripts = FindObjectsOfType<HealthScript>();
        EnemyAI[] EnemyAIs = FindObjectsOfType<EnemyAI>();
        ProjectileScript[] projectileScripts = FindObjectsOfType<ProjectileScript>();
        ExplosionScript[] explosionScripts = FindObjectsOfType<ExplosionScript>();

        foreach (HealthScript hs in healthScripts) {
            hs.enabled = paused;
        }
        foreach (EnemyAI ai in EnemyAIs) {
            ai.enabled = paused;
        }
        foreach (ProjectileScript p in projectileScripts) {
            p.enabled = paused;
        }
        foreach (ExplosionScript e in explosionScripts) {
            e.enabled = paused;
        }

        player.GetComponent<PlayerScript>().enabled = paused;
        player.GetComponent<CharacterController>().enabled = paused;
        player.GetComponent<FirstPersonController>().enabled = paused;

        paused = !paused;

        pauseBackground.enabled = paused;
        pauseCanvas.enabled = paused;

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