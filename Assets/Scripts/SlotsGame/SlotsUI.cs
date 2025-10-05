using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotsUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public Button spinButton;
    public TextMeshProUGUI spinButtonText;
    public Button autoSpinButton;
    public TextMeshProUGUI autoSpinButtonText;

    private void Start()
    {
        PlayerDataManager.Instance.OnCoinChanged += UpdateCoinText;
        SlotsGame.Instance.slotMachine.spinButtonState += UpdateSpinButtonStatus;
        SlotsGame.Instance.slotMachine.autoSpinButtonState += UpdateAutoSpinButtonStatus;
    }

    public void UpdateCoinText(int coinValue)
    {
        coinText.text = coinValue.ToString();
    }

    public void UpdateSpinButtonStatus(string state)
    {
        switch (state)
        {
            case "spin":
                spinButton.interactable = true;
                autoSpinButton.interactable = true;
                spinButtonText.text = "Spin";
                break;
            case "stop":
                spinButton.interactable = true;
                autoSpinButton.interactable = false;
                spinButtonText.text = "Stop";
                break;
            case "wait":
                spinButtonText.text = "Wait";
                spinButton.interactable = false;
                autoSpinButton.interactable = false;
                break;
        }
    }

    public void UpdateAutoSpinButtonStatus(string state)
    {
        switch (state)
        {
            case "spin":
                spinButton.interactable = true;
                autoSpinButtonText.text = "Spin";
                break;
            case "stop":
                spinButton.interactable = false;
                autoSpinButtonText.text = "Stop";
                break;
        }
    }
}
