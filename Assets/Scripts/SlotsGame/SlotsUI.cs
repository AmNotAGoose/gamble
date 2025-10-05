using TMPro;
using UnityEngine;

public class SlotsUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;



    private void Start()
    {
        PlayerDataManager.Instance.OnCoinChanged += UpdateCoinText;
    }

    public void UpdateCoinText(int coinValue)
    {
        coinText.text = coinValue.ToString();

    }
}
