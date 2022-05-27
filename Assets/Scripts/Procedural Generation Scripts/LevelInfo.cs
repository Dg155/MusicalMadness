using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public pos currPlayerPos;
    public DungeonInfo dungeonInfo;
    public ScriptableLevelArtifacts levelArtifacts;
    public GameObject Blocker;
    public int roomSize = 8;
    private (pos, Dir) finalRoom;

    public EnemyManager enemyManagerTEMP; //TEMPORARY REMOVE ONCE EVENT LISTENING ADDED IN
    // Start is called before the first frame update
    void Start()
    {
        currPlayerPos = new pos(0, 0);
        InstantiateLevel IL = this.GetComponent<InstantiateLevel>();
        IL.setArtifacts(levelArtifacts);
        IL.InstantiateFromDungeonInfo(dungeonInfo);
    }

    public void changePos(int x, int y) //currently called by CameraMove.cs script
    {
        currPlayerPos.x += x;
        currPlayerPos.y += y;
        enemyManagerTEMP.InstantiateAdjacentEnemies();

    }

    public void setFinalRoom(pos position, Dir direction)
    {
        GameObject doorBlock;
        finalRoom = (position, direction);
        if (direction == Dir.R) {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(4, 0, 0), Quaternion.Euler(0f, 0f, 90f));}
        else if (direction == Dir.L) {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(-4, 0, 0), Quaternion.Euler(0f, 0f, 90f));}
        else if (direction == Dir.U) {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(0, 4, 0), Quaternion.identity);}
        else {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(0, -4, 0), Quaternion.identity);}
        doorBlock.tag = "FinalBlock";
    }
    
}
