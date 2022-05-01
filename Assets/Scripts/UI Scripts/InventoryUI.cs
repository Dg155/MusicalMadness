using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform UITextsParent;

    Inventory inventory;

    UIText[] UITexts;

    // Start is called before the first frame update
    void Start()
    {
        UITextsParent = gameObject.transform;

        //caches an Inventory.instance reference to save memory (instead of creating a new reference every time by using Inventory.instance every time we need it)
        inventory = Inventory.instance;

        //subscribes the UpdateUI function to the inventory item update callback event (UpdateUI will now know each time inventory is updated)
        inventory.onItemChangedCallback += UpdateUI;

        UITexts = UITextsParent.GetComponentsInChildren<UIText>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateUI()
    {
        for (int i = 0; i < UITexts.Length; i++)
        {
            UITexts[i].UpdateText(inventory.GetItemTypeAmt(UITexts[i].valueType));
        }
    }
}