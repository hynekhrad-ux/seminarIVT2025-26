using UnityEngine;

public class m1911 : MonoBehaviour
{
    
    public Transform particlePlace;
    public GameObject muzlleFlash;
    public Transform firePoint;

    public Animator gunAnimator0;

    private float fireRateID0 = 0.25f;
    private float fireRateTimeStamp;

    public int maxAmmoID0 = 8;
    public int currentAmmoID0 = 9;

    public float reloadTimeSimpleID0 = 1.66f;
    public float reloadTimeZeroID0 = 2f;

    public static int damageID0 = 50;

    playerShooting playerShootingSCR;
    playerReloading playerReloadingSCR;
    public gameManager gameManagerSCR;
    

    private void Awake()
    {
        playerShootingSCR = GetComponentInParent<playerShooting>();
        playerReloadingSCR = GetComponentInParent<playerReloading>();

    }

    public void fireID0()
    {

        if (!playerShootingSCR.canShootID0)
            return;

        if (currentAmmoID0 <= 0)
            return;

       

        currentAmmoID0--;
        playerShootingSCR.canShootID0 = false;
        fireRateTimeStamp  = Time.time + fireRateID0;
        gunAnimator0.SetTrigger("Fire");

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit))
        {

            GameObject particleMuzlleFlash = Instantiate(muzlleFlash, particlePlace.position, Quaternion.identity, particlePlace.transform);
            GameObject impact = Instantiate(muzlleFlash, hit.point, Quaternion.identity);

            Destroy(impact, 1);
            Destroy(particleMuzlleFlash, 1);

            if (hit.collider.gameObject.tag == "resetButton" && gameManager.enyCount == 0)
            {
                Debug.Log("button");
                gameManagerSCR.DeleteArena();
                gameManagerSCR.SpawnArena();
            }
            if (hit.collider.gameObject.tag == "m1911UpgradeButton" && gameManager.enyCount == 0 && hudSCR.score >= gameManager.gunID0UpgradeCost)
            {
                damageID0 += 5;
                hudSCR.score -= gameManager.gunID0UpgradeCost;
                gameManager.gunID0UpgradeCost *= 2;
            }
            if (hit.collider.gameObject.tag == "M4UpgradeButton" && gameManager.enyCount == 0 && hudSCR.score >= gameManager.gunID1UpgradeCost)
            {
                M4.damageID1 += 5;
                hudSCR.score -= gameManager.gunID1UpgradeCost;
                gameManager.gunID1UpgradeCost *= 2;
            }

            enemySCR Enemy = hit.transform.GetComponent<enemySCR>();

            if (Enemy != null)
            {
                if (hit.collider.gameObject.tag == "enemy")
                {
                    Enemy.Damage(damageID0);
                    Debug.Log(damageID0);
                    enemySCR.wasLastHitHeadshot = false;

                }
                else if (hit.collider.gameObject.tag == "enyHead")
                {
                    Enemy.Damage(damageID0 * 2);
                    Debug.Log(damageID0);
                    enemySCR.wasLastHitHeadshot = true;
                    return;
                }
                
            } 
        }
    }
    void Update()
    {
        if (!playerShootingSCR.canShootID0 && fireRateTimeStamp  < Time.time && playerReloadingSCR.timestampReload < Time.time && currentAmmoID0 != 0)
        {
            playerShootingSCR.canShootID0 = true;
        }
    }
}

