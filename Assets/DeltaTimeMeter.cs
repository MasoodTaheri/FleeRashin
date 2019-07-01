using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System;


public class DeltaTimeMeter : MonoBehaviour
{
    public Sprite Bg;
    public Text errorpostxt;
    public Text Lagtxt;
    public Text Ismaster;
    public float errorinx = 0;
    public float erroriny = 0;
    public float lag = 0;
    public GameObject leftbox;
    public GameObject RightBox;
    private networkRigidbody2[] codes;
    //public networkRigidbody2 Playercode;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("GetData", 3, 2);
        //Debug.Log("Call GetData");
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Playercode != null)
        //{
        //    errorpostxt.text = ((Playercode.ErrorInPos.x + Playercode.ErrorInPos.y) / 2).ToString();
        //    Lagtxt.text = (Playercode.lag).ToString();
        //}
        //else
        //{
        //    if (playermanager.PlanePlayer != null)
        //    {
        //        Playercode = playermanager.PlanePlayer.GetComponent<networkRigidbody2>();
        //    }
        //}
    }


    public void GetData()
    {
        //Debug.Log("GetData");
        errorinx = 0;
        erroriny = 0;
        lag = 0;
        codes = GameObject.FindObjectsOfType<networkRigidbody2>();
        //Debug.Log("codes length=" + codes.Length);
        if (codes.Length == 0) return;

        foreach (var item in codes)
        {
            errorinx += item.ErrorInPos.x;
            erroriny += item.ErrorInPos.y;
            lag += item.lag;
        }

        //errorinx /= codes.Length;
        //erroriny /= codes.Length;
        //lag /= codes.Length;

        //errorpostxt.text = errorinx.ToString();
        errorpostxt.text = string.Format("{0:f2}", errorinx);
        Lagtxt.text = string.Format("{0:f2}", lag);
        Ismaster.text = (PhotonNetwork.IsMasterClient) ? "Master" : "";
        leftbox.transform.localPosition = new Vector3(-1 * errorinx / 2.0f, 0, 0);
        RightBox.transform.localPosition = new Vector3(errorinx / 2.0f, 0, 0);

        //if (PhotonNetwork.GetPing() > 300)
        //if (Input.GetKey(KeyCode.P))
        WriteToFile();
    }


    private string AllData;
    private void WriteToFile()
    {
        string str = string.Format("t={0}\n", DateTime.Now);
        foreach (var item in codes)
        {
            str += string.Format("{0},Realpos,{1},{2},{3},Cupos,{4},{5},{6}\n",
                item.pv.ViewID.ToString(),
                item.pos.x,
                item.pos.y,
                item.pos.z,
                item.gameObject.transform.position.x,
                item.gameObject.transform.position.y,
                item.gameObject.transform.position.z);
        }

        AllData += str;
        Debug.Log(AllData);
        File.WriteAllText(Application.persistentDataPath + "\\Report.csv", AllData);
    }


}
