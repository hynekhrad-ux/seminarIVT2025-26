using UnityEngine;

public class scannerScript : MonoBehaviour
{
    

    Transform playerTransform;

    LayerMask layerMask;


    private void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy");
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public bool scan()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, 1000, layerMask))
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
