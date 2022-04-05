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

enum RoomType
{All,R,U,L,D,LR,LU,LD,RU,RD,UD,RUL,RLD,RUD,ULD}

enum Direction
{Right,Up,Left,Down}
public class GenerateLevel : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        pos P = new pos(0,1);
        proceduralGenerationOne(1, 1, P);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void proceduralGenerationOne(int mainrooms, int num_detours, pos detourDepth)
    {
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
            //Filter path_options to not collide with any grids in detours

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

            //CreateRoom

            //Next_Position ← offsetPos(Current_Position, New_Main_Direction)
            pos nextPos = offsetPos(currentPos, New_Main_Direction);
            //MainPaths.add(Nex_Position, {Previous})
            
            //Previous ← NegateDirection(Previous)
            previous = flipDirection(previous);
            //Current_Position ← Next_Position
            currentPos = nextPos;
        }
    }

    private pos offsetPos(pos currentPosition, Direction direction)
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

    private Direction flipDirection(Direction direction)
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
}
