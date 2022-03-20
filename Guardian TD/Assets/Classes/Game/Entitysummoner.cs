using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entitysummoner : MonoBehaviour
{
    /** \List to know which enemies are in game */
    public static List<Enemy> EnemiesInGame;
    /** \Dictionary to call game object that is enemy prefabs */
    public static Dictionary<int, GameObject> EnemyPrefabs;

    public static List<Transform> EnemiesInGameTransform;
    /** \Dictionary to Queue enemy object pool */
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;
    /** \Variable to see if the enemies spawn is intialized*/
    private static bool IsIntialized;

   

    public static void Init()
    {
       if (!IsIntialized)
        {

            /** \Getting enemy from dictionary */
            EnemyPrefabs = new Dictionary<int, GameObject>();
            /** \creating enemy object pool */
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();

            EnemiesInGameTransform = new List<Transform>();
            /** \to check enemies that are present in game */
            EnemiesInGame = new List<Enemy>();


            Enemiessummondata[] Enemies = Resources.LoadAll<Enemiessummondata>("Enemies");
            //Debug.Log(Enemies[0].name);
            foreach (Enemiessummondata enemy in Enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            }
            IsIntialized = true;
       }
    
        else
       {
            Debug.Log("Already intialized");
        }
    }
    /// <summary>
    /// Fuction to summon enemy using enemy ID into game using enemyprefabs
    /// </summary>
    /// <param name="EnemyID"> Passed on enemyID which is unique to each enemy </param>
    /// <returns></returns>
    public static Enemy SummonEnemy(int EnemyID)
    {
        /** \To check the summoned enemy in game */
        Enemy SummonedEnemy = null;
        if (EnemyPrefabs.ContainsKey(EnemyID))
        {

            Queue<Enemy> RefrencedQueue = EnemyObjectPools[EnemyID];

            if (RefrencedQueue.Count > 0)
            {
                //deque
                SummonedEnemy = RefrencedQueue.Dequeue();
                SummonedEnemy.Init();

                SummonedEnemy.gameObject.SetActive(true);
            }
            else
            {
                //intiating
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], GameloopManager.NodePositions[0], Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log("Does not exis {EnemyID}");
            return null;
        }

        EnemiesInGame.Add(SummonedEnemy);
         EnemiesInGameTransform.Add(SummonedEnemy.transform);
        SummonedEnemy.ID = EnemyID;
        return SummonedEnemy;
    }

   /// <summary>
   /// To intiate removing enemy once health is 0
   /// </summary>
   /// <param name="EnemyToRemove"></param>
   public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
        EnemiesInGameTransform.Remove(EnemyToRemove.transform);
        EnemiesInGame.Remove(EnemyToRemove);

    }
}
