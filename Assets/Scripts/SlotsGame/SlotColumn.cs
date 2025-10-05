using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotColumn : MonoBehaviour
{
    // Movement
    public float acceleration;
    public float curVelocity;
    public float maxVelocity;

    public float maxY = 3;
    public float minY = -3;

    public bool isStopped = true;

    // Items
    public List<SlotItem> slotItems;

    private void Start()
    {
        StartCoroutine(StartSpinning());
    }

    private void Update()
    {
        foreach (SlotItem item in slotItems)
        {
            item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y - curVelocity * Time.deltaTime);
        
            if (item.transform.position.y < minY)
            {
                item.transform.position = new Vector2(item.transform.position.x, maxY);
            }
        }
    }


    public IEnumerator StartSpinning()
    {
        isStopped = false;

        while (!isStopped)
        {
            curVelocity = Mathf.Min(curVelocity + acceleration * Time.deltaTime, maxVelocity); // read it carefully
            yield return null;
        }
        curVelocity = 0; 
        yield break;
    }

    // i am NOT doing physics in my spare time, so i wont be implementing rigged / predetermined games
    public void StopSpinning()
    {
        isStopped = true;
    }
}
