using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotColumn : MonoBehaviour
{
    // Movement
    public float acceleration;
    public float curVelocity;
    public float maxVelocity;

    public bool isStopped = true;

    // Items
    public List<SlotItem> slotItems;

    private void Update()
    {
        
    }

    public IEnumerator StartSpinning()
    {
        while (!isStopped)
        {
            curVelocity = Mathf.Min(curVelocity + acceleration * Time.deltaTime, maxVelocity);
        }
        curVelocity = 0; 
        yield return null;
    }

    public void StopSpinning()
    {
        isStopped = true;
    }
}
