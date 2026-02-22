using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Replay : MonoBehaviour
{
    [SerializeField]
    private Button btnReplay;

    private UnityAction onReplay;
    void Awake()
    {
        this.btnReplay.onClick.AddListener(() =>
        {
            this.onReplay?.Invoke();
        });
    }
    public void SetReplayCallback(UnityAction action)
    {
        this.onReplay = action;
    }
}
