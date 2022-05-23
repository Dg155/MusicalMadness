using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    private TextMeshProUGUI valueText;
    public ItemType valueType;

    void Start()
    {
        valueText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int newValue)
    {
        valueText.text = $"{valueType}s: {newValue.ToString()}";
    }
}
