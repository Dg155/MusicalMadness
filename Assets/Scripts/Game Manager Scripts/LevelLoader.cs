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

    public void ReloadLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));

    }

    IEnumerator LoadLevel(int levelIndex){
        transition.SetBool("Dark", true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelIndex);
    }


    public void DarkenScreen(){
        transition.SetBool("Dark", true);
    }
    public void LightenScreen(){
        transition.SetBool("Dark", false);
    }


    public void LoadNextScene(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadPreviousScene(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void LoadTitleMenu(){
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(0));
    }
}
