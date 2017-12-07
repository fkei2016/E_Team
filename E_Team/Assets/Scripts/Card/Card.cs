// ==================================================
// カードの個別のスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour {

    public int number = 0;

    private GameObject back;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        // 背面の取得
        back = transform.GetChild(transform.childCount - 1).gameObject;

        // "EventTrigger"に追加する
        var trigger = gameObject.AddComponent<EventTrigger>();
        // "PointerDown"に処理を登録
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(data => { if (back.activeSelf) Open(); });
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// カードの表裏を設定
    /// </summary>
    /// <param name="isOpen"></param>
    private void Draw(bool isOpen) {
        back.SetActive(isOpen);
    }

    /// <summary>
    /// カードを開く
    /// </summary>
    public void Open() {
        Draw(false);
        Debug.Log(number);
    }

    /// <summary>
    /// カードを閉じる
    /// </summary>
    public void Close() {
        Draw(true);
    }
}
