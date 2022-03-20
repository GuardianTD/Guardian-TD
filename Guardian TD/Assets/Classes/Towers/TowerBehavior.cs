using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// setting behaviour of tower yet to be decided
/// </summary>
public class TowerBehavior : MonoBehaviour
{

    public LayerMask EnemiesLayer;


    public Enemy Target;
    public Transform Crystal;

    public float Damage;
    public float Firerate;
    public float Range;
    public int towerID;

    private float Delay;


    private IDamageMethod CurrentDamageMethodClass;
    // Start is called before the first frame update
    void Start()
    {
        CurrentDamageMethodClass = GetComponent<IDamageMethod>();

        if(CurrentDamageMethodClass == null)
        {
            Debug.LogError("Towers: NO DAMGE CLAASS ATACHED TO GIVEN TOWER");
        }

        else
        {
            CurrentDamageMethodClass.Init(Damage, Firerate);
        }
        Delay = 1 / Firerate;
    }

    public void Tick()
    {
        CurrentDamageMethodClass.DamageTick(Target);
        if(Target != null)
        {
            Crystal.transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        }
    }
}
