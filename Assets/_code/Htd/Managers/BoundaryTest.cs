using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryTest : MonoBehaviour
{
    public bool checkVertexes;
    public bool isInPolygon;
    List<GameObject> vertexes = new List<GameObject>();
    Vector3 medianPoint;

    private void OnDrawGizmos()
    {
        if (checkVertexes)
        {
            checkVertexes = false;
            CheckVertexes();
        }
        
        if (vertexes.Count > 0)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < vertexes.Count; i++)
                Gizmos.DrawLine(vertexes[i].transform.position, vertexes[(i + 1) % vertexes.Count].transform.position);

            isInPolygon = false;

            for (int i = 0; i < vertexes.Count; i++)
            {
                isInPolygon |= PointInTriangle(transform.position, vertexes[i].transform.position, vertexes[(i + 1) % vertexes.Count].transform.position, medianPoint);
            }
        }
    }

    void CheckVertexes()
    {
        vertexes.Clear();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("EditorOnly");
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
            medianPoint += temp[i].transform.position;
        }

        medianPoint /= temp.Length;
        vertexes.Add(tempList[0]);
        tempList.RemoveAt(0);

        while (tempList.Count > 0)
        {
            int vindex = 0;

            for (int i = 0; i < tempList.Count; i++)
            {
                if (Vector3.Distance(vertexes[vertexes.Count - 1].transform.position, tempList[i].transform.position) <
                    Vector3.Distance(vertexes[vertexes.Count - 1].transform.position, tempList[vindex].transform.position))
                {
                    vindex = i;
                }
            }
            
            vertexes.Add(tempList[vindex]);
            tempList.RemoveAt(vindex);
        }
            
    }


    bool PointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = sign(pt, v1, v2);
        d2 = sign(pt, v2, v3);
        d3 = sign(pt, v3, v1);

        has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }

    float sign(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1.x - p3.x) * (p2.z - p3.z) - (p2.x - p3.x) * (p1.z - p3.z);
    }
}
