using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item: ScriptableObject
{
    new public string name = "New Item";
    public ItemType type = ItemType.Soul;
    public int cost = 0;
    public bool isDefaultItem = false;

    public GameObject itemObject;

    public virtual void Use()
    {
        Debug.Log($"Using {name}");
    }


}

public enum ItemType
{
    Weapon,
    Soul,
    Artifact
}