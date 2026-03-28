using Unity.VisualScripting;
using UnityEngine;


public class enemyShooting : MonoBehaviour
{
    public Transform particlePlace;
    public GameObject muzlleFlash;
    public Transform firePoint;

    public bool readyToShoot;

    private float fireRateEnemy = 0.3f;
    private float fireRateTimeStampEnemy;
    private int damageOfEnemy = 10;

    public Animator enemyAnimator;

    enemySCR enemySCR;

    void Awake()
    {
        enemySCR = GetComponent<enemySCR>();
    }

    void enemyShoot()
    {
        enemySCR.enemyAmmo -= 1;

        enemyAnimator.SetTrigger("Fire");
        
        Vector3 fireDir =  Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0) * firePoint.forward;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, fireDir, out hit))
        {

            GameObject particleMuzlleFlash = Instantiate(muzlleFlash, particlePlace.position, Quaternion.identity, particlePlace.transform);
            GameObject impact = Instantiate(muzlleFlash, hit.point, Quaternion.identity);

            Destroy(impact, 1);
            Destroy(particleMuzlleFlash, 1);

            fireRateTimeStampEnemy = Time.time + fireRateEnemy;

            if (hit.collider.name == "player")
            {
                //player.damagePlayer(damageOfEnemy);
            }
            else if (hit.collider.name == "playerHead")
            {
                //player.damagePlayer(damageOfEnemy * 2);
            }

            
        }
    }
    private void Update()
    {
        if (gameManager.gameEnd)
        {
            return;
        }
        
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 15))
        {
            
            if (hit.collider.name == "player" && fireRateTimeStampEnemy < Time.time)
            {
                if (enemySCR.enemyAmmo > 0)
                {
                    enemyShoot();    
                }
                
            }

        }
    }
}
