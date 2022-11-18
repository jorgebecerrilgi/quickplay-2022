using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RomperPinata : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerUI;
	[SerializeField] private GameObject pinata;
	[SerializeField] private GameObject stick;

	private bool pinataUp = false;
	private bool stickSwinging = false;
	private bool stickUp = false;

	[SerializeField] private double pinataTop = 3.2;
	[SerializeField] private double pinataCenter = 0.0;
	[SerializeField] private float pinataRange = 1.0F;
	[SerializeField] private float pinataSpeed = .5F;

	private double currPinataTop;
	private double currPinataCenter;

	[SerializeField] private double stickDefaultRotation = 340.0;
	[SerializeField] private double stickSwingRotation = 400.0;
	[SerializeField] private float stickSpeed = 300;

    private PlayerInput playerInput;
    private InputAction actionTap;
    private TimerBehaviour timer;

    [SerializeField] private Sprite[] hitSprites;

    private int hits = 0;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    [SerializeField] private AudioSource[] hitSounds;

    private bool pinataDead = false;

    [SerializeField] private WinLoss winLoss;

    // Start is called before the first frame update
    private void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
        timer = GetComponent<TimerBehaviour>();

        actionTap = playerInput.actions["Tap"];
    }

    private void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!pinataDead)
            MovePinata();
        MoveStick();
    }

    private void OnEnable()
    {
        actionTap.performed += ActionTapPerformed;
    }

    private void OnDisable()
    {
        actionTap.performed -= ActionTapPerformed;
    }

    private void MovePinata()
    {
    	if (pinataUp)
    	{
    		if (pinata.transform.position.y >= currPinataTop)
    		{
    			SendPinataDown();
    		}
    		else
    		{
    			pinata.transform.position = new Vector2(pinata.transform.position.x, pinata.transform.position.y + pinataSpeed);
    		}
    	}
    	else
    	{
    		if (pinata.transform.position.y <= currPinataCenter)
    		{
    			SendPinataUp();
    		}
    		else
    		{
    			pinata.transform.position = new Vector2(pinata.transform.position.x, pinata.transform.position.y - pinataSpeed);
    		}
    	}
    }

    private void MoveStick()
    {
    	if (!stickSwinging)
    	{
    		return;
    	}

    	if (stickUp)
    	{
    		if (stick.transform.rotation.eulerAngles.z <= stickDefaultRotation && stick.transform.rotation.eulerAngles.z >= 180)
    		{
    			stickUp = false;
    			stickSwinging = false;
    		}
    		else
    		{
    			stick.transform.Rotate(new Vector3(0, 0, -stickSpeed) * Time.fixedDeltaTime);
    		}
    	}
    	else
    	{
    		if (stick.transform.rotation.eulerAngles.z >= stickSwingRotation && stick.transform.rotation.eulerAngles.z <= 180)
    		{
    			stickUp = true;
    			if (pinata.transform.position.y < pinataCenter + pinataRange * 2)
    			{
    				Debug.Log("point");
    				hits++;
                    if (hits < hitSprites.Length)
                    {
                        pinata.GetComponent<SpriteRenderer>().sprite = hitSprites[hits];
                    }
                    if (hits < hitSounds.Length + 1)
                    {
                        hitSounds[hits-1].Play();
                    }
                    Debug.Log(hits);
    				if (hits == 3)
    				{
    					Win();
    				}
    			}
    		}
    		else
    		{
    			stick.transform.Rotate(new Vector3(0, 0, stickSpeed) * Time.fixedDeltaTime);
    		}
    	}
    }

    private void SendPinataDown()
    {
    	currPinataCenter = pinataCenter;// + Random.Range(-pinataRange, pinataRange);
    	pinataUp = false;
    }

    private void SendPinataUp()
    {
    	currPinataTop = pinataTop + Random.Range(-pinataRange, pinataRange);
    	pinataUp = true;
    }

    private void SwingStick()
    {
    	stickSwinging = true;
    	stickUp = false;
        stick.GetComponent<AudioSource>().Play();
    }

    private void ActionTapPerformed(InputAction.CallbackContext obj)
    {
    	if (!stickSwinging)
    	{
    		SwingStick();
		}
    }

    private void Win()
    {
        pinataDead = true;
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
