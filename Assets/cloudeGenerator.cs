using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudeGenerator : MonoBehaviour
{
    public GameObject CloudePrefab;
    public Vector3 pivot;
    public float Radius;
    public float count;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = pivot + new Vector3(Random.Range(-Radius, Radius), 0, Random.Range(-Radius, Radius));
            GameObject go = Instantiate(CloudePrefab, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.eulerAngles = new Vector3(90, 0, 0);
            //go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0.0f, 1.0f));
            go.transform.localScale = Vector3.one * Random.Range(0.5f, 3.0f);

            go.transform.position = pos;
            go.transform.SetParent(this.transform);

        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
