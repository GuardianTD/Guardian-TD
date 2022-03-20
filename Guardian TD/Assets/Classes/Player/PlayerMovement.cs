using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Player movement class added to move around in map in all four directions
/// 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    /** \vemocity of turning around a fix point */
    private Vector3 Velocity;
    /** \WSAD keys for player input */
    private Vector3 PlayerMovementInput;
    /** \mouse input for rotation */
    private Vector2 PlayerMouseInput;
    /** \to plot precise movements*/
    private float xRot;

    /** \Adding camera to player for FPP */
    [SerializeField] private Transform PlayerCamera;
    /** \Adding chracter controller to camera */
    [SerializeField] private CharacterController Controller;
    [Space]
    /** \adding speed of movement */
    [SerializeField] private float Speed;
    /** \sentivity of moving around the mouse */
    [SerializeField] private float Sensitivity;


    private void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
    }
    /// <summary>
    /// Player movement module using getkey funcntion 
    /// </summary>
    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        if (Input.GetKey(KeyCode.Space))
        {

            Velocity.y = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Velocity.y = -1f;
        }

        Controller.Move(MoveVector * Speed * Time.deltaTime);
        Controller.Move(Velocity * Speed * Time.deltaTime);

        Velocity.y = 0f;
    }

    /// <summary>
    /// Moving camera using mouse input to get X and Y rotation
    /// </summary>
    private void MovePlayerCamera()
    {
        if (Input.GetMouseButton(1))
        {
            xRot -= PlayerMouseInput.y * Sensitivity;

            transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
            PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }
    }
}
