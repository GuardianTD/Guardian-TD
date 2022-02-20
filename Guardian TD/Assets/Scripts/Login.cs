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
    
    public TMP_InputField EmailID;//the inputfield for entering the emailid
    public TMP_InputField Password;// the inputfield for entering the password
    public TMP_Text ErrorMessage;// the text area for sending the erroe messages
    public string uri = "https://guardiantd.azurewebsites.net/api/User";// the url to make the get and post calls
    public APIHandler apiHandler = new APIHandler();// instance of the APIHandler class
    
    
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
    /// button used to login
    /// </summary>
    public void LoginButton ()
    {
        string[] credentials = apiHandler.GetLoginCredentials(EmailID.text, Password.text);
        apiHandler.Get(uri);
        JSONNode result = apiHandler.result;
        for(int i = 0; i < result[i]["user_id"]; i++)
        {
            if((result[i]["email"] == credentials[0]) && (result[i]["password"] == credentials[1]))
            {
                SceneManager.LoadScene("HomeMenu");
            }
            else
            {
                ErrorMessage.text = "The email or password doesn't exist!";
            }
        }
        
    }
    /// <summary>
    /// button used to sign up
    /// </summary>
    public void SignUpButton ()
    {
        string[] credentials = apiHandler.GetLoginCredentials(EmailID.text, Password.text);
        bool isEmailValid = IsValidEmail(credentials[0]);
        string isPasswordValid = IsPasswordValid(credentials[1]);
        if(isEmailValid == true && isPasswordValid == "Success")
        {
            string information = "{\r\n        \"FirstName\": \" \",\r\n        \"LastName\": \" \",\r\n        \"Email\": \"" + credentials[0].ToString() + "\",\r\n        \"Password\": \"" + credentials[1].ToString() + "\"\r\n}";
            apiHandler.Post(uri, information, "Application/JSON");
            if(apiHandler.result == "User Added Successfully")
            {
                SceneManager.LoadScene("HomeMenu");
            }
            else
            {
                ErrorMessage.text = "Error registering the User. Please try again!";
            }
        }
        else
        {
            if (isEmailValid == false)
            {
                ErrorMessage.text = "Please enter a valid email address!";
            }
            else
            {
                ErrorMessage.text = isPasswordValid;
            }
            
        }
    }
}
