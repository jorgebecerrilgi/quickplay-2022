using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{
    enum OvenStates
    {
        Default,
        Undercooked,
        Cooked,
        Overcooked,
    }

    [SerializeField] private TextMeshProUGUI UITimer;
    [SerializeField] private GameObject oven;
    [SerializeField] private Sprite[] ovenSprites;
    [SerializeField] private GameObject warning;

    private PlayerInput playerInput;
    private InputAction actionTap;
    private TimerBehaviour timerBehaviour;
    private SpriteRenderer spriteRenderer;
    private OvenStates ovenState = OvenStates.Undercooked;
    private bool hasTapped = false;

    public bool minigameEnded = false;
    public bool minigamePassed = false;

    private void Awake()
    {
        spriteRenderer = oven.GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        actionTap = playerInput.actions["Tap"];
        timerBehaviour = GetComponent<TimerBehaviour>();
    }

    private void OnEnable()
    {
        actionTap.performed += ActionTapPerformed;
    }

    private void OnDisable()
    {
        actionTap.performed -= ActionTapPerformed;
    }

    private void Update()
    {
        UITimer.text = timerBehaviour.Timer.RemainingSeconds.ToString();
    }

    public void ActionTapPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (hasTapped || minigameEnded) return;
        hasTapped = true;
        minigameEnded = true;

        if (ovenState == OvenStates.Undercooked)
        {
            minigamePassed = true;
            spriteRenderer.sprite = ovenSprites[(int)OvenStates.Undercooked];
        }
        else if (ovenState == OvenStates.Cooked)
            spriteRenderer.sprite = ovenSprites[(int)OvenStates.Cooked];
    }

    public void Alert()
    {
        TimerEnd();
        SpriteRenderer warningSR = warning.GetComponent<SpriteRenderer>();
        warningSR.color = Color.white;
    }

    public void TimerEnd()
    {
        ovenState++;
        if (!minigameEnded && ovenState == OvenStates.Overcooked)
        {
            minigameEnded = true;
            spriteRenderer.sprite = ovenSprites[(int)OvenStates.Overcooked];
        }
    }
}
