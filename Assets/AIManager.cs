using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject FighterPrefab;
    public GameObject iconPrefab;
    private GameObject[] EnemyCoincollector;
    private Image[] EnemyIconCoincollector;
    private GameObject[] EnemyFighter;
    private Image[] EnemyIconFighter;
    public Canvas canvas;
    public int CoinCollectorCount;
    public int FighterCount;

    // Use this for initialization
    void Start()
    {
        EnemyCoincollector = new GameObject[CoinCollectorCount];
        EnemyIconCoincollector = new Image[CoinCollectorCount];
        EnemyFighter = new GameObject[FighterCount];
        EnemyIconFighter = new Image[FighterCount];
        //GenerateAI(CoinCollectorCount, FighterCount);
        //        StartCoroutine(regenerateMissedPlane());
    }

    // Update is called once per frame
    void Update()
    {
        if (playermanager.PlanePlayer == null)
        {
            for (int i = 0; i < EnemyCoincollector.Length; i++)
                if (EnemyCoincollector[i] != null)
                    Destroy(EnemyCoincollector[i].gameObject);

            for (int i = 0; i < EnemyFighter.Length; i++)
                if (EnemyFighter[i] != null)
                    Destroy(EnemyCoincollector[i].gameObject);

            return;
        }

        GenerateAI(CoinCollectorCount, FighterCount);

        for (int i = 0; i < EnemyCoincollector.Length; i++)
            if (EnemyCoincollector[i] != null && EnemyIconCoincollector[i] != null)
                UIPOSClass.UIposArrow(EnemyCoincollector[i].transform.position, EnemyIconCoincollector[i]);


        for (int i = 0; i < EnemyFighter.Length; i++)
            if (EnemyFighter[i] != null && EnemyIconFighter[i] != null)
                UIPOSClass.UIposArrow(EnemyFighter[i].transform.position, EnemyIconFighter[i]);

    }

    private void GenerateAI(int coinCollector, int Fighter)
    {
        for (int i = 0; i < CoinCollectorCount; i++)
            if (EnemyCoincollector[i] == null)
            {
                if (EnemyIconCoincollector[i] != null)
                    Destroy(EnemyIconCoincollector[i].gameObject);
                InstatiateAI(true, false, i);
            }

        for (int i = 0; i < FighterCount; i++)
            if (EnemyFighter[i] == null)
            {
                if (EnemyIconFighter[i] != null)
                    Destroy(EnemyIconFighter[i].gameObject);
                InstatiateAI(false, true, i);
            }



        //for (int i = 0; i < coinCollector; i++)
        //    InstatiateAI(true, false, i);
        //for (int i = 0; i < Fighter; i++)
        //    InstatiateAI(false, true, i);
    }

    private void InstatiateAI(bool coinCollector, bool fighter, int ArrayId)
    {
        //Enemy = Instantiate(enemyPrefab, new Vector3(-7.78f, -6, 0.27f), Quaternion.identity);

        GameObject Enemy = Instantiate(fighter ? FighterPrefab : enemyPrefab, Vector3.zero, Quaternion.identity);
        Enemy.transform.position = new Vector3(Random.Range(-50, 50), -6, Random.Range(-50, 50));
        GameObject tmp = Instantiate(iconPrefab) as GameObject;
        Image icon = tmp.GetComponent<Image>();
        tmp.transform.SetParent(canvas.transform, false);


        Enemy.GetComponent<EnemyPlane>().coinCollector = coinCollector;
        Enemy.GetComponent<EnemyPlane>().fightWithPlayer = fighter;

        if (coinCollector)
        {
            EnemyCoincollector[ArrayId] = Enemy;
            EnemyIconCoincollector[ArrayId] = icon;
        }
        else
        {
            EnemyFighter[ArrayId] = Enemy;
            EnemyIconFighter[ArrayId] = icon;
        }
    }

    //IEnumerator regenerateMissedPlane()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(10);
    //        for (int i = 0; i < CoinCollectorCount; i++)
    //            if (EnemyCoincollector[i] == null)
    //            {
    //                Destroy(EnemyIconCoincollector[i].gameObject);
    //                InstatiateAI(true, false, i);
    //            }
    //        for (int i = 0; i < FighterCount; i++)
    //            if (EnemyFighter[i] == null)
    //            {
    //                Destroy(EnemyIconFighter[i].gameObject);
    //                InstatiateAI(false, true, i);
    //            }
    //    }

    //}
}
