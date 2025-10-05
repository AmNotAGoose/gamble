using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public List<SlotColumn> columns = new List<SlotColumn>();
    public List<List<GameObject>> positions = new List<List<GameObject>>();

    bool isAutoSpinning = false;
    bool isSpinning = false;

    public event Action<string> spinButtonState;
    public event Action<string> autoSpinButtonState;

    public SlotMachineEffects effects;

    private void Start()
    {
        //StartCoroutine(AutoSpin(GetSampleValues));
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


    public IEnumerator _AutoSpin(Func<List<List<int>>> func)
    {
        autoSpinButtonState?.Invoke("stop");
        isAutoSpinning = true;
        while (true)
        {
            Spin();
            yield return new WaitForSeconds(3f);
            StartCoroutine(Stop(func));
            yield return new WaitUntil(() => !isSpinning && SlotsGame.Instance.evalDone);

            SlotsGame.Instance.evalDone = false;

            if (!isAutoSpinning)
            {
                autoSpinButtonState?.Invoke("spin");
                yield break;
            }
        }
    }

    public void ToggleSpin()
    {
        StartCoroutine(_ToggleSpin(SlotsGame.Instance.GenerateRandomGame));
    }

    public void ToggleAutoSpin()
    {
        if (isAutoSpinning)
        {
            isAutoSpinning = false;
        } else
        {
            StartCoroutine(_AutoSpin(SlotsGame.Instance.GenerateRandomGame));
        }
    }


    public IEnumerator _ToggleSpin(Func<List<List<int>>> func)
    {
        if (isSpinning)
        {
            StartCoroutine(Stop(func));
            spinButtonState?.Invoke("wait");
            yield return new WaitUntil(() => !isSpinning && SlotsGame.Instance.evalDone);
            SlotsGame.Instance.evalDone = false;
            spinButtonState?.Invoke("spin");
        } else
        {
            Spin();
            spinButtonState?.Invoke("wait");
            yield return new WaitForSeconds(1.5f);
            spinButtonState?.Invoke("stop");
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

        SlotsGame.Instance.EvaluateState(values);
        isSpinning = false;
    }

    public void DisplayMatch(SlotWinEvent match)
    {
        StartCoroutine(effects.PlayWin(match));
    }
}
