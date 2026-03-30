using UnityEngine;

public class coverScript : MonoBehaviour
{
    public GameObject occupyingEnemy = null;

    // Check if the cover is available
    public bool IsAvailable(GameObject enemy)
    {
        return occupyingEnemy == null || occupyingEnemy == enemy;
    }

    // Try to reserve the cover
    public bool TryReserve(GameObject enemy)
    {
        if (occupyingEnemy == null)
        {
            occupyingEnemy = enemy;
            return true;
        }
        return occupyingEnemy == enemy;
    }

    // Release the cover
    public void Vacate(GameObject enemy)
    {
        if (occupyingEnemy == enemy)
            occupyingEnemy = null;
    }
}