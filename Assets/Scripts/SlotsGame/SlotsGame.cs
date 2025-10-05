using UnityEngine;

public class SlotsGame : MonoBehaviour
{
    PlayerDataManager playerDataManager;

    private void Start()
    {
        playerDataManager = PlayerDataManager.Instance;
    }
}
