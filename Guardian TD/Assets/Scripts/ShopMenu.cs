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
    /// Game objects buttons
    /// </summary>
    public Button TowersShop;
    public Button PowerUpsShop;
    /// <summary>
    /// to check which shop is open towers or powerups
    /// </summary>
    private string ActiveShop = "Towers";
    /// <summary>
    /// the primary color of buttons
    /// </summary>
    public Color Green;
    /// <summary>
    /// game objects to edit which shop is open
    /// </summary>
    private GameObject TowerText;
    private GameObject PowerUpsText;
    /// <summary>
    /// instance of api handler class
    /// </summary>
    private APIHandler apiHandler = new APIHandler();
    /// <summary>
    /// URLs for api calls
    /// </summary>
    private string towerURI = "https://guardiantdapi.azurewebsites.net/api/Tower";
    private string userTowerURI = "https://guardiantdapi.azurewebsites.net/api/UserTower";
    /// <summary>
    /// JSONNodes for storing the result of api calls
    /// </summary>
    private JSONNode TowerInfo;
    private JSONNode TowerByIdInfo;
    private JSONNode UserTowerInfo;
    /// <summary>
    /// for assigning font to game objects
    /// </summary>
    private TMP_FontAsset font;
    /// <summary>
    /// to keep track of which item is displayed
    /// </summary>
    private int CurrentId = 1;
    /// <summary>
    /// to keep track of which item was previously displayed
    /// </summary>
    private int PreviousId;
    /// <summary>
    /// total no. items registered in database
    /// </summary>
    private int TotalItems;
    /// <summary>
    /// Game objects to edit the buttons or text in the ui
    /// </summary>
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
    /// <summary>
    /// Update function to keep updating the game objects while on this scene
    /// </summary>
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
    /// <summary>
    /// to change the shop menu on click
    /// </summary>
    /// <param name="ShopMenu">which shop menu to open</param>
    public void ChangeShopMenu(string ShopMenu)
    {
        ActiveShop = ShopMenu;
        CurrentId = 1;
        PreviousId = 0;
    }
    /// <summary>
    /// to move to next item
    /// </summary>
    public void NextButton()
    {
        if(TotalItems > CurrentId)
        {
            
            CurrentId++;
        }
    }
    /// <summary>
    /// to move to previous item
    /// </summary>
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