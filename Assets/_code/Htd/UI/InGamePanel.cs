using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BasePanel
{
    [SerializeField]
    Text starText;
    [SerializeField]
    Text timeText;
    [SerializeField]
    Text pingText;
    [SerializeField]
    Text missionText;
    [SerializeField]
    Image pingMeter;
    [SerializeField]
    Image secondaryWeaponImage;
    [SerializeField]
    Image secondaryWeaponCooldownImage;
    [SerializeField]
    Text secondaryWeaponAmmuCountText;
    [SerializeField]
    GameObject rightJoystick;
    [SerializeField]
    Text fpsText;
    [SerializeField]
    GameObject rocketIndicator;


    private void Start()
    {
        //test
        if(MissionManager.instance != null)
            MissionManager.instance.StartMissionManager();
    }

    public override void Show()
    {
        base.Show();
        PhotonManager photonManager = FindObjectOfType<PhotonManager>();
        //_luncher.pingMeter_text = pingText;
        //_luncher.pingMeter = pingMeter;
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
            fpsText.text = (int)m_lastFramerate + " FPS";
        }

    }
    //end test

    public void SetTime(string time)
    {
        timeText.text = time;
    }

    public void SetStar(string star)
    {
        starText.text = star;
    }

    public void SetPing(string ping)
    {
        pingText.text = ping;
    }

    public void SetMission(string mission)
    {
        missionText.text = mission;
    }

    public void OnPauseClick()
    {
        UIManager.instance.ChangePanel(PanelType.PauseMenu);
    }

    public void OnSecondaryWeaponClick()
    {
        if(LevelManager.Instance.PlayerPlane != null)
            LevelManager.Instance.PlayerPlane.ShootSecondaryWeapon();
    }

    //////////////////////

    public void SetSecondaryWeaponImage(Sprite sprite)
    {
        secondaryWeaponImage.sprite = sprite;
    }

    public void SetSecondaryWeaponCooldown(float value)
    {
        secondaryWeaponCooldownImage.fillAmount = value;
    }

    public void SetSecondaryWeaponAmmuCount(int count)
    {
        secondaryWeaponAmmuCountText.text = count.ToString();
    }

    //////////////////////

    public void SetRightJoystickActive(bool value)
    {
        rightJoystick.SetActive(value);
    }

    //////////////////////

    public void ShowRocketIndicator(float x, float y, float duration, Color color)
    {
        GameObject go = Instantiate(rocketIndicator, FindObjectOfType<UIManager>().ActiveCanvas.transform);
        GameObject.Destroy(go, duration);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(x * (1920 / 2f), y * (1080 / 2f));
        Vector3 eulerAngle = Vector3.zero;
        if (y < 0)
            eulerAngle.z = 180f;
        if (y == 0)
            eulerAngle.z = (x > 0) ? -90f : 90f;
        rt.eulerAngles = eulerAngle;
        go.GetComponent<Image>().color = color;
        rt.SetAsFirstSibling();
    }
}
