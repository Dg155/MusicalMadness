using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentActive;
    public AudioClip buttonPressSFX;
    private SoundEffectPlayer SFXplayer;

    private void Start()
    {
        SFXplayer = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectPlayer>();
    }

    // Update is called once per frame
    public void EnableMenu(GameObject menu)
    {
        SFXplayer.PlaySound(buttonPressSFX);
        currentActive.SetActive(false);
        menu.SetActive(true);

        //Disable QuitToTitle button
        if (menu.name == "SettingsUI")
        {
            menu.transform.GetChild(3).gameObject.SetActive(false);
        }

        currentActive = menu;
    }

    public void Enable_Disable(GameObject menu){
        print("CLICKING");
        menu.SetActive(!menu.activeSelf);
        
    }

}
