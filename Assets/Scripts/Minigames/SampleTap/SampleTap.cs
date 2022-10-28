using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleTap : MonoBehaviour
{
    enum OvenStates
    {
        Default,
        Undercooked,
        Cooked,
        Overcooked,
    }

    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private GameObject oven;
    [SerializeField] private Sprite[] ovenSprites;
    [SerializeField] private GameObject warningSign;

    private PlayerInput playerInput;
    private TimerBehaviour timer;
    private SpriteRenderer ovenSpriteRenderer;
    private InputAction actionTap;

    private OvenStates currentOvenState = OvenStates.Undercooked;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        timer = GetComponent<TimerBehaviour>();
        ovenSpriteRenderer = oven.GetComponent<SpriteRenderer>();

        actionTap = playerInput.actions["Tap"];
    }

    private void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    private void OnEnable()
    {
        actionTap.performed += ActionTapPerformed;
    }

    private void OnDisable()
    {
        actionTap.performed -= ActionTapPerformed;
    }

    private void ActionTapPerformed(InputAction.CallbackContext obj)
    {
        if (MinigameEnded) return;
        MinigameEnded = true;
        actionTap.performed -= ActionTapPerformed;

        switch (currentOvenState)
        {
            case OvenStates.Undercooked:
                MinigamePassed = false;
                ovenSpriteRenderer.sprite = ovenSprites[(int)OvenStates.Undercooked];
                break;
            case OvenStates.Cooked:
                MinigamePassed = true;
                ovenSpriteRenderer.sprite = ovenSprites[(int)OvenStates.Cooked];
                break;
        }
    }

    public void Alert()
    {
        if (MinigameEnded) return;
        currentOvenState++;
        SpriteRenderer warningSignSpriteRenderer = warningSign.GetComponent<SpriteRenderer>();
        warningSignSpriteRenderer.color = Color.white;
    }

    public void TimerEnd()
    {
        if (MinigameEnded) return;
        MinigameEnded = true;
        MinigamePassed = false;
        currentOvenState++;
        ovenSpriteRenderer.sprite = ovenSprites[(int)OvenStates.Overcooked];
    }
}
