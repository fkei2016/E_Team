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
    private Vector3 toRotation;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        // 背面の取得
        back = transform.GetChild(transform.childCount - 1).gameObject;
        // 初期の角度を設定
        toRotation = Vector3.zero;

        // "EventTrigger"に追加する
        var trigger = gameObject.AddComponent<EventTrigger>();
        // "PointerDown"に処理を登録
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(data => { if (back.activeSelf) Open(); });
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        // 回転処理をかける
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(toRotation), Time.deltaTime);

        // 「柄」を表示
        if (back.activeSelf && transform.localEulerAngles.y >= 180F - 90F)
        {
            back.SetActive(false);
        }
        // 「背面」を表示
        if(!back.activeSelf && transform.localEulerAngles.y >= 360F - 90F)
        {
            back.SetActive(true);
        }
    }

    /// <summary>
    /// カードを開く
    /// </summary>
    public void Open() {
        back.SetActive(false);
        toRotation = Vector3.up * 180F;
        CardManager.instance.SendCard(this);
    }

    /// <summary>
    /// カードを閉じる
    /// </summary>
    public IEnumerator Close(float waitTime = 1F) {
        yield return new WaitForSeconds(waitTime);
        toRotation = Vector3.up * 360F;
        back.SetActive(true);
    }
}
