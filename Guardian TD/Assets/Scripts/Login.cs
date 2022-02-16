using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField UserID;
    public TMP_InputField Password;
    
    public string[] getLoginCredentials()
    {
        string[] LoginCredentials = new string[] { UserID.text, Password.text};
        Debug.Log(LoginCredentials[0] + LoginCredentials[1]);
        return LoginCredentials;
    }
    public void LoginButton ()
    {
        getLoginCredentials();
        SceneManager.LoadScene("HomeMenu");
    }
    public void SignUpButton ()
    {
        getLoginCredentials();
    }
    public void GoogleSignIn ()
    {

    }
}
