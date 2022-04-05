using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{

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

    enum Direction
    {Right,Up,Left,Down}
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void proceduralGenerationOne(int mainrooms, int num_detours, pos detourDepth)
    {
        Direction previous = Direction.Left;
        HashSet<Direction> path_Options= new HashSet<Direction>();
        path_Options.Add(Direction.Up); path_Options.Add(Direction.Right); path_Options.Add(Direction.Down);
        pos currentPos = new pos(1,0);
        Dictionary<pos, HashSet<Direction>> detours = new Dictionary<pos, HashSet<Direction>>();
        Dictionary<pos, HashSet<Direction>> mainpaths = new Dictionary<pos, HashSet<Direction>>();
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
