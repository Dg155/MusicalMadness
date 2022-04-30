using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public pos currPlayerPos;
    public DungeonInfo dungeonInfo;

    public EnemyManager enemyManagerTEMP; //TEMPORARY REMOVE ONCE EVENT LISTENING ADDED IN
    // Start is called before the first frame update
    void Start()
    {
        currPlayerPos = new pos(0, 0);
        this.GetComponent<InstantiateLevel>().InstantiateFromDungeonInfo(dungeonInfo);
    }

    public void changePos(int x, int y) //currently called by CameraMove.cs script
    {
        currPlayerPos.x += x;
        currPlayerPos.y += y;
        enemyManagerTEMP.InstantiateAdjacentEnemies();

    }
}
