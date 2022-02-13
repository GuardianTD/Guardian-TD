using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboards : MonoBehaviour
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
        SceneManager.LoadScene("PlayMenu");
    }
    public void SinglePlayerLeaderboard()
    {
        SceneManager.LoadScene("SinglePlayerLeaderboard");
    }
    public void MultiPlayerLeaderboard()
    {
        SceneManager.LoadScene("MultiPlayerLeaderboard");
    }
}
