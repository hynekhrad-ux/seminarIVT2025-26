using UnityEngine;

public class enemyLogic : MonoBehaviour
{
    public bool wantToReload;
    public bool wantToShoot;
    public bool wantToTakeCover;

    public Transform firePoint;
    
    LayerMask layerMask;

    enemySCR enemySCR;
    enemyShooting enemyShootingSCR;
    
    void Awake()
    {
        layerMask = LayerMask.GetMask("Player");
        enemySCR = GetComponent<enemySCR>();
        enemyShootingSCR = GetComponent<enemyShooting>();
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 1000, layerMask))
        {
            
            if (enemySCR.enemyAmmo <= 0)
            {
                wantToShoot = false;
                wantToTakeCover = true;
                wantToReload = true;
            }
            else if(enemySCR.enemyAmmo < 15 && hit.distance >= 20)
            {
                wantToShoot = false;
                wantToTakeCover = true;
                wantToReload = true;
            }
            else if(enemySCR.enemyAmmo < 25 && hit.distance >= 40)
            {
                wantToShoot = false;
                wantToTakeCover = true;
                wantToReload = true;
            }
            else if (hit.distance < 30 && enemyShootingSCR.readyToShoot)
            {
                
                wantToReload =false;
                wantToTakeCover =false;
                wantToShoot = true;
            }
            else
            {
                
                wantToShoot = false;
                
                wantToTakeCover=true;
            }
        }  
    }
}
