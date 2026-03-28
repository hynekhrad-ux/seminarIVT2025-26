using UnityEngine;

public class scannerScript : MonoBehaviour
{
    

    Transform playerTransform;

    
    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public bool scan()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit))
        {
            
            if (hit.collider.name == "player")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }
}
