using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySummonData", menuName = "Create EnemySummonData")] /** \To ccreate a menu file for enemies that are summoned in game and their data like health and max health */
public class Enemiessummondata : ScriptableObject

{
    // Start is called before the first frame update

    /** \To check the summoned enemy in game */
    public GameObject EnemyPrefab;
    /** \unique ID of enemy */
    public int EnemyID;

}
