using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
   
    public void StartGame()
    {
        SceneManager.LoadScene("Stage" + GameStart.Stage.ToString());
    }
    public void NextStage()
    {
        if (GameStart.Stage < GameStart.NumberOfStages)
        {
            GameStart.Stage++;
        }
    }
    public void PrevStage()
    {
        if (GameStart.Stage > 1)
        {
            GameStart.Stage--;
        }
    }
    public void OpenStartPanel()
    {
        GameStart.SPanelActive = true;
    }
    public void CloseStartPanel()
    {
        GameStart.SPanelActive = false;
    }
}
