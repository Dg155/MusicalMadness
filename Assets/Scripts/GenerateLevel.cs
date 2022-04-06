using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct pos
{
    public int x;
    public int y;

    public pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

enum RoomType {All,R,U,L,D,RL,UL,LD,RU,RD,UD,RUL,RLD,RUD,ULD}

enum Direction {Right,Up,Left,Down}

struct DungeonInfo
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
        pos P = new pos(0,1);
        //proceduralGenerationOne(1, 1, P);
        HashSet<Direction> path_Options = new HashSet<Direction>();
        path_Options.Add(Direction.Up); 
        path_Options.Add(Direction.Right); 
        path_Options.Add(Direction.Down);
        path_Options.Add(Direction.Left);
        Debug.Log(convertToRoomType(path_Options));
        Debug.Log("Hello");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private DungeonInfo proceduralGenerationOne(int mainrooms, int num_detours, pos detourDepth)
    //Function creates the structure of the rooms based on specific parameters
    {
        DungeonInfo info = new DungeonInfo();
        Direction previous = Direction.Left;
        HashSet<Direction> path_Options = new HashSet<Direction>();
        path_Options.Add(Direction.Up); path_Options.Add(Direction.Right); path_Options.Add(Direction.Down);
        pos currentPos = new pos(1,0);
        Dictionary<pos, HashSet<Direction>> detours = new Dictionary<pos, HashSet<Direction>>();
        Dictionary<pos, HashSet<Direction>> mainpaths = new Dictionary<pos, HashSet<Direction>>();

        //Add first room At (0,0) one right door

        // Detour points
        for(int i=0; i < mainrooms; ++i)
        {
            //Filter path_options to not collide with any grids in detours UNFINISHED
            HashSet<Direction> filteredPathOptions = filterPathOptions(currentPos, detours, path_Options);
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

            //RoomType ← convertToRoomType(doorOptions)
            //RoomType Room = convertToRoomType(doorOptions);
            //MainPaths.add(Current_Postion, RoomType)
            
            //Previous ← NegateDirection(New_Main_Direction)
            previous = flipDirection(New_Main_Direction);
            //Current_Position ← Next_Position
            currentPos = offsetPos(currentPos, New_Main_Direction);
        }
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
    private HashSet<Direction> filterPathOptions(pos currentPos, Dictionary<pos, HashSet<Direction>> offLimits, HashSet<Direction> directions){
        //Helper function removes directions from a set of directions if there is a collision
        foreach (Direction d in directions){
            if (offLimits.ContainsKey(offsetPos(currentPos, d))){
                directions.Remove(d); //MAKE SURE THAT THIS DOESNT AFFECT THE LOOP WHILE LOOPING THROUGH IT
            }
        }
        return directions;
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
    
}
