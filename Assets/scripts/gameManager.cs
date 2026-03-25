using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    int[,] statusOfArenaTiles = new int[100, 100];
    //odkay na gameObject textu, potreba priradit v inspectoru

    //TXT cislo kola
    public TextMeshProUGUI roundNumberTXT;
    //TXT tlacitka vylepseni m1911
    public TextMeshProUGUI gunID0UpgradeCostTXT;
    //TXT tlacitka vylepseni M4
    public TextMeshProUGUI gunID1UpgradeCostTXT;
    //TXT UI cas do konce kola
    public TextMeshProUGUI timeLeftInRound;
    //TXT pole pro zobrazeni GAMEOVER
    public TextMeshProUGUI youLoseTXT;

    //promena pro ulozeni konceneho casu kola
    private float goalTime;

    //promena pro ulozeni casu kdy vypnout aplikaci
    private float shutdownTimer;

    //promena pro ulozeni nahodneho cisla pro pouziti pri generovani areny
    private int randomNum = 0;

    //odkay na gameObjecty komponety areny, potreba priradit v inspectoru

    //GameObject nepritele
    public GameObject enyPrefab;
    //zed 1
    public GameObject wall3x2;
    //zed 2
    public GameObject wall2x3;
    //sloup
    public GameObject pillar1x10;

    //prazdny gameObject prirazeny v inspectoru pro jednodusi manipulaci s vygenerovanou arenou
    public Transform arenaRoot;

    //promena cisla kola
    public static int roundNumber =0;

    //promena zacatecni ceny vylepseni m1911
    public static int gunID0UpgradeCost = 100;
    //promena zacatecni ceny vylepseni M4
    public static int gunID1UpgradeCost = 100;

    //promena pro ulozeni poctu nepratel zbivajicich v arene
    public static int enyCount;

    //promena pro vypnuti hernich funkcih
    public static bool gameEnd = false;

    //jestli se hra jiz vypina
    private bool shutingDown = false;

    //funkce Start() spustena pri prvnim framu kdy je tento skript zaktivovan
    void Start()
    {
        
        //odkaz na jinou funkci
        SpawnArena();
    }

    //vse co je uvnitr Update() je volano kazdy frame hry od spusteni
    private void Update()
    {
        //nastaveni soucasne ceny vylepseni

        //m1911
        gunID0UpgradeCostTXT.text = "m1911 - UPGRADE     COST: " + gunID0UpgradeCost;
        //M4
        gunID1UpgradeCostTXT.text = "M4 - UPGRADE     COST: " + gunID1UpgradeCost;

        //kontrola jestli kolo neskoncilo vyhrou hrace tj. destrukce vsech nepratel
        if (enyCount != 0)
        {
            //kontrola casu do konce kola (Time.time je globalni promena Unity ktera rika kolik milisekund ubehlo od spusteni hry)
            if (goalTime - Time.time > 0)
            {
                //zmena Textoveho pole 
                //.Tostring("F2") konverze float na STR se zaokrouhlenim na 2 desetina cisla
                timeLeftInRound.text = (goalTime - Time.time).ToString("F2");
            }
            //cas do konce kola bzl prekrocen; konec hry
            else if (goalTime - Time.time < 0)
            { 
                //posledni zmena textoveho pole 
                timeLeftInRound.text = "0";
                //volani funkce end()
                end();
            }
        }
    }

    public void SpawnArena()
    { 
        //dimenze areny rozdeleny do 2D pole 6x6 pro generaci prekazek
        for (int x = 5; x < 99; x +=10)
        {
            for (int z = 5; z < 99; z+=10)
            {
                //pseudonahodne cislo
                randomNum = UnityEngine.Random.Range(0, 6);

                //pseudonahodne cislo pouzite pro generaci areny (kdyz 0 tak sloup atd.)
                //Instantiate() funkce unity; urcene pro tvorbu objektu ve scene behem hry; parametry jsou GameObject, pozice, rotace a zde jeste navic pouzivam "arenaRoot" ktery je prirazen jako parent 
                //pozice pouziva nasobeni a odcitani aby sedela s realnou scenou; tesi pro poteniconali zmenu; hardcoded pro jistou velikost areny
                if (randomNum == 0)
                {
                    Instantiate(pillar1x10, new Vector3(x, 5, z ), Quaternion.identity, arenaRoot);
                    statusOfArenaTiles[x, z] = 1;
                }
                else if (randomNum == 1)
                {
                    Instantiate(wall3x2, new Vector3(x , 1.5f, z ), Quaternion.identity, arenaRoot);
                    statusOfArenaTiles[x-1, z] = 1;
                    statusOfArenaTiles[x, z] = 1;
                    statusOfArenaTiles[x+1, z] = 1;
                }
                else if (randomNum == 2)
                {
                    Instantiate(wall2x3, new Vector3(x , 1.5f, z ), Quaternion.identity, arenaRoot);
                    statusOfArenaTiles[x, z-1] = 1;
                    statusOfArenaTiles[x, z] = 1;
                    statusOfArenaTiles[x, z+1] = 1;

                }
                else if (randomNum <= 4)
                {
                    Instantiate(enyPrefab, new Vector3(x , 1, z), Quaternion.identity, arenaRoot);
                    //zvetseni promeny trakujici pocet nepratel
                    enyCount++;
                }
            }
        }

        //zvetseni promeny trakujici cislo kola
        roundNumber++;
        //update textu zobrazujici cislo kola
        roundNumberTXT.text = "Round " + roundNumber;
        
        //nastaveni ciloveho casu do dokonceni kola
        goalTime = Time.time + 60f;
    }

    public void DeleteArena()
    {
        //jak jsme predim pri generaci areny pouzivali "arenaRoot" pro ulozni jednotlivych prekazek zde jsme schopni kazdy zbivajici child objekt smazat
        foreach (Transform child in arenaRoot)
        {
            Destroy(child.gameObject);
        }

        //nemelo by byt treba ale pro jistotu premazat promenou nepratel
        enyCount = 0;
    }

    void end()
    {
        //jednoduchy if pro zabraneni okapovani prikazu = lepsi vykon + "shutdownTimer = Time.time + 3f;" by jinak probehlo kazdy frame
        if (!shutingDown)
        {
            //pro ostani scripty napr. playerShooting
            gameEnd = true;
            //zmena doposud prazdneho textoveho pole pro zobrazeni napisu
            youLoseTXT.text = "GAME OVER!";
            //kontrola pro tento if
            shutingDown = true;
            //nastaveni casu do vypnuti aplikace; Time.time je cas od spusteni aplikace
            shutdownTimer = Time.time + 3f;
        }
        //kontrola jestli jiz vypnout aplikaci
        if (shutdownTimer <= Time.time)
        {
            //vypnuti aplikace
            Application.Quit();
        }   
    }
}
