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

    [SerializeField] private LayerMask PlacementCheckMask;
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
            RaycastHit HitInfo;
            if (Physics.Raycast(camray, out HitInfo, 100f))
            {
                CurrentPlacingTower.transform.position = HitInfo.point;
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                Destroy(CurrentPlacingTower);
                CurrentPlacingTower = null;
                return;
            }

          
            if (Input.GetMouseButtonDown(0) && HitInfo.collider.gameObject != null)
            {
              if (!HitInfo.collider.gameObject.CompareTag("CantPlace"))
                {
                   BoxCollider TowerCollider = CurrentPlacingTower.gameObject.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;
                    
                    
                   Vector3 BoxCenter = CurrentPlacingTower.gameObject.transform.position + TowerCollider.center;
                   Vector3 HalfExtents = TowerCollider.size / 2;
                if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlacementCheckMask, QueryTriggerInteraction.Ignore))
              {
                        GameloopManager.TowersInGame.Add(CurrentPlacingTower.GetComponent<TowerBehavior>());
                        TowerCollider.isTrigger = false;
                
                        CurrentPlacingTower = null;
                 }
             }
                
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
