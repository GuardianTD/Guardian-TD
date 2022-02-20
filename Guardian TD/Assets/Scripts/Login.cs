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
    public TMP_InputField EmailID;
    /// <summary>
    /// the inputfield for entering the password
    /// </summary>
    public TMP_InputField Password;
    /// <summary>
    /// the text area for sending the erroe messages
    /// </summary>
    public TMP_Text ErrorMessage;
    /// <summary>
    ///  instance of the APIHandler class
    /// </summary>
    public APIHandler apiHandler = new APIHandler();
    /// <summary>
    /// to access userID
    /// </summary>
    public static string userID;
    
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
        apiHandler.Get();
        JSONNode result = apiHandler.result;
        for(int i = 0; i < result[i]["user_id"]; i++)
        {
            if((result[i]["email"] == credentials[0]) && (result[i]["password"] == credentials[1]))
            {
                userID = result[i]["user_id"].ToString();
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
            apiHandler.Post(information, "Application/JSON");
            if(apiHandler.result["message"] == "User Added Successfully")
            {
                userID = apiHandler.result["userid"].ToString();
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
