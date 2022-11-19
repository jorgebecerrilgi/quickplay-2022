using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Endless : MonoBehaviour
{
    [SerializeField] private Image mask;
    [SerializeField] private Image frame;
    [SerializeField] private Image fade;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image scoreBack;
    [SerializeField] private TextMeshProUGUI order;
    [SerializeField] private Sprite[] lifeSprites;
    [SerializeField] private Image life;

    private Vector3 INITIAL_SCALE = Vector3.one;
    private Vector3 TRANSITION_SCALE = new Vector3(15f, 15f, 1f);
    private const float TRANSITION_TIME = 2f;
    private Color INITIAL_COLOR = new Color(0.595f, 0.2975f, 0.3203846f);
    private Color TRANSITION_COLOR = Color.clear;
    private static readonly int[] MINIGAMES_POOL = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private const int MINIGAMES_LENGTH = 9;

    private const float SCORE_DISPLACEMENT = 1000f;
    private Vector3 SCALE_HIDE = Vector3.zero;
    private Vector3 SCALE_SHOW = Vector3.one;

    private System.Random random = new System.Random();
    private AudioListener audioListener;
    private int[] minigames;
    private int currentMinigameIndex;
    private bool insideMinigame = false;
    private MinigameMB currentMinigame;
    private TimerBehaviour timer;
    private int lifeAmount = 3;

    private int completed = 0;
    private float minigameTime = 10f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManagerSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManagerSceneLoaded;
    }

    private void Start()
    {
        timer = GetComponent<TimerBehaviour>();
        audioListener = mainCamera.GetComponent<AudioListener>();
        ShuffleMinigames();
    }

    private void SceneManagerSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg1 != LoadSceneMode.Additive) return;

        // Connect to OnEnd event.
        GameObject gameHandler = arg0.GetRootGameObjects()[0];
        currentMinigame = gameHandler.GetComponent<MinigameMB>();
        currentMinigame.OnEnd += MinigameOnEnd;

        switch (arg0.name)
        {
            case "DecorarAltar":
                order.text = "DECORATE";
                break;
            case "DecorarPapel":
                order.text = "SLIDE";
                break;
            case "GirarPirinola":
                order.text = "SPIN";
                break;
            case "OrganizarPan":
                order.text = "ORGANIZE";
                break;
            case "PintarCalavera":
                order.text = "PAINT";
                break;
            case "PintarCatrina":
                order.text = "PAINT";
                break;
            case "RellenarPan":
                order.text = "FILL";
                break;
            case "RomperPinata":
                order.text = "BREAK";
                break;
            case "SacarTamales":
                order.text = "SERVE";
                break;
            default:
                order.text = "WIN";
                break;
        }

        TransitionIntoMinigame();
    }

    private void MinigameOnEnd(bool hasWon, float remainingSeconds)
    {
        currentMinigame.OnEnd -= MinigameOnEnd;
        insideMinigame = false;
        completed++;

        if (hasWon)
        {
            // Updates score and updates UI.
            int newScore = Int32.Parse(score.text) + (int)((remainingSeconds + 5) * 75);
            score.text = newScore.ToString();
        } else
        {
            lifeAmount--;
            life.sprite = lifeSprites[3 - lifeAmount];
        }

        if (lifeAmount == 0)
        {
            SceneManager.LoadScene("Menu");
        }
        if (completed == 10)
        {
            minigameTime = 8f;
        } else if (completed == 20)
        {
            minigameTime = 6f;
        } else if (completed == 30)
        {
            minigameTime = 4f;
        }

        TransitionBack();
    }

    public void LoadMinigame()
    {
        if (insideMinigame) return;

        audioListener.enabled = false;
        insideMinigame = true;

        // Loads next scene in queue.
        SceneManager.LoadSceneAsync(minigames[currentMinigameIndex], LoadSceneMode.Additive);
    }

    // Tweens the frame into the camera, and reveals the next minigame.
    private void TransitionIntoMinigame()
    {
        // Frame animation.
        frame.transform.LeanScale(TRANSITION_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        mask.transform.LeanScale(TRANSITION_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        // Score moving out of the way, animation.
        scoreText.transform.LeanMoveLocalY(scoreText.transform.localPosition.y + SCORE_DISPLACEMENT, TRANSITION_TIME / 2).setEaseInOutExpo();
        score.transform.LeanMoveLocalY(score.transform.localPosition.y + SCORE_DISPLACEMENT, TRANSITION_TIME / 2).setEaseInOutExpo();
        scoreBack.transform.LeanMoveLocalY(scoreBack.transform.localPosition.y + SCORE_DISPLACEMENT, TRANSITION_TIME / 2).setEaseInOutExpo();
        // Show order.
        order.transform.LeanScale(SCALE_SHOW, TRANSITION_TIME / 2).setEaseInOutExpo().setOnComplete(HideOrder);
        // Move life out
        life.transform.LeanMoveLocalY(life.transform.localPosition.y - SCORE_DISPLACEMENT, TRANSITION_TIME / 2).setEaseInOutExpo();
        // Fades out, and calls UnloadMinigame method after.
        LeanTween.color(fade.rectTransform, TRANSITION_COLOR, TRANSITION_TIME / 2).setEaseInOutExpo().setOnComplete(BeginMinigame);
    }

    private void HideOrder()
    {
        order.transform.LeanScale(SCALE_HIDE, TRANSITION_TIME * 2).setEaseInOutExpo();
    }

    // Tweens the frame back to it's original place, and hides the previous minigame.
    private void TransitionBack()
    {
        frame.transform.LeanScale(INITIAL_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        mask.transform.LeanScale(INITIAL_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        // Score moving back, animation.
        scoreText.transform.LeanMoveLocalY(scoreText.transform.localPosition.y - SCORE_DISPLACEMENT, TRANSITION_TIME).setEaseInOutExpo();
        score.transform.LeanMoveLocalY(score.transform.localPosition.y - SCORE_DISPLACEMENT, TRANSITION_TIME).setEaseInOutExpo();
        scoreBack.transform.LeanMoveLocalY(scoreBack.transform.localPosition.y - SCORE_DISPLACEMENT, TRANSITION_TIME).setEaseInOutExpo();
        // Move life in.
        life.transform.LeanMoveLocalY(life.transform.localPosition.y + SCORE_DISPLACEMENT, TRANSITION_TIME / 2).setEaseInOutExpo();
        // Fades out, and calls UnloadMinigame method after.
        LeanTween.color(fade.rectTransform, INITIAL_COLOR, TRANSITION_TIME).setEaseInOutExpo().setOnComplete(UnloadMinigame);
    }

    private void UnloadMinigame()
    {
        if (SceneManager.sceneCount <= 1) return;

        // Unloads scene.
        SceneManager.UnloadSceneAsync(minigames[currentMinigameIndex++]);
        // Reshuffles the minigames queue after all minigame's have passed.
        if (currentMinigameIndex == MINIGAMES_LENGTH)
            ShuffleMinigames();
        timer.RestartTimer();
    }

    private void BeginMinigame()
    {
        currentMinigame?.BeginMinigame(minigameTime);
    }

    private void ShuffleMinigames()
    {
        currentMinigameIndex = 0;
        minigames = MINIGAMES_POOL.OrderBy(x => random.Next()).ToArray();
    }
}
