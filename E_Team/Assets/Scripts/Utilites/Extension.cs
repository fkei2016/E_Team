// ==================================================
// 拡張メゾットのクラス
// ==================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension {

    /// <summary>
    /// 座標系のリセット
    /// </summary>
    /// <param name="trans">
    /// 拡張メゾット
    /// </param>
    /// <param name="parent">
    /// 親トランスフォーム
    /// </param>
    public static void Reset(this Transform trans, Transform parent = null, bool worldPositionStays = true) {
        trans.SetParent(parent, worldPositionStays);
        trans.localPosition = Vector3.zero;
        trans.rotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    /// <summary>
    /// コンポーネントの追加
    /// </summary>
    /// <typeparam name="Type">
    /// Unityの標準型
    /// </typeparam>
    /// <param name="gameObject">
    /// 拡張メゾット
    /// </param>
    /// <returns>
    /// アタッチする型
    /// </returns>
    public static Type AttachComponet<Type>(this GameObject gameObject) where Type : Component {
        // 既にコンポーネントがあるが検知
        var component = gameObject.GetComponent<Type>();
        // 追加したコンポーネントを返す
        return (component) ? component : gameObject.AddComponent<Type>();
    }
}
