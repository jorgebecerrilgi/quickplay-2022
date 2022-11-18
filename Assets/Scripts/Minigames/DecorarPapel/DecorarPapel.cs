using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecorarPapel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;

    [SerializeField] private GameObject points;

    private TimerBehaviour timer;
    [SerializeField] private PapelPicado papelDraggable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    [SerializeField] private Sprite fullShow;
    [SerializeField] private Sprite partialShow;

    void Start()
    {
        timer = GetComponent<TimerBehaviour>();
        papelDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = partialShow;
        papelDraggable.transform.position = new Vector3(0F, (float)Random.Range(-3.15F, 3.15F), 0F);
    }

    void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    private bool CheckDetection()
    {
		for(int i = 0; i < points.transform.childCount; i++)
		{
			if (!points.transform.GetChild(i).GetComponent<PointPapel>().detected)
			{
				Debug.Log("badd");
				Debug.Log(points.transform.GetChild(i).name);
				return false;
			}
		}
		return true;
    }

    private void OnEnable()
    {
        papelDraggable.OnDragEnd += PapelOnDragEnd;
    }

    private void OnDisable()
    {
    	papelDraggable.OnDragEnd -= PapelOnDragEnd;
    }

    private void ShowFull()
    {
    	papelDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = fullShow;
    }

    private void PapelOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded) return;
        if (CheckDetection())
        {
        	ShowFull();
        }
    }

    public void TimerEnd()
    {
        Debug.Log("ending");
        Debug.Log(MinigamePassed);
        MinigamePassed = CheckDetection();
        MinigameEnded = true;
    }
}
