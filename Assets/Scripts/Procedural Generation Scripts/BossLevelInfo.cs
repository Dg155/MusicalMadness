using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelInfo : MonoBehaviour
{
    public pos currPlayerPos;
    public DungeonInfo dungeonInfo;
    public int roomSize = 8;

    public BossManager enemyManagerTEMP; //TEMPORARY REMOVE ONCE EVENT LISTENING ADDED IN
    // Start is called before the first frame update
    void Start()
    {
        currPlayerPos = new pos(0, 0);
        InstantiateLevel IL = this.GetComponent<InstantiateLevel>();
        IL.InstantiateBossRoom(currPlayerPos);
    }

    public void changePos(int x, int y) //currently called by CameraMove.cs script
    {
        currPlayerPos.x += x;
        currPlayerPos.y += y;
    }
}
