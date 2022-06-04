using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Player"){
            return;
        }
        FindObjectOfType<LevelLoader>().LoadNextScene();
    }
}
