using UnityEngine;

public class enemyShooting : MonoBehaviour
{
    public Transform playerTransform = GameObject.transform.FindWithTag("Player");
    
    
    void Update()
    {
        transform.LookAt(playerTransform);    
    }
}
