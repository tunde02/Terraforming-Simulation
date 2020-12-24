using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [BoxGroup("Texts")] public Text populationText;
    [BoxGroup("Texts")] public Text foodText;
    [BoxGroup("Texts")] public Text DNAText;
    [BoxGroup("Texts")] public Text powerText;

    public void UpdateResourceTexts(Resource[] resources)
    {
        populationText.text = resources[(int)ResourceID.Population].ToString();
        foodText.text       = resources[(int)ResourceID.Food].ToString();
        DNAText.text        = resources[(int)ResourceID.DNA].ToString();
        powerText.text      = resources[(int)ResourceID.Power].ToString();
    }
}
