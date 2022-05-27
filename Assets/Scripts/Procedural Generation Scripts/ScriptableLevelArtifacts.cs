using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelArtifacts", menuName = "ScriptableObjects/LevelArtifacts")]
public class ScriptableLevelArtifacts : ScriptableObject
{
    public List<Item> Artifacts;
}
