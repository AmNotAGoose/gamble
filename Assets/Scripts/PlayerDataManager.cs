using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public int coins = 500;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public void AddCoins(int numberOfCoins)
    {
        coins += numberOfCoins;
    }
}
