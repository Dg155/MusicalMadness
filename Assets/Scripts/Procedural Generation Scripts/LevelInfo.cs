using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class LevelInfo : MonoBehaviour
{
    public pos currPlayerPos;
    public DungeonInfo dungeonInfo;
    public ScriptableLevelArtifacts levelArtifacts;
    public GameObject Blocker;
    public int roomSize = 8;
    GameObject doorBlock;
    private HashSet<Item> artifactsNeeded;
    private TextPopUp popUp;
    private GameObject player;

    public EnemyManager enemyManagerTEMP; //TEMPORARY REMOVE ONCE EVENT LISTENING ADDED IN
    // Start is called before the first frame update
    void Start()
    {
        artifactsNeeded = new HashSet<Item>(levelArtifacts.Artifacts);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 0, 0);
        FindObjectOfType<InventoryUI>().UpdateUI();
        currPlayerPos = new pos(0, 0);
        InstantiateLevel IL = this.GetComponent<InstantiateLevel>();
        IL.setArtifacts(levelArtifacts);
        IL.InstantiateFromDungeonInfo(dungeonInfo);
        popUp = GameObject.FindGameObjectWithTag("PopUpText").GetComponent<TextPopUp>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().setHealth(player.GetComponent<PlayerStats>().getMaxHealth());
    }

    public void popUpInfo()
    {
        popUp.popUp(artifactInformation());
    }

    public void checkArtifacts(List<Item> currentArtifacts)
    {
        artifactsNeeded = new HashSet<Item>(levelArtifacts.Artifacts);
        HashSet<Item> currentArts = new HashSet<Item>(currentArtifacts);
        artifactsNeeded.ExceptWith(currentArts);
        if (artifactsNeeded.Count == 0)
        {
            popUp.popUp("Something Somewhere has Opened Up");
            doorBlock.GetComponent<FinalBlock>().setBool(true);
        }
    }

    public string artifactInformation()
    {
        string info = "You currently need the following artifacts: ";
        foreach(Item item in artifactsNeeded)
        {
            info += item.name;
            info += ", ";
        }
        info = info.Substring(0, info.Length - 2);
        info += " in order to get ";
        info += levelArtifacts.nameOfKey;
        return info;
    }

    public void changePos(int x, int y) //currently called by CameraMove.cs script
    {
        currPlayerPos.x += x;
        currPlayerPos.y += y;
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Animator animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetFloat("Speed", 0);
            }
        }
        enemyManagerTEMP.InstantiateAdjacentEnemies();

    }

    public void setFinalRoom(pos position, Dir direction)
    {
        if (direction == Dir.R) {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(4, 0, 0), Quaternion.Euler(0f, 0f, 90f));}
        else if (direction == Dir.L) {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(-4, 0, 0), Quaternion.Euler(0f, 0f, 90f));}
        else if (direction == Dir.U) {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(0, 4, 0), Quaternion.identity);}
        else {doorBlock = Instantiate(Blocker, new Vector3(position.x * roomSize ,position.y * roomSize, 0) + new Vector3(0, -4, 0), Quaternion.identity);}
    }
    
}
