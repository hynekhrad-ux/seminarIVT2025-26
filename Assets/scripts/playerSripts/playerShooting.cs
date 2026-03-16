using UnityEngine;
using UnityEngine.InputSystem;

public class playerShooting : MonoBehaviour
{
    //reference na mapu ovladani skrze novy Unity InputSystem
    public InputSystem_Actions playerControls;

    //pro ulozeni jednoltive akce
    private InputAction shooting;

    //lokalni promena pro ukladani toho jaka zbran je soucasne vybrana
    private int currentGunID = 0;

    //bool promene menene v ostatnich scriptech napr. playerReloading

    //pro m1911
    public bool canShootID0 = true;
    //pro M4
    public bool canShootID1 = true;

    //promena pro ulozeni soucasneho stavu stisku tlacitka; je drzeno/je pusteno
    private bool isShooting = false;

    //promene pro ulozeni reference ostatnich scriptu
    player playerSCR;
    m1911 m1911SCR;
    M4 M4SCR;

    //volano co nejdriv
    private void Awake()
    {
        //reference predgenerovaneho scriptu InputSystem_Actions
        playerControls = new InputSystem_Actions();

        //reference ostatnich scriptu ulozeny do mistnich promenych
        //script "player" je pripojen ke stejnemu objektu jako tento script proto GetComponent<>()  <- odkazuje se pouze na na komponety pripojeny pouze k tomotu GameObjectu
        playerSCR = GetComponent<player>();
        //GetComponentInChildren<>() velmi podobne jako ^^^^^ jenom hleda v componety v objektech podrazenych tomuto objektu
        m1911SCR = GetComponentInChildren<m1911>();
        M4SCR = GetComponentInChildren<M4>();
    }

    //volano kdyz je povoleno pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnEnable()
    {
        //nastaveni mistni promene shooting na jiz namapovanou akci v unity editoru; vice info v dokumentaci
        shooting = playerControls.Player.Attack;
        //aktivace akce
        shooting.Enable(); 
    }
    //volano kdyz je zakazano pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnDisable()
    {
        //deaktivace akce
        shooting.Disable();
    }
    
    //funkce volana kazdy frame
    void Update()
    {
        //kontrola promene jestli jiz neskocila hra
        if (gameManager.gameEnd)
            //opusteni funkce Update()
            return;

        //nastaveni lokalni promeny pomoci globalni
        currentGunID = playerSCR.CurrentGunID;

        //jestlize vybrana zbran m1911...
        if (currentGunID == 0)
        {
            //...a muzu strilet a akce prirazena k shooting byla zmacknuta tento frame
            if (canShootID0 && shooting.WasPressedThisFrame())
            {
                //zavolani funkce v jinem scriptu
                m1911SCR.fireID0();
            }
        }
        //logika pro M4 narozdil od m1911 je potreba aby byla moznost strelby full auto tj. staci drzet tlacitko
        else if (currentGunID == 1)
        {
            //tlacitko strelby bylo zmacknuto
            if (shooting.WasPressedThisFrame())
            {
                //promena isShooting je true...
                isShooting = true;
            }
            //...dokud nenastane frame kde je tlacitko pusteno
            if (shooting.WasReleasedThisFrame())
            {
                isShooting = false;
            }
            if (isShooting && canShootID1)
            {
                //volani funkce v jinem scriptu
                M4SCR.fireID1();
            }
        }
    }
}