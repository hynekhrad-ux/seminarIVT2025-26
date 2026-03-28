using UnityEngine;

public class enemyReloading : MonoBehaviour
{
    public bool readyToReload;
    public Animator enemyAnimator;
    

    private float enemyReloadTime = 2.25f;
    private float enemyReloadTimestamp;

    enemySCR enemySCR;
    enemyShooting enemyShootingSCR;
    enemyLogic enemyLogicSCR;
    void Awake()
    {
        enemyLogicSCR = GetComponent<enemyLogic>();
        enemyShootingSCR = GetComponent<enemyShooting>();
        enemySCR = GetComponent<enemySCR>();
    }


    // Update is called once per frame
    void Update()
    {
        if (enemyReloadTimestamp < Time.time)
        {
            enemyShootingSCR.readyToShoot = true;

            if (enemyLogicSCR.wantToReload)
            {
                reload();
                enemyLogicSCR.wantToReload =false;
            }
        }
        
    }

    void reload()
    {
        enemyShootingSCR.readyToShoot=false;
        enemyReloadTimestamp = Time.time + enemyReloadTime;
        enemySCR.enemyAmmo = 30;
        enemyAnimator.SetTrigger("Reload");
    }
}
