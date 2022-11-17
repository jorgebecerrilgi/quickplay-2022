using UnityEngine;
using TMPro;
using System.Collections;

public class Luchador : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private GameObject derrotado;
    [SerializeField] private GameObject luchador;
    [SerializeField] private GameObject piso;
    [SerializeField] private GameObject KO;
    [SerializeField] private Sprite KOsprite;
    [SerializeField] private Sprite Failsprite;

    private TimerBehaviour timer;
    private Collider2D LuchadorCollider;
    private Collider2D DerrotadoCollider;
    private Collider2D PisoCollider;
    private Transform luchadorPosition;

    public bool MinigameEnded { get; private set; } = false;
    public bool MinigamePassed { get; private set; } = false;

    void Start()
    {
        timer = GetComponent<TimerBehaviour>();
        //[-2,1.9] y[5.84]
        luchador.SetActive(false);
        luchadorPosition = luchador.GetComponent<Transform>();
        StartCoroutine(executeAfterTime(Random.Range(0, 5)));
        //luchadorPosition.position = new Vector2(Random.Range(-2f, 1.9f), 5.84f);
    }

    void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
        //cayendo

        if (LuchadorCollider.IsTouching(DerrotadoCollider))
        {
            MinigameEnded = true;
            MinigamePassed = true;
            luchador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            KO.GetComponent<SpriteRenderer>().sprite = KOsprite;
            luchador.GetComponent<Draggable>().enabled = false;

        } else if (LuchadorCollider.IsTouching(PisoCollider))
        {
            MinigameEnded = true;
            MinigamePassed = true;
            luchador.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            KO.GetComponent<SpriteRenderer>().sprite = Failsprite;
            luchador.GetComponent<Draggable>().enabled = false;
        }
    }

    private void OnEnable()
    {
        LuchadorCollider = luchador.GetComponent<Collider2D>();
        DerrotadoCollider = derrotado.GetComponent<Collider2D>();
        PisoCollider = piso.GetComponent<Collider2D>();

    }

    public void TimerEnd()
    {
        MinigameEnded = true;
    }

    IEnumerator executeAfterTime(int secs)
    {
        yield return new WaitForSeconds(secs);
        luchadorPosition.position = new Vector2(Random.Range(-2f, 1.9f), 5.84f);
        luchador.SetActive(true);
    }

}
