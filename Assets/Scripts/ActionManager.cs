using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public UIManager uiManager;


    private List<Action> actions;
    public List<ActionType> actionBundle;


    void Awake()
    {
        InitActions();
        UpdateActionBundle();
    }

    private void InitActions()
    {
        actions = new List<Action>(4)
        {
            new Action(ActionType.Breed),
            new Action(ActionType.Hunt),
            new Action(ActionType.Evolve),
            new Action(ActionType.Train)
        };
    }

    public void UpdateActionBundle()
    {
        // Action Bundle UI에 채워진 Action들을 actionBundle 리스트에 순서대로 추가

        actionBundle.Add(ActionType.Hunt);
        actionBundle.Add(ActionType.Hunt);
        actionBundle.Add(ActionType.Hunt);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Evolve);
        actionBundle.Add(ActionType.Evolve);
        actionBundle.Add(ActionType.Hunt);
    }

    public void PerformActionAt(Turn nowTurn, int actionIndex)
    {
        var nowAction = nowTurn.actionBundle[actionIndex];
        var variations = actions[(int)nowAction].PerformAction(nowTurn.endResources[0], nowTurn.endResources[(int)nowAction]);

        uiManager.UpdateResourceStatusTexts(nowTurn.endResources);
        uiManager.UpdateResourceDetailsTexts(nowTurn.endResources);
        uiManager.ShowVariationTexts(variations);
    }

    public bool IsPerformable(List<Resource> resources)
    {
        List<Resource> expectedResources = new List<Resource>(resources);

        foreach (var nowAction in actionBundle)
        {
            if (!actions[(int)nowAction].IsPerformable(expectedResources[0]))
            {
                return false;
            }

            actions[(int)nowAction].PerformAction(expectedResources[0], expectedResources[(int)nowAction]);
        }

        return true;
    }
}
