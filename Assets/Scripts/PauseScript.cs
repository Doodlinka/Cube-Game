using System.Collections;
using System.Collections.Generic;
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
        RangeEnemyAI[] RangeEnemyAIs = FindObjectsOfType<RangeEnemyAI>();
        ProjectileScript[] projectileScripts = FindObjectsOfType<ProjectileScript>();
        ExplosionScript[] explosionScripts = FindObjectsOfType<ExplosionScript>();
        Boss1AI[] boss1AIs = FindObjectsOfType<Boss1AI>();
        Boss2AI[] boss2AIs = FindObjectsOfType<Boss2AI>();

        foreach (HealthScript hs in healthScripts) {
            hs.enabled = paused;
        }
        foreach (EnemyAI ai in EnemyAIs) {
            ai.enabled = paused;
        }
        foreach (RangeEnemyAI ai in RangeEnemyAIs) {
            ai.enabled = paused;
        }
        foreach (Boss1AI ai in boss1AIs) {
            ai.enabled = paused;
        }
        foreach (Boss2AI ai in boss2AIs) {
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
    }
}