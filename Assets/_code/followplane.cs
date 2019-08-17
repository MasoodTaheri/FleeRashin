using UnityEngine;
using System.Collections;

public class followplane : MonoBehaviour //attached to camera
{
    enum CameraState { Simple, Boss1, Boss2 }

    public GameObject plane;
    public Vector3 delta;
    [Range(0,5)]
    public float range;
    public GameObject secondTarget;
    [Range(0, 20)]
    public float secondTargetRange;
    public Vector2 widthThresold;
    public Vector2 heightThresold;
    [Range(0, 20)]
    public float thirdTargetRange;
    public Vector2 zoomRange;
    [Range(5,15)]
    public float alternativeZoom = 5;

    //bool inSecondTargetRange = false;
    Vector3 targetPos = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    CameraState state = CameraState.Simple;

    private void Start()
    {
        widthThresold = new Vector2(50f, Screen.width - 50f);
        heightThresold = new Vector2(50f, Screen.height - 50f);
    }

    void FixedUpdate()
    {
        if (plane == null)
        {
            plane = GameObject.FindGameObjectWithTag("Playerbody");
            //inSecondTargetRange = false;
            return;
        }

        delta = new Vector3(range * Mathf.Sin(plane.transform.eulerAngles.y * Mathf.Deg2Rad), delta.y, range * Mathf.Cos(plane.transform.eulerAngles.y * Mathf.Deg2Rad));
        targetPos = plane.transform.position + delta;

        if (secondTarget == null || !secondTarget.activeInHierarchy)
        {
            //inSecondTargetRange = false;
            state = CameraState.Simple;
        }
        else if (secondTarget != null && secondTarget.activeInHierarchy && Vector3.Distance(plane.transform.position, secondTarget.transform.position) < secondTargetRange)
        {
            if (state != CameraState.Boss1 && state != CameraState.Boss2)
                state = CameraState.Boss1;  
            //inSecondTargetRange = true;
        }

        switch (state)
        {
            case CameraState.Simple:
                break;
            case CameraState.Boss1:
                
                if (Vector3.Distance(plane.transform.position, secondTarget.transform.position) > secondTargetRange)
                {
                    targetPos = secondTarget.transform.position + (-secondTarget.transform.position + plane.transform.position).normalized * secondTargetRange + delta;

                    Vector2 screenPosition = Camera.main.WorldToScreenPoint(plane.transform.position);

                    if (screenPosition.x < widthThresold.x && Vector3.Angle(plane.transform.forward, transform.up) > 3f && Vector3.Angle(plane.transform.forward, -transform.up) > 3f)
                    {
                        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
                        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
                    }
                    if (screenPosition.x > widthThresold.y && Vector3.Angle(plane.transform.forward, transform.up) > 3f && Vector3.Angle(plane.transform.forward, -transform.up) > 3f)
                    {
                        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
                        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
                    }
                    if (screenPosition.y < heightThresold.x && Vector3.Angle(plane.transform.forward, transform.right) > 3f && Vector3.Angle(plane.transform.forward, -transform.right) > 3f)
                    {
                        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
                        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
                    }
                    if (screenPosition.y > heightThresold.y && Vector3.Angle(plane.transform.forward, transform.right) > 3f && Vector3.Angle(plane.transform.forward, -transform.right) > 3f)
                    {
                        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
                        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
                    }

                    //if (Vector3.Distance(plane.transform.position, secondTarget.transform.position) > thirdTargetRange)
                    {
                        float size = Vector3.Distance(plane.transform.position, secondTarget.transform.position) - secondTargetRange;
                        size = size / (thirdTargetRange - secondTargetRange);
                        Camera.main.orthographicSize = (zoomRange.y - zoomRange.x) * size + zoomRange.x;
                    }
                
                }

                break;
            case CameraState.Boss2:
                
                if (Camera.main.orthographicSize < alternativeZoom)
                {
                    Camera.main.orthographicSize += Time.fixedDeltaTime;
                    zoomRange = new Vector2(Camera.main.orthographicSize, zoomRange.y);
                }
                else
                {
                    state = CameraState.Boss1;
                    zoomRange.x = alternativeZoom;
                }

                break;
            default:
                break;
        }
        
        //if (inSecondTargetRange && Vector3.Distance(plane.transform.position, secondTarget.transform.position) > secondTargetRange)
        //{
        //    targetPos = secondTarget.transform.position + (-secondTarget.transform.position + plane.transform.position).normalized * secondTargetRange + delta;

        //    Vector2 screenPosition = Camera.main.WorldToScreenPoint(plane.transform.position);

        //    if (screenPosition.x < widthThresold.x && Vector3.Angle(plane.transform.forward, transform.up) > 3f && Vector3.Angle(plane.transform.forward, -transform.up) > 3f)
        //    {
        //        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
        //        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
        //    }
        //    if (screenPosition.x > widthThresold.y && Vector3.Angle(plane.transform.forward, transform.up) > 3f && Vector3.Angle(plane.transform.forward, -transform.up) > 3f)
        //    {
        //        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
        //        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
        //    }
        //    if (screenPosition.y < heightThresold.x && Vector3.Angle(plane.transform.forward, transform.right) > 3f && Vector3.Angle(plane.transform.forward, -transform.right) > 3f)
        //    {
        //        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
        //        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
        //    }
        //    if (screenPosition.y > heightThresold.y && Vector3.Angle(plane.transform.forward, transform.right) > 3f && Vector3.Angle(plane.transform.forward, -transform.right) > 3f)
        //    {
        //        Vector3 temp = Vector3.Cross(plane.transform.forward, secondTarget.transform.position - plane.transform.position);
        //        plane.GetComponent<DefaultPlayerPlane>().ForceRotate(temp.y < 0, temp.y > 0);
        //    }

        //    //if (Vector3.Distance(plane.transform.position, secondTarget.transform.position) > thirdTargetRange)
        //    {
        //        float size = Vector3.Distance(plane.transform.position, secondTarget.transform.position) - secondTargetRange;
        //        size = size / (thirdTargetRange - secondTargetRange);
        //        Camera.main.orthographicSize = (zoomRange.y - zoomRange.x) * size + zoomRange.x;
        //    }

        //}

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.3f);
        //transform.position = targetPos;
    }

    public void ZoomOut()
    {
        state = CameraState.Boss2;
    }
}
