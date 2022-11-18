using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class SacarTamales : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;

    [SerializeField] private Camera mainCamera;

    private TimerBehaviour timer;
    [SerializeField] private TamalDraggable tamalDraggable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    [SerializeField] private Transform plate;

    [SerializeField] private WinLoss winLoss;

    void Start()
    {
        timer = GetComponent<TimerBehaviour>();
    }

    void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    private void OnEnable()
    {
        tamalDraggable.OnDragEnd += BreadOnDragEnd;
    }

    private void OnDisable()
    {
        tamalDraggable.OnDragEnd -= BreadOnDragEnd;
    }

    private void BreadOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded || worldPosition.y > -2 || worldPosition.y < -4 || worldPosition.x < -1 || worldPosition.x > 1) return;

        tamalDraggable.placed = true;
        tamalDraggable.transform.position = plate.position;
       	MinigamePassed = true;
        StartCoroutine(RealEnd());
    }

    public void TimerEnd()
    {
        StartCoroutine(RealEnd());
    }

    private IEnumerator RealEnd()
    {
        if (!MinigameEnded)
        {
            MinigameEnded = true;
            winLoss.Play(MinigamePassed);
            yield return new WaitForSeconds(1);

            Debug.Log("ending");
        }
    }
}
