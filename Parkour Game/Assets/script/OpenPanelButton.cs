using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] private PanelType type;
    
    private scriptMenu controller;

    void Start()
    {
        controller = FindObjectOfType<scriptMenu>();    
    }

    public void Onclick(){
        controller.OpenPanel(type);
    }
}
