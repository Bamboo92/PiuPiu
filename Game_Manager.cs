using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    public UnityEvent onPlayerSpawn;
    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;
    public UnityEvent onWaveCooldown;

    private bool gameHasEnded = false;
    
    public GameObject gameOverCanvas; // Referenz zu deinem GameOver Canvas GameObject.
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerSpawned()
    {
        onPlayerSpawn?.Invoke();
    }

    public void PlayerDied()
    {
        onPlayerDeath?.Invoke();
    }

    public void EnemyDied()
    {
        onEnemyDeath?.Invoke();
    }

    public void waveCooldown()
    {
        onWaveCooldown?.Invoke();
    }

    public void GameOver()
    {   
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            StartCoroutine(VibrateController(1f, 1f));

            gameOverCanvas.SetActive(true);
        }
    }
        
    public IEnumerator VibrateController(float duration, float intensity)
    {
        // Get all gamepad devices connected
        var gamepads = Gamepad.all;

        // If there is at least one gamepad
        if(gamepads.Count > 0)
        {
            foreach (Gamepad gamepad in gamepads)
            {
                // Vibrate every gamepad found
                gamepad.SetMotorSpeeds(intensity, intensity);
            }

            // Wait for the duration of the vibration
            yield return new WaitForSeconds(duration);

            // If there is at least one gamepad
            if(gamepads.Count > 0){
                foreach (Gamepad gamepad in gamepads)
                {
                    //Stop Vibration
                    gamepad.SetMotorSpeeds(0, 0);
                }
            }
            Time.timeScale = 0;
        }
    }
}
