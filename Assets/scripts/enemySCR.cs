using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class enemySCR : MonoBehaviour
{
    //promena pro ukladani zivotu; zvlast pro kazdy gameObject s timto scriptem prirazenym v inspectoru
    public int enyHealth;

    //globalni promena; kontrola zda posledni zasah do nepritele byl do hlavy; pro vypocet skore
    public static bool wasLastHitHeadshot;

    //funkce Awake() volana driv nez Start(); zde umoznuje nastaveni zivotu nepratelu drive nez se stihne nastavit gameManager.roundNumber; take volano ve stejnou chvili jako Instantiate() pro nepratele, ktery byl zavolan v gameManager.SpawnArena()
    private void Awake()
    {
        //nastaveni zivotu nepitele; vice zivotu podle soucasneho kola 
        enyHealth = 100 + (gameManager.roundNumber * 10);
    }

    //funkce pro udeleni poskozeni nepratelum; volana z jinych scriptu
    public void Damage(int damage)
    {
        //odecteni zivotu podle poskozeni dane zbrane + plus pripadny bonus za trefu do hlavy
        enyHealth -= damage;

        //logika pro smrt nepritele
        if(enyHealth <= 0)
        {
            //odecteni z pocitadla celkoveho poctu nepratel zbivajicich v kole
            gameManager.enyCount--;

            //jesltze posldeni strela terfila hlavu...
            if ( wasLastHitHeadshot )
            {
                //...a hrac byl ve vzduchu...
                if (!playerMovement.playerIsOnGround)
                {
                    //...pricitam +50 za kazdy specialni ukon; proto misto standrtniho +100 sokore +200
                    hudSCR.score += 200;
                }
                else
                {
                    //zde pokud hrac trefil hlavu ale ja na zemi
                    hudSCR.score += 150;
                }
            }
            else
            {
                if(!playerMovement.playerIsOnGround)
                {
                    //zde pri trefe do tela ve vzduch
                    hudSCR.score += 150;
                }
                else
                {
                    //zde trefa do tela na zemi
                    hudSCR.score += 100; 
                }
            }
            //smazani objektu nepritele
            Destroy(gameObject);
        }
    }
}
