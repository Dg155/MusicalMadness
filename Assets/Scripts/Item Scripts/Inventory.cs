using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    public PlayerStats playerStats;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;

        playerStats = gameObject.GetComponent<PlayerStats>();
    }
    #endregion

    //creates an event (message) that updates UI
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public Item mainHand;
    public GameObject mainHandObject;
    public List<Item> items = new List<Item>();
    public int weaponSpace;
    //we are only limiting how many weapons the player may have

    public int GetItemTypeAmt(ItemType requestedItemType)
    {
        //count how many of each type there are in item list
        int numWeapons = 0;
        int numSouls = 0;
        int numArtifacts = 0;
        foreach (Item it in items)
        {
            if (it.type == ItemType.Weapon) { numWeapons += 1; }
            else if (it.type == ItemType.Soul) { numSouls += 1; }
            else if (it.type == ItemType.Artifact) { numArtifacts += 1; }
        }

        if (requestedItemType == ItemType.Weapon) { return numWeapons; }
        else if (requestedItemType == ItemType.Soul) { return numSouls; }
        else if (requestedItemType == ItemType.Artifact) { return numArtifacts; }
        else
        {
            Debug.Log("Invalid requested item type");
            return 0;
        }

    }

    public bool Add (Item item)
    //func returns a bool to say whether the player successfully picked up the item (true --> destroy gameObj, false --> don't destroy)
    {
        int numWeapons = GetItemTypeAmt(ItemType.Weapon);

        //if the item isn't a default item, then we can consider adding it to inventory
        if (!item.isDefaultItem)
        {
            if (item.type == ItemType.Weapon && GetItemTypeAmt(ItemType.Weapon) >= weaponSpace)
            {
                Debug.Log(GetItemTypeAmt(ItemType.Weapon));
                Debug.Log("You're carrying too many weapons");
                return false;
            }
            else if (item.cost > playerStats.getSouls())
            {
                Debug.Log($"Weapon Cost: {item.cost}");
                Debug.Log($"Your souls: {playerStats.getSouls()}");
                Debug.Log("This weapon is too expensive");
                return false;
            }

            //there's no reason to not add the item to inventory, so let's pick it up. If we're not holding anything in the main hand, make the item the main hand
            if (item.type == ItemType.Weapon && mainHand == null)
            {
                SetMainHand(item, false);
                return true;
            }

            if (item != null)
            {
                items.Add(item);
            }

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

    public void SetMainHand(Item newMainHand, bool switchedFromItemInInventory = true)
    {
        //destroy the instance of the mainHand's item object prefab if it exists
        if (mainHand != null)
        {
            Destroy(mainHandObject);
        }

        //if you're setting a new main hand bc you're switching w/ a weapon from your inventory's item list, remove the new main hand from the item list
        //if you're setting a new main hand bc you were unarmed and you're picking up a weapon from the ground or a chest, don't remove anything from the inventory's item list. This is why the bool is only used in the Add function above and is false by default
        if (switchedFromItemInInventory)
        {
            items.Remove(newMainHand); //don't use the Inventory's Remove method bc we don't want the callback called twice with just one function
        }

        if (mainHand != null) //why is this condition checked? If mainHand is null and we try to add it to the items list, it will actually add a "None(Item)" element to the list instead of doing nothing, causing errors
        {
            items.Add(mainHand); //don't use the Inventory's Add method bc of the previous reason and we don't need to check the weaponSpace condition (we're not actually adding anything to our inventory)
        }

        mainHand = newMainHand;

        mainHandObject = Instantiate(newMainHand.itemObject);
        playerStats.setMainHand(mainHandObject);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
