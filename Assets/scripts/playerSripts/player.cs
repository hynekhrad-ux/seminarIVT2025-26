using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class player : MonoBehaviour
{
    public TextMeshProUGUI TextAmmo;

    public Animator gunAnimator0;
    public Animator gunAnimator1;   

    public GameObject gun0;
    public GameObject gun1;

    public InputSystem_Actions playerControls;
  
    private InputAction Gun0;
    private InputAction Gun1;
    
    public int CurrentGunID = 0;
    /*
    private float recoilX;
    
    private float elapsedTimeInRecoil;

    private Quaternion startRotationRecoil;
    private Quaternion endRotationRecoil;


    private float timestampReaload;
    private float timestampFireRate;

    private bool isRecoiling = false;
    */
    m1911 m1911SCR;
    playerReloading playerReloadingSCR;
    playerShooting playerShootingSCR;
    M4 M4SCR;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

        m1911SCR = GetComponentInChildren<m1911>();
        M4SCR = GetComponentInChildren<M4>();
        playerReloadingSCR = GetComponent<playerReloading>();
        playerShootingSCR = GetComponent<playerShooting>();


    }
    private void OnEnable()
    {        
        Gun0 = playerControls.Player.Gun0;
        Gun1 = playerControls.Player.Gun1;

        Gun0.Enable();
        Gun1.Enable();
    }
    private void OnDisable()
    {
        Gun0.Disable();
        Gun1.Disable();
    }

    private void Start()
    {
        TextAmmo.text = m1911SCR.currentAmmoID0.ToString() + "/" + m1911SCR.maxAmmoID0.ToString();

        gunAnimator1.enabled = false;
    }

    void Update()
    {
        if (gameManager.gameEnd)
        {
            gunAnimator0.enabled = false;
            gunAnimator1.enabled=false;
            return;
        }

        if (playerReloadingSCR.timestampReload < Time.time && CurrentGunID == 0)
        {
            TextAmmo.text = m1911SCR.currentAmmoID0.ToString() + "/" + m1911SCR.maxAmmoID0.ToString();
        }
        if (playerReloadingSCR.timestampReload < Time.time && CurrentGunID == 1)
        {
            TextAmmo.text = M4SCR.currentAmmoID1.ToString() + "/" + M4SCR.maxAmmoID1.ToString();
        }



        if (Gun0.WasPressedThisFrame() && (playerShootingSCR.canShootID1 || M4SCR.currentAmmoID1 == 0) && playerReloadingSCR.timestampReload < Time.time )
        {
            CurrentGunID = 0;

            gun0.transform.localPosition = new Vector3(0, 0, 0);
            gun0.transform.localRotation = Quaternion.Euler(0, -180, 0);

            gun1.transform.localPosition = new Vector3(0, -1, 0);
            gun1.transform.localRotation = Quaternion.Euler(-60, -180, 0);

            gunAnimator0.enabled = true;
            gunAnimator1.enabled = false;
            TextAmmo.text = m1911SCR.currentAmmoID0.ToString() + "/" + m1911SCR.maxAmmoID0.ToString();
        }
        if (Gun1.WasPressedThisFrame() && (playerShootingSCR.canShootID0 || m1911SCR.currentAmmoID0 == 0) && playerReloadingSCR.timestampReload < Time.time )
        {
            CurrentGunID = 1;

            gun0.transform.localPosition = new Vector3(0, -1, 0);
            gun0.transform.localRotation = Quaternion.Euler(-60, -180, 0);

            gun1.transform.localPosition = new Vector3(0.1f, 0, 0);
            gun1.transform.localRotation = Quaternion.Euler(0, -180, 0);

            gunAnimator0.enabled = false;
            gunAnimator1.enabled = true;
            TextAmmo.text = M4SCR.currentAmmoID1.ToString() + "/" + M4SCR.maxAmmoID1.ToString();
        }

    }

    /*
    void startRecoil()
    {
        recoilX = Random.Range(-2f, -4f);
        elapsedTimeInRecoil = 0f;

        startRotationRecoil = recoilHandler.localRotation;

        endRotationRecoil = startRotationRecoil * Quaternion.Euler(recoilX, Random.Range(-0.5f, 0.5f),0f);

        isRecoiling = true;
    }
    void handleRecoli()
    {
        if (!isRecoiling)
            return;

        elapsedTimeInRecoil += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTimeInRecoil / 0.1f);

        recoilHandler.localRotation = Quaternion.Slerp(startRotationRecoil, endRotationRecoil, t);

        if (t >= 1f)
            isRecoiling = false;

        headTransform.localRotation =headTransform.localRotation * recoilHandler.localRotation;
        recoilHandler.localRotation = Quaternion.Euler(0f, 0f, 0f);
        recoilX =0; 
    }
    */

}