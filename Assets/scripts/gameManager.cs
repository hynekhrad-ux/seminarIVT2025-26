using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{

    public TextMeshProUGUI roundNumberTXT;
    public TextMeshProUGUI gunID0UpgradeCostTXT;
    public TextMeshProUGUI gunID1UpgradeCostTXT;
    public TextMeshProUGUI timeLeftInRound;
    public TextMeshProUGUI youLoseTXT;

    
    private float goalTime;

    private float shutdownTimer;

    private int randomNum = 0;
    public GameObject enyPrefab;
    public GameObject wall3x2;
    public GameObject wall2x3;
    public GameObject pillar1x10;

    public Transform arenaRoot;

    public static int roundNumber =0;

    public static int gunID0UpgradeCost = 100;
    public static int gunID1UpgradeCost = 100;

    public static int enyCount;

    public static bool gameEnd = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnArena();
    }

    private void Update()
    {
        gunID0UpgradeCostTXT.text = "m1911 - UPGRADE     COST: " + gunID0UpgradeCost;
        gunID1UpgradeCostTXT.text = "M4 - UPGRADE     COST: " + gunID1UpgradeCost;

        if (enyCount != 0)
        {
            if (goalTime - Time.time > 0)
            {
                timeLeftInRound.text = (goalTime - Time.time).ToString("F2");
            }
            else if (goalTime - Time.time < 0)
            { 
                timeLeftInRound.text = "0";
                end();
            }
        }
    }

    public void SpawnArena()
    {
        
        Debug.Log("spawn");
        for (int x = 0; x < 6; x++)
        {
            for (int z = 0; z < 6; z++)
            {
                randomNum = UnityEngine.Random.Range(0, 6);
                if (randomNum == 0)
                {
                    Instantiate(pillar1x10, new Vector3(x * 8 - 20, 5, z * 8 - 20), Quaternion.identity, arenaRoot);
                }
                else if (randomNum == 1)
                {
                    Instantiate(wall3x2, new Vector3(x * 8 - 20, 1, z * 8 - 20), Quaternion.identity, arenaRoot);
                }
                else if (randomNum == 2)
                {
                    Instantiate(wall2x3, new Vector3(x * 8 - 20, 1, z * 8 - 20), Quaternion.identity, arenaRoot);
                }
                else if (randomNum <= 4)
                {
                    Instantiate(enyPrefab, new Vector3(x * 8 - 20, 1, z * 8 - 20), Quaternion.identity, arenaRoot);
                    enyCount++;
                }
            }
        }
        roundNumber++;
        roundNumberTXT.text = "Round " + roundNumber;
        
        goalTime = Time.time + 10f;
    }

    public void DeleteArena()
    {
        Debug.Log("delete");
        foreach (Transform child in arenaRoot)
        {
            Destroy(child.gameObject);
        }

        enyCount = 0;
    }

    void end()
    {
        gameEnd = true;
        youLoseTXT.text = "GAME OVER!";
        Debug.Log("you loose");
        shutdownTimer = Time.time + 3f;
        if (shutdownTimer <= Time.time)
        {
            Application.Quit();
        }   
    }
}
