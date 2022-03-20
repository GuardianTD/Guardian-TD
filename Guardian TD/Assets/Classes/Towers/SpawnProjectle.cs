using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// spawing projectile on head of tower 
/// </summary>
public class SpawnProjectle : MonoBehaviour
{
    // Start is called before the first frame update
    /** \set tower crystal as firepoint */
    public GameObject firePoint;
    /** \creating list of Vfx that are used to spawn a projectile */
    public List<GameObject> vfx = new List<GameObject> ();

    /** \to set effect of spawining on crystal of tower */
    private GameObject effectToSpawn;
    void Start()
    {
        effectToSpawn = vfx  [0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            SpawnVFX();
        }
    }
    /// <summary>
    /// spawning vfx 
    /// </summary>
   void SpawnVFX()
    {
        GameObject vfx;
        if(firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("no fire point");
        }
    }
}
