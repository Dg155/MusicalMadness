using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item: ScriptableObject
{
    new public string name = "New Item";
    public ItemType type = ItemType.Weapon;
    public Sprite icon = null;
    public bool isDefaultItem = false;

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