using UnityEngine;
using UnityEngine.UI;

public class UIIndicator : MonoBehaviour
{
    public GameObject turretUIPrefab;
    GameObject objecttUI;
    Image uiIndic;
    void Start()
    {
        objecttUI = GameObject.Instantiate(turretUIPrefab) as GameObject;
        objecttUI.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        uiIndic = objecttUI.GetComponent<Image>();
    }
    
    void Update()
    {
        UIPOSClass.UIposArrow(transform.position, uiIndic);
    }

    public void DestroyUIIndecator()
    {
        GameObject.Destroy(uiIndic.gameObject);
    }

    private void OnDestroy()
    {
        GameObject.Destroy(objecttUI);
    }
}
