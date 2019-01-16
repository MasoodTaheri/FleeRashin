using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class posinscreen : MonoBehaviour
{

    public Camera cam;
    public Vector3 ret;
    public Image img;
    public Vector2 imppos;

    public Vector3 targetDir;
    public float angle;
    // Use this for initialization
    void Start()
    {
        Debug.Log(Screen.width + "  " + Screen.height);
        Debug.Log(transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        //img.transform.position = RocketUIpos(this.transform.position);
        //RocketUIpos(this.transform.position, img);



    }


}
