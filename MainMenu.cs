using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene("MapSelection");
    }

    public void GridBox()
    {
        SceneManager.LoadScene("GridBox");
    }

    public void Dungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void ToonCity()
    {
        SceneManager.LoadScene("ToonCity");
    }

    public void BlossomBazaar()
    {
        SceneManager.LoadScene("BlossomBazaar");
    }

    public void Boxy()
    {
        SceneManager.LoadScene("Boxy");
    }

    public void Spiral()
    {
        SceneManager.LoadScene("Spiral");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
