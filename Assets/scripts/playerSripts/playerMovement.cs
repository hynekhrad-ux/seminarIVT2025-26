using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform headTransform;

    private int moveSpeed = 10;
    private float gravity = -9.81f;

    private Vector3 velocity;

    public InputSystem_Actions playerControls;
    private InputAction movement;
    private InputAction jumping;

    private bool canDoubleJump = true;

    public static bool playerIsOnGround;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        movement = playerControls.Player.Move;
        movement.Enable();
        jumping = playerControls.Player.Jump;
        jumping.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        jumping.Disable();
    }

    void Update()
    {
        if (gameManager.gameEnd)
            return;

        Vector2 moveDirection = movement.ReadValue<Vector2>();

        Vector3 camForward = headTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = headTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = camForward * moveDirection.y + camRight * moveDirection.x;

        if (controller.isGrounded && velocity.y < 0)
        {
            playerIsOnGround =true;
            canDoubleJump = true;
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        if (controller.isGrounded && jumping.WasPressedThisFrame())
        { 
            velocity.y = Mathf.Sqrt(1 * -2f * gravity);
            playerIsOnGround = false;
        }
        else if(!controller.isGrounded && jumping.WasPressedThisFrame() && canDoubleJump)
        {
            velocity.y = Mathf.Sqrt(1 * -2f * gravity);
            canDoubleJump = false;
        }

        controller.Move(move * moveSpeed * Time.deltaTime + new Vector3(0,1,0) * velocity.y * Time.deltaTime);
    }
}
