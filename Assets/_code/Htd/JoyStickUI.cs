using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickUI : MonoBehaviour
{
    [SerializeField][Range(0,100)]
    float joystickMaxDistance;
    [SerializeField][Range(0,1)]
    float joystickSensitivity;
    [SerializeField]
    Image container;
    [SerializeField]
    Image joystick;
    [SerializeField][Range(0,5)]
    float minAngleDiffrence;
    [SerializeField]
    bool isLeft = true;
    Vector2 startPos;
    bool isJoystickActive;
    Rect acceptableSpace;
    float fadeDuration = 0.3f;
    float joystickOpacity;

    float angle;
    Vector2 pos;
    int touchIndex;
    bool isThereATouch;

    private void Start()
    {
        if(isLeft)
            acceptableSpace = new Rect(0, 0, Screen.width / 2, Screen.height - joystickMaxDistance);
        else
            acceptableSpace = new Rect(Screen.width / 2 , 0, Screen.width / 2, Screen.height - joystickMaxDistance);

        joystickOpacity = 0;
        container.color = joystick.color = new Color(1, 1, 1, joystickOpacity);
    }

    private void Update()
    {
        CheckTouches();

        if (!isJoystickActive && isThereATouch /*Input.touchCount > 0 && acceptableSpace.Contains(Input.GetTouch(0).position)*/)
        {
            isJoystickActive = true;
            startPos = Input.GetTouch(touchIndex).position;
            ShowJoystick();
        }
        else if (isJoystickActive && isThereATouch /*Input.touchCount > 0*/)
        {
            Vector2 joystickPos = Input.GetTouch(touchIndex).position;
            joystickPos = (Vector2.Distance(joystickPos, startPos) > joystickMaxDistance * joystickSensitivity) ? startPos + (joystickPos - startPos).normalized * joystickMaxDistance : startPos;
            joystick.rectTransform.anchoredPosition = new Vector2(joystickPos.x / Screen.width * 1920f, joystickPos.y / Screen.height * 1080f);

            if (Vector2.Distance(joystickPos, startPos) > joystickMaxDistance * joystickSensitivity)
                CalculateJoystickValue(joystickPos);
        }
        else if (isJoystickActive && !isThereATouch /*Input.touchCount == 0*/)
        {
            isJoystickActive = false;

            if (LevelManager.Instance.PlayerPlane != null)
            {
                if (isLeft)
                    LevelManager.Instance.PlayerPlane.SteerLeft(0);
                else
                    LevelManager.Instance.PlayerPlane.SteerRight(0);
            }
        }

        SetJoystickColor();
    }

    void CheckTouches()
    {
        isThereATouch = false;

        switch (Input.touchCount)
        {
            case 1:

                if (acceptableSpace.Contains(Input.GetTouch(0).position))
                {
                    touchIndex = 0;
                    isThereATouch = true;
                }
                break;
            case 2:

                if (acceptableSpace.Contains(Input.GetTouch(0).position))
                {
                    touchIndex = 0;
                    isThereATouch = true;
                }
                else if (acceptableSpace.Contains(Input.GetTouch(1).position))
                {
                    touchIndex = 1;
                    isThereATouch = true;
                }
                break;
            default:
                break;
        }
    }

    void ShowJoystick()
    {
        container.rectTransform.anchoredPosition = new Vector2(startPos.x / Screen.width * 1920f, startPos.y / Screen.height * 1080);
        joystick.rectTransform.anchoredPosition = new Vector2(startPos.x / Screen.width * 1920f, startPos.y / Screen.height * 1080);
    }

    void SetJoystickColor()
    {
        if (isJoystickActive && joystickOpacity < 0.5f)
        {
            joystickOpacity += Time.deltaTime / fadeDuration;
            joystickOpacity = (joystickOpacity > 0.5f) ? 0.5f : joystickOpacity;
            container.color = joystick.color = new Color(1, 1, 1, joystickOpacity);
        }

        if (!isJoystickActive && joystickOpacity > 0)
        {
            joystickOpacity -= Time.deltaTime / fadeDuration;
            joystickOpacity = (joystickOpacity < 0) ? 0 : joystickOpacity;
            container.color = joystick.color = new Color(1, 1, 1, joystickOpacity);
        }
    }

    void CalculateJoystickValue(Vector2 currentPos)
    {
        if (currentPos.x - startPos.x != 0)
        {
            angle = Mathf.Atan(Mathf.Abs(currentPos.y - startPos.y) / Mathf.Abs(currentPos.x - startPos.x)) * Mathf.Rad2Deg;

            if (currentPos.y > startPos.y)
            {
                if (currentPos.x > startPos.x)
                {
                    angle = 90f - angle;
                }
                else
                {
                    angle = 270f + angle;
                }

            }
            else if(currentPos.y <= startPos.y)
            {
                if (currentPos.x > startPos.x)
                {
                    angle = 90f + angle;
                }
                else
                {
                    angle = 270f - angle;
                }
            }
        }
        else
            angle = 0;

        

        //calculate pos
        pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        ////////
        SetDiraction(angle);
    }

    public void SetDiraction(float angle)
    {
        if (LevelManager.Instance.PlayerPlane == null)
            return;

        float currentAngle;
        if(isLeft)
            currentAngle = (LevelManager.Instance.PlayerPlane.GetLeftSteerObject().eulerAngles.y < 0) ? LevelManager.Instance.PlayerPlane.GetLeftSteerObject().eulerAngles.y + 360f : LevelManager.Instance.PlayerPlane.GetLeftSteerObject().eulerAngles.y;
        else
            currentAngle = (LevelManager.Instance.PlayerPlane.GetRightSteerObject().eulerAngles.y < 0) ? LevelManager.Instance.PlayerPlane.GetRightSteerObject().eulerAngles.y + 360f : LevelManager.Instance.PlayerPlane.GetRightSteerObject().eulerAngles.y;

        bool turnLeft = (angle - currentAngle < 0);
        turnLeft = (Mathf.Abs(angle - currentAngle) > 180) ? !turnLeft : turnLeft;

        if (Mathf.Abs(currentAngle - angle)  > minAngleDiffrence && Mathf.Abs(currentAngle - angle) < 360f - minAngleDiffrence)
        {
            if (turnLeft)
            {
                if (isLeft)
                    LevelManager.Instance.PlayerPlane.SteerLeft(-1, true);
                else
                    LevelManager.Instance.PlayerPlane.SteerRight(-1, true);
            }
            else
            {
                if (isLeft)
                    LevelManager.Instance.PlayerPlane.SteerLeft(1, true);
                else
                    LevelManager.Instance.PlayerPlane.SteerRight(1, true);
            }
        }
        else
        {
            if (isLeft)
                LevelManager.Instance.PlayerPlane.SteerLeft(0, true);
            else
                LevelManager.Instance.PlayerPlane.SteerRight(0, true);
        }
    }

}
