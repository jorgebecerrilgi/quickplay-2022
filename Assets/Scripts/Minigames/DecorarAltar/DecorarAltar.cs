using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecorarAltar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;

    private TimerBehaviour timer;
    [SerializeField] private DecoracionAltar[] decoracionDraggables;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    private int decoracionesCount;

    private GameObject[] decoraciones;

    [SerializeField] private GameObject[] locs;

    [SerializeField] private GameObject decoracionPrefab;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform points;

    [SerializeField] private WinLoss winLoss;

    private void Awake()
    {
    	PlaceDecoraciones();

    	ActivatePoints();
    }

    void Start()
    {
        timer = GetComponent<TimerBehaviour>();
        UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
    }

    private void ActivatePoints()
    {
    	decoraciones = new GameObject[3];
    	GameObject[] totalDecoraciones = new GameObject[6];
		for(int i = 0; i < points.childCount; i++)
		{
		   totalDecoraciones[i] = points.GetChild(i).gameObject;
		}

		List<int> listNumbers = new List<int>();
		int number;
		for (int i = 0; i < 3; i++)
		{
			do
			{
		    	number = Random.Range(0, 6);
		 	}
		 	while (listNumbers.Contains(number));
			listNumbers.Add(number);
		}

		for (int i = 0; i < 3; i++)
		{
			decoraciones[i] = totalDecoraciones[listNumbers[i]];
			decoraciones[i].GetComponent<DecoracionPoint>().Activate();
		}
    }

    private void PlaceDecoraciones()
    {
    	for (int i = 0; i < 3; i++)
    	{
    		decoracionDraggables[i].Locate(locs[i].transform.position);
    		decoracionDraggables[i].mainCamera = mainCamera;
    	}
    }

    void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    private void OnEnable()
    {
    	for (int i = 0; i < 3; i++)
    	{
        	decoracionDraggables[i].OnDragEnd += DecoracionOnDragEnd;
    	}
    }

    private void OnDisable()
    {
    	for (int i = 0; i < 3; i++)
    	{
        	decoracionDraggables[i].OnDragEnd -= DecoracionOnDragEnd;
	    }
    }

    private void DecoracionOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded) return;

        decoracionesCount++;

        if (decoracionesCount < 3)
        	return;

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
