// ==================================================
// オブジェクトを生成するスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour {

    /// <summary>
    /// オブジェクトの作成
    /// </summary>
    /// <param name="name">
    /// オブジェクト名
    /// </param>
    /// <param name="parent">
    /// 親トランスフォーム
    /// </param>
    /// <returns>
    /// 生成済みオブジェクト
    /// </returns>
    static public GameObject Create(string name, Transform parent) {
        // オブジェクトを生成してUIに必要な設定をする
        var obj = new GameObject(name);

        obj.AddComponent<RectTransform>();
        obj.transform.Reset(parent, false);
        return obj;
    }


    static public GameObject CreateNetworkObj(string name, Transform parent)
    {

        var gobj = PhotonNetwork.Instantiate(name, parent.position, parent.rotation, 0);

        gobj.AddComponent<RectTransform>();
        gobj.transform.Reset(parent, false);

        return gobj;
    }



    /// <summary>
    /// イメージの作成
    /// </summary>
    /// <param name="name">
    /// オブジェクト名
    /// </param>
    /// <param name="parent">
    /// 親トランスフォーム
    /// </param>
    /// <param name="tex">
    /// テクスチャ
    /// </param>
    /// <returns>
    /// 生成済みオブジェクト
    /// </returns>
    static public GameObject Create(string name, Transform parent, Texture2D tex) {
        var obj = Create(name, parent);
        AttachImage(obj, tex);
        return obj;
    }

    /// <summary>
    /// イメージの追加
    /// </summary>
    /// <param name="owner">
    /// アタッチ先
    /// </param>
    /// <param name="tex">
    /// テクスチャ
    /// </param>
    static public void AttachImage(GameObject owner, Texture2D tex) {
        // テクスチャのサイズを取得
        var size = new Vector2(tex.width, tex.height);

       // "Image"コンポーネントを取得
       var image = owner.AttachComponet<Image>();
        // サイズとスプライトの設定
        image.rectTransform.sizeDelta = size;
        image.sprite = Sprite.Create(tex, new Rect(Vector2.zero, size), Vector2.one * 0.5F);
    }
}
