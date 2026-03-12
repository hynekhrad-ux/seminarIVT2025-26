using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class enemySCR : MonoBehaviour
{
    public int enyHealth;
    public static bool wasLastHitHeadshot;



    private void Awake()
    {
       
        enyHealth = 100 + (gameManager.roundNumber * 10);
        Debug.Log(enyHealth);
    }
    public void Damage(int damage)
    {
        enyHealth -= damage;

        if(enyHealth <= 0)
        {
            gameManager.enyCount--;

            if ( wasLastHitHeadshot )
            {
                if(!playerMovement.playerIsOnGround)
                    hudSCR.score += 200;
                else
                {
                   hudSCR.score += 150; 
                }
            }
            else
            {
                if(!playerMovement.playerIsOnGround)
                    hudSCR.score += 150;
                else
                {
                   hudSCR.score += 100; 
                }
                
            }
            Destroy(gameObject);
        }
    }
}
