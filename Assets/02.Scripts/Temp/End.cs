using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class End : MonoBehaviour
{
    [SerializeField]
    private Button btnReplay;

    private UnityAction onEnd;
    void Awake()
    {
        this.btnReplay.onClick.AddListener(() =>
        {
            this.onEnd?.Invoke();
        });
    }
    public void SetReplayCallback(UnityAction action)
    {
        this.onEnd = action;
    }
}
