using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject timerFrame;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.position = timerFrame.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Transform handsTransform = this.transform;
        Vector3 worldAngle = handsTransform.eulerAngles;
        worldAngle.z -= 20.0f * Time.deltaTime;
        handsTransform.eulerAngles = worldAngle; 

    }
}
