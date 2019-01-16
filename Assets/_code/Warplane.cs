using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DefaultPlayerPlane : MovableObject
{
    private float Leftweight;
    private float Rightweight;
    public Vector2 rotateFactor;//winghit

    private float RotateLeftFactor;//touch left
    private float RotateRightFactor;//touch right
    public GameObject flareObj;
    [SerializeField]
    Wing leftWing;
    [SerializeField]
    Wing rightWing;
    public bool readytodestroy = false;
    private gun planeGun;

    public void StartShoot()
    {
        if (planeGun == null)
            planeGun = obj.GetComponent<gun>();
        planeGun.Shoot(true);
    }
    public void EndShoot()
    {
        if (planeGun == null)
            planeGun = obj.GetComponent<gun>();
        planeGun.Shoot(false);
    }

    public DefaultPlayerPlane(float _forwardSpeed, float _rotateSpeed, float _lifetime, Sprite _sprite, GameObject prefab) :
         base(_forwardSpeed, _rotateSpeed, _lifetime, _sprite, prefab)
    {
        //obj = _obj;
        obj = GameObject.Instantiate(prefab, new Vector3(0, -6, 0), Quaternion.identity) as GameObject;
        rb = obj.GetComponent<Rigidbody>();
        rotateFactor = Vector2.one;
        RotateLeftFactor = 0;
        RotateRightFactor = 0;
        flareObj = Resources.Load("flare") as GameObject;
        leftWing = new Wing(0, 0, 0, obj.transform.GetChild(2).gameObject, this);
        rightWing = new Wing(0, 0, 0, obj.transform.GetChild(1).gameObject, this);
        //obj.AddComponent<ColliderCallback>()
        readytodestroy = false;


        obj.gameObject.AddComponent<ColliderCallback>().enter += Collision;



    }

    public bool ishit()
    {
        if (leftWing.Health < 1) return true;
        if (rightWing.Health < 1) return true;

        return false;
    }
    protected override void rotate()
    {
        obj.transform.Rotate(0, rotateSpeed * Time.deltaTime * (Rightweight - Leftweight), 0);
    }

    public void setsterrtoleft(float PaddelValue)//touch controller
    {
        RotateLeftFactor = PaddelValue;

    }

    public void setsterrtoRight(float PaddelValue)//touch controller
    {
        RotateRightFactor = PaddelValue;

    }

    private void InputManager()
    {


        //rotateFactor += new Vector2(1 - leftWing.Health, 1 - rightWing.Health);
        if (rotateFactor.x > 0.5 && leftWing.Health < 0) rotateFactor.x += leftWing.Health;
        if (rotateFactor.y > 0.5 && rightWing.Health < 0) rotateFactor.y += rightWing.Health;




        //    rotateFactor += new Vector2(
        //(isLeft && (rotateFactor.x > 0.5f)) ? minvalue : 0,
        //  (isRight && (rotateFactor.y > 0.5f)) ? minvalue : 0);


        Rightweight = (b2f(Input.GetKey(KeyCode.RightArrow)) + RotateRightFactor) * rotateFactor.y;
        Leftweight = (b2f(Input.GetKey(KeyCode.LeftArrow)) + RotateLeftFactor) * rotateFactor.x;
    }

    public override void Update()
    {
        //Debug.Log("Update");
        InputManager();
        moveforward();
        rotate();
        leftWing.Update();
        rightWing.Update();

    }


    public IEnumerator flareDropIE()
    {
        if (PlayerDataClass.Flare > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(flareObj, obj.transform.TransformPoint(0, 0, -0.5f), obj.transform.rotation);
                for (int j = 0; j < 10; j++)
                    yield return null;
            }
            PlayerDataClass.Flare--;
        }
    }

    //public void flareDropIE2()
    //{
    //    GameObject.Instantiate(flareObj, obj.transform.TransformPoint(0, 0, -0.5f), obj.transform.rotation);
    //}

    public override void Collision(Collision collision, GameObject me)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Star")
        {
            //Debug.Log("Star");
            GameObject.Destroy(collision.gameObject);
            uiController.Instanse.IncStarPickedup();
        }

        if (collision.gameObject.tag == "Repair")
        {
            //Debug.Log("Repair");
            GameObject.Destroy(collision.gameObject);
            if (!leftWing.inchealth())
            {
                rightWing.inchealth();
                rotateFactor.y = 1;
            }
            else
            {
                rotateFactor.x = 1;
            }

        }
    }

    //public void winghit(bool isLeft, bool isRight, float minvalue)
    //{

    //    Debug.Log("plane.rotateFactor =" + rotateFactor);
    //}

    public void destroy()
    {
        readytodestroy = true;
        //Debug.Log("bodey destroy");
        //if (flareObj!=null)
        //GameObject.Destroy(flareObj);
        GameObject.Destroy(obj);
        //this = null;


    }
}
