using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAll : MonoBehaviour
{
    public GameObject menuControlToReload;
    public GameObject menuControlDisplay;

    void Start()
    {
        PlayFabManager menuControlScriptFirst = menuControlToReload.GetComponent<PlayFabManager>();
        menuControlScriptFirst.Start();
        MenuControl menuControlScript = menuControlDisplay.GetComponent<MenuControl>();
        menuControlScript.ReloadData();
    }

    
}
