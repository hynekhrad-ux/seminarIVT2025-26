using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public bool readyToShoot;
    public GameObject scannerPrefab;
    
    public Transform scannerRoot;
    
    scannerScript scannerScriptSCR;   

    void Awake()
    {
        
    }
    Transform takeCover()
    {
        for(int i = 0; i < 100; i++)
        {
            var results = scannerSpawn(i,false);
            if (results.Item1)
            {
                return results.Item2;
            }
        }
        return null;
    }
    Transform engagePlayer()
    {
        for(int i = 0; i < 100; i++)
        {
            var results = scannerSpawn(i,true);
            if (results.Item1)
            {
                return results.Item2;
            }
        }
        return null;
    }

    (bool,Transform) scannerSpawn(int depth, bool wantPlayer)
    {
        for (int x = -depth; x <= depth; x++)
        {
            for (int z = -depth; z <= depth; z++)
            {
                if (x == 0 && z == 0)
                {
                    continue; 
                }

                Vector3 offset = new Vector3(x, 0, z) * 1; 
                Vector3 pos = transform.position + offset;

                

                GameObject obj = Instantiate(scannerPrefab, pos, Quaternion.identity, scannerRoot);
                
            }
        }
        if (wantPlayer)
        {
           foreach (Transform child in scannerRoot)
            {
                scannerScript script = child.GetComponent<scannerScript>();
                
                if (script.scan())
                {
                    for (int i = scannerRoot.childCount - 1; i >= 0; i--)
                    {
                        Destroy(scannerRoot.GetChild(i).gameObject);
                    }
                    return(true, child);
                }
                
                    
                
            } 
            
        }
        else if (!wantPlayer)
        {
            foreach (Transform child in scannerRoot)
            {
                scannerScript script = child.GetComponent<scannerScript>();
                
                if (!script.scan())
                {
                    for (int i = scannerRoot.childCount - 1; i >= 0; i--)
                    {
                        Destroy(scannerRoot.GetChild(i).gameObject);
                    }
                    return(true, child);
                }
                
                    
                
            } 
            
        }
        for (int i = scannerRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(scannerRoot.GetChild(i).gameObject);
        }
        return(false, null);
        
    }
}
