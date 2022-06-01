using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item: ScriptableObject
{
    new public string name = "New Item";
    public ItemType type = ItemType.Soul;
    public Sprite image = null;
    public int cost = 0;
    public float itemWorth = 0; //Variable that is unique to each item; for souls it will be how much the soul is worth, for healing it will be how much health the player will gain.
    public bool isDefaultItem = false;
    public GameObject itemObject;
    public AudioClip itemSFX;

    public virtual void Use()
    {
        Debug.Log($"Using {name}");
    }

    public void setItemWorth(float worth)
    {
        itemWorth = worth;
    }

    public void setCost(int Cost)
    {
        cost = Cost;
    }


}

public enum ItemType
{
    Weapon,
    Soul,
    Artifact,
    Healing
}