using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }
    public void Leaderboards()
    {
        SceneManager.LoadScene("Leaderboards");
    }
}
