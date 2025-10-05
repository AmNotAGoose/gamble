using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public int coins = 500;

    public event Action<int> OnCoinChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        OnCoinChanged?.Invoke(coins);
    }

    public void AddCoins(int numberOfCoins)
    {
        coins += numberOfCoins;
        OnCoinChanged?.Invoke(coins);
    }
}
