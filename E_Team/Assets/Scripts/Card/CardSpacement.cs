// ==================================================
// カードの並びを均等にするスクリプト
// ==================================================
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardSpacement : MonoBehaviour {

    private float rectWidth;
    private GridLayoutGroup layout;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        // レイアウトの横幅を取得
        rectWidth = GetComponent<RectTransform>().sizeDelta.x;
        // 均等に並ぶようにレイアウトを設定
        layout = GetComponent<GridLayoutGroup>();
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        // レイヤーとカードの横幅から1枚ごとの間隔を計算
        var pairCardNum = CardManager.instance.Pair;
        var cardsWidth = CardManager.instance.CardSize.x * pairCardNum;
        var spacing = (rectWidth - cardsWidth) / pairCardNum;

        // 均等に並ぶようにレイアウトを設定
        layout.padding.left = (int)spacing;
        layout.spacing = new Vector2(spacing / 2, 10);
    }
}
