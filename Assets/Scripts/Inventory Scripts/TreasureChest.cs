using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    public Sprite openChest;
    public GameObject item;
    public AudioClip chestOpenSFX;
    private Item itemInfo;
    private bool Closed = true;

    private void Start() {  
    }

    public void setItem(Item chestItem)
    {
        itemInfo = chestItem;
    }

    public GameObject getItem()
    {
        return item;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (Closed && other.tag == "Player")
        {
            Closed = false;
            GetComponent<SpriteRenderer>().sprite = openChest;
            item.GetComponent<ItemObject>().setDetails(itemInfo);
            GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectPlayer>().PlaySound(chestOpenSFX);
            if (itemInfo.type == ItemType.Healing){itemInfo.setItemWorth(500);}
            GameObject Item = Instantiate(item, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            Item.GetComponent<ItemObject>().justSpawned();
        }
    }

}
