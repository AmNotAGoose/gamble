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
            new List<int>() { 3, 1, 2, 3 },
            new List<int>() { 3, 3, 3, 3 },
            new List<int>() { 0, 1, 3, 3 },
            new List<int>() { 0, 3, 1, 3 },
            new List<int>() { 3, 1, 2, 3 },
        };

        for (int i = 0; i < game.Count; i++)
        {
            for (int j = 0; j < game[i].Count; j++)
            {
                game[i][j] = UnityEngine.Random.Range(0, itemValueSprites.Count);
            }
        }


        return game;
    }

    public void EvaluateState(List<List<int>> boardState)
    {
        EvalVertical(boardState);
        EvalHorizontal(boardState);
        EvalTShapes(boardState);
        EvalSpecialPatterns(boardState);
        StartCoroutine(ProcessWinnings());
    }

    private void EvalTShapes(List<List<int>> boardState)
    {
        int cols = boardState.Count;
        int rows = boardState[0].Count;

        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                // up
                if (c >= 1 && c < cols - 1 && r < rows - 1)
                {
                    var coords = new List<(int, int)>
                {
                    (c-1,r), (c,r), (c+1,r), (c,r+1)
                };
                    CheckPattern(coords, boardState, 60);
                }

                // down
                if (c >= 1 && c < cols - 1 && r > 0)
                {
                    var coords = new List<(int, int)>
                {
                    (c-1,r), (c,r), (c+1,r), (c,r-1)
                };
                    CheckPattern(coords, boardState, 60);
                }

                // left
                if (r >= 1 && r < rows - 1 && c < cols - 1)
                {
                    var coords = new List<(int, int)>
                {
                    (c,r-1), (c,r), (c,r+1), (c+1,r)
                };
                    CheckPattern(coords, boardState, 60);
                }

                // right
                if (r >= 1 && r < rows - 1 && c > 0)
                {
                    var coords = new List<(int, int)>
                {
                    (c,r-1), (c,r), (c,r+1), (c-1,r)
                };
                    CheckPattern(coords, boardState, 60);
                }
            }
        }
    }

    public void EvalSpecialPatterns(List<List<int>> boardState)
    {
        // Diagonals
        CheckPattern(new List<(int, int)> { (0, 0), (1, 1), (2, 2), (3, 3) }, boardState, 50);
        CheckPattern(new List<(int, int)> { (4, 0), (3, 1), (2, 2), (1, 3) }, boardState, 50);
        // V
        CheckPattern(new List<(int, int)> { (0, 0), (1, 1), (2, 2), (3, 1), (4, 0) }, boardState, 75);
        // inverted V
        CheckPattern(new List<(int, int)> { (0, 3), (1, 2), (2, 1), (3, 2), (4, 3) }, boardState, 75);
        // W
        CheckPattern(new List<(int, int)> { (0, 0), (1, 1), (2, 0), (3, 1), (4, 0) }, boardState, 100);
        // M
        CheckPattern(new List<(int, int)> { (0, 3), (1, 2), (2, 3), (3, 2), (4, 3) }, boardState, 100);

        for (int col = 0; col < boardState.Count - 1; col++)
        {
            for (int row = 0; row < boardState[col].Count - 1; row++)
            {
                var coords = new List<(int, int)>
            {
                (col,row), (col+1,row),
                (col,row+1), (col+1,row+1)
            };
                CheckPattern(coords, boardState, 40);
            }
        }
    }

    private void CheckPattern(List<(int col, int row)> coords, List<List<int>> boardState, int worth)
    {
        if (coords.Count == 0) return;

        int firstVal = boardState[coords[0].col][coords[0].row];
        foreach (var (c, r) in coords)
        {
            if (boardState[c][r] != firstVal)
                return;
        }

        SlotWinEvent winEvent = new SlotWinEvent();
        winEvent.winningItems = new List<SlotItem>();
        foreach (var (c, r) in coords)
            winEvent.winningItems.Add(GetSlotItem(c, r));

        winEvent.winWorth = worth;
        winningEvents.Add(winEvent);
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
        return slotMachine.columns[col].slotItems[row + 1];
    }
}
