using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelEnemies", menuName = "ScriptableObjects/LevelEnemies")]
public class ScriptableLevelEnemies : ScriptableObject
{
    public Dictionary<int, Dictionary<Monsters, int>> dungeonTiers;
    public int[] ViolinEnemy = new int[3];
    public int[] TambourineEnemy = new int[3];
    public int[] DemonEnemy = new int[3];


    public void Generate(){
        dungeonTiers = new Dictionary<int, Dictionary<Monsters, int>>();
        dungeonTiers.Add(0, new Dictionary<Monsters, int>());
        dungeonTiers.Add(1, new Dictionary<Monsters, int>());
        dungeonTiers.Add(2, new Dictionary<Monsters, int>());
        for (int i = 0; i < 3; i ++){
            if (ViolinEnemy[i] > 0){
                dungeonTiers[i].Add(Monsters.Violin, ViolinEnemy[i]);
                }
            if (TambourineEnemy[i] > 0){
            dungeonTiers[i].Add(Monsters.Tambourine, TambourineEnemy[i]);}
            if (DemonEnemy[i] > 0){
            dungeonTiers[i].Add(Monsters.Demon, DemonEnemy[i]);
            }
        }
    }
}
