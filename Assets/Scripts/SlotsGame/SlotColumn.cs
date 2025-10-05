using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class SlotColumn : MonoBehaviour
{
    // Movement
    public float acceleration;
    public float curVelocity;
    public float maxVelocity;
    public float minVelocity;

    public float maxY = 3;
    public float minY = -3;

    public float centerY = 0f;

    public float itemSpacing = 2f;

    public bool isStopped = true;
    public bool minVelocityReached = false;

    // Items
    public List<SlotItem> slotItems;
    public SlotItem leadItem;

    private void Start()
    {
        leadItem = slotItems[0];
        ArrangeItems();

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        StartCoroutine(StartSpinning());
        yield return new WaitForSeconds(2f);
        StartCoroutine(StopSpinning());
    }

    void ArrangeItems()
    {
        for (int i = 0; i < slotItems.Count; i++)
        {
            slotItems[i].transform.position = new Vector2(slotItems[i].transform.position.x, maxY - i * itemSpacing);
        }
    }

    private void Update()
    {
        for (int i = 0; i < slotItems.Count; i++)
        {
            SlotItem curItem = slotItems[i];

            curItem.transform.position += Vector3.down * curVelocity * Time.deltaTime;

            if (curItem.transform.position.y < minY)
            {
                float highestY = float.MinValue;
                foreach (SlotItem other in slotItems)
                {
                    if (other != curItem && other.transform.position.y > highestY)
                    {
                        highestY = other.transform.position.y;
                    }
                }

                if (minVelocityReached && curItem == leadItem)
                {
                    curVelocity = 0f;
                    isStopped = false;
                }

                curItem.transform.position = new Vector2(curItem.transform.position.x, highestY + itemSpacing);
            }
        }
    }
     
    public IEnumerator StartSpinning()
    {
        isStopped = false;

        while (!isStopped)
        {
            curVelocity = Mathf.Min(curVelocity + acceleration * Time.deltaTime, maxVelocity);
            yield return null;
        }
        yield break;
    }

    public IEnumerator StopSpinning()
    {
        isStopped = true;

        while (isStopped)
        {
            curVelocity = Mathf.Max(curVelocity - acceleration * Time.deltaTime, minVelocity);

            if (curVelocity == minVelocity)
            {
                minVelocityReached = true;
            }
             
            yield return null;
        }

        minVelocityReached = false;
        yield break;
    }
}
