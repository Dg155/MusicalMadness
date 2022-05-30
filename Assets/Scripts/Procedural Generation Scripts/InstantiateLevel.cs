using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateLevel : MonoBehaviour
{
    Dictionary<Dir, GameObject> rooms;
    public GameObject grid;
    public int roomSize = 8;
    public GameObject All, R, U, L, D, RU, RL, RD, UL, UD, LD, RUL, ULD, RUD, RLD;
    public GameObject TreasureChest;
    private List<Item> artifacts = new List<Item>();
    private List<GameObject> spawnedChests = new List<GameObject>();
    System.Random random = new System.Random();
    public GameObject ambushChest;
    public Item chestHeart;
    public GameObject transporter;

    void Awake(){
        rooms = new Dictionary<Dir, GameObject>();
        rooms.Add(Dir.D|Dir.L|Dir.R|Dir.U,All);
        rooms.Add(Dir.R,R);
        rooms.Add(Dir.U,U);
        rooms.Add(Dir.L,L);
        rooms.Add(Dir.D,D);
        rooms.Add(Dir.R | Dir.L,RL);
        rooms.Add(Dir.U | Dir.L,UL);
        rooms.Add(Dir.L | Dir.D, LD);
        rooms.Add(Dir.R|Dir.U,RU);
        rooms.Add(Dir.R|Dir.D,RD);
        rooms.Add(Dir.U|Dir.D,UD);
        rooms.Add(Dir.R|Dir.U|Dir.L,RUL);
        rooms.Add(Dir.R|Dir.L|Dir.D,RLD);
        rooms.Add(Dir.R|Dir.U|Dir.D,RUD);
        rooms.Add(Dir.U|Dir.L|Dir.D,ULD);
    }

    void Start(){
    }

    public void setArtifacts(ScriptableLevelArtifacts levelArtifacts)
    {
        artifacts = new List<Item>(levelArtifacts.Artifacts);
    }

    void InstantiateRoom(pos position, Dir roomType){
        var newRoom = Instantiate (rooms[roomType], new Vector3(position.x * roomSize ,position.y * roomSize, 0) , Quaternion.identity);
        newRoom.transform.parent = grid.transform;
    }

    void InstantiateDeadEnd(pos position, DeadEnds roomType, Dir direction){
        if (roomType == DeadEnds.Chest) {
            var newChest = Instantiate(TreasureChest, new Vector3(position.x * roomSize ,position.y * roomSize, 0), Quaternion.identity);
            spawnedChests.Add(newChest);
        }
        if (roomType == DeadEnds.Amush) {
            var newChest = Instantiate(ambushChest, new Vector3(position.x * roomSize ,position.y * roomSize, 0), Quaternion.identity);
            AmbushTrigger chestScript = newChest.GetComponent<AmbushTrigger>();
            chestScript.setMonster(Monsters.Violin);
            chestScript.setRoomPos(position);
            chestScript.setBlocker(direction);
        }
    }

    public void InstantiateTransport(pos position)
    {
        Instantiate(transporter, new Vector3(position.x * roomSize ,position.y * roomSize, 0), Quaternion.identity);
    }

    void populateChests()
    {
        while (spawnedChests.Count != 0)
        {
            var newChest = spawnedChests[random.Next(spawnedChests.Count)];
            spawnedChests.Remove(newChest);
            if (artifacts.Count > 0)
            {
                Item artifact = artifacts[random.Next(artifacts.Count)];
                artifacts.Remove(artifact);
                newChest.GetComponent<TreasureChest>().setItem(artifact);
            }
            else {newChest.GetComponent<TreasureChest>().setItem(chestHeart);}
        }
    }

    public void InstantiateFromDungeonInfo(DungeonInfo info){
        Dictionary<pos, Dir> mainPaths = info.mainPaths;
        foreach(var room in mainPaths){
            InstantiateRoom(room.Key, room.Value);
        }
        Dictionary<pos, Dir> detourPaths = info.detourPaths;
        foreach(var room in detourPaths){
            InstantiateRoom(room.Key, room.Value);
        }
        Dictionary<pos, DeadEnds> deadEnds = info.deadEnds;
        foreach(var room in deadEnds)
        {
            InstantiateDeadEnd(room.Key, room.Value, detourPaths[room.Key]);
        }
        populateChests();
    }
}

