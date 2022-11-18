using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PintarCatrina : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;

    private TimerBehaviour timer;

    [SerializeField] private Transform points;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    public int point = 0;

    [SerializeField] private WinLoss winLoss;

    private void Awake()
    {
        timer = GetComponent<TimerBehaviour>();
    }

    private void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    public void TryPassed()
    {
        if (Passed())
        {
            MinigamePassed = true;
            StartCoroutine(RealEnd());
        }
    }

    private bool Passed()
    {
        int missing = 0;
        Debug.Log("passinggg");
        for (int i = 0; i < points.childCount; i++)
        {
            if (!points.GetChild(i).GetComponent<Point>().detected)
            {
                missing += 1;
                Debug.Log(points.GetChild(i).name);
                if (missing == 5)
                    return false;
            }
        }
        return true;
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