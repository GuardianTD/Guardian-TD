using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class created to place enemies on map forcefully
/// </summary>
public class Enemy_placer : MonoBehaviour
{
    /** \Collider mask so eneies wont overlap */
    [SerializeField] private LayerMask PlacementColliderMask;
    /** \To Attach enemies to player camera and drag them to fit location for spawn */
    [SerializeField] private Camera PlayerCamera;
    /** \To check the summoned enemy in game */
    private GameObject CurrentPlacingEnemy;
    // Start is called before the first frame update


    //  void Start()
    //{

    //}

    // Update is called once per frame
   
    void Update()
    {
        if (CurrentPlacingEnemy != null)
        {
            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hitinfo;
            if (Physics.Raycast(camray, out RaycastHit hitInfo, 100f))
            {
                CurrentPlacingEnemy.transform.position = hitInfo.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                // if (hitInfo.collider.gameObject.CompareTag("CantPlace"))
                //{
                CurrentPlacingEnemy = null;
                //}

            }
        }
    }

    /// <summary>
    /// to place enemies in game same as set tower function
    /// </summary>
    /// <param name="SlimePBR"> this obejct os prefab of slime</param>

    public void SetTowerToPlace(GameObject SlimePBR)
    {
        CurrentPlacingEnemy = Instantiate(SlimePBR, Vector3.zero, Quaternion.identity);
    }
}
