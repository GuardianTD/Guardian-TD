using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;

public class JSONHandler : MonoBehaviour
{
    public string jsonURL;
    private const string HOST = "GTD_Login";
    private const string API_KEY = "GTD_Login";

    void Start()
    {
        jsonURL = Application.dataPath + "/APIs/GTD.postman_collection.json";
        StartCoroutine(processJsonData(jsonURL));
    }

    public IEnumerator processJsonData(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("X-RapidAPI-Host", HOST);
            request.SetRequestHeader("X-RapidAPI-Key", API_KEY);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JSONNode itemsData = JSON.Parse(request.downloadHandler.text);

            }
        }
    }
}
