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

        var root = new GameObject("ComboString");
        //root.AddComponent<RectTransform>();
        var canvasGroup = root.AddComponent<CanvasGroup>();
        root.transform.Reset(TargetCanvas.transform);
        //root.transform.SetParent(this.TargetCanvas.transform);
        //root.transform.localPosition = Vector3.zero;
        //root.transform.localScale=Vector3.one;
        //root.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        //root.GetComponent<RectTransform>().anchorMax = Vector2.zero;
        //root.transform.localPosition = Vector3.zero;
        //root.transform.localScale = Vector3.one;

        //print("aaa= " + root.GetComponent<RectTransform>().transform.localPosition);
        //root.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

        // 文字幅の計算
        var fontSize = (PopupTextObject.GetComponent<Text>().fontSize * 0.7F);
        //var canvasGroup = root.AddComponent<CanvasGroup>();
        //print(PopupString.Length);
        //文字中央寄せ
        pos.x = pos.x - ((PopupString.Length-1) / 2 * fontSize);
        foreach (var s in this.PopupString)
        {
            var obj = new GameObject("ValueText");
            obj.transform.Reset(pos, Quaternion.identity, Vector3.one, root.transform);
            //obj.AddComponent<RectTransform>();
            //obj.transform.SetParent(root.transform);
            //obj.transform.localPosition = pos;
            //obj.transform.localScale = Vector3.one;


            // 1文字ずつ生成
            var valueText = Instantiate(this.PopupTextObject, Vector3.zero, Quaternion.identity);
            var textComp = valueText.GetComponent<Text>();
            textComp.text = s.ToString();
            valueText.transform.Reset(obj.transform);
            //valueText.transform.SetParent(obj.transform);
            //valueText.transform.localPosition = Vector3.zero;
            //valueText.transform.localScale = Vector3.one;
            texts.Add(valueText.GetComponent<Combo>());

            // 0.03秒待つ(適当)
            yield return new WaitForSeconds(0.03f);

            // 次の位置
            pos.x += fontSize;
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