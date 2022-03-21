using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using SimpleJSON;



public class SignUp : MonoBehaviour
{

    public TMP_InputField SignUpEmailID;
    public TMP_InputField SignUpUserName;
    public TMP_InputField SignUpPassword;
    /// <summary>
    /// the text area for sending the error messages
    /// </summary>
    public TMP_Text ErrorMessageSignUp;
    /// <summary>
    /// instance of the APIHandler class
    /// </summary>
    public APIHandler apiHandler = new APIHandler();
    /// <summary>
    /// the url to make the get and post calls
    /// </summary>
    private string uri = "https://guardiantdapi.azurewebsites.net/api/User";



    /// <summary>
    /// checks if the email is valid
    /// </summary>
    /// <param name="email">email entered by the user</param>
    /// <returns>returns true if the email is valid</returns>
    public bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// checks if the password matches the criteria
    /// </summary>
    /// <param name="password">user's entered password</param>
    /// <returns>returns a string containing error message depending on the missing criteria</returns>
    public string IsPasswordValid(string password)
    {
        int length = password.Length;
        bool hasUpper = false;
        bool hasLower = false;
        bool hasDigit = false;
        for (int i = 0; i < password.Length && !(hasUpper && hasLower && hasDigit); i++)
        {
            char c = password[i];
            if (!hasUpper) hasUpper = char.IsUpper(c);
            if (!hasLower) hasLower = char.IsLower(c);
            if (!hasDigit) hasDigit = char.IsDigit(c);
        }
        if (length < 8)
        {
            return "Password must be atleast 8 characters long!";
        }
        else if (!hasUpper)
        {
            return "Password must contain atleast one upper case!";
        }
        else if (!hasLower)
        {
            return "Password must contain atleast one lower case!";
        }
        else if (!hasDigit)
        {
            return "Password must contain atleast one digit!";
        }
        else
        {
            return "Success";
        }
    }

    /// <summary>
    /// button used to sign up
    /// </summary>
    public void SignUpButton()
    {
        string[] credentials = apiHandler.GetSignUpCredentials(SignUpEmailID.text, SignUpUserName.text, SignUpPassword.text);
        Debug.Log(SignUpEmailID.text);
        Debug.Log(SignUpUserName.text);
        Debug.Log(SignUpPassword.text);
        Debug.Log(credentials);
        bool isEmailValid = IsValidEmail(credentials[0]);
        string isPasswordValid = IsPasswordValid(credentials[2]);
        if (isEmailValid == true && isPasswordValid == "Success")
        {
            string information = "{\r\n \"UserName\": \"" + credentials[1].ToString() + "\",\r\n \"Email\": \"" + credentials[0].ToString() + "\",\r\n \"Password\": \"" + credentials[2].ToString() + "\"\r\n}";
            apiHandler.Post(uri, information, "Application/JSON");
            if (apiHandler.result["message"] == "User Added Successfully")
            {
                Login.userID = apiHandler.result["userid"].ToString();
                Debug.Log(Login.userID);
                SceneManager.LoadScene("HomeMenu");
            }
            else
            {
                ErrorMessageSignUp.text = "Error registering the User. Please try again!";
            }
        }
        else
        {
            if (isEmailValid == false)
            {
                ErrorMessageSignUp.text = "Please enter a valid email address!";
            }
            else
            {
                ErrorMessageSignUp.text = isPasswordValid;
            }
        }
    }
    /// <summary>
    /// to move back to login page
    /// </summary>
    public void LoginButton()
    {
        SceneManager.LoadScene("LoginMenu");
    }
}