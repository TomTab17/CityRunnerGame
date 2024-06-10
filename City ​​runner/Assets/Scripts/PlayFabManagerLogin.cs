using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public Text messageText;
    public InputField emailInput;
    public InputField passwordInput;
    public GameObject RegisterPanel;
    public GameObject LoginRegisterPanel;
    public Button LogoutBut;
    public GameObject AlertLog;

    public GameObject totalCoinsDisplay;
    public GameObject totalDistDisplay;

    public GameObject rowPrefab;
    public Transform rowsParent;

    public GameObject AlertLead;

    public void Start()
    {
        if (emailInput != null && passwordInput != null)
        {
            if (PlayerPrefs.HasKey("Email") && PlayerPrefs.HasKey("Password"))
            {
                emailInput.text = PlayerPrefs.GetString("Email");
                passwordInput.text = PlayerPrefs.GetString("Password");
                LoginButton();
            }
        }
        if (LogoutBut != null)
        {
            if (PlayerPrefs.HasKey("LogoutButtonState"))
            {
                int logoutButtonState = PlayerPrefs.GetInt("LogoutButtonState");

                if (LogoutBut != null)
                {
                    LogoutBut.gameObject.SetActive(logoutButtonState == 1);
                }
            }
        }
        if (AlertLog != null)
        {
            if (PlayerPrefs.HasKey("AlertLogState"))
            {
                int alertLogState = PlayerPrefs.GetInt("AlertLogState");

                if (AlertLog != null)
                {
                    AlertLog.SetActive(alertLogState == 1);
                }
            }
        }
    }



    public void RegisterButtonNext()
    {
        RegisterPanel.SetActive(true);
        LoginRegisterPanel.SetActive(false);
    }
    public void CloseButtonLogin()
    {
        LoginRegisterPanel.SetActive(false);
    }



    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "3619A"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }




    public void LogoutButton()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        messageText.text = "Logged out!";
        Debug.Log("Logout successful");
        LogoutBut.gameObject.SetActive(false);
        AlertLog.gameObject.SetActive(true);
        SaveLogoutButtonState();
        totalCoinsDisplay.GetComponent<Text>().text = "" + 0;
        totalDistDisplay.GetComponent<Text>().text = "" + 0;
        emailInput.text = "";
        passwordInput.text = "";
        PlayerPrefs.DeleteKey("Email");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.Save();

    }
    private void SaveLogoutButtonState()
    {
        PlayerPrefs.SetInt("LogoutButtonState", LogoutBut.gameObject.activeSelf ? 1 : 0);
        PlayerPrefs.SetInt("AlertLogState", AlertLog.activeSelf ? 1 : 0);
        PlayerPrefs.Save();
    }



    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset mail sent!";
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in!";
        Debug.Log("Successful login/account create!");
        AlertLog.gameObject.SetActive(false);
        LogoutBut.gameObject.SetActive(true);
        SaveLogoutButtonState();

        string playFabId = result.PlayFabId;
        string savedPlayFabId = PlayerPrefs.GetString("CurrentUserId", "");
        if (playFabId != savedPlayFabId)
        {
            PlayerPrefs.SetString("CurrentUserId", playFabId);
            PlayerPrefs.SetInt("TotalDist", 0);
            PlayerPrefs.SetInt("TotalCoins", 0);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetString("Email", emailInput.text);
        PlayerPrefs.SetString("Password", passwordInput.text);
        PlayerPrefs.Save();

        FetchUserData();
    }

    void FetchUserData()
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = PlayerPrefs.GetString("CurrentUserId")
        };
        PlayFabClientAPI.GetUserData(request, OnUserDataReceived, OnError);
    }

    void OnUserDataReceived(GetUserDataResult result)
    {
        if (result.Data != null)
        {
            if (result.Data.ContainsKey("TotalDist"))
            {
                int totalDist = int.Parse(result.Data["TotalDist"].Value);
                PlayerPrefs.SetInt("TotalDist", totalDist);
                totalDistDisplay.GetComponent<Text>().text = totalDist.ToString();
            }

            if (result.Data.ContainsKey("TotalCoins"))
            {
                int totalCoins = int.Parse(result.Data["TotalCoins"].Value);
                PlayerPrefs.SetInt("TotalCoins", totalCoins);
                totalCoinsDisplay.GetComponent<Text>().text = totalCoins.ToString();
            }

            PlayerPrefs.Save();
        }
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetLeaderboard()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            AlertLead.gameObject.SetActive(false);
            var request = new GetLeaderboardRequest
            {
                StatisticName = "PlatformScore",
                StartPosition = 0,
                MaxResultsCount = 10
            };
            PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        }
        else
        {
            AlertLead.gameObject.SetActive(true);
            foreach (Transform item in rowsParent)
            {
                Destroy(item.gameObject);
            }
        }
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            var request = new GetAccountInfoRequest
            {
                PlayFabId = item.PlayFabId
            };
            PlayFabClientAPI.GetAccountInfo(request, (GetAccountInfoResult accountInfoResult) =>
            {
                string username = accountInfoResult.AccountInfo.Username;
                texts[1].text = username;
            }, OnError);
            texts[2].text = item.StatValue.ToString();
        }

    }

}
