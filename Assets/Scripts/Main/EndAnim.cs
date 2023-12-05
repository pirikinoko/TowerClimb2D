using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnim : MonoBehaviour
{
    float pastTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pastTime +=Time.deltaTime;
        if(pastTime > 0.4f)
        {
            Destroy(gameObject);
        }
    }
    public void OnAnimationEnd()
    {
        // アニメーションが終了したときに行いたい処理をここに書く
        Destroy(gameObject);
    }
}
