using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    [SerializeField]
    Image loadingImage;
    [SerializeField]
    Text pingText;
    [SerializeField]
    Image pingSprite;

    public override void Show()
    {
        base.Show();
        //FindObjectOfType<luncher>().loading = loadingImage;
    }

    public void OnStartOfflineClick()
    {
        FindObjectOfType<MainManager>().StartGameOffline();
    }

    public void OnStartOnlineClick()
    {
        //FindObjectOfType<playermanager>().startGame();
        FindObjectOfType<MainManager>().ShowOnlineRoomSelection();
    }

    public void SetPingText(int ping, Color color)
    {
        pingText.text = ping.ToString();
        pingSprite.color = color;
    }
}
