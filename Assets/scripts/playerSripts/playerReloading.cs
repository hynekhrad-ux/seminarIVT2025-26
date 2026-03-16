using UnityEngine;
using UnityEngine.InputSystem;

public class playerReloading : MonoBehaviour
{
    //reference na mapu ovladani skrze novy Unity InputSystem
    public InputSystem_Actions playerControls;

    //reference na komponety typu Animator; potreba priradit v inspectoru
    public Animator gunAnimator0;
    public Animator gunAnimator1;

    //pro ulozeni jednoltive akce
    private InputAction reloading;

    //promena pro ulozeni casu do konce prebijeni
    public float timestampReload;

    //promena pro kontrolu zda li je mozen prebijet
    public bool canReload = true;

    //promene pro ulozeni reference ostatnich scriptu
    player playerSCR;
    m1911 m1911SCR;
    M4 M4SCR;
    playerShooting playerShootingSCR;

    //volano co nejdriv
    private void Awake()
    {
        //script "playerShooting" a "player" je pripojen ke stejnemu objektu jako tento script proto GetComponent<>()  <- odkazuje se pouze na na komponety pripojeny pouze k tomotu GameObjectu
        playerShootingSCR = GetComponent<playerShooting>();
        playerSCR = GetComponent<player>();
        //GetComponentInChildren<>() velmi podobne jako ^^^^^ jenom hleda v componety v objektech podrazenych tomuto objektu
        m1911SCR = GetComponentInChildren<m1911>();
        M4SCR = GetComponentInChildren<M4>();
        

        //reference predgenerovaneho scriptu InputSystem_Actions
        playerControls = new InputSystem_Actions();
    }

    //volano kdyz je povoleno pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnEnable()
    {
        //nastaveni mistni promene shooting na jiz namapovanou akci v unity editoru; vice info v dokumentaci
        reloading = playerControls.Player.Reload;
        //aktivace akce
        reloading.Enable();
    }
    //volano kdyz je zakazano pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnDisable()
    {
        //deaktivace akce
        reloading.Disable();
    }

    //funkce volana kazdy frame
    void Update()
    {
        //kontrola promene jestli jiz neskocila hra
        if (gameManager.gameEnd)
            //opusteni funkce Update()
            return;

        //pokud hrac zmackl tlacitko prebijeni a ubehlo dost casu od posledniho prebiti
        if (reloading.WasPressedThisFrame() && Time.time > timestampReload)
        {
            //rozhodnuti jestli je naboj nabity v komore, podle toho pridani munice; pro gunID0 (m1911)
            if (m1911SCR.currentAmmoID0 != 0 && playerSCR.CurrentGunID == 0 && !(m1911SCR.currentAmmoID0>m1911SCR.maxAmmoID0))
            {
                //naboj byl v komore; cas prebijeni je kratsi
                //nastaveni casu kdy skonci prebijeni
                timestampReload = Time.time + m1911SCR.reloadTimeSimpleID0;
                //nastaveni munice
                m1911SCR.currentAmmoID0 = m1911SCR.maxAmmoID0 + 1;

                //spusteni animace
                gunAnimator0.SetTrigger("ReloadSimple");
                //zakaz strelby behem prebijeni; povoleni v scriptu m1911 (fuckass spagheti code)
                playerShootingSCR.canShootID0 = false;
            }
            //naboj neni v komore
            else if (playerSCR.CurrentGunID == 0 && !(m1911SCR.currentAmmoID0 > m1911SCR.maxAmmoID0))
            {
                //delsi doba prebijeni; mensi zasoba munice
                //nastaveni casu kdy skonci prebijeni
                timestampReload = Time.time + m1911SCR.reloadTimeZeroID0;
                //nastaveni munice
                m1911SCR.currentAmmoID0 = m1911SCR.maxAmmoID0;

                //spusteni animace
                gunAnimator0.SetTrigger("ReloadZero");
                //zakaz strelby behem prebijeni; povoleni v scriptu m1911
                playerShootingSCR.canShootID0 = false;
            }
            //rozhodnuti jestli je naboj nabity v komore, podle toho pridani munice; pro gunID1 (M4)
            if (M4SCR.currentAmmoID1 != 0 && playerSCR.CurrentGunID == 1)
            {
                //naboj byl v komore; cas prebijeni je kratsi
                //nastaveni casu kdy skonci prebijeni
                timestampReload = Time.time + M4SCR.realoadTimeSimpleID1;
                //nastaveni munice
                M4SCR.currentAmmoID1 = M4SCR.maxAmmoID1 + 1;

                //spusteni animace
                gunAnimator1.SetTrigger("ReloadSimple");
                //zakaz strelby behem prebijeni; povoleni v scriptu M4
                playerShootingSCR.canShootID1= false;

            }
            else if (playerSCR.CurrentGunID == 1)
            {
                //naboj byl v komore; cas prebijeni je kratsi
                //nastaveni casu kdy skonci prebijeni
                timestampReload = Time.time + M4SCR.reloadTimeZeroID1;
                //nastaveni munice
                M4SCR.currentAmmoID1 = M4SCR.maxAmmoID1;

                //spusteni animace
                gunAnimator1.SetTrigger("ReloadZero");
                //zakaz strelby behem prebijeni; povoleni v scriptu M4
                playerShootingSCR.canShootID1= false;
            }
        }
    }
}
