using UnityEngine;
using System.Collections;

public class followplane : MonoBehaviour //attached to camera
{
    public GameObject plane;
    public Vector3 delta;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (plane == null)
        {
            if (playermanager.PlanePlayer != null)
                plane = playermanager.PlanePlayer.gameObject;
            //GameObject.FindGameObjectWithTag("Playerbody");
            //if (plane.GetComponent<movement>() == null) plane = null;
            return;
        }
        transform.position = plane.transform.position + delta;

    }
}
