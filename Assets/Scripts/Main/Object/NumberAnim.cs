using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberAnim : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float fadeSpeed = 0.02f;
    public GameObject canvasObj;
    private Text textComponent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        textComponent = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();

        // テキストを上に移動するコルーチンを開始
        StartCoroutine(MoveAndFade());
    }

    IEnumerator MoveAndFade()
    {
        while (true)
        {
            // 上に移動
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // テキストが画面外に移動したら破棄
            if (transform.position.y > Screen.height)
            {
                Destroy(gameObject);
                yield break; // コルーチンを終了
            }

            yield return null;
        }
    }
}
