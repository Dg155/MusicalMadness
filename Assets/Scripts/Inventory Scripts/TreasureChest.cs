using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{

    public Sprite openChest;
    public GameObject item;
    private Item itemInfo;
    private bool Closed = true;

    private void Start() {
        itemInfo = item.GetComponent<Item>();
        if (itemInfo.type == ItemType.Healing)
        {
            itemInfo.setItemWorth(500);
        }
    }

    public void setItem(GameObject chestItem)
    {
        item = chestItem;
    }

    public GameObject getItem()
    {
        return item;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (Closed && other.gameObject.tag == "Player")
        {
            Closed = false;
            GetComponent<SpriteRenderer>().sprite = openChest;
            Instantiate(item, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

}
