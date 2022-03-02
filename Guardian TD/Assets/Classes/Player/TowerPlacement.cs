using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class for placing tower using a button
/// </summary>
public class TowerPlacement : MonoBehaviour
{
    /** \masking the tower to not collide with other objects */
    [SerializeField] private LayerMask PlacementColliderMask;
    /** \To attach tower to player camera */
    [SerializeField] private Camera PlayerCamera;
    /** \To check the summoned tower in game */
    private GameObject CurrentPlacingTower;
    // Start is called before the first frame update


    //  void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (CurrentPlacingTower != null)
        {
            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hitinfo;
            if (Physics.Raycast(camray, out RaycastHit hitInfo, 100f))
            {
                CurrentPlacingTower.transform.position = hitInfo.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
               // if (hitInfo.collider.gameObject.CompareTag("CantPlace"))
                //{
                    CurrentPlacingTower = null;
                //}
                
            }
        }
    }


    /// <summary>
    /// setting tower to place on map
    /// </summary>
    /// <param name="tower"> this is tower that is spawned in game</param>
    public void SetTowerToPlace(GameObject tower)
    {
        CurrentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }
}
