using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class player : MonoBehaviour
{
    //odkay na gameObject textu zobrayujici soucasny stav munice, potreba priradit v inspectoru
    public TextMeshProUGUI TextAmmo;

    //reference na komponety typu Animator; potreba priradit v inspectoru
    public Animator gunAnimator0;
    public Animator gunAnimator1;

    //reference na GameOjecty zbrani; potreba priradit v inspectoru
    public GameObject gun0;
    public GameObject gun1;

    //reference na mapu ovladani skrze novy Unity InputSystem
    public InputSystem_Actions playerControls;

    //pro ulozeni jednoltive akce
    //zmena na zbran m1911
    private InputAction Gun0;
    //zmena na zbran M4
    private InputAction Gun1;
    
    //ID soucasne zbrane
    public int CurrentGunID = 0;

    public static int playerHealth = 100;

    //promene pro ulozeni reference ostatnich scriptu
    m1911 m1911SCR;
    playerReloading playerReloadingSCR;
    playerShooting playerShootingSCR;
    M4 M4SCR;

    //volano co nejdriv
    private void Awake()
    {
        //reference predgenerovaneho scriptu InputSystem_Actions
        playerControls = new InputSystem_Actions();

        //script "playerShooting" a "player" je pripojen ke stejnemu objektu jako tento script proto GetComponent<>()  <- odkazuje se pouze na na komponety pripojeny pouze k tomotu GameObjectu
        playerReloadingSCR = GetComponent<playerReloading>();
        playerShootingSCR = GetComponent<playerShooting>();
        //GetComponentInChildren<>() velmi podobne jako ^^^^^ jenom hleda v componety v objektech podrazenych tomuto objektu
        m1911SCR = GetComponentInChildren<m1911>();
        M4SCR = GetComponentInChildren<M4>();
    }
    //volano kdyz je povoleno pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnEnable()
    {
        //nastaveni mistni promene shooting na jiz namapovanou akci v unity editoru; vice info v dokumentaci
        Gun0 = playerControls.Player.Gun0;
        Gun1 = playerControls.Player.Gun1;

        //aktivace akce
        Gun0.Enable();
        Gun1.Enable();
    }
    //volano kdyz je zakazano pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnDisable()
    {
        //deaktivace akce
        Gun0.Disable();
        Gun1.Disable();
    }

    //volano prvni frame od incializace
    private void Start()
    {
        //nastaveni textu soucasneho munice
        TextAmmo.text = m1911SCR.currentAmmoID0.ToString() + "/" + m1911SCR.maxAmmoID0.ToString();

        //vypnuti animatoru zbrane M4
        gunAnimator1.enabled = false;
    }

    //funkce volana kazdy frame
    void Update()
    {
        //kontrola promene jestli jiz neskocila hra
        if (gameManager.gameEnd)
        {
            //vypnuti animatoru zbrani
            gunAnimator0.enabled = false;
            gunAnimator1.enabled=false;
            //opusteni funkce Update()
            return;
        }

        //pokud byla zbran prebijena a byla tato akci jiz dokoncena tak nastavit text soucasne munice na aktualni stav
        if (playerReloadingSCR.timestampReload < Time.time && CurrentGunID == 0)
        {
            TextAmmo.text = m1911SCR.currentAmmoID0.ToString() + "/" + m1911SCR.maxAmmoID0.ToString();
        }
        if (playerReloadingSCR.timestampReload < Time.time && CurrentGunID == 1)
        {
            TextAmmo.text = M4SCR.currentAmmoID1.ToString() + "/" + M4SCR.maxAmmoID1.ToString();
        }

        //zvoleni zbrane ID0 (m1911)
        if (Gun0.WasPressedThisFrame() && (playerShootingSCR.canShootID1 || M4SCR.currentAmmoID1 == 0) && playerReloadingSCR.timestampReload < Time.time )
        {
            //zmena promene ukladajici ID soucasne zbrane
            CurrentGunID = 0;

            //rotace a pohyb zbrani aby sla videt nove zvolena zbran a nesla videt stara
            gun0.transform.localPosition = new Vector3(0, 0, 0);
            gun0.transform.localRotation = Quaternion.Euler(0, -180, 0);

            gun1.transform.localPosition = new Vector3(0, -1, 0);
            gun1.transform.localRotation = Quaternion.Euler(-60, -180, 0);

            //prepnuti animatoru
            gunAnimator0.enabled = true;
            gunAnimator1.enabled = false;
            //update textu munice
            TextAmmo.text = m1911SCR.currentAmmoID0.ToString() + "/" + m1911SCR.maxAmmoID0.ToString();
        }
        if (Gun1.WasPressedThisFrame() && (playerShootingSCR.canShootID0 || m1911SCR.currentAmmoID0 == 0) && playerReloadingSCR.timestampReload < Time.time )
        {
            //zmena promene ukladajici ID soucasne zbrane
            CurrentGunID = 1;

            //rotace a pohyb zbrani aby sla videt nove zvolena zbran a nesla videt stara
            gun0.transform.localPosition = new Vector3(0, -1, 0);
            gun0.transform.localRotation = Quaternion.Euler(-60, -180, 0);

            gun1.transform.localPosition = new Vector3(0.1f, 0, 0);
            gun1.transform.localRotation = Quaternion.Euler(0, -180, 0);

            //prepnuti animatoru
            gunAnimator0.enabled = false;
            gunAnimator1.enabled = true;
            //update textu munice
            TextAmmo.text = M4SCR.currentAmmoID1.ToString() + "/" + M4SCR.maxAmmoID1.ToString();
        }
    }
    
    public static void damagePlayer(int damage)
    {
        playerHealth -= damage;
        Debug.Log(playerHealth);

        if (playerHealth <= 0)
        {
            gameManager.gameEnd =true;
        }
    }
}