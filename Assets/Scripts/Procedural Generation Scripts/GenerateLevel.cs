using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
Edits: 
Made 4/14 by Hung Bui 8:42 AM
    Created new datatype Dir
    Commented in replacement code
    Created choose random Dir function
    Marked data structures/methods to be deleted/changed
*/



public struct pos
{
    public int x;
    public int y;

    public pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public enum RoomType {All,R,U,L,D,RL,UL,LD,RU,RD,UD,RUL,RLD,RUD,ULD}//DELETE ONCE DONE

[Flags] //NEW ROOM DATA STRUCTURE
public enum Dir{R = 1, U = 2, L = 4, D = 8}

public enum Direction {Right,Up,Left,Down} //DELETE ONCE DONE

public struct DungeonInfo //CHANGE ONCE DONE
{
    //public Dictionary<pos, Dir> mainRoom
    //public Dictionary<pos, Dir> detourRoom
    public Dictionary<pos, RoomType> mainPaths;
    public Dictionary<pos, RoomType> detourPaths;
    public List<pos> orderedMainRoom;
    public List<pos> deadEnds;
}

public class GenerateLevel : MonoBehaviour
{
    
    public int amountOfRooms = 10;
    public int numOfDetours = 3;
    public int minDetourDepth = 3;
    public int maxDetourDepth = 5;

    // Start is called before the first frame update
    void Start()
    {
        DungeonInfo temp = proceduralGenerationOne(amountOfRooms, numOfDetours, new pos(minDetourDepth,maxDetourDepth));
        this.GetComponent<InstantiateLevel>().InstantiateFromDungeonInfo(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DungeonInfo proceduralGenerationOne(int mainrooms, int num_of_detours, pos detourDepth)
    //CHANGE(currently untouched)
    //Function creates the structure of the rooms based on specific parameters
    {
        //Create the dungeon information
        DungeonInfo info = new DungeonInfo();
        Dictionary<pos, RoomType> mainpaths = new Dictionary<pos, RoomType>();
        Dictionary<pos, RoomType> detours = new Dictionary<pos, RoomType>();
        List<pos> orderedMainRoom = new List<pos>();
        List<pos> deadEnds = new List<pos>();

        //Directions for the Main Path (Right, Up, Down), and a set of all directions for detours
        HashSet<Direction> path_Options = new HashSet<Direction>();
        path_Options.Add(Direction.Up); path_Options.Add(Direction.Right); path_Options.Add(Direction.Down);
        HashSet<Direction> all_Directions = new HashSet<Direction>();
        all_Directions.Add(Direction.Up); all_Directions.Add(Direction.Right); all_Directions.Add(Direction.Down); all_Directions.Add(Direction.Left);

        //Attributes needed for main room generation
        Direction previous = Direction.Left;
        pos currentPos = new pos(1,0);
        HashSet<int> detourPoints = createDetourPoints(mainrooms, num_of_detours);

        //Add first room At (0,0) with one right door
        mainpaths.Add(new pos(0,0), RoomType.R);

        //Loop through and generate each main path room and mark the start of detours
        for(int i=0; i < mainrooms; ++i)
        {
            //Filter path_options to not collide with any already generated rooms
            HashSet<Direction> filteredPathOptions = filterPathOptions(currentPos, mainpaths, detours, path_Options);
            //Create an empty set that will hold all directions the room can go
            HashSet<Direction> doorOptions = new HashSet<Direction>();
            //Add the direction of the previous room
            doorOptions.Add(previous);
            //Remove the direction of the previous room and randomly select another direction for the room to lead to
            HashSet<Direction> possible_options = new HashSet<Direction>(filteredPathOptions);
            possible_options.Remove(previous);
            Direction[] hashSetArray = new Direction[possible_options.Count];
            possible_options.CopyTo(hashSetArray);
            Direction New_Main_Direction = hashSetArray[UnityEngine.Random.Range(0, hashSetArray.Length)];
            //Add the new direction to the set of directions
            doorOptions.Add(New_Main_Direction);

            // If the room is designated as a room that leads to a detour, mark the detour
            if (detourPoints.Contains(i))
            {
                //Remove the already taken directions in the room, and randomly pick another one to lead to the detour
                HashSet<Direction> possible_detours = new HashSet<Direction>(filteredPathOptions);
                possible_detours.ExceptWith(doorOptions);
                Direction[] hashSetDetourArray = new Direction[possible_detours.Count];
                possible_detours.CopyTo(hashSetDetourArray);
                //Confirm that there is a direction to put the detour in
                if (hashSetDetourArray.Length != 0)
                {
                    //Select new direction for detour and add it to the set
                    Direction New_Detour_Direction = hashSetDetourArray[UnityEngine.Random.Range(0, hashSetDetourArray.Length)];
                    doorOptions.Add(New_Detour_Direction);
                    //Mark the beginning of the detour with a room so later main path rooms do not overlap
                    HashSet<Direction> detourRoom = new HashSet<Direction>();
                    detourRoom.Add(flipDirection(New_Detour_Direction));
                    detours.Add(offsetPos(currentPos, New_Detour_Direction), convertToRoomType(detourRoom));
                }
            }
            //Convert the set of directions into the corresponding room
            RoomType Room = convertToRoomType(doorOptions);
            //MainPaths.add(Current_Postion, RoomType)
            mainpaths.Add(currentPos, Room);
            //Add coordinates to orderedMainRooms
            orderedMainRoom.Add(currentPos);
            //Previous ← NegateDirection(New_Main_Direction)
            previous = flipDirection(New_Main_Direction);
            //Current_Position ← Next_Position
            currentPos = offsetPos(currentPos, New_Main_Direction);
        }
        
        // Loop to create the detours
        Dictionary<pos, RoomType> detoursCopy = new Dictionary<pos, RoomType>(detours); //Copy so we are not changing data strucutre while iterating over it
        foreach(var item in detoursCopy)
        {
            //Determine previous direction from marked room type
            Direction detourPrevious;
            if (item.Value == RoomType.R){detourPrevious = Direction.Right;}
            else if (item.Value == RoomType.U){detourPrevious = Direction.Up;}
            else if (item.Value == RoomType.L){detourPrevious = Direction.Left;}
            else{detourPrevious = Direction.Down;}
            pos detourPos = item.Key;
            //Detour will have a random depth
            int detourIter = UnityEngine.Random.Range(detourDepth.x, detourDepth.y);
            for (int j=1; j <= detourIter; ++j)
            {
                //Filter out all the directions taken up by rooms
                HashSet<Direction> filteredDetourPathOptions = filterPathOptions(detourPos, mainpaths, detours, all_Directions);
                //If there are no available directions then detour can't go anywhere and is a dead end
                if (filteredDetourPathOptions.Count == 0) {deadEnds.Add(detourPos); break;}
                //detourDoorOptions ← empty set
                HashSet<Direction> detourDoorOptions = new HashSet<Direction>();
                //detourDoorOptions.add(detourPrevious)
                detourDoorOptions.Add(detourPrevious);
                //If it is not the last room in the detour, then add another direction to lead to next detour room
                if (j != detourIter)
                {
                    //Remove already taken direction from available directions, then randomly pick from remaining ones
                    HashSet<Direction> possible_detours = new HashSet<Direction>(filteredDetourPathOptions);
                    possible_detours.Remove(detourPrevious);
                    Direction[] hashSetDetourArray = new Direction[possible_detours.Count];
                    possible_detours.CopyTo(hashSetDetourArray);
                    Direction New_Detour_Direction = hashSetDetourArray[UnityEngine.Random.Range(0, hashSetDetourArray.Length)];
                    //detourDoorOptions.add(New_Detour_Direction)
                    detourDoorOptions.Add(New_Detour_Direction);
                    //detourRoom ← convertToRoomType(detourdoorOptions)
                    RoomType detourRoom = convertToRoomType(detourDoorOptions);
                    //Add/replace the detour room into the dictionary
                    if (detours.ContainsKey(detourPos)) {detours[detourPos] = detourRoom;}
                    else {detours.Add(detourPos, detourRoom);}
                    //detourPrevious ← NegateDirection(New_Detour_Direction)
                    detourPrevious = flipDirection(New_Detour_Direction);
                    //detourPos ← offsetPos(detourtPos, New_Detour_Direction)
                    detourPos = offsetPos(detourPos, New_Detour_Direction);
                }
                else
                //If the room is the last one in the detour path
                {
                    //Add to deadEnds list
                    deadEnds.Add(detourPos);
                    RoomType detourRoom = convertToRoomType(detourDoorOptions);
                    //Add/replace the detour room into the dictionary
                    if (detours.ContainsKey(detourPos)) {detours[detourPos] = detourRoom;}
                    else {detours.Add(detourPos, detourRoom);}
                }
            }
        }

        //Copy info from function into struct
        info.mainPaths = mainpaths;
        info.detourPaths = detours;
        info.orderedMainRoom = orderedMainRoom;
        info.deadEnds = deadEnds;
        return info;
    }

    private Dir chooseRandomDir(Dir direction){
        //NEW function randomly selects a direction
        //Optimize later(eliminate the need for creating new List and new Random every time)
        System.Random rnd = new System.Random(); 
        List<Dir> possibleDir = new List<Dir>();
        foreach(Dir d in new List<Dir>{Dir.R, Dir.U, Dir.L, Dir.D}){
            if ((direction & d) != 0){
                possibleDir.Add(d);
            }   
        }
        return possibleDir[rnd.Next(possibleDir.Count)];
    }
    private pos offsetPos(pos currentPosition, Direction direction)//CHANGE
    //Helper Function receives a position, a direction, returning the position in that direction
    {
        switch(direction)
        {
            case Direction.Right:
                currentPosition.x += 1;
                break;
            case Direction.Up:
                currentPosition.y += 1;
                break;
            case Direction.Left:
                currentPosition.x -= 1;
                break;
            case Direction.Down:
                currentPosition.y -= 1;
                break;
        }
        return currentPosition;
    }
    private HashSet<Direction> filterPathOptions(pos currentPos, Dictionary<pos, RoomType> offMainLimits, Dictionary<pos, RoomType> offDetourLimits, HashSet<Direction> directions)
    //CHANGE
    //Helper function removes directions from a set of directions if there is a collision
    {
        HashSet<Direction> filteredDirections = new HashSet<Direction>(directions);
        foreach (Direction d in directions){
            if ((offMainLimits.ContainsKey(offsetPos(currentPos, d))) || (offDetourLimits.ContainsKey(offsetPos(currentPos, d))))
            {
                filteredDirections.Remove(d);
            }
        }
        return filteredDirections;
    }


    private Dir flipDir(Dir direction) 
    //NEW (replaces change) (can rename later)
    //Helper Function reverses the direction
    {
        if (direction == Dir.R){
            return Dir.L;
        }
        else if (direction == Dir.U){
            return Dir.D;
        }
        else if (direction == Dir.L){
            return Dir.R;
        }
        else{
            return Dir.U;
        }

    }
    private Direction flipDirection(Direction direction) //CHANGE
    //Helper Function reverses the direction
    {
        if (direction == Direction.Right)
        {
            return Direction.Left;
        }
        else if (direction == Direction.Up)
        {
            return Direction.Down;
        }
        else if (direction == Direction.Left)
        {
            return Direction.Right;
        }
        else
        {
            return Direction.Up;
        }
    }

    
    private RoomType convertToRoomType(HashSet<Direction> doorOptions) //DELETE 
    //Helper function returns a Room Type based off of directions in the set
    {
        if (doorOptions.Contains(Direction.Right))
        {
            if (doorOptions.Contains(Direction.Up))
            {
                if (doorOptions.Contains(Direction.Left))
                {
                    if (doorOptions.Contains(Direction.Down))
                    {
                        return RoomType.All;
                    }
                    return RoomType.RUL;
                }
                if (doorOptions.Contains(Direction.Down)){
                    return RoomType.RUD;
                }
                return RoomType.RU;
            }
            if (doorOptions.Contains(Direction.Left))
            {
                if (doorOptions.Contains(Direction.Down))
                {
                    return RoomType.RLD;
                }
                return RoomType.RL;
            }
            if (doorOptions.Contains(Direction.Down)){
                return RoomType.RD;
            }
            return RoomType.R;
        }
        if (doorOptions.Contains(Direction.Up))
        {
            if (doorOptions.Contains(Direction.Left))
            {
                if (doorOptions.Contains(Direction.Down))
                {
                    return RoomType.ULD;
                }
                return RoomType.UL;
            }
            if (doorOptions.Contains(Direction.Down)){
                return RoomType.UD;
            }
            return RoomType.U;
        }
        if (doorOptions.Contains(Direction.Left))
            {
                if (doorOptions.Contains(Direction.Down))
                {
                    return RoomType.LD;
                }
                return RoomType.L;
            }
        return RoomType.D;
    }

    private HashSet<int> createDetourPoints(int num_of_rooms, int size) 
    // Helper function which determines which rooms are going to have detours
    {
        List<int> nums = new List<int>();
        for(int i = 1; i < num_of_rooms; ++i)
        {
            nums.Add(i);
        }
        HashSet<int> detourPoints = new HashSet<int>();
        System.Random random = new System.Random(); //remove this once a global random is created
        for(int i=0; i < size; ++i)
        {
            int roomNum = random.Next(nums.Count);
            if (nums[roomNum] != num_of_rooms / 2) {detourPoints.Add(nums[roomNum]);}
            nums.RemoveAt(roomNum);
        }
        return detourPoints;
    }
    
}
