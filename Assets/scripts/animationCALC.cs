using UnityEngine;

public class animationCALC : MonoBehaviour
{
    public Transform magWellDIR;
    public Transform mag;
    public Transform magEnd;
    private float distance = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Vector3 direction = magWellDIR.localRotation * Vector3.down;

        magEnd.localPosition=mag.localPosition + direction * distance;
        Debug.Log(magEnd.localPosition);
    }

   
}
