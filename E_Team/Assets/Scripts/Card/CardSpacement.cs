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
    /// 生成時の処理
    /// </summary>
    private void Awake() {
        // レイアウトの横幅を取得
        rectWidth = GetComponent<RectTransform>().sizeDelta.x;
        // 均等に並ぶようにレイアウトを設定
        layout = GetComponent<GridLayoutGroup>();
    }

    /// <summary>
    /// レイアウトの調整
    /// </summary>
    /// <param name="rowCardNum"></param>
    /// <param name="cardSize"></param>
    public void AdjustmentLayout(int rowCardNum, Vector2 cardSize) {
        // レイヤーとカードの横幅から1枚ごとの間隔を計算
        var cardsWidth = cardSize.x * rowCardNum;
        var spacing = (rectWidth - cardsWidth) / rowCardNum;

        // 均等に並ぶようにレイアウトを設定
        layout.cellSize = cardSize;
        layout.padding.left = (int)spacing;
        layout.spacing = new Vector2(spacing / 2, layout.spacing.y);
    }
}
