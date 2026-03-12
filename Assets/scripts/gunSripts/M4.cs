using UnityEngine;

public class M4 : MonoBehaviour
{
    public Transform particlePlace;
    public GameObject muzlleFlash;
    public Transform firePoint;

    public Animator gunAnimator1;

    public int maxAmmoID1 = 30;
    public int currentAmmoID1 = 31;
    private float fireRateID1 = 0.1f;

    public float realoadTimeSimpleID1 = 2f;
    public float reloadTimeZeroID1 = 2.25f;

    private float fireRateTimeStamp;

    public static int damageID1 = 34;

    playerShooting playerShootingSCR;
    playerReloading playerReloadingSCR;
    public gameManager gameManagerSCR;
    private void Awake()
    {
        playerShootingSCR = GetComponentInParent<playerShooting>();
        playerReloadingSCR = GetComponentInParent<playerReloading>();
    }

    public void fireID1()
    {
        if (!playerShootingSCR.canShootID1)
            return;

        if (currentAmmoID1 <= 0)
            return;

        

        currentAmmoID1--;
        playerReloadingSCR.canReload = true;
        playerShootingSCR.canShootID1 = false;
        fireRateTimeStamp = Time.time + fireRateID1;

        gunAnimator1.SetTrigger("Fire");

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
                m1911.damageID0 += 5;
                hudSCR.score -= gameManager.gunID0UpgradeCost;
                gameManager.gunID0UpgradeCost *= 2;
            }
            if (hit.collider.gameObject.tag == "M4UpgradeButton" && gameManager.enyCount == 0 && hudSCR.score >= gameManager.gunID1UpgradeCost)
            {
                damageID1 += 5;
                hudSCR.score -= gameManager.gunID1UpgradeCost;
                gameManager.gunID1UpgradeCost *= 2;
            }

            enemySCR Enemy = hit.transform.GetComponent<enemySCR>();

            if (Enemy != null)
            {
                if (hit.collider.gameObject.tag == "enemy")
                {
                    Enemy.Damage(damageID1);
                    Debug.Log(damageID1);
                    enemySCR.wasLastHitHeadshot = false;

                }
                else if (hit.collider.gameObject.tag == "enyHead")
                {
                    Enemy.Damage(damageID1 * 2);
                    Debug.Log(damageID1);
                    enemySCR.wasLastHitHeadshot = true;
                    return;
                }



            }

        }
        
    }
    void Update()
    {
        if (!playerShootingSCR.canShootID1 && fireRateTimeStamp < Time.time && playerReloadingSCR.timestampReload < Time.time && currentAmmoID1 != 0)
        {
            playerShootingSCR.canShootID1 = true;
        }
    }
}
