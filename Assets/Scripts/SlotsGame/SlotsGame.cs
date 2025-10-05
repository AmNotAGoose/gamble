using System.Collections.Generic;
using UnityEngine;

public class SlotsGame : MonoBehaviour
{
    public static SlotsGame Instance;

    PlayerDataManager playerDataManager;

    // Item Values

    public List<Sprite> itemValueSprites;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerDataManager = PlayerDataManager.Instance;
    }
}
