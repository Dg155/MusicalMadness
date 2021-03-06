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
        //Create the dungeon information
        DungeonInfo info = new DungeonInfo();
        Dictionary<pos, Dir> mainpaths = new Dictionary<pos, Dir>();
        Dictionary<pos, Dir> detours = new Dictionary<pos, Dir>();
        List<pos> orderedMainRoom = new List<pos>();
        Dictionary<pos, DeadEnds> deadEnds = new Dictionary<pos, DeadEnds>();
        List<pos> deadEndAssignment = new List<pos>();

        //Create datastructures for enemy generation
        Dictionary<int, List<pos>> roomTiers = new Dictionary<int, List<pos>>();
        for (int i = 0; i < 3; i++){roomTiers[i] = new List<pos>();}
        int lastTierOne = mainrooms/3;
        int lastTierTwo = lastTierOne * 2;

        //Directions for the Main Path (Right, Up, Down), and all directions for detours
        Dir path_Options = Dir.R | Dir.U | Dir.D;
        Dir all_Directions = Dir.R | Dir.U | Dir.L | Dir.D;


        //Attributes needed for main room generation
        Dir previous = Dir.L;
        pos currentPos = new pos(1,0);
        HashSet<int> detourPoints = createDetourPoints(mainrooms, num_of_detours);

        //Add first room At (0,0) with one right door
        mainpaths.Add(new pos(0,0), Dir.R);

        //Loop through and generate each main path room and mark the start of detours
        for(int i=0; i < mainrooms; ++i)
        {
            //Filter path_options to not collide with any already generated rooms
            Dir filteredPathOptions = filterPathOptions(currentPos, mainpaths, detours, path_Options);
            //Create empty binary that will hold all directions the room can go
            Dir doorOptions = 0;
            //Add the direction of the previous room
            doorOptions |= previous;
            //Remove the direction of the previous room and randomly select another direction for the room to lead to
            Dir possible_options = (filteredPathOptions & ~previous);
            Dir New_Main_Direction = chooseRandomDir(possible_options);
            //Add the new direction to the binary of directions
            doorOptions |= New_Main_Direction;

            
            // If the room is designated as a room that leads to a detour, mark the detour
            if (detourPoints.Contains(i))
            {
                //Remove the already taken directions in the room, and randomly pick another one to lead to the detour
                Dir possible_detours = filteredPathOptions;
                possible_detours &= ~doorOptions;
                //Confirm that there is a direction to put the detour in
                if (possible_detours != 0)
                {
                    //Select new direction for detour and add it to the binary
                    Dir New_Detour_Direction = chooseRandomDir(possible_detours);
                    doorOptions |= New_Detour_Direction;
                    //Mark the beginning of the detour with a room so later main path rooms do not overlap
                    Dir detourRoom = 0;
                    detourRoom |= flipDir(New_Detour_Direction);
                    pos detourPos = offsetPos(currentPos, New_Detour_Direction);
                    detours.Add(detourPos, detourRoom);
                    //Add coordinates to roomTiers
                    if (i < lastTierOne) {roomTiers[0].Add(detourPos);}
                    else if (i < lastTierTwo) {roomTiers[1].Add(detourPos);}
                    else {roomTiers[2].Add(detourPos);}
                }
            }
            //Add coordinates and the room to mainpaths dictionary
            mainpaths.Add(currentPos, doorOptions);
            //Add coordinates to orderedMainRooms
            orderedMainRoom.Add(currentPos);
            //Add coordinates to roomTiers
            if (i < lastTierOne) {roomTiers[0].Add(currentPos);}
            else if (i < lastTierTwo) {roomTiers[1].Add(currentPos);}
            else {roomTiers[2].Add(currentPos);}
            if (i == mainrooms -1)
            {
                this.GetComponent<LevelInfo>().setFinalRoom(currentPos, New_Main_Direction);
                currentPos = offsetPos(currentPos, New_Main_Direction);
                this.GetComponent<InstantiateLevel>().InstantiateTransport(currentPos);
                mainpaths.Add(currentPos, flipDir(New_Main_Direction));
            }
            else
            {
                //Update previous to be the opposite of the new direction
                previous = flipDir(New_Main_Direction);
                //Update current position depending on the new direction
                currentPos = offsetPos(currentPos, New_Main_Direction);
            }
        }
        
        // Loop to create the detours
        Dictionary<pos, Dir> detoursCopy = new Dictionary<pos, Dir>(detours); //Copy so we are not changing data strucutre while iterating over it
        foreach(var item in detoursCopy)
        {
            //Determine previous direction, coordinates, and roomtier from marked room type
            Dir detourPrevious = item.Value;
            pos detourPos = item.Key;
            int detourRoomTier = 0;
            for (int i = 0; i < 3; ++i)
            {
                if (roomTiers[i].Contains(detourPos)) {detourRoomTier = i;}
            }
            //Detour will have a random depth between two boundaries
            int detourIter = UnityEngine.Random.Range(detourDepth.x, detourDepth.y);
            for (int j=1; j <= detourIter; ++j)
            {
                //Filter out all the directions taken up by already determined rooms
                Dir filteredDetourPathOptions = filterPathOptions(detourPos, mainpaths, detours, all_Directions);
                //If there are no available directions then detour can't go anywhere and is a dead end
                if (filteredDetourPathOptions == 0) 
                {
                    //Add to deadEnds list
                    deadEnds.Add(detourPos, DeadEnds.Nothing);
                    deadEndAssignment.Add(detourPos);
                    break;
                }
                //Create empty binary to store the detour room directions
                Dir detourDoorOptions = 0;
                //Add the previous direction to the binary
                detourDoorOptions |= detourPrevious;
                //If it is not the last room in the detour, then add another direction to lead to next detour room
                if (j != detourIter)
                {
                    //Remove already taken direction from available directions, then randomly pick from remaining ones
                    Dir possible_detours = (filteredDetourPathOptions & ~detourPrevious);
                    Dir New_Detour_Direction = chooseRandomDir(possible_detours);
                    //Add the new direction to the binary of directions
                    detourDoorOptions |= New_Detour_Direction;
                    //Add/replace the detour room into the dictionary
                    if (detours.ContainsKey(detourPos)) {detours[detourPos] = detourDoorOptions;}
                    else {detours.Add(detourPos, detourDoorOptions);}
                    //Add detour room into its respective tier
                    if (!roomTiers[detourRoomTier].Contains(detourPos)) {roomTiers[detourRoomTier].Add(detourPos);}
                    //Update previous to be the opposite of the new direction
                    detourPrevious = flipDir(New_Detour_Direction);
                    //Update current position depending on the new direction
                    detourPos = offsetPos(detourPos, New_Detour_Direction);
                }
                else
                //If the room is the last one in the detour path
                {
                    //Remove from roomTiers if already in it because we don't want mosters spawning in deadends.
                    if (roomTiers[detourRoomTier].Contains(detourPos)) {roomTiers[detourRoomTier].Remove(detourPos);}
                    //Add to deadEnds list
                    deadEnds.Add(detourPos, DeadEnds.Nothing);
                    deadEndAssignment.Add(detourPos);
                    //Add/replace the detour room into the dictionary
                    if (detours.ContainsKey(detourPos)) {detours[detourPos] = detourDoorOptions;}
                    else {detours.Add(detourPos, detourDoorOptions);}
                }
            }
        }

        Dictionary<pos, List<Monsters>> monstersPerRoom = assignMonsters(roomTiers);

        /*foreach(pos position in monstersPerRoom.Keys)
        {
            Debug.Log(position.x + ", " + position.y);
        }*/

        while (num_of_chestrooms != 0)
        {
            if (deadEndAssignment.Count > 0)
            {
                pos room = deadEndAssignment[random.Next(deadEndAssignment.Count)];
                deadEndAssignment.Remove(room);
                deadEnds[room] = DeadEnds.Chest;
            }
            num_of_chestrooms--;
        }
        while (num_of_amushrooms != 0)
        {
            if (deadEndAssignment.Count > 0)
            {
                pos room = deadEndAssignment[random.Next(deadEndAssignment.Count)];
                deadEndAssignment.Remove(room);
                deadEnds[room] = DeadEnds.Amush;
            }
            num_of_amushrooms--;
        }

        //Copy info from function into struct
        info.mainPaths = mainpaths;
        info.detourPaths = detours;
        info.orderedMainRoom = orderedMainRoom;
        info.deadEnds = deadEnds;
        info.monstersPerRoom = monstersPerRoom;
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
