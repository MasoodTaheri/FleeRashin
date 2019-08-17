using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MiniMapObject
{
    public string tag;
    public Sprite texture;
}

public class MiniMap : MonoBehaviour
{
    public Vector2 range;
    public Vector2 maxMiniMapSize;
    float coefficient;
    public MiniMapObject[] objects = new MiniMapObject[0];
    [Range(1f,10f)]
    public float iconSize = 1f;
    Vector2 minIconSize = new Vector2(5f, 5f);
    public Image iconPrefab;
    RectTransform rt;
    bool isRoadDrawn;
    public Sprite roadSprite;

    List<GameObject> planeList;
    List<Image> planeIconList;
    List<GameObject> rocketList;
    List<Image> rocketIconlist;
    List<GameObject> towerList;
    List<Image> towerIconlist;
    List<GameObject> truckList;
    List<Image> truckIconlist;
    List<GameObject> bossList;
    List<Image> bossIconlist;


    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        planeList = new List<GameObject>();
        planeIconList = new List<Image>();
        rocketList = new List<GameObject>();
        rocketIconlist = new List<Image>();
        towerList = new List<GameObject>();
        towerIconlist = new List<Image>();
        truckList = new List<GameObject>();
        truckIconlist = new List<Image>();
        bossList = new List<GameObject>();
        bossIconlist = new List<Image>();
    }

    private void OnEnable()
    {
        if (range.x / maxMiniMapSize.x > range.y / maxMiniMapSize.y)
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, range.y / range.x * maxMiniMapSize.x);
        else
            rt.sizeDelta = new Vector2(range.x / range.y * maxMiniMapSize.y, rt.sizeDelta.y);

        coefficient = rt.sizeDelta.x / range.x / 2f;

        if (!isRoadDrawn)
            DrawRoad();
    }

    private void Update()
    {
        SetItemIcon(rocketList, rocketIconlist, "Rocket");
        SetItemIcon(planeList, planeIconList, "Playerbody");
        SetItemIcon(towerList, towerIconlist, "Tower");
        SetItemIcon(truckList, truckIconlist, "Vehicle");
        SetItemIcon(bossList, bossIconlist, "Boss");
    }

    void DrawRoad()
    {
        VehicleWaypoints waypoints = FindObjectOfType<VehicleWaypoints>();
        if (waypoints == null)
            return;

        Vector3 differenceVector;
        Vector2 pointA;
        Vector2 pointB;
        for (int i = 0; i < waypoints.waypoints.Length-1; i++)
        {
            pointA = new Vector2(waypoints.waypoints[i].transform.position.x, waypoints.waypoints[i].transform.position.z) * coefficient;
            pointB = new Vector2(waypoints.waypoints[i + 1].transform.position.x, waypoints.waypoints[i + 1].transform.position.z) * coefficient;
            differenceVector = pointB - pointA;

            Image roadPiece = Instantiate(iconPrefab);
            roadPiece.gameObject.name = "RoadPiece_" + i.ToString();
            roadPiece.rectTransform.SetParent(transform,false);
            roadPiece.rectTransform.pivot = new Vector2(0, 0.5f);
            roadPiece.rectTransform.localPosition = new Vector3(pointA.x, pointA.y, 0);
            roadPiece.rectTransform.sizeDelta = new Vector2(differenceVector.magnitude , 15);
            roadPiece.sprite = roadSprite;

            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            roadPiece.rectTransform.eulerAngles = new Vector3(0, 0, angle);
        }

        if (waypoints.loop)
        {
            pointA = new Vector2(waypoints.waypoints[waypoints.waypoints.Length -1].transform.position.x, waypoints.waypoints[waypoints.waypoints.Length - 1].transform.position.z) * coefficient;
            pointB = new Vector2(waypoints.waypoints[0].transform.position.x, waypoints.waypoints[0].transform.position.z) * coefficient;
            differenceVector = pointB - pointA;

            Image roadPiece = Instantiate(iconPrefab);
            roadPiece.gameObject.name = "RoadPiece_" + (waypoints.waypoints.Length - 1).ToString();
            roadPiece.rectTransform.SetParent(transform, false);
            roadPiece.rectTransform.pivot = new Vector2(0, 0.5f);
            roadPiece.rectTransform.localPosition = new Vector3(pointA.x, pointA.y, 0);
            roadPiece.rectTransform.sizeDelta = new Vector2(differenceVector.magnitude, 15);
            roadPiece.sprite = roadSprite;

            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            roadPiece.rectTransform.eulerAngles = new Vector3(0, 0, angle);
        }

        isRoadDrawn = true;
    }

    void SetItemIcon(List<GameObject> itemList, List<Image> itemIconList, string tag)
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject item in tempArray)
        {
            if (!itemList.Contains(item))
            {
                Image icon = Instantiate(iconPrefab);
                icon.transform.SetParent(transform, false);

                foreach (MiniMapObject mMObject in objects)
                {
                    if (mMObject.tag == tag)
                    {
                        icon.sprite = mMObject.texture;
                    }
                }

                if (tag == "Playerbody")
                {
                    DefaultPlayerPlane temp = item.GetComponent<DefaultPlayerPlane>();
                    if (temp != null)
                        icon.color = temp.GetMyColor();
                }

                itemList.Add(item);
                itemIconList.Add(icon);
            }
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
            {
                GameObject.Destroy(itemIconList[i].gameObject);
                itemList.RemoveAt(i);
                itemIconList.RemoveAt(i--);
                continue;
            }

            itemIconList[i].rectTransform.sizeDelta = minIconSize * iconSize;
            itemIconList[i].rectTransform.localPosition = new Vector2(itemList[i].transform.position.x, itemList[i].transform.position.z) * coefficient;


            itemIconList[i].rectTransform.eulerAngles = new Vector3(0, 0, -itemList[i].transform.eulerAngles.y);

            
        }
    }
}
