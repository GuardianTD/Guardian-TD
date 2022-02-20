using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Achievements : MonoBehaviour
{
    /// <summary>
    /// to go back to the previous scene
    /// </summary>
    public void BackButton()
    {
        SceneManager.LoadScene("HomeMenu");
    }
}
