using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public static class PlayerDataClass
{
    private static int _stars;
    public static int stars
    {
        get
        {
            if (!isinit) init();
            return _stars;
        }
        set
        {
            if (!isinit) init();
            _stars = value;
        }
    }

    private static int _Score;
    public static int Score
    {
        get
        {
            if (!isinit) init();
            return _Score;
        }
        set
        {
            if (!isinit) init();
            _Score = value;
        }
    }

    private static int _Flare;
    public static int Flare
    {
        get
        {
            if (!isinit) init();
            return _Flare;
        }
        set
        {
            if (!isinit) init();
            _Flare = value;
        }
    }



	private static int _Planehit;
	public static int Planehit
	{
		get
		{
			if (!isinit) init();
			return _Planehit;
		}
		set
		{
			if (!isinit) init();
			_Planehit = value;
		}
	}


    private static bool isinit = false;
    public static void init()
    {
        isinit = true;
        stars = PlayerPrefs.GetInt("stars", 0);
        Score = PlayerPrefs.GetInt("Score", 0);
		Flare = PlayerPrefs.GetInt("Flare", 0);
		Planehit = PlayerPrefs.GetInt("Planehit", 0);
        
    }

    public static int calcScore()
    {
        int sc = Mathf.RoundToInt(
            uiController.Instanse.get_Stars() * 10 +
            uiController.Instanse.get_CuTime() +
             uiController.Instanse.get_Rockethit() * 10+
             uiController.Instanse.get_PlaneHit() * 20);
        Score += sc;
        writedata();

        return sc;
    }

    public static void writedata()
    {
        PlayerPrefs.SetInt("stars", stars);
        PlayerPrefs.SetInt("Score", Score);
		PlayerPrefs.SetInt("Flare", Flare);
		PlayerPrefs.SetInt("Planehit", Planehit);
    }
}
