// ==================================================
// 拡張メゾットのクラス
// ==================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using EventCall = UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>;

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

    /// <summary>
    /// イベントトリガーの追加
    /// </summary>
    /// <param name="gameObject">
    /// 拡張メゾット
    /// </param>
    /// <param name="type">
    /// EventTriggerの種類
    /// </param>
    /// <param name="call">
    /// イベント実行時の処理
    /// </param>
    public static EventTrigger AddEventTrigger(this GameObject gameObject, EventTriggerType type, EventCall call) {
        // "EventTrigger"コンポーネントの取得
        var trigger = gameObject.AttachComponet<EventTrigger>();
        // イベント実行時の処理を登録
        var entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(call);
        // イベントの追加
        trigger.triggers.Add(entry);
        return trigger;
    }

    public static Animation AddAnimationClip(this GameObject gameObject, string[] animationNames) {
        // フェードアウトのアニメーションを追加
        var anim = gameObject.AttachComponet<Animation>();
        foreach (var name in animationNames)
        {
            var clip = (AnimationClip)Resources.Load(name);
            anim.AddClip(clip, clip.name);
        }
        return anim;
    }
}
