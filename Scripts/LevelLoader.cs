using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private int menu;

    public float transitionTime = 1f;

    PlayerInput playerInput;

    public void Awake()
    {
        playerInput = new PlayerInput();
    }

    public void Start()
    {
        playerInput.Menu.Menu.performed += _ => Escape();
    }

    public void Escape()
    {
        if (SceneManager.GetActiveScene().buildIndex != 7){
            LoadNextLevel();
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 7) {
            LoadPreviousLevel();
        }
    }

    public void LoadNextLevel()
    {
        menu = 7 - SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + menu));
    }

    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 7));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // play animation
        transition.SetTrigger("Start");

        // wait
        yield return new WaitForSeconds(transitionTime);

        // load scene
        SceneManager.LoadScene(levelIndex);
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
