using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Home : MonoBehaviour
{
    public void PlayMenu()
    {
        SceneManager.LoadScene("PlayMenu");
    }
    public void ShopMenu()
    {
        SceneManager.LoadScene("ShopMenu");
    }
    public void Achievements()
    {
        SceneManager.LoadScene("Achievements");
    }
}
