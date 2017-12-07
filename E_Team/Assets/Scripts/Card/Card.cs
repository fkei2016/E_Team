// ==================================================
// カードの個別のスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour {

    public int number = 0;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        if (GetComponent<EventTrigger>() == null)
        {
            var trigger = gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(data => { Open(); });
            trigger.triggers.Add(entry);
        }
    }

    /// <summary>
    /// カードの表裏をフラグで設定
    /// </summary>
    /// <param name="isOpen"></param>
    private void Draw(bool isOpen) {
        var back = transform.GetChild(transform.childCount - 1).gameObject;
        back.SetActive(isOpen);
    }

    /// <summary>
    /// カードを開く
    /// </summary>
    public void Open() {
        Draw(false);
    }

    /// <summary>
    /// カードを閉じる
    /// </summary>
    public void Close() {
        Draw(true);
    }
}
