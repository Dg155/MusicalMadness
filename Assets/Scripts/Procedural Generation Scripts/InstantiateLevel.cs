using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Edits: 
Made 4/14 by Hung Bui 8:42 AM
    Commented in replacement code
*/
public class InstantiateLevel : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<RoomType, GameObject> rooms;
    //Uncomment/Use when ready
    //Dictionary<Dir, GameObject> rooms;
    public GameObject grid;
    public int roomSize = 8;
    public GameObject All, R, U, L, D, RU, RL, RD, UL, UD, LD, RUL, ULD, RUD, RLD;
    void Awake(){
        /*
        //Uncomment/Use when ready
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
        */

        rooms = new Dictionary<RoomType, GameObject>();
        rooms.Add(RoomType.All,All);
        rooms.Add(RoomType.R,R);
        rooms.Add(RoomType.U,U);
        rooms.Add(RoomType.L,L);
        rooms.Add(RoomType.D,D);
        rooms.Add(RoomType.RL,RL);
        rooms.Add(RoomType.UL,UL);
        rooms.Add(RoomType.LD, LD);
        rooms.Add(RoomType.RU,RU);
        rooms.Add(RoomType.RD,RD);
        rooms.Add(RoomType.UD,UD);
        rooms.Add(RoomType.RUL,RUL);
        rooms.Add(RoomType.RLD,RLD);
        rooms.Add(RoomType.RUD,RUD);
        rooms.Add(RoomType.ULD,ULD);
    }
    void Start(){
    }
    void InstantiateRoom(pos position, RoomType roomType){ //CHANGE
        var newRoom = Instantiate (rooms[roomType], new Vector3(position.x * roomSize ,position.y * roomSize, 0) , Quaternion.identity);
        newRoom.transform.parent = grid.transform;
    }

    public void InstantiateFromDungeonInfo(DungeonInfo info){ //CHANGE
        //UNFINISHED
        Dictionary<pos, RoomType> mainPaths = info.mainPaths;
        foreach(var room in mainPaths){
            InstantiateRoom(room.Key, room.Value);
        }
        Dictionary<pos, RoomType> detourPaths = info.detourPaths;
        foreach(var room in detourPaths){
            InstantiateRoom(room.Key, room.Value);
        }
    }
}

