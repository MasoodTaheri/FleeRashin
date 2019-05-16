using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pickupmanager2 : MonoBehaviour
{


    public GameObject Coin;
    public int CoinCount;
    public GameObject canvas;
    public int range;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateItem()
    {
        PickupAbleABSClass.range = range;
        for (int i = 0; i < CoinCount; i++)
        {
            GameObject go = PhotonNetwork.InstantiateSceneObject(Coin.name, Vector3.zero,
                Quaternion.Euler(90, 0, 0)) as GameObject;
            //go.GetComponent<coin>().CanvasRoot = canvas;
            //go.GetComponent<coin>().Initialize(range);
        }

        //PickupAbleABSClass.ReposAll();
    }

    public void RemoveAll()
    {
        PickupAbleABSClass.DestroyAll();
    }
}
