using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SinglePlayerLeaderboard : MonoBehaviour
{
    /// <summary>
    /// to back to previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("Leaderboards");
    }
}
