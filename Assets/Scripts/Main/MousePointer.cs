using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public static Vector3 mouse, pointer;

    void Update()
    {
        mouse = Input.mousePosition;
        pointer = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));
        this.transform.position = pointer;
    }
}
