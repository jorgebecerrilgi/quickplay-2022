using UnityEngine;
using TMPro;

public class GirarPirinola : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;

    [SerializeField] private int requiredSpins = 3;

    private int spins = 0;

    private TimerBehaviour timer;
    [SerializeField] private PirinolaDraggable pirinolaDraggable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    [SerializeField] private Sprite[] spinSprites;

    private void Spin()
    {
        pirinolaDraggable.gameObject.GetComponent<SpriteRenderer>().sprite = spinSprites[spins];
    }

    void Start()
    {
        timer = GetComponent<TimerBehaviour>();
        Spin();
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
        Spin();
        // TODO: animate

        if (spins == requiredSpins)
        {
        	MinigameEnded = true;
        	MinigamePassed = true;
        	Debug.Log("winn");
        }
    }

    public void TimerEnd()
    {
        Debug.Log("ending");
        MinigameEnded = true;
    }
}
