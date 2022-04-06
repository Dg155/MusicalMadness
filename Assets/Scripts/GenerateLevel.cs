using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public enum RoomType {All,R,U,L,D,RL,UL,LD,RU,RD,UD,RUL,RLD,RUD,ULD}

public enum Direction {Right,Up,Left,Down}

public struct DungeonInfo
{
    public Dictionary<pos, RoomType> mainPaths;
    public Dictionary<pos, RoomType> detourPaths;
    public List<pos> orderedMainPath;
    public List<pos> deadEnds;
}
public class GenerateLevel : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DungeonInfo proceduralGenerationOne(int mainrooms, int num_of_detours, pos detourDepth)
    //Function creates the structure of the rooms based on specific parameters
    {
        DungeonInfo info = new DungeonInfo();
        Direction previous = Direction.Left;
        HashSet<Direction> path_Options = new HashSet<Direction>();
        path_Options.Add(Direction.Up); path_Options.Add(Direction.Right); path_Options.Add(Direction.Down);
        pos currentPos = new pos(1,0);
        Dictionary<pos, RoomType> detours = new Dictionary<pos, RoomType>();
        Dictionary<pos, RoomType> mainpaths = new Dictionary<pos, RoomType>();
        HashSet<int> detourPoints = createDetourPoints(mainrooms, num_of_detours);
        List<pos> deadEnds = new List<pos>();
        HashSet<Direction> all_Directions = new HashSet<Direction>();
        all_Directions.Add(Direction.Up); all_Directions.Add(Direction.Right); all_Directions.Add(Direction.Down); all_Directions.Add(Direction.Left);

        //Add first room At (0,0) one right door
        mainpaths.Add(new pos(0,0), RoomType.R);


        //THE BUG: The bug occurs when you create a mainPath room in the spot that covers the opening for where a detour should be
        //the bug occurs because we select the next direction, then generate the detours(before adding in the next main path)
        //which causes collision
        
        //instead of generating the mainPath and detours simulataneously(as it's done in what you wrote)

        //I was saying we instead generate the entire main path first, marking where the detours begin (which are adjacent to the mainPath),
        // also checking to make sure the next main path node does not violate a detour beginning(which is what is causing the current bug)
        //then in a separate loop, we loop through all the detour starts and make detours that do not interfere with the main path

        //With this method, there would also be no nested looping, just two separate loops




        // Detour points
        for(int i=0; i < mainrooms; ++i)
        {
            //Filter path_options to not collide with any grids in detours
            HashSet<Direction> filteredPathOptions = filterPathOptions(currentPos, mainpaths, detours, all_Directions);
            //doorOptions ← empty set
            HashSet<Direction> doorOptions = new HashSet<Direction>();
            //doorOptions.add(Previous)
            doorOptions.Add(previous);
            //New_Main_Direction ← random selection from (path_opt - previous)
            HashSet<Direction> possible_options = new HashSet<Direction>(path_Options);
            possible_options.Remove(previous);
            Direction[] hashSetArray = new Direction[possible_options.Count];
            possible_options.CopyTo(hashSetArray);
            Direction New_Main_Direction = hashSetArray[Random.Range(0, hashSetArray.Length)];
            //doorOptions.add(New_Main_Direction)
            doorOptions.Add(New_Main_Direction);

            // Detour Loop
            if (detourPoints.Contains(i))
            {
                
                Debug.Log(i);
                //New_Detour_Direction ← random selection from (path_options - doorOptions)
                HashSet<Direction> possible_detours = new HashSet<Direction>(path_Options);
                possible_detours.ExceptWith(doorOptions);
                Direction[] hashSetDetourArray = new Direction[possible_detours.Count];
                possible_detours.CopyTo(hashSetDetourArray);
                Direction New_Detour_Direction = hashSetDetourArray[Random.Range(0, hashSetDetourArray.Length)];
                //doorOptions.add(New_Direction)
                doorOptions.Add(New_Detour_Direction);
                //detourPrevious ← NegateDirection(New_Detour_Direction)
                Direction detourPrevious = flipDirection(New_Detour_Direction);
                //detourPos ← offsetPos(currentPos, New_Detour_Direction)
                pos detourPos = offsetPos(currentPos, New_Detour_Direction);
                int detourIter = Random.Range(detourDepth.x, detourDepth.y);
                for (int j=1; j <= detourIter; ++j)
                {
                    //Filter out all the taken directions
                    HashSet<Direction> filteredDetourPathOptions = filterPathOptions(detourPos, mainpaths, detours, all_Directions);
                    if (filteredDetourPathOptions.Count == 0) {deadEnds.Add(detourPos); break;}
                    //detourDoorOptions ← empty set
                    HashSet<Direction> detourDoorOptions = new HashSet<Direction>();
                    //detourDoorOptions.add(detourPrevious)
                    detourDoorOptions.Add(detourPrevious);
                    //New_Detour_Direction ← random(filteredpath-detourPrevious)
                    if (j != detourIter)
                    {
                        possible_detours = new HashSet<Direction>(filteredDetourPathOptions);
                        possible_detours.Remove(detourPrevious);
                        hashSetDetourArray = new Direction[possible_detours.Count];
                        possible_detours.CopyTo(hashSetDetourArray);
                        New_Detour_Direction = hashSetDetourArray[Random.Range(0, hashSetDetourArray.Length)];
                        //detourDoorOptions.add(New_Detour_Direction)
                    detourDoorOptions.Add(New_Detour_Direction);
                    }
                    //detourRoom ← convertToRoomType(detourdoorOptions)
                    RoomType detourRoom = convertToRoomType(detourDoorOptions);
                    //detours.Add(detourPos, detourRoom)
                    detours.Add(detourPos, detourRoom);
                    //detourPrevious ← NegateDirection(New_Detour_Direction)
                    detourPrevious = flipDirection(New_Detour_Direction);
                    //detourPos ← offsetPos(detourtPos, New_Detour_Direction)
                    detourPos = offsetPos(detourPos, New_Detour_Direction);
                }
            }

            //RoomType ← convertToRoomType(doorOptions)
            RoomType Room = convertToRoomType(doorOptions);
            //MainPaths.add(Current_Postion, RoomType)
            mainpaths.Add(currentPos, Room);
            //Previous ← NegateDirection(New_Main_Direction)
            previous = flipDirection(New_Main_Direction);
            //Current_Position ← Next_Position
            currentPos = offsetPos(currentPos, New_Main_Direction);
        }
        info.mainPaths = mainpaths;
        info.detourPaths = detours;
        return info;
    }

    private pos offsetPos(pos currentPosition, Direction direction)
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
    private Direction flipDirection(Direction direction)
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

    
    private RoomType convertToRoomType(HashSet<Direction> doorOptions)
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
    {
        HashSet<int> detourPoints = new HashSet<int>();
        while(detourPoints.Count < size)
        {
            int roomNum = Random.Range(1, num_of_rooms);
            if (roomNum != num_of_rooms / 2) {detourPoints.Add(roomNum);}
        }
        return detourPoints;
    }
    
}
