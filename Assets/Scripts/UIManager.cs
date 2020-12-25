using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ResourceStatusUI resourceStatusUI;

    public void UpdateResourceTexts(Resource[] resources)
    {
        resourceStatusUI.UpdateResourceTexts(resources);
    }
}
