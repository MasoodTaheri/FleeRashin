using UnityEngine;
using UnityEngine.UI;
using System.Collections;




public class uiController : MonoBehaviour
{
    public static uiController Instanse;
    playermanager pm;
    [SerializeField]
    private Text timer;
    [SerializeField]
    private Text StarCounter;
    [SerializeField]
    private Text RocketHitLabel;
    [SerializeField]
    private Text PlaneHitLabel;

    public GameObject mainmenu;
    public GameObject Ingame;
    public GameObject Powerup;
    public GameObject waitingroom;



    private float CuTime;
    private int Stars;
    private int Rockethit;
    private int PlaneHit;
    float t = 0;
    public float get_CuTime() { return CuTime; }
    public int get_Stars() { return Stars; }
    public float get_Rockethit() { return Rockethit; }
    public float get_PlaneHit() { return PlaneHit; }

    // Use this for initialization
    void Start()
    {
        pm = GetComponent<playermanager>();
        Instanse = this;
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pm.outofgame)
        {
            CuTime = Time.time - t;
            timer.text = CuTime.ToString("f1");
        }

    }

    public void IncStarPickedup()
    {
        Stars++;
        StarCounter.text = Stars.ToString();
        PlayerDataClass.stars++;
    }
    public void DoubleStar()
    {
        PlayerDataClass.stars += Stars;
        Stars *= 2;

    }
    public void IncRockethit()
    {
        Rockethit++;
        RocketHitLabel.text = Rockethit.ToString();
    }


    public void IncPlaneHit()
    {
        PlaneHit++;
        PlaneHitLabel.text = PlaneHit.ToString();
    }


    public void resetUIelements()
    {
        Stars = 0;
        CuTime = 0;
        Rockethit = 0;
        PlaneHit = 0;
        StarCounter.text = Stars.ToString();
        timer.text = CuTime.ToString("f1");
        RocketHitLabel.text = Rockethit.ToString();
        PlaneHitLabel.text = PlaneHit.ToString();
        t = Time.time;
    }

    public void shopButtonType(int id)
    {
        Debug.Log("shopButtonType=" + id);
        if (id == 1)
        {//5 star for 5 flare
            if (PlayerDataClass.stars >= 5)
            {
                PlayerDataClass.stars -= 5;
                PlayerDataClass.Flare += 5;
                Debug.Log("inc 5 flare");
            }
        }
        else if (id == 2)
        {//10 star for 15 flare
            if (PlayerDataClass.stars >= 10)
            {
                PlayerDataClass.stars -= 10;
                PlayerDataClass.Flare += 15;
                Debug.Log("inc 15 flare");
            }
        }
        else if (id == 3)
        {//20 star for 30 flare
            if (PlayerDataClass.stars >= 20)
            {
                PlayerDataClass.stars -= 20;
                PlayerDataClass.Flare += 30;
                Debug.Log("inc 30 flare");
            }
        }

        if (id == 4)
        {//video for flare
            IAPandAD.showad("getstar");
        }

        PlayerDataClass.writedata();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
