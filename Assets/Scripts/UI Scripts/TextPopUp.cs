using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class TextPopUp : MonoBehaviour
{

    TextMeshProUGUI TM;
    public const string kalphaCode = "<color=#00000000>";
    public int characterPopupDelay = 50;

    // Start is called before the first frame update
    void Start()
    {
        TM = GetComponentInChildren<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().name != "GrandPianoBoss") {firstMessage();}
        else {gameObject.SetActive(false);}
    }

    private async void firstMessage()
    {
        await Task.Delay(80);
        popUp(FindObjectOfType<LevelInfo>().artifactInformation());
    }

    public async void popUp(string message)
    {
        gameObject.SetActive(true);
        await DisplayText(message);
        await waitForKeyPress(KeyCode.Mouse0);
        gameObject.SetActive(false);
    }

    private async Task DisplayText(string message)
    {
        TM.text = "";
        string displayText = "";
        int alphaIndex = 0;

        foreach(char c in message.ToCharArray())
        {
            alphaIndex++;
            TM.text = message;
            displayText = TM.text.Insert(alphaIndex, kalphaCode);
            TM.text = displayText;
            await Task.Delay(characterPopupDelay);
        }
        return;
    }

    private async Task waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while(!done) // essentially a "while true", but with a bool to break out naturally
        {
            if(Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            await Task.Yield(); // wait until next frame, then continue execution from here (loop continues)
        }
        return;
        // now this function returns
    }

}
