using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    LevelInfo levelInfo;
    int roomscale = 8;
    Dictionary <Monsters, GameObject> monsters;
    public GameObject violin, tambourine, demon;

    HashSet<pos> alreadyInstantiated;

    public Dictionary<pos, List<EnemyAI>> instantiatedMonsters;
    // Start is called before the first frame update

    void Update(){
        if (instantiatedMonsters.ContainsKey(levelInfo.currPlayerPos)){
            foreach (EnemyAI e in instantiatedMonsters[levelInfo.currPlayerPos]){
                Debug.Log(levelInfo.currPlayerPos.x.ToString() + "," + levelInfo.currPlayerPos.y.ToString());
                e.OnUpdate(levelInfo.currPlayerPos);
            }
        }
    }
    void Start()
    {
        instantiatedMonsters = new Dictionary<pos, List<EnemyAI>>();
        alreadyInstantiated = new HashSet<pos>();
        levelInfo = this.GetComponent<LevelInfo>();
        monsters = new Dictionary <Monsters, GameObject>();
        monsters.Add(Monsters.Violin, violin);
        monsters.Add(Monsters.Tambourine, violin);//change once several enemies implemented
        monsters.Add(Monsters.Demon, violin);//change once several enemies implemented
        InstantiateAdjacentEnemies();
        pos origin = new pos(0,0);
        InstantiateEnemiesInRoom(levelInfo.dungeonInfo.monstersPerRoom[origin], origin);
    }

    public void InstantiateAdjacentEnemies(){
        Debug.Log("DOING");
        foreach(pos currPos in AdjacentPositions(levelInfo.currPlayerPos)){
            Debug.Log(currPos.x);
            Debug.Log(currPos.y);
            if (
                !alreadyInstantiated.Contains(currPos)
                &&
                (levelInfo.dungeonInfo.mainPaths.ContainsKey(currPos) ||
                levelInfo.dungeonInfo.detourPaths.ContainsKey(currPos))
                &&
                levelInfo.dungeonInfo.monstersPerRoom.ContainsKey(currPos))

            {
                Debug.Log("Success!");
                InstantiateEnemiesInRoom(levelInfo.dungeonInfo.monstersPerRoom[currPos], currPos);
            }
        }
    }
    List<pos> AdjacentPositions(pos roomPos){
        List<pos> adjacentPositions = new List<pos>();

        adjacentPositions.Add(new pos(roomPos.x + 0, roomPos.y + 1));
        adjacentPositions.Add(new pos(roomPos.x + 0, roomPos.y + -1));
        adjacentPositions.Add(new pos(roomPos.x + 1, roomPos.y + 0));
        adjacentPositions.Add(new pos(roomPos.x + -1, roomPos.y + 0));
                
        return adjacentPositions;
    }
    // Update is called once per frame
    void InstantiateEnemiesInRoom(List<Monsters> monsters, pos roomPos){
        foreach(Monsters m in monsters){
            InstantiateEnemyInRoom(m, roomPos);
        }
        alreadyInstantiated.Add(roomPos);
    }
    void InstantiateEnemyInRoom(Monsters monster, pos roomPos){
        int spawnAreaOffset = roomscale/2;
        int offsetx = UnityEngine.Random.Range(-roomscale, roomscale);
        int offsety = UnityEngine.Random.Range(-roomscale, roomscale);
        var enemy = Instantiate(monsters[monster], new Vector3(roomPos.x * roomscale, roomPos.y * roomscale,0), Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>(); //get the enemy's specific AI script
        if (enemyAI != null){
            if (!instantiatedMonsters.ContainsKey(roomPos)){
                instantiatedMonsters.Add(roomPos, new List<EnemyAI>());
            }
            instantiatedMonsters[roomPos].Add(enemyAI);
        }
    }
}
