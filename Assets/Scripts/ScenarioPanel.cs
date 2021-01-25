using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class ScenarioPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private Transform scenarioTransform;
    [SerializeField] private Image turnGauage;

    readonly private float slotStartX   = -460;
    readonly private float slotEndX     = 460;
    readonly private float slotY        = 0;
    readonly private float slotHeight   = 110;
    readonly private float slotOffset   = 8;
    private GameManager gameManager;
    private List<GameObject> slotObjectList;


    [Inject]
    public void Construct(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void Awake()
    {
        Turn.OnTurnStatusChanged += DealTurnGauge;
        Turn.OnScenarioChanged += UpdateSlotObjectList;
    }

    void Start()
    {
        InitSlotList();
    }

    private void InitSlotList()
    {
        int lockedIndex = gameManager.LockedIndex;
        float slotWidth = (slotEndX - slotStartX - (lockedIndex - 1) * slotOffset) / lockedIndex;

        slotObjectList = new List<GameObject>();

        for (int i = 0; i < lockedIndex; i++)
        {
            slotObjectList.Add(Instantiate(slotPrefabs[4], scenarioTransform));
            slotObjectList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + slotOffset), slotY);
            slotObjectList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    private void DealTurnGauge(Turn turn, TurnStatus prevStatus)
    {
        if (prevStatus == TurnStatus.WAITING && turn.Status == TurnStatus.PLAYING)
        {
            PlayTurnGauageAnimation(turn.Period);
        }
        else if (prevStatus == TurnStatus.PLAYING && turn.Status == TurnStatus.PAUSED)
        {
            StopTurnGaugeAnimation();
        }
        else if (prevStatus == TurnStatus.PAUSED && turn.Status == TurnStatus.PLAYING)
        {
            PlayTurnGauageAnimation(turn.Period - turn.PlayTime);
        }
        else if (prevStatus == TurnStatus.PLAYING && turn.Status == TurnStatus.WAITING)
        {
            ResetTurnGauge();
        }
        else
        {
            Debug.LogError("Invalid TurnStatus change : ScenarioPanel.cs - DealTurnGauge()");
            Debug.LogError($"{prevStatus} -> {turn.Status}");
        }
    }

    public void PlayTurnGauageAnimation(float duration)
    {
        turnGauage.rectTransform.DOScaleX(9300f, duration).SetEase(Ease.Linear);
    }

    public void StopTurnGaugeAnimation()
    {
        turnGauage.rectTransform.DOPause();
    }

    public void ResetTurnGauge()
    {
        turnGauage.rectTransform.DOScaleX(1f, 0f);
    }

    public void UpdateSlotObjectList(Turn turn)
    {
        float slotWidth = (slotEndX - slotStartX - (turn.LockedIndex - 1) * slotOffset) / turn.LockedIndex;

        foreach (var slot in slotObjectList)
            Destroy(slot);

        slotObjectList = new List<GameObject>();

        for (int i = 0; i < turn.LockedIndex; i++)
        {
            slotObjectList.Add(Instantiate(slotPrefabs[(int)turn.Scenario[i].PlacedAction.Type], scenarioTransform));
            slotObjectList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + slotOffset), slotY);
            slotObjectList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }
}
