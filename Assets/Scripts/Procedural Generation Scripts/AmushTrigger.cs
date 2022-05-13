using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmushTrigger : MonoBehaviour
{
    public GameObject Blocker;
    private bool Ambushed = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!Ambushed && other.tag == "Player")
        {
            Ambushed = true;
            Instantiate(Blocker, this.transform.position, Quaternion.identity);
        }
    }
}
