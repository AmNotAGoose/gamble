using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SlotsGame : MonoBehaviour
{
    public static SlotsGame Instance;

    public SlotMachine slotMachine;

    public List<SlotWinEvent> winningEvents = new List<SlotWinEvent>();

    // Item Values

    public List<Sprite> itemValueSprites;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(slotMachine.AutoSpin(GenerateRandomGame));
        
    }

    public List<List<int>> GenerateRandomGame()
    {
            
        List<List<int>> game = new List<List<int>>()
        {
            new List<int>() { 1, 1, 1, 1 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
        };

        return game;
    }

    public void EvaluateState(List<List<int>> boardState)
    {
        EvalVertical(boardState);
        ProcessWinnings();
    }

    public void ProcessWinnings()
    {
        foreach (SlotWinEvent win in winningEvents)
        {
            PlayerDataManager.Instance.AddCoins(win.winWorth);
            slotMachine.DisplayMatch(win);
        }

        winningEvents.Clear();
    }

    public void EvalVertical(List<List<int>> boardState)
    {
        for (int i = 0; i < boardState.Count; i++)
        {
            bool allEqual = true;

            List<SlotItem> curItems = new List<SlotItem>();
            curItems.Add(GetSlotItem(i, 0));
            int prev = boardState[i][0];

            for (int j = 1; j < boardState[i].Count; j++)
            {
                curItems.Add(GetSlotItem(i, j));
                if (boardState[i][j] != prev)
                {
                    print("here");
                    allEqual = false;
                    break;
                }
                prev = boardState[i][j];
            }

            if (allEqual)
            {
                SlotWinEvent winEvent = new SlotWinEvent();
                winEvent.winningItems = curItems;
                winEvent.winWorth = 30;
                winningEvents.Add(winEvent);
            }
        }
    }

    public SlotItem GetSlotItem(int col, int row)
    {
        return slotMachine.columns[col].slotItems[row];
    }
}
