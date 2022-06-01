using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject SettingsMenuUI;
    public GameObject ControlsUI;
    public GameObject HowToPlayUI;
    public bool GameIsPaused;
    public MenuManager menu;

    public void Resume()
    {
        SettingsMenuUI.SetActive(false);
        ControlsUI.SetActive(false);
        HowToPlayUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;

    }

    void Pause()
    {
        SettingsMenuUI.SetActive(true);
        menu.currentActive = SettingsMenuUI;

        Time.timeScale = 0;
        GameIsPaused = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
