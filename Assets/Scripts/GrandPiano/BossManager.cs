using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BossManager : MonoBehaviour
{
    BossLevelInfo levelInfo;
    Dictionary<int, GameObject> monsters;
    [SerializeField] GameObject violin, tambourine, demon, grandpiano;
    [SerializeField] private Vector3 spawnPosition;

    [SerializeField] private List<Vector3> minionSpawnPositions;

    public List<(EnemyStats, EnemyAI)> instantiatedMonsters;
    public (EnemyStats, E_GrandPianoAI) instantiatedBoss;

    public LevelLoader levelLoader;
    public float returnDelay;

    public GameObject minionSpawnParticle;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedMonsters = new List<(EnemyStats, EnemyAI)>();
        levelInfo = this.GetComponent<BossLevelInfo>();
        monsters = new Dictionary<int, GameObject>();
        monsters.Add(0, violin);
        monsters.Add(1, tambourine);//change once several enemies implemented
        monsters.Add(2, demon);//change once several enemies implemented
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

        if (!instantiatedBoss.Item2.getAlive()) {
            instantiatedBoss.Item1.destroyEnemy();
            instantiatedBoss = (null, null);
            ReturnToTitleMenu();
        }
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
            InstantiateSingle(spawnPoint + instantiatedBoss.Item2.transform.position);
        }

    }

    async void InstantiateSingle(Vector3 spawnPos) //can add randomizer of which enemies spawn
    {
        Instantiate(minionSpawnParticle, spawnPos, Quaternion.identity);
        float waitTime = 0f;
        while (waitTime < 0.5f) {waitTime += Time.deltaTime; await Task.Yield();}
        var enemy = Instantiate(monsters[Random.Range(0, 2)], spawnPos, Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        if (enemyAI != null)
        {
            instantiatedMonsters.Add((enemyStats, enemyAI));
        }
    }

    async void ReturnToTitleMenu()
    {
        float waitTime = 0f;
        while (waitTime < returnDelay) {waitTime += Time.deltaTime; await Task.Yield();}
        levelLoader.LoadTitleMenu();
    }
}
