using UnityEngine;
using TMPro;

public class SampleDrag : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private GameObject oven;
    [SerializeField] private Sprite ovenFullSprite;
    [SerializeField] private GameObject bread;

    private TimerBehaviour timer;
    private Draggable breadDraggable;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

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
        breadDraggable = bread.GetComponent<Draggable>();
        breadDraggable.OnDragEnd += BreadOnDragEnd;
    }

    private void OnDisable()
    {
        breadDraggable.OnDragEnd -= BreadOnDragEnd;
    }

    private void BreadOnDragEnd(Vector3 worldPosition)
    {
        if (MinigameEnded || worldPosition.y < 0) return;

        MinigameEnded = true;
        MinigamePassed = true;
        oven.GetComponent<SpriteRenderer>().sprite = ovenFullSprite;
        Destroy(bread);
    }

    public void TimerEnd()
    {
        MinigameEnded = true;
    }
}
