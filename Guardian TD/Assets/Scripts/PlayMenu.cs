using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using SimpleJSON;



public class PlayMenu : MonoBehaviour
{
    /// <summary>
    /// GameObject for PlayerIcon
    /// </summary>
    public TextMeshProUGUI PlayerIcon;
    /// <summary>
    /// GameObject for Playername
    /// </summary>
    public TextMeshProUGUI PlayerName;
    /// <summary>
    /// instance of APIHandler class
    /// </summary>
    public APIHandler apiHandler = new APIHandler();
    /// <summary>
    /// the url to make the get and post calls
    /// </summary>
    private string uri = "https://guardiantdapi.azurewebsites.net/api/User";
    /// <summary>
    /// gets the player information and uses that info in PlayerIcon and PlayerName
    /// </summary>
    void Start()
    {
        apiHandler.GetByID(Login.userID, uri);
        JSONNode PlayerInfo = apiHandler.result;
        Debug.Log(apiHandler.result[0]["user_name"]);
        PlayerIcon.text = PlayerInfo[0]["user_name"].ToString().Replace("\"", "").Substring(0, 1);
        PlayerName.text = PlayerInfo[0]["user_name"].ToString().Replace("\"", "");
    }
    public void SinglePlayerButton()
    {
        SceneManager.LoadScene("Garden");
    }
    /// <summary>
    /// to go back to previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }
    /// <summary>
    /// to go to leaderboard scene
    /// </summary>
    public void Leaderboards()
    {
        SceneManager.LoadScene("Leaderboards");
    }
}