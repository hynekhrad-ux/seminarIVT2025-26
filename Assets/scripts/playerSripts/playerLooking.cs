using UnityEngine;
using UnityEngine.InputSystem;

public class playerLooking : MonoBehaviour
{
    public InputSystem_Actions playerControls;
    private InputAction looking;

    private Vector2 look;

    public Transform headTransform;

    private int sensitivity = 10;
    
    private float xRotation;
    
    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        looking = playerControls.Player.Look;
        looking.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDisable() 
    {
        looking.Disable();
    }

    void Update()
    {
        if (gameManager.gameEnd)
            return;

        look = looking.ReadValue<Vector2>();

        float mouseX = look.x * sensitivity * Time.deltaTime;
        float mouseY = look.y * sensitivity * Time.deltaTime;
   
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        transform.Rotate(new Vector3(0,1,0)*mouseX);
        headTransform.localRotation = Quaternion.Euler(xRotation, 0, 0f);
    }
}