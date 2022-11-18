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

    [SerializeField] private WinLoss winLoss;

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
        GetComponent<AudioSource>().Play();
    	papelDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = fullShow;
    }

    private void PapelOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded) return;
        if (CheckDetection())
        {
        	ShowFull();
            MinigamePassed = true;
            StartCoroutine(RealEnd());
        }
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
