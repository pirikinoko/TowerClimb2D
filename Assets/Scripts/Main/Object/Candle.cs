using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Candle : MonoBehaviour
{
    private Light2D light;
    float intensityDefault;
    // Start is called before the first frame update
    void Start()
    {
        light = this.gameObject.GetComponent<Light2D>();
        intensityDefault = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        float rnd = Random.Range(100, 120);
        light.intensity = intensityDefault * (rnd / 100);
    }
}
