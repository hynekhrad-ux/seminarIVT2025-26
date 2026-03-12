using UnityEngine;
using UnityEngine.InputSystem;

public class playerReloading : MonoBehaviour
{
    public InputSystem_Actions playerControls;
    public Animator gunAnimator0;
    public Animator gunAnimator1;

    private InputAction reloading;

    public float timestampReload;

    public bool canReload = true;

    player playerSCR;
    m1911 m1911SCR;
    M4 M4SCR;
    playerShooting playerShootingSCR;

    private void Awake()
    {
        playerSCR = GetComponent<player>();
        m1911SCR = GetComponentInChildren<m1911>();
        M4SCR = GetComponentInChildren<M4>();
        playerShootingSCR = GetComponent<playerShooting>();

        playerControls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        reloading = playerControls.Player.Reload;

        reloading.Enable();
    }
    private void OnDisable()
    {
        reloading.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameEnd)
            return;

        if (reloading.WasPressedThisFrame() && Time.time > timestampReload)
        {
            if (m1911SCR.currentAmmoID0 != 0 && playerSCR.CurrentGunID == 0)
            {
                timestampReload = Time.time + m1911SCR.reloadTimeSimpleID0;

                m1911SCR.currentAmmoID0 = m1911SCR.maxAmmoID0 + 1;

                gunAnimator0.SetTrigger("ReloadSimple");

                canReload = false;
                playerShootingSCR.canShootID0 = false;
            }
            else if (playerSCR.CurrentGunID == 0)
            {
                timestampReload = Time.time + m1911SCR.reloadTimeZeroID0;

                gunAnimator0.SetTrigger("ReloadZero");

                m1911SCR.currentAmmoID0 = m1911SCR.maxAmmoID0;

                playerShootingSCR.canShootID0 = false;
            }
            
            if (M4SCR.currentAmmoID1 != 0 && playerSCR.CurrentGunID == 1)
            {
                timestampReload = Time.time + M4SCR.realoadTimeSimpleID1;

                M4SCR.currentAmmoID1 = M4SCR.maxAmmoID1 + 1;

                gunAnimator1.SetTrigger("ReloadSimple");

                canReload = false;
            }
            else if (playerSCR.CurrentGunID == 1)
            {
                timestampReload = Time.time + M4SCR.reloadTimeZeroID1;

                M4SCR.currentAmmoID1 = M4SCR.maxAmmoID1;

                gunAnimator1.SetTrigger("ReloadZero");
            }
            
        }
    }
}
