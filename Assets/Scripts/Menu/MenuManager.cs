using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<MenuPanel> menuPanels;
    public MenuPanel curPanel;

    private void Start()
    {
        curPanel = menuPanels[0];
    }

    public void ChangeMenuPanel(string menuName)
    {
        foreach (MenuPanel panel in menuPanels)
        {
            if (panel.menuName == menuName)
            {
                curPanel.gameObject.SetActive(false);
                curPanel = panel;
                curPanel.gameObject.SetActive(true);
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
