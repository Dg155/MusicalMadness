using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateLevel : MonoBehaviour
{
    Dictionary<Dir, GameObject> rooms;
    public GameObject grid;
    public int roomSize = 8;
    public GameObject All, R, U, L, D, RU, RL, RD, UL, UD, LD, RUL, ULD, RUD, RLD;
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
    void InstantiateRoom(pos position, Dir roomType){
        var newRoom = Instantiate (rooms[roomType], new Vector3(position.x * roomSize ,position.y * roomSize, 0) , Quaternion.identity);
        newRoom.transform.parent = grid.transform;
    }

    public void InstantiateFromDungeonInfo(DungeonInfo info){
        //UNFINISHED
        Dictionary<pos, Dir> mainPaths = info.mainPaths;
        foreach(var room in mainPaths){
            InstantiateRoom(room.Key, room.Value);
        }
        Dictionary<pos, Dir> detourPaths = info.detourPaths;
        foreach(var room in detourPaths){
            InstantiateRoom(room.Key, room.Value);
        }
    }
}

