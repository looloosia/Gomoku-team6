using System;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class MarkerSelectPanelController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button blackMarkerBtn;
    [SerializeField] private Button whiteMarkerBtn;
    [SerializeField] private Button randomMarkerBtn;

    public event Action<MarkerChoice> OnMarkerSelectedEvent;

    void Start()
    {
        gameObject.SetActive(true);
        BindButtons();
    }

    private void BindButtons()
    {
        blackMarkerBtn.onClick.AddListener(() => OnClickMarker(MarkerChoice.Black));
        whiteMarkerBtn.onClick.AddListener(() => OnClickMarker(MarkerChoice.White));
        randomMarkerBtn.onClick.AddListener(() => OnClickMarker(MarkerChoice.Random));
    }

    private void OnClickMarker(MarkerChoice choice)
    {
        OnMarkerSelectedEvent?.Invoke(choice);

        gameObject.SetActive(false);
    }
}
