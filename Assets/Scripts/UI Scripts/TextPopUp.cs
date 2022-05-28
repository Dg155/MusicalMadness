using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class TextPopUp : MonoBehaviour
{

    TextMeshProUGUI TM;

    // Start is called before the first frame update
    void Start()
    {
        TM = GetComponentInChildren<TextMeshProUGUI>();
        firstMessage();
    }

    private async void firstMessage()
    {
        await Task.Delay(80);
        popUp(FindObjectOfType<LevelInfo>().artifactInformation());
    }

    public async void popUp(string message)
    {
        gameObject.SetActive(true);
        TM.text = message;
        await waitForKeyPress(KeyCode.Mouse0);
        gameObject.SetActive(false);
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
