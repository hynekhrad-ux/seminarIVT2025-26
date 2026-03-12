using UnityEngine;
using UnityEngine.InputSystem;

public class playerShooting : MonoBehaviour
{
    public InputSystem_Actions playerControls;
    private InputAction shooting;

    private int currentGunID = 0;

    private player playerScript;

    public bool canShootID0 = true;
    public bool canShootID1 = true;

    private bool isShooting = false;

    

    m1911 m1911SCR;
    M4 M4SCR;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

        playerScript = GetComponent<player>();
        m1911SCR = GetComponentInChildren<m1911>();
        M4SCR = GetComponentInChildren<M4>();
    }
    private void OnEnable()
    {
        shooting = playerControls.Player.Attack;
        shooting.Enable(); 
    }
    private void OnDisable()
    {
        shooting.Disable();
    }
    
    void Update()
    {
        if (gameManager.gameEnd)
            return;

        currentGunID = playerScript.CurrentGunID;

        if (canShootID0 && shooting.WasPressedThisFrame() && currentGunID == 0)
        {
            // v jednotlivych scriptech fireRate tam odkaz na recoil
           
            m1911SCR.fireID0();
            Debug.Log("0");
        }
        if (shooting.WasPressedThisFrame())
        {
            isShooting=true;
        }
        if (shooting.WasReleasedThisFrame())
        {
            isShooting = false; 
        }
        if (isShooting && canShootID1 && currentGunID == 1)
        {
            M4SCR.fireID1();
            Debug.Log("1");
        }
    }
}