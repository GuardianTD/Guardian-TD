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
    public Button TowersShop;
    public Button PowerUpsShop;
    private string ActiveShop = "Towers";
    public Color Green;
    private GameObject TowerText;
    private GameObject PowerUpsText;
    private bool isDisplayed = false;
    private APIHandler apiHandler = new APIHandler();
    private string towerURI = "https://guardiantdapi.azurewebsites.net/api/Tower";
    private string userTowerURI = "https://guardiantdapi.azurewebsites.net/api/UserTower";
    private JSONNode TowerInfo;
    private JSONNode TowerByIdInfo;
    private JSONNode UserTowerInfo;
    private TMP_FontAsset font;
    private int CurrentId = 1;
    private int PreviousId;
    private int TotalItems;
    public GameObject Items;
    public GameObject Next;
    public GameObject Previous;
    public GameObject BuyButton;

    /// <summary>
    /// gets the player information and uses that info in PlayerIcon and PlayerName
    /// </summary>
    void Start()
    {
        TowerText = TowersShop.transform.GetChild(0).gameObject;
        PowerUpsText = PowerUpsShop.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if(PreviousId != CurrentId)
        {
            if (TotalItems > CurrentId)
            {
                Next.GetComponent<Image>().color = Green;
            }
            else
            {
                Next.GetComponent<Image>().color = Color.grey;
            }
            if (CurrentId > 1)
            {
                Previous.GetComponent<Image>().color = Green;
            }
            else
            {
                Previous.GetComponent<Image>().color = Color.grey;
            }
            if (ActiveShop == "Towers")
            {
                TowersShop.GetComponent<Image>().color = Green;
                TowerText.GetComponent<TextMeshProUGUI>().color = Color.white;
                PowerUpsShop.GetComponent<Image>().color = Color.white;
                PowerUpsText.GetComponent<TextMeshProUGUI>().color = Color.black;

                apiHandler.Get(towerURI);
                TowerInfo = apiHandler.result;
                TotalItems = TowerInfo.Count;
                apiHandler.GetByID(CurrentId.ToString(), towerURI);
                TowerByIdInfo = apiHandler.result;
                apiHandler.Get(userTowerURI);
                UserTowerInfo = apiHandler.result;

                for(int i = 0; i < UserTowerInfo.Count; i++)
                {
                    if(TowerByIdInfo[CurrentId-1]["tower_id"] == UserTowerInfo[i]["tower_id"])
                    {
                        BuyButton.GetComponent<Image>().color = Color.grey;
                        break;
                    }
                    else
                    {
                        BuyButton.GetComponent<Image>().color = Green;
                    }
                }

                
                Items.GetComponent<TextMeshProUGUI>().text = TowerByIdInfo[CurrentId - 1]["tower_name"].ToString().Replace("\"", "") + "\r\n" + TowerByIdInfo[CurrentId - 1]["tower_type"].ToString().Replace("\"", "") + "\r\n" + "Damage: " + TowerByIdInfo[CurrentId - 1]["tower_damage"].ToString() + "\r\n" + "Range: " + TowerByIdInfo[CurrentId - 1]["tower_range"].ToString();
                Items.GetComponent<TextMeshProUGUI>().color = Color.black;
                Items.GetComponent<TextMeshProUGUI>().font = font;
                Items.GetComponent<TextMeshProUGUI>().fontSize = 30;
            }
            else
            {
                PowerUpsShop.GetComponent<Image>().color = Green;
                PowerUpsText.GetComponent<TextMeshProUGUI>().color = Color.white;
                TowersShop.GetComponent<Image>().color = Color.white;
                TowerText.GetComponent<TextMeshProUGUI>().color = Color.black;

                Items.GetComponent<TextMeshProUGUI>().text = "Enter Item Details Here";
                Items.GetComponent<TextMeshProUGUI>().color = Color.black;
                Items.GetComponent<TextMeshProUGUI>().font = font;
                Items.GetComponent<TextMeshProUGUI>().fontSize = 30;
            }
            PreviousId = CurrentId;
        }
        //apiHandler.GetByID(Login.userID, uri);
    }
    public void ChangeShopMenu(string ShopMenu)
    {
        ActiveShop = ShopMenu;
        CurrentId = 1;
        PreviousId = 0;
    }
    public void NextButton()
    {
        if(TotalItems > CurrentId)
        {
            
            CurrentId++;
        }
    }
    public void PreviousButton()
    {
        if (CurrentId > 1)
        {
            CurrentId--;
        }
    }
    /// <summary>
    /// to go back to the previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }
}