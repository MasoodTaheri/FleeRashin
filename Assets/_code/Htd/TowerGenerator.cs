using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public GameObject tower;
    public Vector2 towerPosRange;
    public int towerCount = 20;
    void Start()
    {
        for (int i = 0; i < towerCount; i++)
        {
            Vector3 newTowerPos = new Vector3(Random.Range(-towerPosRange.x / 2, towerPosRange.x / 2), tower.transform.position.y , Random.Range(-towerPosRange.y / 2, towerPosRange.y / 2));
            GameObject go = (GameObject)GameObject.Instantiate(tower, null);
            go.transform.position = newTowerPos;
            go.transform.localScale = tower.transform.localScale;
        }
    }
}
