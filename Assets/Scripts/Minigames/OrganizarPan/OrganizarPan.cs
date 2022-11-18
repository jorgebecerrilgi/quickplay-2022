using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class OrganizarPan : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private GameObject bread;

    [SerializeField] private Camera mainCamera;

    private TimerBehaviour timer;
    private PanDraggable breadDraggable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    private int breadId;

    [SerializeField] private WinLoss winLoss;

    [SerializeField] private float[] breadSizes;

    void Start()
    {
        timer = GetComponent<TimerBehaviour>();
        UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
        breadId	= UnityEngine.Random.Range(1, 4);
        bread.GetComponent<SpriteRenderer>().sprite = Resources.Load(String.Format("OrganizarPan/bread{0}", breadId), typeof(Sprite)) as Sprite;
        bread.transform.localScale = new Vector3(.25F, .25F, 1);
    }

    void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    private void OnEnable()
    {
        breadDraggable = bread.GetComponent<PanDraggable>();
        breadDraggable.OnDragEnd += BreadOnDragEnd;
    }

    private void OnDisable()
    {
        breadDraggable.OnDragEnd -= BreadOnDragEnd;
    }

    private void BreadOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded || worldPosition.y < -1) return;

        Debug.Log(worldPosition.x);
        if (worldPosition.y > 3)
        {
        	MinigamePassed = breadId == 1;
            breadDraggable.transform.position = new Vector3(0, 4, 1);
        }
        else if (worldPosition.y > 1)
        {
            breadDraggable.transform.position = new Vector3(0, 2, 1);
        	MinigamePassed = breadId == 2;
        }
        else
        {
            breadDraggable.transform.position = new Vector3(0, 0, 1);
        	MinigamePassed = breadId == 3;
        }

        Debug.Log(MinigamePassed);
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
