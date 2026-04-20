using UnityEngine;
using UnityEngine.InputSystem;

public class SettingPanelToggle : MonoSingleton<SettingPanelToggle>
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject detailsPanel;

    [SerializeField] private GameObject clickBlockPanel;

    private bool onSettingPanel = false;
    private bool ondetailsUI = false;

    private void Awake()
    {
        settingPanel.SetActive(false);
        detailsPanel.SetActive(false);
        clickBlockPanel.SetActive(false);
    }

    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!onSettingPanel)
                if(!ondetailsUI) OpenSettingPanel();
                else OpendetailsSetting();

            else
                if (!ondetailsUI) CloseSettingPanel();
                else CloseDetailsSetting();
        }
    }


    public void ExitGame() => Application.Quit();

    public void OpenSettingPanel()
    {
        Time.timeScale = 0;
        onSettingPanel = true;
        settingPanel.SetActive(true);
        clickBlockPanel.SetActive(true);
    }

    public void OpendetailsSetting()
    {
        Time.timeScale = 0;
        ondetailsUI = true;
        detailsPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        Time.timeScale = 1;
        onSettingPanel = false;
        settingPanel.SetActive(false);
        clickBlockPanel.SetActive(false);
    }

    public void CloseDetailsSetting()
    {
        ondetailsUI = false;
        detailsPanel.SetActive(false);
    }
}
