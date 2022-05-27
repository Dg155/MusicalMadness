using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ItemObject : MonoBehaviour
{
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.image;
        if (item.type == ItemType.Soul) {destroySoul();}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDetails(Item newItem)
    {
        item = newItem;
    }

    public async void justSpawned()
    {
        CircleCollider2D CC = GetComponent<CircleCollider2D>();
        CC.enabled = false;
        await Task.Delay(1000);
        CC.enabled = true;
    }

    async void destroySoul()
    {
        await Task.Delay(50000);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool gotPickedUp = Inventory.instance.Add(item);
            if (gotPickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
