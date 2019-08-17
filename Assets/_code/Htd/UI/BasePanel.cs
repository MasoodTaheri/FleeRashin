using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    public enum PanelType { MainMenu, InGame, PauseMenu, GameOver, OnlineInGame, OnLineRoomSelection }
    Animator animator;
    [SerializeField]
    protected PanelType type;
    public PanelType Type { get { return type; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
