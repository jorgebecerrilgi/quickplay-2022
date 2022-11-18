using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PintarCalavera : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;

    private TimerBehaviour timer;

    [SerializeField] private Transform points;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    public int point = 0;

    private void Awake()
    {
        timer = GetComponent<TimerBehaviour>();
    }

    private void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
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
                if (missing == 1)
                    return false;
            }
        }
        return true;
    }

    public void TimerEnd()
    {
        MinigameEnded = true;

        MinigamePassed = Passed();
        Debug.Log(MinigamePassed);
    }
}