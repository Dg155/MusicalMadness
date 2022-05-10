using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool gotPickedUp = Inventory.instance.Add(item);
            Debug.Log(gotPickedUp);
            if (gotPickedUp)
            {
                if (item.name == "Soul")
                {
                    other.GetComponent<PlayerStats>().addSouls(1);
                }
                Destroy(gameObject);
            }
        }
    }
}
