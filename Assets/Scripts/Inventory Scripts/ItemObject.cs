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
        float waitTime = 0f;
        while (waitTime < 1f) {waitTime += Time.deltaTime; await Task.Yield();}
        CC.enabled = true;
    }

    async void destroySoul()
    {
        float waitTime = 0f;
        while (waitTime < 50f) {waitTime += Time.deltaTime; await Task.Yield();}
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool gotPickedUp = Inventory.instance.Add(item);
            if (item.itemSFX != null) {GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectPlayer>().PlaySound(item.itemSFX, 2.0f);}
            if (gotPickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
