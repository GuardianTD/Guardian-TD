using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using UnityEngine;
using SimpleJSON;

public class GameloopManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    /** \To check the summoned enemy in game */
    public static Vector3[] NodePositions;
 
    public static List<TowerBehavior> TowersInGame;
    private static Queue<EnemyDamageData> DamageData;
    public static float[] NodeDistances;
    public static Queue<Enemy> EnemiesToRemove;
    private static Queue<int> EnemyIDsToSummon;

    public Transform NodeParent;
    /** \To know about game end or wave end */
    public bool Loopshouldend;

    /// <summary>
    ///  instance of the APIHandler class
    /// </summary>
    public APIHandler apiHandler = new APIHandler();
    /// <summary>
    /// the url to make the get and post calls
    /// </summary>
    public string userGameMappingURI = "https://guardiantdapi.azurewebsites.net/api/Game/";
    public string gameTowerInfoURI = "https://guardiantdapi.azurewebsites.net/api/GameTower/UserGame/";
    public string postTowerInfoURI = "https://guardiantdapi.azurewebsites.net/api/GameTower/";
    /** \To check the summoned enemy in game */
    private int UserGameId;
    public JSONNode LatestGameInfo;
    public JSONNode TowerInfo;
    public static int enemyID;

    //remove these when u impliment an actual wave
    public int totalEnemiesSpawnned;
    public int enemiesPerWave = 10;
    public int noOfEnemiesPresent;

    private void Start()
    {
        apiHandler.Get(userGameMappingURI);
        if (apiHandler.result == null)
        {
            UserGameId = 1;
            string userGameInfo = "{\r\n \"UserGameId\": \"" + UserGameId + "\",\r\n \"GameType\": \"" + "single" + "\",\r\n \"UserScore\": \"" + "0" + "\",\r\n \"EnemyId\": \"" + enemyID + "\"\r\n}";
            apiHandler.Post(userGameMappingURI, userGameInfo, "Application/JSON");
        }
        else
        {

            UserGameId = int.Parse(apiHandler.result[apiHandler.result.Count - 1]["user_game_id"].ToString());
            Debug.Log(UserGameId);
            enemyID = int.Parse(apiHandler.result[apiHandler.result.Count - 1]["enemy_id"].ToString());
            apiHandler.GetByID(UserGameId.ToString(), userGameMappingURI);
            LatestGameInfo = apiHandler.result;
            apiHandler.GetByID(UserGameId.ToString(), gameTowerInfoURI);//post method not working for this uri
            TowerInfo = apiHandler.result;
        }
        DamageData = new Queue<EnemyDamageData>();

        TowersInGame = new List<TowerBehavior>();
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();
        Entitysummoner.Init();

        NodePositions = new Vector3[NodeParent.childCount];

        for (int i = 0;i< NodePositions.Length; i++)
        {
            NodePositions[i] = NodeParent.GetChild(i).position;
        }

        NodeDistances = new float[NodePositions.Length-1];

        for (int i = 0; i < NodeDistances.Length; i++)
        {
            NodeDistances[i] = Vector3.Distance(NodePositions[i], NodePositions[i + 1]);
        }

        StartCoroutine(Gameloop());
        InvokeRepeating("SummonTest", 0f, 1f);
       // InvokeRepeating("RemoveTest", 0f, 0.5f);

    }
    //void RemoveTest()
    //{
      // if (Entitysummoner.EnemiesInGame.Count > 0)
        //{
          //  Entitysummoner.RemoveEnemy(Entitysummoner.EnemiesInGame[Random.Range(0, Entitysummoner.EnemiesInGame.Count)]);
        //}
    //}

    /// <summary>
    /// Enemy summon test case to summon one slime ID 1
    /// </summary>
    void SummonTest()
    {
        if (totalEnemiesSpawnned < enemiesPerWave)
        {
            EnqueueEnemyIDToSummon(enemyID);
            totalEnemiesSpawnned++;
            //enemyID++;
        }
        if (noOfEnemiesPresent == 0 && totalEnemiesSpawnned == enemiesPerWave)
        {
            string userGameInfo = "{\r\n \"UserGameId\": \"" + UserGameId + "\",\r\n \"UserScore\": \"" + "0" + "\",\r\n \"EnemyId\": \"" + enemyID + "\"\r\n}";
            apiHandler.Patch(userGameMappingURI, userGameInfo, "Application/JSON");
            List<string> towerGameInfo = new List<string>();
            for (int i = 0; i < TowersInGame.Count; i++)
            {
                if (i == TowersInGame.Count - 1)
                {
                    towerGameInfo.Add("{\r\n \"UserGameId\": \"" + UserGameId + "\",\r\n \"TowerId\": \"" + TowersInGame[i].towerID + "\",\r\n \"Vector\": \"" + TowersInGame[i].transform.position + "\"\r\n}");
                }
                else
                {
                    towerGameInfo.Add("{\r\n \"UserGameId\": \"" + UserGameId + "\",\r\n \"TowerId\": \"" + TowersInGame[i].towerID + "\",\r\n \"Vector\": \"" + TowersInGame[i].transform.position + "\"\r\n},");
                }
            }
            string towerGameStr = string.Join("\r\n", towerGameInfo.ToArray());
            apiHandler.Delete(gameTowerInfoURI, UserGameId.ToString());
            apiHandler.Post(postTowerInfoURI, "[" + towerGameStr + "]", "Application/JSON");
            Debug.Log("put method called \r\n" + apiHandler.result.ToString());
            totalEnemiesSpawnned = 0;
            enemiesPerWave = 10 + enemiesPerWave;
        }
        if (noOfEnemiesPresent == 0)
        {
            noOfEnemiesPresent = enemiesPerWave;
        }
    }
    /// <summary>
    /// Gameloop to start and end game after a wave or so
    /// </summary>
    /// <returns></returns>
   IEnumerator Gameloop()
    {
        while (Loopshouldend==false)
        {


            //spawn enemies

            if (EnemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    Entitysummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }

            //spawn towers

            //move enemies
            NativeArray<Vector3> NodesToUse = new NativeArray<Vector3>(NodePositions, Allocator.TempJob);
            NativeArray<int> NodeIndices = new NativeArray<int>(Entitysummoner.EnemiesInGame.Count, Allocator.TempJob);
            NativeArray<float> EnemySpeeds = new NativeArray<float>(Entitysummoner.EnemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray EnemyAccess = new TransformAccessArray(Entitysummoner.EnemiesInGameTransform.ToArray(), 2);


            for(int i=0;i< Entitysummoner.EnemiesInGame.Count; i++)
            {
                EnemySpeeds[i] = Entitysummoner.EnemiesInGame[i].Speed;
                NodeIndices[i] = Entitysummoner.EnemiesInGame[i].NodeIndex;
            }


            MoveEnemiesJob MoveJob = new MoveEnemiesJob
            {
                NodePositions= NodesToUse,
                EnemySpeed=EnemySpeeds,
                NodeIndex=NodeIndices,
                deltaTime=Time.deltaTime
            };

            JobHandle MoveJobHandle = MoveJob.Schedule(EnemyAccess);
            MoveJobHandle.Complete();

            for(int i=0;i< Entitysummoner.EnemiesInGame.Count; i++)
            {
                Entitysummoner.EnemiesInGame[i].NodeIndex = NodeIndices[i];

                if(Entitysummoner.EnemiesInGame[i].NodeIndex==NodePositions.Length)
                {
                    EnqueueEnemyToRemove(Entitysummoner.EnemiesInGame[i]);
                }
            }

            NodesToUse.Dispose();
            EnemySpeeds.Dispose();
            NodeIndices.Dispose();
            EnemyAccess.Dispose();
            //tick towers


            foreach(TowerBehavior tower in TowersInGame)
            {
                tower.Target = TowerTargeting.GetTarget(tower, TowerTargeting.TargetType.First);
                tower.Tick();
            }

            //Apply effects


            //damage Enemies enemies
            if (DamageData.Count > 0)
            {
                for (int i = 0; i < DamageData.Count; i++)
                {
                    EnemyDamageData CurrentDamageData = DamageData.Dequeue();
                    CurrentDamageData.TargetedEnemy.Health -= CurrentDamageData.TotalDamage / CurrentDamageData.Resistance;

                    if(CurrentDamageData.TargetedEnemy.Health <= 0F) 
                    {
                        EnqueueEnemyToRemove(CurrentDamageData.TargetedEnemy);
                    }
                }
            }
            //remove Enemies

            if (EnemiesToRemove.Count > 0)
            {
                for(int i=0; i < EnemiesToRemove.Count; i++)
                {
                    Entitysummoner.RemoveEnemy(EnemiesToRemove.Dequeue());
                    if (noOfEnemiesPresent > 0)
                    {
                        noOfEnemiesPresent--;
                        //Debug.Log("enemies persent"+noOfEnemiesPresent);
                    }
                }
            }

            //remove towers
            yield return null;

        }
    }


    public static void EnqueueDamageData(EnemyDamageData damagedata)
    {
        DamageData.Enqueue(damagedata);
    }

    /// <summary>
    /// Quesuing enemies to spawn in game using their ID to summon
    /// </summary>
    /// <param name="ID"></param>

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }
    public static void EnqueueEnemyToRemove(Enemy EnemyToRemove)
    {
        EnemiesToRemove.Enqueue(EnemyToRemove);

    }
}

public struct EnemyDamageData
{

    public EnemyDamageData(Enemy target, float damage, float resistance)
    {
        TargetedEnemy = target;
        TotalDamage = damage;
        Resistance = resistance;
    }
    public Enemy TargetedEnemy;
    public float TotalDamage;
    public float Resistance;
}

public struct MoveEnemiesJob: IJobParallelForTransform
{

    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> NodePositions;

    [NativeDisableParallelForRestriction]
    public NativeArray<float> EnemySpeed;

    [NativeDisableParallelForRestriction]
    public NativeArray<int> NodeIndex;

    public float deltaTime;
  
    public void Execute(int index, TransformAccess transform)
    {
        if (NodeIndex[index] < NodePositions.Length)
        {
            Vector3 PositionToMoveTo = NodePositions[NodeIndex[index]];
            transform.position = Vector3.MoveTowards(transform.position, PositionToMoveTo, EnemySpeed[index] * deltaTime);


            if (transform.position == PositionToMoveTo)
            {
                NodeIndex[index]++;
            }
        }

    }
}