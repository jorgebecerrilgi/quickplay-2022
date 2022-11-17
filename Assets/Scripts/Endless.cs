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

    private Vector3 INITIAL_SCALE = Vector3.one;
    private Vector3 TRANSITION_SCALE = new Vector3(15f, 15f, 1f);
    private const float TRANSITION_TIME = 2f;
    private Color INITIAL_COLOR = new Color(0.595f, 0.2975f, 0.3203846f);
    private Color TRANSITION_COLOR = Color.clear;
    private static readonly int[] MINIGAMES_POOL = { 0 };
    private const int MINIGAMES_LENGTH = 1;

    private System.Random random = new System.Random();
    private AudioListener audioListener;
    private int[] minigames;
    private int currentMinigameIndex;
    private bool insideMinigame = false;
    private MinigameMB currentMinigame;
    private TimerBehaviour timer;

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

        TransitionIntoMinigame();
    }

    private void MinigameOnEnd(bool hasWon, float remainingSeconds)
    {
        currentMinigame.OnEnd -= MinigameOnEnd;
        insideMinigame = false;

        // Updates score and updates UI.
        int newScore = Int32.Parse(score.text) + (int)(remainingSeconds * 100);
        score.text = newScore.ToString();

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
        frame.transform.LeanScale(TRANSITION_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        mask.transform.LeanScale(TRANSITION_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        // Fades out, and calls UnloadMinigame method after.
        LeanTween.color(fade.rectTransform, TRANSITION_COLOR, TRANSITION_TIME).setEaseInOutExpo().setOnComplete(BeginMinigame);
    }

    // Tweens the frame back to it's original place, and hides the previous minigame.
    private void TransitionBack()
    {
        frame.transform.LeanScale(INITIAL_SCALE, TRANSITION_TIME).setEaseInOutExpo();
        mask.transform.LeanScale(INITIAL_SCALE, TRANSITION_TIME).setEaseInOutExpo();
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
        currentMinigame?.BeginMinigame(10f);
    }

    private void ShuffleMinigames()
    {
        currentMinigameIndex = 0;
        minigames = MINIGAMES_POOL.OrderBy(x => random.Next()).ToArray();
    }
}
