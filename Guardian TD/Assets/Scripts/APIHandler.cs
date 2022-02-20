using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;

public class APIHandler
{
    
    public JSONNode result;//to store result of the Get or Post methods
    public string[] LoginCredentials;// to handle login credentials
                                     // <summary>
    /// Get the user's login credentials
    /// </summary>

    /// <param name="emailID"> email of the user </param>
    /// <param name="password">password of the user </param>
    /// <returns>returns the user's login credentials</returns>
    public string[] GetLoginCredentials(string emailID, string password)
    {
        LoginCredentials = new string[] { emailID, password };
        return LoginCredentials;
    }
    /// <summary>
    /// to make the get call from the db
    /// </summary>
    /// <param name="uri">the url for making the get request</param>
    public void Get(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            result = JSON.Parse(reader.ReadToEnd());
        }
    }
    /// <summary>
    /// to make the post call to the db
    /// </summary>
    /// <param name="uri">the url for making the post call</param>
    /// <param name="data">the content of the body for posting to db</param>
    /// <param name="contentType">the type of the body</param>
    /// <param name="method">Post method</param>
    public void Post(string uri, string data, string contentType, string method = "POST")
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        request.ContentLength = dataBytes.Length;
        request.ContentType = contentType;
        request.Method = method;

        using (Stream requestBody = request.GetRequestStream())
        {
            requestBody.Write(dataBytes, 0, dataBytes.Length);
        }

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            result = JSON.Parse(reader.ReadToEnd());
        }
    }
}
