using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    //creates an event (message) that updates UI
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public int weaponSpace;
    //we are only limiting how many weapons the player may have

    public bool Add (Item item)
    //func returns a bool to say whether the player successfully picked up the item (true --> destroy gameObj, false --> don't destroy)
    {
        //count how many weapons there are in item list
        int numWeapons = 0;
        foreach (Item it in items)
        {
            if (it.type == "weapon") { numWeapons += 1; }
        }

        //if the item isn't a default item, then we can consider adding it to inventory
        if (!item.isDefaultItem)
        {
            if (item.type == "weapon" && numWeapons >= weaponSpace)
            {
                Debug.Log(numWeapons);
                Debug.Log("You're carrying too many weapons");
                return false;
            }

            items.Add(item);

            //if statement checks if any methods are subscribed to this event
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;

    }
    public void Remove (Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

}
