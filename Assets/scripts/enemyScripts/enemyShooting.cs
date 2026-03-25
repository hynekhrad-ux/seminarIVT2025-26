using Unity.VisualScripting;
using UnityEngine;


public class enemyShooting : MonoBehaviour
{
    public Transform particlePlace;
    public GameObject muzlleFlash;
    public Transform firePoint;

    private float fireRateEnemy = 0.3f;
    private float fireRateTimeStampEnemy;
    private float damageEnemy = 10f;

    void enemyShoot()
    {
        Vector3 fireDir =  Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0) * firePoint.forward;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, fireDir, out hit))
        {

            GameObject particleMuzlleFlash = Instantiate(muzlleFlash, particlePlace.position, Quaternion.identity, particlePlace.transform);
            GameObject impact = Instantiate(muzlleFlash, hit.point, Quaternion.identity);

            Destroy(impact, 1);
            Destroy(particleMuzlleFlash, 1);

            fireRateTimeStampEnemy = Time.time + fireRateEnemy;
            Debug.Log("Hit: " + hit.collider.name);
        }
    }
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 15))
        {
            
            if (hit.collider.name == "player" && fireRateTimeStampEnemy < Time.time)
            {
                enemyShoot();
            }

        }
    }
}
