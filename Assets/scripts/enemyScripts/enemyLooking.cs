using UnityEngine;

public class enemyLooking : MonoBehaviour
{
    public Transform playerTransform;
    public Transform gunTranform;
    public Transform firePointTransform;


    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        firePointTransform.transform.LookAt(playerTransform);

        gunTranform.rotation = Quaternion.LookRotation(playerTransform.position - gunTranform.position) ;
    }
}
