using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateLevel : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<RoomType, GameObject> rooms;
    public GameObject grid;
    public int roomSize = 8;
    public GameObject All, R, U, L, D, RU, RL, RD, UL, UD, LD, RUL, ULD, RUD, RLD;
    void Awake(){
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
    void InstantiateRoom(pos position, RoomType roomType){
        var newRoom = Instantiate (rooms[roomType], new Vector3(position.x * roomSize ,position.y * roomSize, 0) , Quaternion.identity);
        newRoom.transform.parent = grid.transform;
    }

    public void InstantiateFromDungeonInfo(DungeonInfo info){
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

