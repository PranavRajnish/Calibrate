using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int timeToWait = 4;
    int currentSceneIndex;
    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start Screen");
        Time.timeScale = 1;
    }
    public void LoadOptionsMenu()
    {
        SceneManager.LoadScene("Options Screen");
    }
    public void LoadYouLose()
    {
        SceneManager.LoadScene("Lose Screen");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

