using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentActive;

    // Update is called once per frame
    public void EnableMenu(GameObject menu)
    {
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

    public void setCurrentActive(GameObject menu)
    {
        Debug.Log(menu);
        currentActive = menu;
    }
}
