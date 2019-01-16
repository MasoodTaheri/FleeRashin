using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContentDataClass
{
    public string name;
    public float Contentheight;
    public GameObject item;
    public float itemYpos;
    public bool _Default;
}

public class Contentsizer : MonoBehaviour
{
    public RectTransform Content;
    public List<ContentDataClass> Contentdata;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < Contentdata.Count; i++)
        {
            if (Contentdata[i]._Default)
            {
                setItemIndex(i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setItemIndex(int id)
    {
        for (int i = 0; i < Contentdata.Count; i++)
        {
            Contentdata[i].item.SetActive(false);
        }
        Contentdata[id].item.SetActive(true);
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, Contentdata[id].Contentheight);

        Vector3 pos = Contentdata[id].item.transform.localPosition;
        pos.y = Contentdata[id].itemYpos;
        Contentdata[id].item.GetComponent<RectTransform>().localPosition = pos;
    }
}
