using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GainScoresAnime : MonoBehaviour
{
    public float scoreDisplay, diffGoal;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.name.Contains("Multi"))
        {
            StartCoroutine(MultiTextDisplay());
        }
        else
        {
            StartCoroutine(GainScore());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //倍率表示
    IEnumerator MultiTextDisplay()
    {
        float buffMulti = 3.0f;
        Text multiText = gameObject.GetComponent<Text>();
        multiText.text = String.Format("x" + "{0:####}", buffMulti);
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
    }
    IEnumerator GainScore()
    {
        Text scoreText = gameObject.GetComponent<Text>();
        scoreText.text = String.Format("{0:####}", scoreDisplay);
        if (diffGoal < 0)
        {
            scoreText.text = "-" + String.Format("{0:####}", scoreDisplay);
        }

        yield return new WaitForSeconds(0.4f);
       

        while (scoreDisplay < diffGoal)
        {
            scoreDisplay += diffGoal / 100;

            scoreText.text = String.Format("{0:####}", scoreDisplay);
            if (diffGoal < 0)
            {
                scoreText.text = "-" + String.Format("{0:####}", scoreDisplay);
            }
            yield return new WaitForSeconds(0.03f * Time.deltaTime);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
