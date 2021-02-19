using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


/*
 * Turn의 status 변화에 따른 event에 등록하는 순서 관련해서
 * ScenarioPanel이 먼저 실행해야 하는 메소드가 있고, TurnManager가 먼저 실행해야 하는 메소드가 있어
 * event 등록을 Awake()에서도 하고 Start()에서도 한 상황임
 */
public class ScenarioPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private Transform scenarioTransform;
    [SerializeField] private Image turnGauge;

    readonly private float slotStartX   = -460;
    readonly private float slotEndX     = 460;
    readonly private float slotY        = 0;
    readonly private float slotHeight   = 110;
    readonly private float slotOffset   = 8;
    private GameManager gameManager;
    private TurnManager turnManager;
    private List<GameObject> slotObjectList;


    [Inject]
    public void Construct(GameManager gameManager, TurnManager turnManager)
    {
        this.gameManager = gameManager;
        this.turnManager = turnManager;
    }

    void Awake()
    {
        slotObjectList = new List<GameObject>();

        Turn.OnScenarioChanged += UpdateSlotObjectList;
        Turn.OnTurnFinished += ResetTurnGauge;
    }

    void Start()
    {
        

        Turn.OnTurnStarted += PlayTurnGauageAnimation;
        Turn.OnTurnPaused += StopTurnGaugeAnimation;
        Turn.OnTurnResumed += PlayTurnGauageAnimation;
        
        //InitSlotList();
    }

    private void InitSlotList()
    {
        // 필요없을듯

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

    //private void DealTurnGauge(Turn turn, TurnStatus prevStatus)
    //{
    //    if (prevStatus == TurnStatus.WAITING && turn.Status == TurnStatus.PLAYING)
    //    {
    //        //PlayTurnGauageAnimation(turn.Period);
    //    }
    //    else if (prevStatus == TurnStatus.PLAYING && turn.Status == TurnStatus.PAUSED)
    //    {
    //        StopTurnGaugeAnimation();
    //    }
    //    else if (prevStatus == TurnStatus.PAUSED && turn.Status == TurnStatus.PLAYING)
    //    {
    //        //PlayTurnGauageAnimation(turn.Period - turn.PlayTime);
    //    }
    //    else if (prevStatus == TurnStatus.PLAYING && turn.Status == TurnStatus.WAITING)
    //    {
    //        ResetTurnGauge();
    //    }
    //    else
    //    {
    //        Debug.LogError("Invalid TurnStatus change : ScenarioPanel.cs - DealTurnGauge()");
    //        Debug.LogError($"{prevStatus} -> {turn.Status}");
    //    }
    //}

    public void PlayTurnGauageAnimation()
    {
        float duration = turnManager.NowTurn.Period - turnManager.NowTurn.PlayTime;
        //Debug.Log($"play turn gauge : {duration}");

        turnGauge.rectTransform.DOScaleX(9300f, duration).SetEase(Ease.Linear);
    }

    public void StopTurnGaugeAnimation()
    {
        turnGauge.rectTransform.DOPause();
    }

    public void ResetTurnGauge()
    {
        //Debug.Log("reset turn gauge");
        turnGauge.rectTransform.DOScaleX(1f, 0f);
    }

    private void UpdateSlotObjectList(List<ActionSlot> scenario)
    {
        float lockedIndex = gameManager.LockedIndex;
        float slotWidth = (slotEndX - slotStartX - (lockedIndex - 1) * slotOffset) / lockedIndex;

        foreach (var slot in slotObjectList)
            Destroy(slot);

        slotObjectList = new List<GameObject>();

        for (int i = 0; i < lockedIndex; i++)
        {
            slotObjectList.Add(Instantiate(slotPrefabs[(int)scenario[i].PlacedAction.Type], scenarioTransform));
            slotObjectList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + slotOffset), slotY);
            slotObjectList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    public void UpdateSlotObjectList(int lockedIndex, List<ActionSlot> scenario)
    {
        float slotWidth = (slotEndX - slotStartX - (lockedIndex - 1) * slotOffset) / lockedIndex;

        foreach (var slot in slotObjectList)
            Destroy(slot);

        slotObjectList = new List<GameObject>();

        for (int i = 0; i < lockedIndex; i++)
        {
            slotObjectList.Add(Instantiate(slotPrefabs[(int)scenario[i].PlacedAction.Type], scenarioTransform));
            slotObjectList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + slotOffset), slotY);
            slotObjectList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }
}
