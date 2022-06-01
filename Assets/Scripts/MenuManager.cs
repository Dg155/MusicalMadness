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
     currentActive = menu;
    }

    public void Enable_Disable(GameObject menu){
        print("CLICKING");
        menu.SetActive(!menu.active);
        
    }
}
