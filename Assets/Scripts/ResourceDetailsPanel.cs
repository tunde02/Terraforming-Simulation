using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceDetailsPanel : MonoBehaviour
{
    [SerializeField] private Text[] resourceDetailsTexts;


    void Awake()
    {
        Resource.OnStorageSet += SetResourceDetailsText;
    }

    public void SetResourceDetailsText(Resource resource)
    {
        resourceDetailsTexts[(int)resource.Type].text = string.Format("{0:#,0}", resource.Storage);
    }
}
