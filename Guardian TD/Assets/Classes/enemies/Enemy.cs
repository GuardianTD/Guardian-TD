using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Enemy : MonoBehaviour

{
    public int NodeIndex;
    public Transform RootPart;
    public float DamageResistance = 1f;
    /** \Max health of enemies in float */
    public float MaxHealth;
    /** \health of enemies in float at an instance*/
    public float Health;
    /** \speed of enemies movement in float */
    public float Speed;
    /** \ enemies ID */
    public int ID;

    public APIHandler apiHandler = new APIHandler();
    public int level;
    public JSONNode EnemyInfo;
    public string enemyInfoURI = "https://guardiantdapi.azurewebsites.net/api/Enemy";
    /// <summary>
    /// initializes the healthpool of enemies to max health of enemies
    /// </summary>
    public void Init()
    {
        apiHandler.GetByID(GameloopManager.enemyID.ToString(), enemyInfoURI);
        EnemyInfo = apiHandler.result;
        ID = EnemyInfo["enemy_id"];
        MaxHealth = EnemyInfo["max_health"];
        level = EnemyInfo["enemy_level"];
        Health = MaxHealth;
        transform.position = GameloopManager.NodePositions[0];
        NodeIndex = 0;

    }
}
