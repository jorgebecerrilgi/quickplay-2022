using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GirarPirinola : MinigameMB
{
    [SerializeField] private TextMeshProUGUI timerUI;

    [SerializeField] private int requiredSpins = 3;

    private int spins = 0;

    private TimerBehaviour timer;
    [SerializeField] private PirinolaDraggable pirinolaDraggable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    [SerializeField] private float speed = .125F;

    [SerializeField] private Sprite[] spinSprites;

    [SerializeField] private WinLoss winLoss;

    private IEnumerator spinning;

    private IEnumerator Spin()
    {
        GetComponent<AudioSource>().Play();
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[0];
        yield return new WaitForSeconds(speed);
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[1];
        yield return new WaitForSeconds(speed);
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[2];
        yield return new WaitForSeconds(speed);
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[0];
        yield return new WaitForSeconds(speed);
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[1];
        yield return new WaitForSeconds(speed);
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[2];
        yield return new WaitForSeconds(speed);
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[3];
        GetComponent<AudioSource>().Stop();
    }

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
        pirinolaDraggable.OnDragEnd += PirinolaOnDragEnd;
    }

    private void OnDisable()
    {
        pirinolaDraggable.OnDragEnd -= PirinolaOnDragEnd;
    }

    private void PirinolaOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded) return;

        spins += 1;
        Debug.Log("spinned");
        StartCoroutine(Spin());
        // TODO: animate

        if (spins == requiredSpins)
        {
        	MinigamePassed = true;
        	Debug.Log("winn");
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
            timer.Stop();
            winLoss.Play(MinigamePassed);
            yield return new WaitForSeconds(1);

            InvokeOnEnd(MinigamePassed, timer.Timer.RemainingSeconds);
            Debug.Log("ending");
        }
    }

    public override void BeginMinigame(float remainingSeconds)
    {
        timer.BeginTimer(remainingSeconds);
    }
}
