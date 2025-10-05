using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public List<SlotColumn> columns = new List<SlotColumn>();
    public List<List<GameObject>> positions = new List<List<GameObject>>();

    private void Start()
    {
        Spin();


        List<List<int>> sampleValues = new List<List<int>>()
        {
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
        };

        StartCoroutine(Stop(sampleValues));
    }

    public void AutoSpin()
    {

    }

    public void Spin()
    {
        foreach (SlotColumn col in columns)
        {
            col.StartSpinning();
        }
    }

    public IEnumerator Stop(List<List<int>> values)
    {
        for (int i = 0; i < columns.Count; i++)
        {
            StartCoroutine(columns[i].StopSpinning(values[i]));
            yield return new WaitForSeconds(0.5f);
        }
    }
}
