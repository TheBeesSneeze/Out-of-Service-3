using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LoadGame();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LiamScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
