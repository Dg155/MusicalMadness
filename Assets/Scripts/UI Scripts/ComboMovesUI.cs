using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboMovesUI : MonoBehaviour
{
    Inventory inventory;

    Weapon mainHand;

    TextMeshProUGUI comboMovesText;
    public Color leftColor, rightColor, noColor;
    public UnityEngine.UI.Image[] images = new UnityEngine.UI.Image[4];
    private Animation comboAnim;

    void Start()
    {
        inventory = Inventory.instance;

        comboMovesText = gameObject.GetComponent<TextMeshProUGUI>();

        inventory.onItemChangedCallback += UpdateMainHand;

        comboAnim = this.GetComponent<Animation>();
    }

    void UpdateMainHand()
    {
        if (inventory.mainHandObject != null)
        {
            mainHand = inventory.mainHandObject.GetComponent<Weapon>();
            mainHand.onWeaponMoveCallback += UpdateDisplay;
            mainHand.onComboActivatedCallback += EndCombo;
        }
    }

    void EndCombo(bool comboSuccessful)
    {
        if (comboSuccessful)
        {
            StartCoroutine(_DisplayCombo());
        }
        else
        {
            foreach (var image in images){
                image.color = noColor;
            }
        }
    }

    IEnumerator _DisplayCombo(){ //comes before UpdateDisplay
        comboAnim.Play();
        yield return new WaitForSeconds(1);
        foreach(var image in images){
            image.color = noColor;
        }
    }

    void UpdateDisplay(List<weaponMove> lastMoves, float cooldown)
    {
        if (lastMoves.Count > 0)
        {
            foreach(var image in images){
                image.color = noColor;
            }
        }
        int ind = 0;
        foreach(weaponMove move in lastMoves)
        {
            if (move  == weaponMove.trumpetPrimary || move == weaponMove.drumPrimary){
                images[ind].color = leftColor;
            }
            else if (move == weaponMove.trumpetSecondary || move == weaponMove.drumSecondary){
                images[ind].color = rightColor;
            }
            ind += 1;
        }
    }
}
