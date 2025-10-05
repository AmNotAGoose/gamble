using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public List<SlotColumn> columns = new List<SlotColumn>();
    public List<List<GameObject>> positions = new List<List<GameObject>>();

    bool isAutoSpinning = false;
    bool isSpinning = false;

    private void Start()
    {
        StartCoroutine(AutoSpin(GetSampleValues));
    }

    List<List<int>> GetSampleValues()
    {
        List<List<int>> sampleValues = new List<List<int>>()
        {
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
        };

        return sampleValues;
    }

    public IEnumerator AutoSpin(Func<List<List<int>>> func)
    {
        while (true)
        {
            Spin();
            yield return new WaitForSeconds(3f);
            StartCoroutine(Stop(func));
            yield return new WaitUntil(() => !isSpinning);

            if (isAutoSpinning) yield break;
        }
    }

    public void Spin()
    {
        foreach (SlotColumn col in columns)
        {
            StartCoroutine(col.StartSpinning());
        }
        isSpinning = true;
    }

    public IEnumerator Stop(Func<List<List<int>>> func) 
    {
        List<List<int>> values = func();
        for (int i = 0; i < columns.Count; i++)
        {
            StartCoroutine(columns[i].StopSpinning(values[i]));
            yield return new WaitForSeconds(0.5f);
        }
        isSpinning = false;
    }
}
