using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class CollectableControl : MonoBehaviour
{

    public static int coinCount;
    public GameObject coinCountDisplay;
    public GameObject coinEndDisplay;

    
    void Start()
    {
        coinCount = 0;
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        coinCountDisplay.GetComponent<Text>().text = "" + totalCoins;
        coinEndDisplay.GetComponent<Text>().text = "" + totalCoins;
    }

    
    void Update()
    {
        coinCountDisplay.GetComponent<Text>().text = "" + coinCount;
        coinEndDisplay.GetComponent<Text>().text = "" + coinCount;
    }

    void OnDisable()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += coinCount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"TotalCoins", totalCoins.ToString()}
                }
            };
            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        }
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Successfully updated user data");
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Error updating user data: " + error.GenerateErrorReport());
    }



}
