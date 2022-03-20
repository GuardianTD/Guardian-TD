using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using SimpleJSON;




public class Home : MonoBehaviour
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
    private APIHandler apiHandler = new APIHandler();
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
    /// <summary>
    /// Changes the Scene to PlayMenu
    /// </summary>
    public void PlayMenu()
    {
        SceneManager.LoadScene("PlayMenu");
    }
    /// <summary>
    /// Changes the Scene to ShopMenu
    /// </summary>
    public void ShopMenu()
    {
        SceneManager.LoadScene("ShopMenu");
    }
    /// <summary>
    /// Changes the Scene to Achievements
    /// </summary>
    public void Achievements()
    {
        SceneManager.LoadScene("Achievements");
    }
}