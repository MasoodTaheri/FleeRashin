using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public const int mainMenuSceneIndex = 1;
    public const int OfflineSceneIndex = 2;
    UIManager uiManager;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        uiManager = FindObjectOfType<UIManager>();
        SceneManager.sceneLoaded += CustomOnLevelWasLoaded;
        LoadMainMenu();
    }

    private void CustomOnLevelWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        uiManager.CustomOnLevelWasLoaded(scene, loadSceneMode);

        switch (scene.buildIndex)
        {
            case mainMenuSceneIndex:
                uiManager.ChangePanel(BasePanel.PanelType.MainMenu);
                break;
            case OfflineSceneIndex:
                LevelManager.Instance.StartGame();
                uiManager.ChangePanel(BasePanel.PanelType.InGame);
                break;
            default:
                break;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void StartGameOffline()
    {
        SceneManager.LoadSceneAsync(OfflineSceneIndex);
    }

    public void ShowOnlineRoomSelection()
    {
        ///SceneManager.LoadSceneAsync("OnlineScene");
        uiManager.ChangePanel(BasePanel.PanelType.OnLineRoomSelection);
    }

    public void StartGameOnline()
    {
        SceneManager.LoadSceneAsync("OnlineScene");
    }


}
