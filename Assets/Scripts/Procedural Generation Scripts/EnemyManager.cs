using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    LevelInfo levelInfo;
    int roomscale = 8;
    Dictionary <Monsters, GameObject> monsters;
    private List<(EnemyStats, EnemyAI)> ambushMonsters;
    private bool ambushRoomActivated = false;
    public GameObject violin, tambourine, demon;
    public AudioClip classicalMusic;

    HashSet<pos> alreadyInstantiated;

    public Dictionary<pos, List<(EnemyStats, EnemyAI)>> instantiatedMonsters;
    // Start is called before the first frame update

    void Update(){
        if (instantiatedMonsters.ContainsKey(levelInfo.currPlayerPos)){
            foreach ((EnemyStats, EnemyAI) e in instantiatedMonsters[levelInfo.currPlayerPos]){
                if (!e.Item2.getAlive()) {instantiatedMonsters[levelInfo.currPlayerPos].Remove(e); e.Item1.destroyEnemy();}
                //Debug.Log(levelInfo.currPlayerPos.x.ToString() + "," + levelInfo.currPlayerPos.y.ToString());
                e.Item2.OnUpdate(levelInfo.currPlayerPos);
            }
        }
        if (ambushMonsters.Count != 0){
            foreach((EnemyStats, EnemyAI) e in ambushMonsters)
            {
                if (!e.Item2.getAlive()) {ambushMonsters.Remove(e); e.Item1.destroyEnemy();}
                e.Item2.OnUpdate(levelInfo.currPlayerPos);
            }
        }
        if (ambushRoomActivated && ambushMonsters.Count == 0)
        {
            StartCoroutine(CompleteAmbushRoom());
        }
    }
    IEnumerator CompleteAmbushRoom(){
        ambushRoomActivated = false;
        GameObject door = GameObject.FindGameObjectWithTag("DoorBlock");
        door.GetComponent<Animator>().SetTrigger("ambushBeaten");
        AudioSource musicPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        musicPlayer.clip = classicalMusic;
        musicPlayer.Play();
        yield return new WaitForSeconds(.8f);
        Destroy(door);
        //Reward the player
    }
    void Start()
    {
        instantiatedMonsters = new Dictionary<pos, List<(EnemyStats, EnemyAI)>>();
        alreadyInstantiated = new HashSet<pos>();
        ambushMonsters = new List<(EnemyStats, EnemyAI)>();
        levelInfo = this.GetComponent<LevelInfo>();
        monsters = new Dictionary <Monsters, GameObject>();
        monsters.Add(Monsters.Violin, violin);
        monsters.Add(Monsters.Tambourine, tambourine);//change once several enemies implemented
        monsters.Add(Monsters.Demon, demon);//change once several enemies implemented
        InstantiateAdjacentEnemies();
        pos origin = new pos(0,0);
    }

    public void InstantiateAdjacentEnemies(){
        //Debug.Log("DOING");
        foreach(pos currPos in AdjacentPositions(levelInfo.currPlayerPos)){
            if (
                !alreadyInstantiated.Contains(currPos)
                &&
                (levelInfo.dungeonInfo.mainPaths.ContainsKey(currPos) ||
                levelInfo.dungeonInfo.detourPaths.ContainsKey(currPos))
                &&
                levelInfo.dungeonInfo.monstersPerRoom.ContainsKey(currPos))

            {
                //Debug.Log("Success!");
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

    List<(int, int)> AmbushEnemiesSpawn(pos roomPos)
    {
        List<(int, int)> spawnPositions = new List<(int, int)>();
        int roomCenterx = roomPos.x * roomscale;
        int roomCentery = roomPos.y * roomscale;
        for (int i = -1; i<2; ++i)
        {
            if (i!=0)
            {
                for (int j = -1; j<2; ++j)
                {
                    if (j!=0){spawnPositions.Add((roomCenterx + (2*i), roomCentery + (2*j)));}
                }
            }
        }

        return spawnPositions;
    }

    void InstantiateEnemiesInRoom(List<Monsters> monsters, pos roomPos){
        foreach(Monsters m in monsters){
            InstantiateEnemyInRoom(m, roomPos);
        }
        alreadyInstantiated.Add(roomPos);
    }
    void InstantiateEnemyInRoom(Monsters monster, pos roomPos){
        int spawnAreaOffset = roomscale/2 - 2;
        int offsetx = UnityEngine.Random.Range(-spawnAreaOffset, spawnAreaOffset);
        int offsety = UnityEngine.Random.Range(-spawnAreaOffset, spawnAreaOffset);
        var enemy = Instantiate(monsters[monster], new Vector3(roomPos.x * roomscale + offsetx, roomPos.y * roomscale + offsety,0), Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>(); //get the enemy's specific AI script
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>(); //get the enemy's specific Stats Script
        if (enemyAI != null){
            if (!instantiatedMonsters.ContainsKey(roomPos)){
                instantiatedMonsters.Add(roomPos, new List<(EnemyStats, EnemyAI)>());
            }
            instantiatedMonsters[roomPos].Add((enemyStats, enemyAI));
        }
    }
    public void InstantiateAmbushRoomEnemies(Monsters monster, pos roomPos)
    {
        List<(int, int)> spawnPositions = AmbushEnemiesSpawn(roomPos);
        foreach(var coordinate in spawnPositions)
        {
            var enemy = Instantiate(monsters[monster], new Vector3(coordinate.Item1, coordinate.Item2,0), Quaternion.identity);
            ambushMonsters.Add((enemy.GetComponent<EnemyStats>(), enemy.GetComponent<EnemyAI>()));
        }
        ambushRoomActivated = true;
    }
}
