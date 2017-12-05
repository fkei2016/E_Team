// ==================================================
// カードの個別クラス
// ==================================================
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class Card : MonoBehaviour {

    //XXX:スクリプト内でUIを生成するべき
    private GameObject frame;
    private GameObject design;
    private GameObject back;

    //public Card(Texture2D backTex, Texture2D frameTex, Texture2D designTex, Vector2 rectSize) {
    //    // 枠画像の生成
    //    frame = CreateUI.Create("Frame", transform);
    //    CreateUI.Attach(frame, frameTex, rectSize);

    //    // デザイン画像の生成
    //    design = CreateUI.Create("Design", transform);
    //    CreateUI.Attach(design, designTex, rectSize);

    //    // 背面画像の生成
    //    back = CreateUI.Create("Back", transform);
    //    CreateUI.Attach(back, backTex, rectSize);
    //    back.SetActive(true);
    //}

    /// <summary>
    /// 生成時に実行
    /// </summary>
    private void Awake() {
        // イベント駆動となるEventTriggerを取得
        var trigger = GetComponent<EventTrigger>();
        // "PointerDown"時の処理を登録
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(data => { Click(); });
        // EventTriggerにイベントを追加
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// クリックされたときに実行
    /// </summary>
    public void Click() {
        back = transform.GetChild(2).gameObject;
        // 背面画像の表示有無で表裏を再現
        back.SetActive(false);
    }
}
