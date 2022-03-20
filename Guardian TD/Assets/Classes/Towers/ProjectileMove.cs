using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// projectile move behaviour
/// </summary>
public class ProjectileMove : MonoBehaviour
{
    /** \set spee of projectile */
    public float speed;
    /** \set firerate of tower */
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No speed");
        }
    }
}
