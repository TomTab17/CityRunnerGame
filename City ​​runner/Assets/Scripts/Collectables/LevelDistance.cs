using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LevelDistance : MonoBehaviour
{

    public GameObject disDisplay;
    public GameObject disEndDisplay;
    public int disRun;
    public bool addingDis = false;
    public float disDelay = 0.60f;

    public PlayFabManager playfabManager;

    void Update()
    {
        if(addingDis == false)
        {
            addingDis = true;
            StartCoroutine(AddingDis());

        }
    }

    IEnumerator AddingDis()
    {
        disRun += 1;
        disDisplay.GetComponent<Text>().text = "" + disRun;
        disEndDisplay.GetComponent<Text>().text = "" + disRun;
        yield return new WaitForSeconds(disDelay);
        addingDis = false;
    }

    void OnDisable()
    {
        int totalDist = PlayerPrefs.GetInt("TotalDist", 0);
        totalDist += disRun;
        PlayerPrefs.SetInt("TotalDist", totalDist);
        PlayerPrefs.Save();
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
            {
                {"TotalDist", totalDist.ToString()}
            }
            };
            PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        }
        SendLeaderboard(disRun);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Successfully updated user data");
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Error updating user data: " + error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }
}
