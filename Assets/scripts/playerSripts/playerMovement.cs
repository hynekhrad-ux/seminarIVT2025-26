using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    //reference na komponet typu CharacterController; potreba priradit v inspectoru
    public CharacterController controller;

    //reference na Transform Hlavy
    public Transform headTransform;

    //nastaveni rychlosti pohybu
    private int moveSpeed = 10;
    //nastaveni sily gravitace
    private float gravity = -9.81f;

    //3D vector promena pro vypocty gravitace 
    private Vector3 velocity;

    //reference na mapu ovladani skrze novy Unity InputSystem
    public InputSystem_Actions playerControls;

    //pro ulozeni jednoltive akce
    private InputAction movement;
    private InputAction jumping;

    //bool jestli je hrac schopen doubleJumpu
    private bool canDoubleJump = true;

    //verejna promena kontrolujici jestli je hrac na zemi
    public static bool playerIsOnGround;

    //volano co nejdriv
    private void Awake()
    {
        //reference predgenerovaneho scriptu InputSystem_Actions
        playerControls = new InputSystem_Actions();
    }

    //volano kdyz je povoleno pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnEnable()
    {
        //nastaveni mistni promene shooting na jiz namapovanou akci v unity editoru; vice info v dokumentaci
        movement = playerControls.Player.Move;
        //aktivace akce
        movement.Enable();
        //nastaveni mistni promene shooting na jiz namapovanou akci v unity editoru; vice info v dokumentaci
        jumping = playerControls.Player.Jump;
        //aktivace akce
        jumping.Enable();
    }
    //volano kdyz je zakazano pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnDisable()
    {
        //deaktivace akce
        movement.Disable();
        //deaktivace akce
        jumping.Disable();
    }

    //funkce volana kazdy frame
    void Update()
    {
        //kontrola promene jestli jiz neskocila hra
        if (gameManager.gameEnd)
            //opusteni funkce Update()
            return;

        //cteni inputu hrace a ukladani do moveDirection; opatrne ukldano jako X,Y i kdyz v kontextu se jenda o svetove souradnice X,Z
        Vector2 moveDirection = movement.ReadValue<Vector2>();

        //jakym smerem je dopredu vuci rotaci hlavy
        Vector3 camForward = headTransform.forward;
        //osa y vynulovana; zde nepotrebna
        camForward.y = 0f;
        //normalizace vektoru; delka vekotoru bude 1; zachovany smer
        camForward.Normalize();

        //jakym smerem je doprava vuci rotaci hlavy
        Vector3 camRight = headTransform.right;
        //osa y vynulovana; zde nepotrebna
        camRight.y = 0f;
        //normalizace vektoru; delka vekotoru bude 1; zachovany smer
        camRight.Normalize();

        //vypocet finalniho smeru pohybu
        Vector3 move = camForward * moveDirection.y + camRight * moveDirection.x;

        //jestlize je hrac na zemi
        if (controller.isGrounded && velocity.y < 0)
        {
            //nastaveni promenych potrebnych pro skok
            playerIsOnGround =true;
            canDoubleJump = true;
            //nastaveni rychlosti dolu pro lepsi detekci kolizi se zemi
            velocity.y = -2f;
        }

        //aplikovani gravitace na hrace
        velocity.y += gravity * Time.deltaTime;

        //skok
        if (controller.isGrounded && jumping.WasPressedThisFrame())
        { 
            //vypocet pocatecni rychlosti pri skoku pomoci torriceliho vzore    v = (2 * g * h)^0.5
            velocity.y = Mathf.Sqrt(-2f * gravity * 1);
            
            playerIsOnGround = false;
        }
        //druhy skok ve vzduchu
        else if(!controller.isGrounded && jumping.WasPressedThisFrame() && canDoubleJump)
        {
            //vypocet pocatecni rychlosti pri skoku pomoci torriceliho vzore    v = (2 * g * h)^0.5
            velocity.y = Mathf.Sqrt( -2f * gravity);

            canDoubleJump = false;
        }
        //posunuti hrace pomoci funkce Move(); vyuziti vsech veci ktery byly postupne pres cely tenhle script pocitany; Time.deltaTime (ubehly cas mezi jednotlivimy snimky) umoznuje konzisteni rycholst pohybu bez ohledu na pocet snimku za sekundu
        controller.Move(move * moveSpeed * Time.deltaTime + new Vector3(0,1,0) * velocity.y * Time.deltaTime);
    }
}