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

    private bool readyToSnap = false;

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
            slotItems[i].SetStartingY();
        }
    }
    private void Update()
    {
        for (int i = 0; i < slotItems.Count; i++)
        {
            SlotItem curItem = slotItems[i];

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

                curItem.transform.position = new Vector2(curItem.transform.position.x, highestY + itemSpacing);
            }

            float moveAmount = curVelocity * Time.deltaTime;
            curItem.transform.position += moveAmount * Vector3.down;
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

        foreach (var item in slotItems)
        {
            curVelocity = 0;
            yield return null;
            item.GoToStartingY(0);
        }

        yield break;
    }
}
