using UnityEngine;
using UnityEngine.InputSystem;

public class playerLooking : MonoBehaviour
{
    //reference na mapu ovladani skrze novy Unity InputSystem
    public InputSystem_Actions playerControls;
    //pro ulozeni jednoltive akce
    private InputAction looking;

    //promena na ukladani hracova inputu
    private Vector2 look;

    //transform hlavy hrace
    public Transform headTransform;

    //citilivost mysi
    private int sensitivity = 10;
    
    //promena pro pomocne vypocty rotace hlavy
    private float xRotation;

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
        looking = playerControls.Player.Look;
        //aktivace akce
        looking.Enable();

        //zamknuti pohybu kurzoru
        Cursor.lockState = CursorLockMode.Locked;
        //znevidetlelneni kurzoru
        Cursor.visible = false;
    }
    //volano kdyz je zakazano pouzivat tento script; zde nevyuzito; jedna se o konvenci
    private void OnDisable() 
    {
        //deaktivace akce
        looking.Disable();
    }

    //funkce volana kazdy frame
    void Update()
    {
        //kontrola promene jestli jiz neskocila hra
        if (gameManager.gameEnd)
            //opusteni funkce Update()
            return;

        //cteni inputu hrace; ukladano jako delta pohybu mysi za posledni frame
        look = looking.ReadValue<Vector2>();

        //nasobeni inputu citlyvosti a Time.deltaTime;
        float mouseX = look.x * sensitivity * Time.deltaTime;
        float mouseY = look.y * sensitivity * Time.deltaTime;
   
        //nastaveni mezipromene
        xRotation -= mouseY;
        //Clamp() nastaveni dolniho a horniho limitu jiste promene v tomto pripadu xRotation
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        //otoceni tela hrace kolem osy Y 
        transform.Rotate(new Vector3(0,1,0)*mouseX);
        //otoceni hlavy hrace kolem osy X
        headTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}