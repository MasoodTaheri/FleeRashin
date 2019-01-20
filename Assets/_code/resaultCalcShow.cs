using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class resaultCalcShow : MonoBehaviour
{
    public static resaultCalcShow instance;
    public Text stars;
    public Text timer;
    public Text Rckets;
    public Text total;
    public Text PlaneHit;
    public Button doublestarbut;


    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void OnEnable()
    {
        stars.text = uiController.Instanse.get_Stars().ToString() + "*10";
        timer.text = uiController.Instanse.get_CuTime().ToString("f1") + " S";
        Rckets.text = uiController.Instanse.get_Rockethit().ToString() + "*10";
        PlaneHit.text = uiController.Instanse.get_PlaneHit().ToString() + "*20";
        total.text = PlayerDataClass.calcScore().ToString();
        doublestarbut.gameObject.SetActive(true);

        //doublestarbut.interactable = (uiController.Instanse.get_Stars() > 2);



    }

    // Update is called once per frame
    void Update()
    {

    }

}
