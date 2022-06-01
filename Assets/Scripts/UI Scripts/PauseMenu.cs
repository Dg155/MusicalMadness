using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject SettingsMenuUI;
    public bool GameIsPaused;

    void Resume()
    {
        Debug.Log("pause called off");
        SettingsMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        Debug.Log("pause called");
        SettingsMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape key pressed");
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
