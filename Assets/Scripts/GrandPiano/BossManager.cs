using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    BossLevelInfo levelInfo;
    Dictionary<Monsters, GameObject> monsters;
    [SerializeField] GameObject violin, tambourine, demon, grandpiano;
    [SerializeField] private Vector3 spawnPosition;

    [SerializeField] private List<Vector3> minionSpawnPositions;

    public List<(EnemyStats, EnemyAI)> instantiatedMonsters;
    public (EnemyStats, E_GrandPianoAI) instantiatedBoss;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedMonsters = new List<(EnemyStats, EnemyAI)>();
        levelInfo = this.GetComponent<BossLevelInfo>();
        monsters = new Dictionary<Monsters, GameObject>();
        monsters.Add(Monsters.Violin, violin);
        monsters.Add(Monsters.Tambourine, tambourine);//change once several enemies implemented
        monsters.Add(Monsters.Demon, demon);//change once several enemies implemented
        InstantiateBoss();
        pos origin = new pos(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        foreach ((EnemyStats, EnemyAI) e in instantiatedMonsters)
        {
            if (!e.Item2.getAlive()) { instantiatedMonsters.Remove(e); e.Item1.destroyEnemy(); }
            //Debug.Log(levelInfo.currPlayerPos.x.ToString() + "," + levelInfo.currPlayerPos.y.ToString());
            e.Item2.OnUpdate(levelInfo.currPlayerPos);
        }

        if (!instantiatedBoss.Item2.getAlive()) { instantiatedBoss.Item1.destroyEnemy(); instantiatedBoss = (null, null); }
        instantiatedBoss.Item2.OnUpdate(levelInfo.currPlayerPos);
    }

    void InstantiateBoss()
    {
        //called in Start

        var enemy = Instantiate(grandpiano, spawnPosition, Quaternion.identity);
        E_GrandPianoAI bossAI = enemy.GetComponent<E_GrandPianoAI>();
        EnemyStats bossStats = enemy.GetComponent<EnemyStats>();
        if (bossAI != null)
        {
            instantiatedBoss = (bossStats, bossAI);
        }
    }

    public void InstantiateMinions()
    {
        //should behave similarly to InstantiateEnemyInRoom
        foreach (var spawnPoint in minionSpawnPositions)
        {
            InstantiateSingle(spawnPoint);
        }

    }

    void InstantiateSingle(Vector3 spawnPos) //can add randomizer of which enemies spawn
    {
        var enemy = Instantiate(violin, spawnPos, Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        if (enemyAI != null)
        {
            instantiatedMonsters.Add((enemyStats, enemyAI));
        }
    }
}
