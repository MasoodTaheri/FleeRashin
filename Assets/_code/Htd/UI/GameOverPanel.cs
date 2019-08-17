using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    [SerializeField]
    Text starText;
    [SerializeField]
    Text timeText;
    [SerializeField]
    Text RocketText;
    [SerializeField]
    Text PlaneText;
    [SerializeField]
    Text TotalScoreText;

    public void SetResultData(string star, string time, string rocket, string plane, string totalScore)
    {
        starText.text = star;
        timeText.text = time;
        RocketText.text = rocket;
        PlaneText.text = plane;
        TotalScoreText.text = totalScore;
    }

    public void OnHomeClick()
    {
        FindObjectOfType<MainManager>().LoadMainMenu();
    }
}
