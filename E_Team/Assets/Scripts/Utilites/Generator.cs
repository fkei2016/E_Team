// ==================================================
// オブジェクトを生成するスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour {

    /// <summary>
    /// UIオブジェクトの作成
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    static public GameObject CreateUI(string name, Transform parent) {
        // オブジェクトを生成してUIに必要な設定をする
        var obj = new GameObject(name);
        obj.AddComponent<RectTransform>();
        obj.transform.SetParent(parent, false);
        obj.transform.localScale = Vector2.one;
        return obj;
    }

    /// <summary>
    /// UIイメージの追加
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="tex"></param>
    /// <param name="size"></param>
    static public void AttachImage(GameObject owner, Texture2D tex) {
        // テクスチャのサイズを取得
        var size = new Vector2(tex.width, tex.height);

        // "Image"コンポーネントを取得
        var image = owner.GetComponent<Image>();
        if(image == null)
        {
            image = owner.AddComponent<Image>();
        }

        // サイズとスプライトの設定
        image.rectTransform.sizeDelta = size;
        image.sprite = Sprite.Create(tex, new Rect(Vector2.zero, size), Vector2.one * 0.5F);
    }
}
