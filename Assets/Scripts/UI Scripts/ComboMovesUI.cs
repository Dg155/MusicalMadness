using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboMovesUI : MonoBehaviour
{
    Inventory inventory;

    Weapon mainHand;

    TextMeshProUGUI comboMovesText;

    void Start()
    {
        inventory = Inventory.instance;

        comboMovesText = gameObject.GetComponent<TextMeshProUGUI>();

        inventory.onItemChangedCallback += UpdateMainHand;
    }

    void UpdateMainHand()
    {
        try
        {
            mainHand = inventory.mainHandObject.GetComponent<Weapon>();
            mainHand.onWeaponMoveCallback += UpdateText;
        }
        catch
        {
            throw;
        }
    }

    void UpdateText(List<weaponMove> lastMoves)
    {
        string newText = "";
        if (lastMoves.Count > 0)
        {
            newText = "Combo:\n";
        }
        foreach(weaponMove move in lastMoves)
        {
            newText = newText + $"{move}\n";
        }
        comboMovesText.text = newText;
    }
}
