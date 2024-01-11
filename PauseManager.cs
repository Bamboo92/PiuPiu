using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    PlayerInput playerInput;

    bool paused = false;

    public void Awake()
    {
        playerInput = new PlayerInput();
    }

    public void Start()
    {
        playerInput.Menu.PauseGame.performed += _ => DeterminePause();
    }

    void DeterminePause()
    {
        if(paused){
            ResumeGame();
        } else {
            PauseGame();
        }
    }
    // Start is called before the first frame update
    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        paused = false;
    }
    
    void OnEnable()
    {
        // enable the character controls action map
        playerInput.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        playerInput.Disable();
    }
}
