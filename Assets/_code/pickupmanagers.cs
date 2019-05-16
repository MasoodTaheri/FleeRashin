using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class pickupmanagers : MonoBehaviour
{


    public GameObject root;
    public GameObject straPrefab;
    public GameObject repairPrefab;
    public GameObject straPrefabIcon;
    public GameObject repairPrefabIcon;
    public GameObject[] Collectableobjs = new GameObject[5];
    GameObject[] CollectableIcon = new GameObject[5];
    Image[] CollectableImage = new Image[5];
    //const float DestructableTime = 15;
    GameObject playerPlane;
    public Canvas canvas;
    public static pickupmanagers instance;


    public Vector3 pivot;
    public float Radius;
    //public float count;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPlane == null)
        {
            playerPlane = GameObject.FindWithTag("Playerbody");
            return;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Collectableobjs[i] == null)
            {
                generateItem((Random.Range(0, 100) > 75) ? straPrefab : repairPrefab, i);

                //if (!playermanager.PlanePlayer.ishit())
                //    generateItem(straPrefab, i);
                //else
                //    generateItem((Random.Range(0, 100) > 50) ? straPrefab : repairPrefab, i);
            }
            else
                UIPOSClass.UIposArrow(Collectableobjs[i].transform.position, CollectableImage[i]);
        }
    }

    private void generateItem(GameObject prefab, int idOfArray)
    {
        Collectableobjs[idOfArray] = Instantiate(straPrefab) as GameObject;
        Collectableobjs[idOfArray].transform.SetParent(root.transform);

        Destroy(CollectableIcon[idOfArray]);
        Destroy(CollectableImage[idOfArray]);
        CollectableIcon[idOfArray] = Instantiate(straPrefabIcon) as GameObject;
        CollectableIcon[idOfArray].transform.SetParent(canvas.transform, false);
        CollectableImage[idOfArray] = CollectableIcon[idOfArray].GetComponent<Image>();

        Collectableobjs[idOfArray].transform.position = playerPlane.transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

    }
}
