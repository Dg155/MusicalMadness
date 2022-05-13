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
}
