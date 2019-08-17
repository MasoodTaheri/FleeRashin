using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineInGamepanel : BasePanel
{
    [SerializeField]
    Text pingText;
    [SerializeField]
    Text timeText;
    [SerializeField]
    Text coinText;
    [SerializeField]
    Text rocketText;
    [SerializeField]
    Text planeText;
    [SerializeField]
    Text remainingRocketText;
    [SerializeField]
    Text remainingFlareText;
    [SerializeField]
    Text fpsText;
    [SerializeField]
    Image pingSprite;
    [SerializeField]
    Image rocketReloadSprite;
    [SerializeField]
    Image flareReloadSprite;

    private void Reset()
    {
        type = PanelType.OnlineInGame;
    }

    // test
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    float m_refreshTime = 0.5f;

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
            SetFPS((int)m_lastFramerate);
        }

    }
    //end test

    public void OnPauseClick()
    {
    }

    public void OnLeftClick()
    {
    }

    public void OnRightClick()
    {
    }

    public void OnShootClick()
    {
    }

    public void OnRocketClick()
    {
    }

    public void OnFlareClick()
    {
    }

    public void SetRemainingRocket(int count)
    {
        remainingRocketText.text = count.ToString();
    }

    public void SetRocketReloadState(float value)
    {
        rocketReloadSprite.fillAmount = value;
    }

    public void SetRemainingFlare(int count)
    {
        remainingFlareText.text = count.ToString();
    }

    public void SetFlareReloadState(float value)
    {
        flareReloadSprite.fillAmount = value;
    }

    public void SetTimeText(int timeInSec)
    {
        timeText.text = timeInSec.ToString();
    }

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void SetRocketText(int rocket)
    {
        rocketText.text = rocket.ToString();
    }

    public void SetPlaneText(int plane)
    {
        planeText.text = plane.ToString();
    }

    public void SetPingText(int ping, Color color)
    {
        pingText.text = ping.ToString();
        pingSprite.color = color;
    }

    public void SetFPS(int fps)
    {
        fpsText.text = fps.ToString() + " FPS";
    }
}
