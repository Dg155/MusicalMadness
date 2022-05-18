using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour

{
    public Animator transition;
    // Start is called before the first frame update
    void Awake()
    {
        if (transition == null){
            transition = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    public void ReloadLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));

    }

    IEnumerator LoadLevel(int levelIndex){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadSettingsScene(){
        SceneManager.LoadScene("Settings");
    }

    public void LoadTitleScreenScene(){
        SceneManager.LoadScene("TitleScreen");
    }

    public void LoadCreditsScene(){
        SceneManager.LoadScene("Credits");
    }

    public void LoadHowToPlayScene(){
        SceneManager.LoadScene("HowToPlay");
    }

    public void LoadControlsScene(){
        SceneManager.LoadScene("Controls");
    }
}
