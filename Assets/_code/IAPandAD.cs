using UnityEngine;
using System.Collections;
//using TapsellSDK;
using System.Linq;


[System.Serializable]
public class IAPandAD : MonoBehaviour
{
    //TapsellAd ad;
    static IAPandAD instance;
    public string whyad;

    // Use this for initialization
    void Start()
    {
        instance = this;
        Debug.Log("Tapsell init");
        //Tapsell.initialize("tdprasekkplckdgtrjgkpabheeasdksissbsidlmsholciqjqpfselhregemjrafgconid");
        Debug.Log("Tapsell init2");
        //Tapsell.setRewardListener(OnFinishAd);

        //requestAd();
    }

    public static void showad(string why)
    {
        if (instance != null)
            instance.showAD(why);
    }

    public void showAD(string why)
    {
        whyad = why;
        Debug.Log("showad   whyad=" + whyad);
        //TapsellShowOptions showOptions = new TapsellShowOptions();
        //showOptions.backDisabled = false;
        //showOptions.immersiveMode = false;
        //showOptions.rotationMode = TapsellShowOptions.ROTATION_UNLOCKED;
        //showOptions.showDialog = true;
        //Tapsell.showAd(IAPandAD.instance.ad, showOptions);
    }

    [ContextMenu("CheckOnFinishAd")]
    public void CheckOnFinishAd()
    {
        whyad = "doubleStar";
        Debug.Log("CheckOnFinishAd   whyad=" + whyad);
        if (whyad == "doubleStar")
        {
            uiController.Instanse.DoubleStar();
            resaultCalcShow.instance.OnEnable();
            //resaultCalcShow.instance.doublestarbut.enabled = false;
        }

        if (whyad == "getstar")
        {
            PlayerDataClass.stars++;
            PlayerDataClass.writedata();
        }
        whyad = "";
    }

    //public void OnFinishAd(TapsellAdFinishedResult result)
    //{
    //    // you may give rewards to user if result.completed and
    //    // result.rewarded are both true
    //    Debug.Log("OnFinishAd   whyad=" + whyad);
    //    if (result.completed)
    //    {
    //        Debug.Log("ShowAd");
    //        if (whyad == "doubleStar")
    //        {
    //            uiController.Instanse.DoubleStar();
    //            resaultCalcShow.instance.OnEnable();
    //            //resaultCalcShow.instance.doublestarbut.enabled = false;

    //        }
    //        if (whyad == "getflare")
    //        {
    //            PlayerDataClass.Flare++;
    //            PlayerDataClass.writedata();
    //        }
    //        if (whyad == "getstar")
    //        {
    //            PlayerDataClass.stars++;
    //            PlayerDataClass.writedata();
    //        }
    //    }
    //    whyad = "";
    //    requestAd();
    //}

    //public void requestAd()
    //{
    //    Debug.Log("requestAd");
    //    Tapsell.requestAd("59f60f88468465181fe1e8c3", true,
    //     (TapsellAd result) =>
    //     {
    //         // onAdAvailable
    //         Debug.Log("Action: onAdAvailable");
    //         ad = result; // store this to show the ad later
    //     },

    //     (string zoneId) =>
    //     {
    //         // onNoAdAvailable
    //         Debug.Log("No Ad Available");
    //     },

    //     (TapsellError error) =>
    //     {
    //         // onError
    //         Debug.Log(error.error);
    //     },

    //     (string zoneId) =>
    //     {
    //         // onNoNetwork
    //         Debug.Log("No Network");
    //     },

    //     (TapsellAd result) =>
    //     {
    //         // onExpiring
    //         Debug.Log("Expiring");
    //         // this ad is expired, you must download a new ad for this zone
    //     }
    // );
    //}

    // Update is called once per frame
    void Update()
    {

    }


}
