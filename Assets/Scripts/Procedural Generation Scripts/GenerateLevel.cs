using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

[Flags] //NEW ROOM DATA STRUCTURE
public enum Dir{R = 1, U = 2, L = 4, D = 8}
public enum Monsters{Violin, Tambourine, Demon}
public enum DeadEnds{Nothing, Chest, Amush}

[System.Serializable]
public struct DungeonInfo
{
    public Dictionary<pos, Dir> mainPaths;
    public Dictionary<pos, Dir> detourPaths;
    public List<pos> orderedMainRoom;
    public Dictionary<pos, DeadEnds> deadEnds;
    public Dictionary<pos, List<Monsters>> monstersPerRoom;
}

public class GenerateLevel : MonoBehaviour
{
    public ScriptableLevelLayout levelLayout;
    public ScriptableLevelEnemies levelEnemies;
    System.Random random = new System.Random();

    [SerializeField] public Dictionary<int, Dictionary<Monsters, int>> dungeonTiers;

    void Awake()
    {
        levelEnemies.Generate();
        dungeonTiers = levelEnemies.dungeonTiers;
        DungeonInfo Roomlayout = proceduralGenerationOne(levelLayout.amountOfRooms, levelLayout.numOfDetours, new pos(levelLayout.minDetourDepth,levelLayout.maxDetourDepth), levelLayout.chestRooms, levelLayout.amushRooms);
        this.GetComponent<LevelInfo>().dungeonInfo = Roomlayout;
    }

    public DungeonInfo proceduralGenerationOne(int mainrooms, int num_of_detours, pos detourDepth, int num_of_chestrooms, int num_of_amushrooms)
    //Function creates the structure of the rooms based on specific parameters
    {
        /*** 
        /   Here we are creating the data structures that hold all the information to generate the level
        /   Your job is to populate each of these data structures, and finally return the DungeonInfo struct
        ***/
        DungeonInfo info = new DungeonInfo();
        Dictionary<pos, Dir> mainpaths = new Dictionary<pos, Dir>();
        Dictionary<pos, Dir> detours = new Dictionary<pos, Dir>();
        List<pos> orderedMainRoom = new List<pos>();
        Dictionary<pos, DeadEnds> deadEnds = new Dictionary<pos, DeadEnds>();
        List<pos> deadEndAssignment = new List<pos>();

        /*** 
        /   Here is an additional data structure we will use to help spawn the enemies within the level.
        /   It will be a dictionary with the key being the tier of the room (0, 1, or 2, with each tier being the difficulty of enemy spawns)
        /   The value will be a list of positions, for each room that is in that tier. For example, since the first room spawns at (0,0),
        /   and has to be the lowest tier, we would assign the dictionary entry dungeonTiers[0] to be a list containing the position (0,0)
        ***/
        Dictionary<int, List<pos>> roomTiers = new Dictionary<int, List<pos>>();
        for (int i = 0; i < 3; i++){roomTiers[i] = new List<pos>();}
        int lastTierOne = mainrooms/3;
        int lastTierTwo = lastTierOne * 2;

        /*** 
        /   We need to create two Dir enums, one for the main path, and one for all directions.
        /   We will use these to filter out directions that are not possible for the room to go in.
        /   The reason there are two is because we always want the generation of rooms to be going to the right, so the enum
        /   for the main path will be missing the left direction, and the enum for all directions will have all directions.
        ***/


        /*** 
        /   Since the first room will always be at (0,0), and will just be a room with a right door,
        /   we manually add it to the mainpaths dictionary with that inforamtion.
        ***/

        /*** 
        /   Now set up the variables needed for the main path generation loop.
        /   You will need to set up the previous direction, the current position, and the set of detour points.
        /   Remember, we are working off of the starting room, so your variables should reflect the second room in the main path.
        /   You can create a database of detour points by calling the createDetourPoints function, with num_of_detours as the parameter.
        ***/

        /*** 
        /   With all the information established, we can begin the main loop of room generation.
        ***/

        for (int i=0; i < mainrooms; ++i)
        {
            break;
            /*** 
            /   First, we need to filter out the directions that are not possible for the room to go in.
            /   You may use the filterPathOptions helper function
            ***/

            /*** 
            /   Next, we need to create a new Dir enum that will hold all the directions that the room will have doors in.
            /   We will start by adding the previous direction to the empty enum, since there will always be a door coming from that direction
            /   Then, we need to remove the previous direction from the filtered directions, and randomly select a new direction
            /   using the chooseRandomDir helper function.
            /   Finally, we need to add the new direction to the enum of room directions.
            ***/

            /***
            /   Before adding the room to the mainpaths dictionary, we need to check if the room is a detour point.
            /   (detours are rooms that have a third direction that leads to a smaller path)
            /   check if the detour points list contains the current room number
            /   If it does, create a new Dir from the filtered directions, making sure to remove the two already established directions
            /   If there are still possible directions to go, choose a random direction from the possible ones, add it to enum of room directions
            ***/

            /***
            /   While in the detour if statement, we need to add the room to the detours dictionary.
            /   First create a new Dir enum that will hold all the directions that the room will have doors in.
            /   We will start by adding the previous direction to the empty enum, since there will always be a door coming from that direction
            /   Then, use the offsetPost helper function to get the new position of the room, and add it to the detours dictionary.
            /   Finally, add it to the roomTiers dictionary, with the tier being the current tier.
            /***

            /*** 
            /   Now that we have the directions that the room can go in, we need to add the room to the mainpaths dictionary.
            /   Also be sure to add the coordinates to the orderedMainRoom list, and add the room to the roomTiers dictionary based on the current tier.
            ***/

            /*** 
            /   Finally, we need to update the previous direction, and the current position.
            /   There will be two cases, one if the room is the last room in the main path, and one if it is not.
            /   If it is the last room, utilize the setFinalRoom helper function in this gameObjects LevelInfo script.
            /   update current position with offsetPos helper function, called InstantiateTransport from this gameObjects Instantiate Level script, and add the room to mainpaths
            /   If it is not the last room, update current position with offsetPos helper function, and update previous direction with the flipDir helper function
            ***/
        }

        /*** 
        /   Replicate a similar loop to the one above, but for the detours.
        ***/
        Dictionary<pos, Dir> detoursCopy = new Dictionary<pos, Dir>(detours); //Copy so we are not changing data strucutre while iterating over it
        foreach(var item in detoursCopy)
        {
            break;
            /***
            /   Determine previous direction, and coordinates, from the item key and value
            ***/

            /***
            /   Mark each room with a tier
            ***/
            // int detourRoomTier = 0;
            // for (int i = 0; i < 3; ++i)
            // {
            //     if (roomTiers[i].Contains(detourPos)) {detourRoomTier = i;}
            // }

            /***
            /   Establish the depth (number of rooms in the detour) of the room using detourDepth
            /   Iterate through the depth, and create a new room at each iteration
            ***/
            int detourIter = UnityEngine.Random.Range(detourDepth.x, detourDepth.y);
            for (int j=1; j <= detourIter; ++j)
            {
                break;
                /***
                /   Filter out the directions that are not possible for the room to go in.
                /   If there are no available directions then detour can't go anywhere and is a dead end
                /   Add to deadEnds list and deadEndAssignment list and break the loop
                ***/

                /***
                /   Create a new Dir enum that will hold all the directions that the room will have doors in.
                /   We will start by adding the previous direction to the empty enum, since there will always be a door coming from that direction
                /   Then we chech if it is the last room in the detour or not
                ***/
                // if (j != detourIter)
                // {
                    /***
                    /   If it is not the last room, remove the previous direction from the filtered directions, and randomly select a new direction
                    /   Add the new direction to the enum of room directions.
                    ***/

                    /***
                    /   Add/replace the detour room into the detours dictionary
                    /   Add detour room into its respective tier in roomTiers dictionary
                    /   Update previous to be the opposite of the new direction
                    /   Update the current position with the offsetPos helper function
                    ***/
                // }
                // else
                // //If the room is the last one in the detour path
                // {
                        /***
                        /   If it is the last room, Remove from roomTiers if already in it because we don't want mosters spawning in deadends.
                        /   Add to deadEnds list and deadEndAssignment list
                        /   Finally, Add/replace the detour room into the dictionary
                        ***/
                // }
            }
        }

        /*** 
        /   With all the rooms generated, we can assign the monsters to each room.
        ***/
        // Dictionary<pos, List<Monsters>> monstersPerRoom = assignMonsters(roomTiers);

        /*** 
        /   These last loops are optional, and just establish chest and ambush rooms within the layout. 
        ***/
        // while (num_of_chestrooms != 0)
        // {
        //     if (deadEndAssignment.Count > 0)
        //     {
        //         pos room = deadEndAssignment[random.Next(deadEndAssignment.Count)];
        //         deadEndAssignment.Remove(room);
        //         deadEnds[room] = DeadEnds.Chest;
        //     }
        //     num_of_chestrooms--;
        // }
        // while (num_of_amushrooms != 0)
        // {
        //     if (deadEndAssignment.Count > 0)
        //     {
        //         pos room = deadEndAssignment[random.Next(deadEndAssignment.Count)];
        //         deadEndAssignment.Remove(room);
        //         deadEnds[room] = DeadEnds.Amush;
        //     }
        //     num_of_amushrooms--;
        // }

        //Copy info from function into struct
        info.mainPaths = mainpaths;
        info.detourPaths = detours;
        info.orderedMainRoom = orderedMainRoom;
        info.deadEnds = deadEnds;
        //info.monstersPerRoom = monstersPerRoom;
        return info;
    }

    private Dir chooseRandomDir(Dir direction)
    {
        //NEW function randomly selects a direction
        //Optimize later(eliminate the need for creating new List and new Random every time)
        List<Dir> possibleDir = new List<Dir>();
        foreach(Dir d in new List<Dir>{Dir.R, Dir.U, Dir.L, Dir.D}){
            if ((direction & d) != 0){
                possibleDir.Add(d);
            }
        }
        return possibleDir[random.Next(possibleDir.Count)];
    }
    private pos offsetPos(pos currentPosition, Dir direction)
    //Helper Function receives a position, a direction, returning the position in that direction
    {
        switch(direction)
        {
            case Dir.R:
                currentPosition.x += 1;
                break;
            case Dir.U:
                currentPosition.y += 1;
                break;
            case Dir.L:
                currentPosition.x -= 1;
                break;
            case Dir.D:
                currentPosition.y -= 1;
                break;
        }
        return currentPosition;
    }

    private Dir filterPathOptions(pos currentPos, Dictionary<pos, Dir> offMainLimits, Dictionary<pos, Dir> offDetourLimits, Dir directions)
    {
        Dir filteredDirections = directions;
        foreach(Dir d in new List<Dir>{Dir.R, Dir.U, Dir.L, Dir.D}){
            if ((directions & d) != 0)
            {
                if ((offMainLimits.ContainsKey(offsetPos(currentPos, d))) || (offDetourLimits.ContainsKey(offsetPos(currentPos, d))))
                {
                    filteredDirections &= ~d;
                }
            }   
        }
        return filteredDirections;
    }


    private Dir flipDir(Dir direction) 
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

    private HashSet<int> createDetourPoints(int num_of_rooms, int size) 
    // Helper function which determines which rooms are going to have detours
    {
        List<int> nums = new List<int>();
        for(int i = 1; i < num_of_rooms; ++i)
        {
            nums.Add(i);
        }
        HashSet<int> detourPoints = new HashSet<int>();
        for(int i=0; i < size; ++i)
        {
            int roomNum = random.Next(nums.Count);
            if (nums[roomNum] != num_of_rooms / 2) {detourPoints.Add(nums[roomNum]);}
            nums.RemoveAt(roomNum);
        }
        return detourPoints;
    }

    private Dictionary<pos, List<Monsters>> assignMonsters(Dictionary<int, List<pos>> roomTiers)
    {
        Dictionary<pos, List<Monsters>> monstersPerRoom = new Dictionary<pos, List<Monsters>>();
        for (int i=0; i<3; ++i)
        {
            while (dungeonTiers[i].Count != 0)
            {
                List<Monsters> monsterList = new List<Monsters>(dungeonTiers[i].Keys);
                Monsters monster = monsterList[random.Next(monsterList.Count)];
                dungeonTiers[i][monster] -= 1;
                if (dungeonTiers[i][monster] == 0) {dungeonTiers[i].Remove(monster);}
                pos position = roomTiers[i][random.Next(roomTiers[i].Count)];
                if (!monstersPerRoom.ContainsKey(position)) {monstersPerRoom.Add(position, new List<Monsters>());}
                monstersPerRoom[position].Add(monster);
            }
        }
        return monstersPerRoom;
    }

    private void checkMonsterRooms(Dictionary<int, List<pos>> roomTiers)
    {
        for (int i=0; i <2; ++i)
        {
            foreach(pos position in roomTiers[0])
            {
                Debug.Log(position.x + ", " + position.y);
            }
        }
    }
    
}
