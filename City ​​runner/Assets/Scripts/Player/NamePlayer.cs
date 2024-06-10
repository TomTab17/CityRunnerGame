using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class NamePlayer : MonoBehaviour
{

    public Text usernameTextField;

    void Start()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            var request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
        }
        else
        {
            Debug.LogWarning("The user is not authenticated");
        }
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        string username = result.AccountInfo.Username;
        usernameTextField.text = username;
    }

    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError("Error retrieving account information: " + error.ErrorMessage);
    }
}
