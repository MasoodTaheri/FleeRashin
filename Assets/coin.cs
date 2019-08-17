using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public enum pickupable_enu { health, coin, rocket, bullet }
public abstract class PickupAbleABSClass : MonoBehaviourPun, IPunObservable
{
    public pickupable_enu type;
    public GameObject IconPrefab;
    public GameObject Icon;
    protected Image Iconimage;
    public GameObject CanvasRoot;
    private Vector3 oldpos;
    public static int range;

    static List<PickupAbleABSClass> Objs;


    public void Initialize()
    {
        if (Objs == null)
            Objs = new List<PickupAbleABSClass>();

        Objs.Add(this);
        /*Icon = Instantiate(IconPrefab) as GameObject;
        Icon.transform.SetParent(CanvasRoot.transform);
        Icon.GetComponent<RectTransform>().localScale = Vector3.one;
        Iconimage = Icon.GetComponent<Image>();*/
        Iconimage = FindObjectOfType<UIManager>().InstantiateIndicator(IconPrefab);
        //Debug.Log("PickupAbleABSClass count=" + Objs.Count);
        //range = _range;
    }

    public void Repos()
    {
        transform.position = new Vector3(Random.Range(-1 * range, range), -6, Random.Range(-1 * range, range));
    }

    //public static void ReposAll()
    //{
    //    foreach (PickupAbleABSClass obj in Objs)
    //        obj.Repos();
    //}


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
        if (stream.IsWriting)
        {
            if (oldpos != transform.position)
            {
                stream.SendNext(transform.position);
                //Debug.Log("writepos");
                oldpos = transform.position;
            }

        }
        else
        {
            //Debug.Log("readpos");
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("triggered but not master");
            return;
        }

        //Debug.Log("Trigger hit " + type.ToString());
        Repos();
    }


    public static void DestroyAll()
    {
        foreach (PickupAbleABSClass go in Objs)
            go.Destroy();
        Objs.Clear();
    }

    public void Destroy()//when game finished
    {
        Destroy(Icon);
        Destroy(Iconimage);
        Destroy(this.gameObject);
    }


    public void updateUI()
    {

    }


}


public class coin : PickupAbleABSClass
{

    // Use this for initialization
    void Start()
    {
        CanvasRoot = GameObject.FindObjectOfType<Pickupmanager2>().canvas;
        Initialize();
        if (PhotonNetwork.IsMasterClient)
            Repos();

    }

    //// Update is called once per frame
    void Update()
    {
        UIPOSClass.UIposArrow(transform.position, Iconimage);
        //if (PhotonNetwork.IsMasterClient)
        //    Debug.Log("I am masterClient");

        //Debug.Log(photonView.Owner.NickName);
    }
    void OnDestroy()
    {
        Destroy();
    }

}
