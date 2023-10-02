using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBar : MonoBehaviour
{
    Slider difficultyBar;
    // Start is called before the first frame update
    void Start()
    {
        difficultyBar = this.gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        difficultyBar.value = Player.avgSpeedY;
    }
}
