using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class UIManager : MonoBehaviour
{
    [SerializeField] private ResourceStatusPanel resourceStatusPanel;
    [SerializeField] private ResourceDetailsPanel resourceDetailsPanel;
    [SerializeField] private ScenarioPanel scenarioPanel;
    [SerializeField] private ScenarioDetailsPanel scenarioDetailsPanel;
    [SerializeField] private GameObject scenarioDetailsCanvas;
    [SerializeField] private GameObject alertWindowPrefab;

    private ActionManager actionManager;


    [Inject]
    public void Construct(ActionManager actionManager)
    {
        this.actionManager = actionManager;
    }

    public void ShowAlertWindow(string alertType)
    {
        // TODO: add click sound
        GameObject alertWindow = Instantiate(alertWindowPrefab, scenarioDetailsCanvas.transform);
        Text alertContent = alertWindow.GetComponentInChildren<Text>();
        Button leftButton = alertWindow.GetComponentsInChildren<Button>()[0];
        Button rightButton = alertWindow.GetComponentsInChildren<Button>()[1];

        switch (alertType)
        {
            case "No More Empty Slot":
                alertContent.text = "슬롯이 가득 찼습니다.";

                leftButton.gameObject.SetActive(false);

                rightButton.GetComponentInChildren<Text>().text = "확인";
                rightButton.onClick.AddListener(() => {
                    Destroy(alertWindow);
                });
                break;
            case "Not Enough Slots":
                alertContent.text = "슬롯이 다 채워지지 않았습니다.";

                leftButton.gameObject.SetActive(false);

                rightButton.GetComponentInChildren<Text>().text = "확인";
                rightButton.onClick.AddListener(() => {
                    Destroy(alertWindow);
                });
                break;
            case "Scenario Unsaved":
                alertContent.text = "시나리오가 저장되지 않았습니다.\n시나리오를 저장하지 않고 창을 닫으시겠습니까?";
                alertContent.fontSize = 24;

                leftButton.onClick.AddListener(() => {
                    actionManager.ResetScenario();
                    scenarioDetailsCanvas.SetActive(false);
                    Destroy(alertWindow);
                });

                rightButton.onClick.AddListener(() => {
                    Destroy(alertWindow);
                });
                break;
            case "Impossible Scenario":
                alertContent.text = "불가능한 시나리오입니다.";

                leftButton.gameObject.SetActive(false);

                rightButton.GetComponentInChildren<Text>().text = "확인";
                rightButton.onClick.AddListener(() => {
                    Destroy(alertWindow);
                });
                break;
            case "Saved Successfully":
                alertContent.text = "시나리오가 저장되었습니다.";

                leftButton.gameObject.SetActive(false);

                rightButton.GetComponentInChildren<Text>().text = "확인";
                rightButton.onClick.AddListener(() => {
                    Destroy(alertWindow);
                });
                break;
            default:
                Debug.LogError("Invalid alertType : UIManager.cs - ShowAlert()");
                break;
        }
    }
}
