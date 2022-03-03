using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameloopManager : MonoBehaviour
{
    // Start is called before the first frame update
    /** \To check the summoned enemy in game */
    private static Queue<int> EnemyIDsToSummon;
    /** \To know about game end or wave end */
    public bool Loopshouldend;
   
    private void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        Entitysummoner.Init();

        StartCoroutine(Gameloop());
        InvokeRepeating("SummonTest", 0f, 1f);
        //InvokeRepeating("RemoveTest", 0f, 0.5f);

    }
    //void RemoveTest()
    //{
     //   if (Entitysummoner.EnemiesInGame.Count > 0)
     //   {
     //       Entitysummoner.RemoveEnemy(Entitysummoner.EnemiesInGame[Random.Range(0, Entitysummoner.EnemiesInGame.Count)]);
     //   }
   // }

    /// <summary>
    /// Enemy summon test case to summon one slime ID 1
    /// </summary>
    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
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
            //tick towers

            //Apply effects


            //Damage enemies
            //remove enemies
            //remove towers
            yield return null;

        }
    }

    /// <summary>
    /// Quesuing enemies to spawn in game using their ID to summon
    /// </summary>
    /// <param name="ID"></param>

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }
}
