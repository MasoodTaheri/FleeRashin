using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playermanager : MonoBehaviour
{
    public static DefaultPlayerPlane PlanePlayer;
    public GameObject playerPrefab;
    //public GameObject player;
    public GameObject MainMenu;
    public GameObject ingameMenu;
    public GameObject ResaultMenu;
    public bool outofgame = true;
    public AudioSource music;
    public List<AudioClip> InGameMusic;
    public float forwardSpeed;
    public float rotateSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlanePlayer == null)
        {
            //Debug.Log("11111111");
            if (!outofgame)
            {
                music.Stop();
                StartCoroutine(result_show());
                outofgame = true;
                if (PlayerPrefs.GetInt("LoosecountForAd", 0) > 6)
                {
                    PlayerPrefs.SetInt("LoosecountForAd", 0);
                    StartCoroutine(result_ad());
                }
                else
                {
                    PlayerPrefs.SetInt("LoosecountForAd", PlayerPrefs.GetInt("LoosecountForAd", 0) + 1);
                }

                //Debug.Log("LoosecountForAd=" + PlayerPrefs.GetInt("LoosecountForAd", 0));
            }

        }
        else
        {
            if (PlanePlayer.readytodestroy)
                PlanePlayer = null;
            else
                PlanePlayer.Update();
        }

    }
    public void startGame()
    {

        PlanePlayer = new DefaultPlayerPlane(forwardSpeed, rotateSpeed, -1, null, playerPrefab);
        music.clip = InGameMusic[Random.Range(0, InGameMusic.Count)];
        music.Play();


        //player = Instantiate(playerPrefab, new Vector3(0, -6, 0), Quaternion.identity) as GameObject;
        outofgame = false;
        uiController.Instanse.resetUIelements();
    }

    IEnumerator result_show()
    {
        yield return new WaitForSeconds(2.5f);
        ingameMenu.SetActive(false);
        ResaultMenu.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //IAPandAD.showad("");

    }

    IEnumerator result_ad()
    {
        yield return new WaitForSeconds(2f);
        IAPandAD.showad("");
    }


}
