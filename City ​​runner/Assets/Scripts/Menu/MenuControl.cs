using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class MenuControl : MonoBehaviour
{
    public GameObject totalCoinsDisplay;
    public GameObject totalDistDisplay;

    void Start()
    {
        LoadUserData();
    }

    void LoadUserData()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
        }
    }

    void OnDataReceived(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("TotalCoins"))
        {
            totalCoinsDisplay.GetComponent<Text>().text = result.Data["TotalCoins"].Value;
        }
        if (result.Data != null && result.Data.ContainsKey("TotalDist"))
        {
            totalDistDisplay.GetComponent<Text>().text = result.Data["TotalDist"].Value;
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
    }

    public void ReloadData()
    {
        StartCoroutine(ReloadDataCoroutine());
    }

    IEnumerator ReloadDataCoroutine()
    {
        yield return new WaitForSeconds(2f); 
        Start();
    }

}
