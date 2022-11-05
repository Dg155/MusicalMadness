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
    public float characterPopupDelay = 0.2f;
    private bool finishedText;
    private bool skipText;

    // Start is called before the first frame update
    void Start()
    {
        finishedText = true;
        skipText = false;
        TM = GetComponentInChildren<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().name != "GrandPianoBoss") {StartCoroutine("firstMessage");}
        else {gameObject.SetActive(false);}
    }

    private void Update() {
        if (!finishedText) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {skipText = true;}
        }
    }

    private IEnumerator firstMessage()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        popUp(FindObjectOfType<LevelInfo>().artifactInformation());
    }

    public async void popUp(string message)
    {
        while (!finishedText) {await Task.Yield();}
        finishedText = false;
        skipText = false;
        gameObject.SetActive(true);
        StartCoroutine("StartDisplay", message);     
    }

    private IEnumerator StartDisplay(string message)
    {
        yield return StartCoroutine("DisplayText", message);
        yield return StartCoroutine("waitForKeyPress", KeyCode.Mouse0);
        gameObject.SetActive(false);
        finishedText = true; 
    }

    private IEnumerator DisplayText(string message)
    {
        TM.text = "";
        int alphaIndex = 0;

        foreach(char c in message.ToCharArray())
        {
            Debug.Log("Hey");
            if ((skipText) && (alphaIndex < message.ToCharArray().Length - 5)) {TM.text = message; break;}
            alphaIndex++;
            TM.text = message;
            TM.text = TM.text.Insert(alphaIndex, kalphaCode);
            yield return new WaitForSecondsRealtime(characterPopupDelay);
        }
        yield return new WaitForSecondsRealtime(characterPopupDelay);
    }

    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while(!done) // essentially a "while true", but with a bool to break out naturally
        {
            if(Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return 0; // wait until next frame, then continue execution from here (loop continues)
        }
        yield return null;
        // now this function returns
    }

}
