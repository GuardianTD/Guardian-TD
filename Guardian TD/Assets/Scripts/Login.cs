using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using SimpleJSON;



public class Login : MonoBehaviour
{
    /// <summary>
    /// the inputfield for entering the emailid
    /// </summary>
    public TMP_InputField UserName;
    /// <summary>
    /// the inputfield for entering the password
    /// </summary>
    public TMP_InputField Password;
    /// <summary>
    /// the text area for sending the erroe messages
    /// </summary>
    public TMP_Text ErrorMessageLogin;
    /// <summary>
    ///  instance of the APIHandler class
    /// </summary>
    private APIHandler apiHandler = new APIHandler();
    /// <summary>
    /// to access userID
    /// </summary>
    public static string userID;
    /// <summary>
    /// the url to make the get and post calls
    /// </summary>
    private string uri = "https://guardiantdapi.azurewebsites.net/api/User";

    /// <summary>
    /// button used to login
    /// </summary>
    public void LoginButton()
    {
        string[] credentials = apiHandler.GetLoginCredentials(UserName.text, Password.text);
        apiHandler.Get(uri);
        JSONNode result = apiHandler.result;
        for (int i = 0; i < result[i]["user_id"]; i++)
        {
            if ((result[i]["user_name"] == credentials[0]) && (result[i]["password"] == credentials[1]))
            {
                userID = result[i]["user_id"].ToString();
                ErrorMessageLogin.text = "";
                SceneManager.LoadScene("HomeMenu");
            }
            else
            {
                ErrorMessageLogin.text = "The User Name or Password doesn't exist!";
            }
        }

    }
    /// <summary>
    /// button used to go to sign up page
    /// </summary>
    public void SignUpButton()
    {
        SceneManager.LoadScene("SignUpMenu");
    }
}