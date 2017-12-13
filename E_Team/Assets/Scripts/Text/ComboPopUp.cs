using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ComboPopUp : MonoBehaviour
{

    /// <summary>
    /// 出力先キャンバス
    /// </summary>
    public GameObject TargetCanvas;

    /// <summary>
    /// 表示する文字
    /// </summary>
    public string PopupString;

    /// <summary>
    /// ポップアップするテキストオブジェクト
    /// NumberTextScript付き
    /// </summary>
    public GameObject PopupTextObject;

    /// <summary>
    /// ポップアップする位置
    /// </summary>
    public Vector3 PopupPosition;

    /// <summary>
    /// 1文字の幅
    /// </summary>
    public float PopupTextWidth;

    /// <summary>
    /// ポップアップの実行
    /// </summary>
    public void Popup()
    {
        StartCoroutine(Execute());
    }

    /// <summary>
    /// ポップアップ実行
    /// </summary>
    private IEnumerator Execute()
    {
        var pos = this.PopupPosition;
        var texts = new List<Combo>();

        var root = new GameObject();
        root.AddComponent<RectTransform>();
        root.transform.SetParent(this.TargetCanvas.transform);
        root.transform.localPosition = Vector3.zero;
        root.transform.localScale = Vector3.one;

        var canvasGroup = root.AddComponent<CanvasGroup>();

        foreach (var s in this.PopupString)
        {
            var obj = new GameObject();
            obj.AddComponent<RectTransform>();
            obj.transform.SetParent(root.transform);
            obj.transform.localPosition = pos;
            obj.transform.localScale = Vector3.one;

            // 1文字ずつ生成
            var valueText = (GameObject)Instantiate(this.PopupTextObject, Vector3.zero, Quaternion.identity);
            var textComp = valueText.GetComponent<Text>();
            textComp.text = s.ToString();
            valueText.transform.SetParent(obj.transform);
            valueText.transform.localPosition = Vector3.zero;
            valueText.transform.localScale = Vector3.one;
            texts.Add(valueText.GetComponent<Combo>());

            // 0.03秒待つ(適当)
            yield return new WaitForSeconds(0.03f);

            // 次の位置
            pos.x += this.PopupTextWidth;
        }

        // 適当に待ち
        while (!texts.TrueForAll(t => t.IsFinish))
        {
            yield return new WaitForSeconds(0.1f);
        }

        // フェードアウト
        for (int n = 9; n >= 0; n--)
        {
            canvasGroup.alpha = n / 10.0f;
            yield return new WaitForSeconds(0.01f);
        }

        // 破棄
        Destroy(root);
        Destroy(gameObject);
    }
}