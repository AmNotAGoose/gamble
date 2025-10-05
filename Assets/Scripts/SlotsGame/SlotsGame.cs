using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SlotsGame : MonoBehaviour
{
    public static SlotsGame Instance;

    public SlotMachine slotMachine;

    public List<SlotWinEvent> winningEvents = new List<SlotWinEvent>();

    // Item Values

    public List<Sprite> itemValueSprites;

    public bool evalDone = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //StartCoroutine(slotMachine.AutoSpin(GenerateRandomGame));
    }

    public List<List<int>> GenerateRandomGame()
    {

        List<List<int>> game = new List<List<int>>()
        {
            new List<int>() { 2, 1, 2, 3 },
            new List<int>() { 3, 1, 1, 1 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
            new List<int>() { 0, 1, 2, 3 },
        };

        //for (int i = 0; i < game.Count; i++)
        //{
        //    for (int j = 0; j < game[i].Count; j++)
        //    {
        //        game[i][j] = UnityEngine.Random.Range(0, itemValueSprites.Count);
        //    }
        //}


        return game;
    }

    public void EvaluateState(List<List<int>> boardState)
    {
        EvalVertical(boardState); 
        EvalHorizontal(boardState);
        StartCoroutine(ProcessWinnings());
    }

    public IEnumerator ProcessWinnings()
    {
        
        foreach (SlotWinEvent win in winningEvents)
        {
            PlayerDataManager.Instance.AddCoins(win.winWorth);
            slotMachine.DisplayMatch(win);

            yield return new WaitForSeconds(0.5f);
        }

        winningEvents.Clear();
        evalDone = true;
    }

    public void EvalHorizontal(List<List<int>> boardState)
    {
        for (int row = 0; row < boardState[0].Count; row++)
        {
            bool allEqual = true;

            List<SlotItem> curItems = new List<SlotItem>();
            curItems.Add(GetSlotItem(0, row));
            int prev = boardState[0][row];

            for (int col = 1; col < boardState.Count; col++)
            {
                curItems.Add(GetSlotItem(col, row));
                if (boardState[col][row] != prev)
                {
                    allEqual = false;
                    break;
                }
                prev = boardState[col][row];
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
