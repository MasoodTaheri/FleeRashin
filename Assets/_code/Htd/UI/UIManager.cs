using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    Canvas canvas;
    public Canvas ActiveCanvas
    {
        get
        {
            if (canvas == null)
                canvas = FindObjectOfType<Canvas>();

            return canvas;
        }
    }

    string path = "UI/";
    Dictionary<BasePanel.PanelType, BasePanel> instantiatedPanels = new Dictionary<BasePanel.PanelType, BasePanel>();
    BasePanel activePanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //SceneManager.sceneLoaded += CustomOnLevelWasLoaded;
    }

    public void CustomOnLevelWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        instantiatedPanels.Clear();
        canvas = FindObjectOfType<Canvas>();
    }

    public void ChangePanel(BasePanel.PanelType newPanelType)
    {
        BasePanel newPanel;

        if (instantiatedPanels.ContainsKey(newPanelType))
        {
            newPanel = instantiatedPanels[newPanelType];
        }
        else
        {
            newPanel = Instantiate<BasePanel>(Resources.Load<BasePanel>(path + newPanelType.ToString()), ActiveCanvas.transform);
            instantiatedPanels.Add(newPanelType, newPanel);
        }

        if(activePanel != null)
            activePanel.Hide();

        activePanel = newPanel;
        activePanel.Show();
    }

    public Image InstantiateIndicator(GameObject prefab)
    {
        GameObject Icon = Instantiate(prefab);
        Icon.transform.SetParent(canvas.transform);
        Icon.GetComponent<RectTransform>().localScale = Vector3.one;
        return Icon.GetComponent<Image>();
    }
}
