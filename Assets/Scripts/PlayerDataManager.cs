using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public int coins = 500;

    public List<Action<int>> coinChangeSubscribers = new List<Action<int>>();

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
    private void OnLevelWasLoaded(int level)
    {
        InformSubscribers();
    }

    public void InformSubscribers()
    {
        foreach (Action<int> subscriber in coinChangeSubscribers)
        {
            subscriber(coins);
        }
    }

    public void AddCoins(int numberOfCoins)
    {
        coins += numberOfCoins;
        foreach (Action<int> subscriber in coinChangeSubscribers)
        {
            subscriber(coins);
        }
    }

    public void SubscribeToCoinChanges(Action<int> action)
    {
        coinChangeSubscribers.Add(action);
    }
}
