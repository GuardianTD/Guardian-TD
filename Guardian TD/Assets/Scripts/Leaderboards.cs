using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboards : MonoBehaviour
{
    /// <summary>
    /// to go back to previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("PlayMenu");
    }
    /// <summary>
    /// to go to SinglePlayerLeaderboard scene
    /// </summary>
    public void SinglePlayerLeaderboard()
    {
        SceneManager.LoadScene("SinglePlayerLeaderboard");
    }
    /// <summary>
    /// to go to MultiPlayerLeaderboard scene
    /// </summary>
    public void MultiPlayerLeaderboard()
    {
        SceneManager.LoadScene("MultiPlayerLeaderboard");
    }
}
