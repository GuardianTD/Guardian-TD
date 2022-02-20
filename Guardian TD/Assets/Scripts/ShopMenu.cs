using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using SimpleJSON;

public class ShopMenu : MonoBehaviour
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
    /// gets the player information and uses that info in PlayerIcon and PlayerName
    /// </summary>
    void Start()
    {
        apiHandler.GetByID(Login.userID);
        JSONNode PlayerInfo = apiHandler.result;
        Debug.Log(apiHandler.result[0]["email"]);
        PlayerIcon.text = PlayerInfo[0]["email"].ToString().Replace("\"", "").Substring(0, 1);
        int index = PlayerInfo[0]["email"].ToString().Replace("\"", "").LastIndexOf("@");
        PlayerName.text = PlayerInfo[0]["email"].ToString().Replace("\"", "").Substring(0, index);
    }
    /// <summary>
    /// to go back to the previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }
}
