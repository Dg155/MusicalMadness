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


public struct DungeonInfo
{
    public Dictionary<pos, Dir> mainPaths;
    public Dictionary<pos, Dir> detourPaths;
    public List<pos> orderedMainRoom;
    public List<pos> deadEnds;
}

public class GenerateLevel : MonoBehaviour
{
    
    public int amountOfRooms = 10;
    public int numOfDetours = 3;
    public int minDetourDepth = 3;
    public int maxDetourDepth = 5;
    System.Random random = new System.Random();

    void Start()
    {
        DungeonInfo temp = proceduralGenerationOne(amountOfRooms, numOfDetours, new pos(minDetourDepth,maxDetourDepth));
        this.GetComponent<InstantiateLevel>().InstantiateFromDungeonInfo(temp);
    }

    public DungeonInfo proceduralGenerationOne(int mainrooms, int num_of_detours, pos detourDepth)
    //Function creates the structure of the rooms based on specific parameters
    {
        //Create the dungeon information
        DungeonInfo info = new DungeonInfo();
        Dictionary<pos, Dir> mainpaths = new Dictionary<pos, Dir>();
        Dictionary<pos, Dir> detours = new Dictionary<pos, Dir>();
        List<pos> orderedMainRoom = new List<pos>();
        List<pos> deadEnds = new List<pos>();

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
                    detours.Add(offsetPos(currentPos, New_Detour_Direction), detourRoom);
                }
            }
            //Add coordinates and the room to mainpaths dictionary
            mainpaths.Add(currentPos, doorOptions);
            //Add coordinates to orderedMainRooms
            orderedMainRoom.Add(currentPos);
            //Update previous to be the opposite of the new direction
            previous = flipDir(New_Main_Direction);
            //Update current position depending on the new direction
            currentPos = offsetPos(currentPos, New_Main_Direction);
        }
        
        // Loop to create the detours
        Dictionary<pos, Dir> detoursCopy = new Dictionary<pos, Dir>(detours); //Copy so we are not changing data strucutre while iterating over it
        foreach(var item in detoursCopy)
        {
            //Determine previous direction and coordinates from marked room type
            Dir detourPrevious = item.Value;
            pos detourPos = item.Key;
            //Detour will have a random depth between two boundaries
            int detourIter = UnityEngine.Random.Range(detourDepth.x, detourDepth.y);
            for (int j=1; j <= detourIter; ++j)
            {
                //Filter out all the directions taken up by already determined rooms
                Dir filteredDetourPathOptions = filterPathOptions(detourPos, mainpaths, detours, all_Directions);
                //If there are no available directions then detour can't go anywhere and is a dead end
                if (filteredDetourPathOptions == 0) {deadEnds.Add(detourPos); break;}
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
                    //Update previous to be the opposite of the new direction
                    detourPrevious = flipDir(New_Detour_Direction);
                    //Update current position depending on the new direction
                    detourPos = offsetPos(detourPos, New_Detour_Direction);
                }
                else
                //If the room is the last one in the detour path
                {
                    //Add to deadEnds list
                    deadEnds.Add(detourPos);
                    //Add/replace the detour room into the dictionary
                    if (detours.ContainsKey(detourPos)) {detours[detourPos] = detourDoorOptions;}
                    else {detours.Add(detourPos, detourDoorOptions);}
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
    
}
