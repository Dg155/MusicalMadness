using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class PlayerStats : BaseStats
{

    Inventory inventory;
    Weapon mainHand;
    float cooldownEndTime;

    public float hitboxVisualizerRadius;

    public int souls;

    public CooldownBarScript CB;

    void Awake(){
        if (HB == null){
            HB = FindObjectOfType<HealthBarScript>();
        }
    }

    override protected void Start()
    {
        CB = this.transform.Find("CooldownBar").GetComponent<CooldownBarScript>();
        CB.gameObject.SetActive(false);
        inventory = Inventory.instance;
        Inventory.instance.onItemChangedCallback += UpdateMainHand;
    }

    override protected void Die(){
        LevelLoader l= FindObjectOfType<LevelLoader>();
        if (this.GetComponent<EntVisAudFX>() != null){
            this.GetComponent<EntVisAudFX>().DeathEffect();
        }
        if (l != null){
            l.ReloadLevel();
        }
        else{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    protected override void Render()
    {
        //This entire render stuff should be moved to a separate script later
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x - this.transform.position.x > 0 && !facingRight || mousePos.x - this.transform.position.x < 0 && facingRight)
        {
            flip();
            if (facingRight){ CB.transform.localScale = new Vector3(.36f, .3f, 1); }
            else { CB.transform.localScale = new Vector3(-.36f, .3f, 1); }
        }
    }

    void UpdateMainHand()
    {
        if (Inventory.instance.mainHandObject != null)
        {
            mainHand = inventory.mainHandObject.GetComponent<Weapon>();
            mainHand.onWeaponMoveCallback += UpdateCooldownBar;
        }
    }

    public async void UpdateCooldownBar(List<weaponMove> lastMovesUsed, float cooldown)
    {
        CB.gameObject.SetActive(true);
        cooldownEndTime = Time.time + cooldown;
        while (Time.time < cooldownEndTime)
        {
            float barSize = (cooldownEndTime - Time.time) / cooldown;
            if (barSize > 1) { barSize = 1; }
            CB.setCooldownBar(barSize);
            float waitTime = 0f;
            while (waitTime < 0.003f) {waitTime += Time.deltaTime; await Task.Yield();}
        }
        CB.gameObject.SetActive(false);
    }

    public int getSouls(){
        return souls;
    }
    public void setSouls(int quantity){
        souls = quantity;
    }
    public void addSouls(int quantity){
        souls += quantity;
        if (souls < 0){souls = 0;}
    }

    // Method to be called every time player starts a new floor
    public void resetPlayer()
    {
        if (HB == null){
            HB = FindObjectOfType<HealthBarScript>();
        }
        transform.position = new Vector3(0, 0, 0);
        setHealth(getMaxHealth());
        Inventory.instance.resetArtifacts();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitboxVisualizerRadius);
    }
}