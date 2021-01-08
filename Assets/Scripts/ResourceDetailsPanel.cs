using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDetailsPanel : MonoBehaviour
{
    public Text populationText;
    public Text foodText;
    public Text DNAText;
    public Text powerText;

    void Awake()
    {
        populationText.text = "";
        foodText.text = "";
        DNAText.text = "";
        powerText.text = "";
    }

    public void UpdateResourceDetailsTexts(Resource[] resources)
    {
        populationText.text = string.Format("{0:#,0}", resources[0].Storage);
        foodText.text       = string.Format("{0:#,0}", resources[1].Storage);
        DNAText.text        = string.Format("{0:#,0}", resources[2].Storage);
        powerText.text      = string.Format("{0:#,0}", resources[3].Storage);
    }
}
