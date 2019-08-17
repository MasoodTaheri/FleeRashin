using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPanel : BasePanel
{
    public override void Show()
    {
        base.Show();
        Time.timeScale = 0;
    }

    public override void Hide()
    {
        base.Hide();
        Time.timeScale = 1f;
    }
    public void OnResumeClick()
    {
        FindObjectOfType<UIManager>().ChangePanel(PanelType.InGame);
        Time.timeScale = 1f;
    }

    public void OnQuitClick()
    {
        FindObjectOfType<MainManager>().LoadMainMenu();
        Time.timeScale = 1f;
    }
}
