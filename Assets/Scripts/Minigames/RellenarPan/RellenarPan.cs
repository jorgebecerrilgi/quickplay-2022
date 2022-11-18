using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RellenarPan : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerUI;

    private TimerBehaviour timer;
    [SerializeField] private GameObject holdableObject;
    private Holdable holdable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    private float growth = 0.0F;
    [SerializeField] private float barr1 = 1.5F;
    [SerializeField] private float barr2 = 3.5F;
    [SerializeField] private float barr3 = 6.0F;

    [SerializeField] private WinLoss winLoss;
    
    private int MinigameTimeLeft = 0;

    private int state = 1;

    [SerializeField] private Sprite[] panSprites;

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
        holdable = holdableObject.GetComponent<Holdable>();
    }

    private void OnDisable()
    {
        holdable.OnHoldEnd -= OnHoldEnd;
    }

    private void OnHoldEnd(Vector3 worldPosition)
    {
        if (MinigameEnded) return;

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

    public void UpdateSprite()
    {
        holdableObject.gameObject.GetComponent<SpriteRenderer>().sprite = panSprites[state];
        state++;
    }

    public void Grow(float _growth)
    {
    	growth += _growth;

    	if (growth >= barr1 && state == 1)
    	{
    		UpdateSprite();
    	}
        else if (growth >= barr2 && state == 2)
        {
            MinigamePassed = true;
            MinigameTimeLeft = Mathf.CeilToInt(timer.Timer.RemainingSeconds);
            Debug.Log(MinigameTimeLeft);
            UpdateSprite();
        }
    	if (growth >= barr3 && state == 3)
    	{
    		Debug.Log("whoops");
    		MinigamePassed = false;
            UpdateSprite();
    	}
    }
}
