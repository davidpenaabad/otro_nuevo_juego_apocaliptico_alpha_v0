using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    bool enableESC = true;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !gameIsPaused && enableESC)
        {
            LaunchPauseMenu();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && gameIsPaused && enableESC)
        {
            ResumeGame();
            enableESC = true;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void LaunchPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameIsPaused = true;
        this.transform.GetComponentInChildren<Shadow>().enabled = false;
    }

    public void LaunchSettings()
    {
        settingsMenu.SetActive(true);
        enableESC = false;
    }

    public void BackSettingsMenu() {
        settingsMenu.SetActive(false);
        enableESC = true;
        settingsMenu.GetComponentInChildren<Shadow>().enabled = false;
    }

    public void LaunchMainSecene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
