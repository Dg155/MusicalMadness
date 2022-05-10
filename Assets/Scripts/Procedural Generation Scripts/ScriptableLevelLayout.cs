using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelLayout", menuName = "ScriptableObjects/LevelLayout")]
public class ScriptableLevelLayout : ScriptableObject
{
    public int amountOfRooms;
    public int numOfDetours;
    public int minDetourDepth;
    public int maxDetourDepth;
}
