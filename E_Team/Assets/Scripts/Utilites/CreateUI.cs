// ==================================================
// 動的にUIを生成してくれる神スクリプト
// ==================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateUI : MonoBehaviour {

    /// TODO : Create関数の引数にUI関連コンポーネントを追加したいので、
    /// 匿名関数を使い"Image","Button","Text"をアタッチできるようにする

    /// <summary>
    /// UIオブジェクトの生成
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    static public GameObject Create(string objName, Transform parent = null, Action act = null) {
        // RectTransformを追加してUIにする
        var obj = new GameObject(objName);
        obj.AddComponent<RectTransform>();
        // 親子設定とスケールの補完
        obj.transform.SetParent(parent);
        obj.transform.localScale = Vector3.one;
        return obj;
    }

    /// <summary>
    /// UIに対して画像をアタッチ
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="sprite"></param>
    /// <param name="rectSize"></param>
    static public void Attach(GameObject ui, Texture2D sprite, Vector2 rectSize) {
        // UIオブジェクトにImageを追加
        var image = ui.AddComponent<Image>();
        // 画像を作成してUIにアタッチ
        image.rectTransform.sizeDelta = rectSize;
        image.rectTransform.anchoredPosition = Vector2.zero;
        image.sprite = Sprite.Create(sprite, new Rect(Vector2.zero, rectSize), Vector2.one * 0.5F);
    }
}
