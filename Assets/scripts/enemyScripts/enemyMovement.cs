using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyMovement : MonoBehaviour
{
    public bool readyToShoot;
    public GameObject scannerPrefab;

    public Transform playerTransform;
    public NavMeshAgent agent;

    GameObject[] coverPos;
    LayerMask layerMask;

    enemyLogic enemyLogicSCR;
    gameManager gameManagerSCR;

    Transform currentCover = null;

  
    

    void Awake()
    {

        playerTransform = GameObject.FindWithTag("Player").transform;
        layerMask = LayerMask.GetMask("Enemy");
        coverPos = GameObject.FindGameObjectsWithTag("coverPoint");
        enemyLogicSCR = GetComponent<enemyLogic>();
        gameManagerSCR = GetComponent<gameManager>();
    }
    void Update()
    {

        // If current cover is no longer valid, reset path
        if (currentCover != null)
        {
            var coverScr = currentCover.GetComponent<coverScript>();
            if (!coverScr.IsAvailable(gameObject) || HasLineOfSight(transform.position, playerTransform))
            {
                agent.ResetPath();
                currentCover = null;
            }
        }

        // Pick a new cover if needed
        if (enemyLogicSCR.wantToTakeCover && currentCover == null)
        {
            currentCover = FindCover();
            if (currentCover != null)
            {
                agent.SetDestination(currentCover.position);
            }
        }
    }



    Transform FindCover()
    {
        float bestScore = float.MaxValue;
        Transform bestCover = null;

        foreach (var cover in coverPos)
        {
            var coverScr = cover.GetComponent<coverScript>();

            // Skip covers already reserved by another enemy
            if (!coverScr.IsAvailable(gameObject))
                continue;

            // Skip covers with line of sight
            if (HasLineOfSight(cover.transform.position, playerTransform))
                continue;

            float distance = Vector3.Distance(transform.position, cover.transform.position);
            if (distance < bestScore)
            {
                bestScore = distance;
                bestCover = cover.transform;
            }
        }

        // Reserve the cover immediately
        if (bestCover != null)
            bestCover.GetComponent<coverScript>().TryReserve(gameObject);

        return bestCover;
    }



    bool HasLineOfSight(Vector3 from, Transform player)
    {
        Vector3 dir = (player.position - from);

        if (Physics.Raycast(from, dir, out RaycastHit hit, 1000, ~layerMask))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
            return false;
        }

        return false;
    }




}