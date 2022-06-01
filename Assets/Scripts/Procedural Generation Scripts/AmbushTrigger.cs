using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushTrigger : MonoBehaviour
{
    public GameObject Blocker;
    public EnemyManager enemyManager;
    public Monsters ambushMonster;
    public AudioClip ambushMusic;
    private bool Ambushed = false;
    private pos roomPos;
    private Dir roomDirection;
    private int blockerNumber;

    private void Start() {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    public void setRoomPos(pos room)
    {
        roomPos = room;
    }

    public void setMonster(Monsters monster)
    {
        ambushMonster = monster;
    }
    public void setBlocker(Dir direction)
    {
        roomDirection = direction;
    }

    void InstantiateBlocker()
    {
        if (roomDirection == Dir.R) {Instantiate(Blocker, this.transform.position + new Vector3(4, 0, 0), Quaternion.Euler(0f, 0f, 90f));}
        else if (roomDirection == Dir.L) {Instantiate(Blocker, this.transform.position + new Vector3(-4, 0, 0), Quaternion.Euler(0f, 0f, 90f));}
        else if (roomDirection == Dir.U) {Instantiate(Blocker, this.transform.position + new Vector3(0, 4, 0), Quaternion.identity);}
        else {Instantiate(Blocker, this.transform.position + new Vector3(0, -4, 0), Quaternion.identity);}
    }

    IEnumerator startAmbush()
    {
        Ambushed = true;
        InstantiateBlocker();
        // Darken the Screen
        LevelLoader l= FindObjectOfType<LevelLoader>();
        if (l != null){
            l.DarkenScreen();
        }
        AudioSource musicPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        musicPlayer.clip = ambushMusic;
        musicPlayer.Play();
        yield return new WaitForSeconds(3.4f);
        if (l != null){
            l.LightenScreen();
        }
        yield return new WaitForSeconds(0.3f);
        enemyManager.InstantiateAmbushRoomEnemies(ambushMonster, roomPos);

        // Lighten the screen
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!Ambushed && other.tag == "Player")
        {
            StartCoroutine("startAmbush");
        }
    }
}
