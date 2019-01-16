using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuManager : MonoBehaviour
{
    public Text stars;
    //public Text flare;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        stars.text = PlayerDataClass.stars.ToString();
        //flare.text = PlayerDataClass.Flare.ToString();
    }

    void OnEnable()
    {

    }
}
