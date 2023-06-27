using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public static int Stage, NumberOfStages = 1;
    public GameObject StartPanel;
    public static bool SPanelActive;
    void Start()
    {
        SPanelActive = false;
        Stage = 1;
    }
    void Update()
    {
        PanelActive();

    }
    void PanelActive()
    {
        if (SPanelActive)
        {
            StartPanel.gameObject.SetActive(true);
        }
        else
        {
            StartPanel.gameObject.SetActive(false);
        }
    }

}
