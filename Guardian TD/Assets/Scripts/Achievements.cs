using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using SimpleJSON;

public class Achievements : MonoBehaviour
{
    private string UserAchievementsURI = "https://guardiantdapi.azurewebsites.net/api/UserAchievements/User";
    private string UserDataURI = "https://guardiantdapi.azurewebsites.net/api/UserAchievements/All/User";
    private string AllAchievementsURI = "https://guardiantdapi.azurewebsites.net/api/Achievements";
    private string ChangeAchievementStatus = "https://guardiantdapi.azurewebsites.net/api/UserAchievements";
    private APIHandler apiHandler = new APIHandler();
    private JSONNode UserAchievements;
    private JSONNode UserData;
    private JSONNode AllAchievements;
    public GameObject Panel;
    public TMP_FontAsset font;
    private bool IsDisplayed = false;
    /// <summary>
    /// to go back to the previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }
    void Update()
    {
        apiHandler.GetByID("3", UserAchievementsURI);
        UserAchievements = apiHandler.result;
        apiHandler.GetByID("3", UserDataURI);
        UserData = apiHandler.result;
        apiHandler.Get(AllAchievementsURI);
        AllAchievements = apiHandler.result;
        if (IsDisplayed == false)
        {
            for (int i = 0; i < AllAchievements.Count; i++)
            {
                GameObject Achievements = new GameObject("Achievement" + (i + 1));
                GameObject ClaimButton = new GameObject("ClaimButton" + (i + 1));
                GameObject ClaimText = new GameObject("ClaimText" + (i + 1));
                Achievements.transform.parent = Panel.transform;
                Achievements.AddComponent<CanvasRenderer>();
                Achievements.AddComponent<TextMeshProUGUI>();

                ClaimButton.transform.parent = Panel.transform;
                ClaimButton.AddComponent<Button>();
                ClaimButton.AddComponent<Image>();
                ClaimText.transform.parent = ClaimButton.transform;
                ClaimText.AddComponent<TextMeshProUGUI>();

                Achievements.transform.position = transform.position + new Vector3(-80, -(i * 60) + 0, 0);
                Achievements.GetComponent<TextMeshProUGUI>().text = AllAchievements[i]["name"] + "  - " + AllAchievements[i]["claim_value"];
                Achievements.GetComponent<TextMeshProUGUI>().color = Color.black;
                Achievements.GetComponent<TextMeshProUGUI>().font = font;
                Achievements.GetComponent<TextMeshProUGUI>().fontSize = 20;
                Achievements.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 30f);

                ClaimButton.transform.position = transform.position + new Vector3(200, -(i * 55), 0);
                ClaimButton.GetComponent<Image>().color = Color.grey;
                ClaimButton.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 30f);
                ClaimText.transform.position = transform.position + new Vector3(200, -(i * 55), 0);
                ClaimText.GetComponent<TextMeshProUGUI>().text = "Claim";
                ClaimText.GetComponent<TextMeshProUGUI>().color = Color.black;
                ClaimText.GetComponent<TextMeshProUGUI>().font = font;
                ClaimText.GetComponent<TextMeshProUGUI>().fontSize = 20;
                ClaimText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                ClaimText.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 50f);

                if (UserAchievements[i]["claim_status"] == "claimed")
                {
                    ClaimText.GetComponent<TextMeshProUGUI>().text = "Claimed";
                    ClaimButton.GetComponent<Image>().color = Color.grey;
                }
                if (UserData[i]["value"] == AllAchievements[i - 1]["claim_value"])
                {
                    ClaimText.GetComponent<TextMeshProUGUI>().text = "Claim";
                    ClaimButton.GetComponent<Image>().color = Color.green;
                }
                ClaimButton.GetComponent<Button>().onClick.AddListener(() => UpdateAchievementStatus(i, ClaimText, ClaimButton, Achievements));
                IsDisplayed = true;
            }
        }
        
    }
    void UpdateAchievementStatus(int i, GameObject ClaimText, GameObject ClaimButton, GameObject Achievements)
    {
        if (UserAchievements[i - 1]["claim_status"] != "claimed")
        {
            Debug.Log(i);
            string ChangeStatus = "{\r\n \"UserId\": \"" + "3" + "\",\r\n \"AchievementId\": \"" + UserAchievements[i - 1]["achievement_id"] + "\"\r\n}";
            apiHandler.Post(ChangeAchievementStatus, ChangeStatus, "Application/JSON");
            Destroy(ClaimText);
            Destroy(ClaimButton);
            Destroy(Achievements);
            IsDisplayed = false;
        }
    }
}
