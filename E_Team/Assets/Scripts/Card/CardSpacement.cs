// ==================================================
// カードの並びを均等にするスクリプト
// ==================================================
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardSpacement : MonoBehaviour {

    private Rect rectSize;
    private GridLayoutGroup layout;
    private RectTransform rectTransform;

    public RectTransform RectTransform { get { return rectTransform; } }

    /// <summary>
    /// 生成時の処理
    /// </summary>
    private void Awake() {
        // 均等に並ぶようにレイアウトを設定
        layout = GetComponent<GridLayoutGroup>();
        // 矩形のトランスフォームを取得
        rectTransform = GetComponent<RectTransform>();
        // レイアウトの矩形を取得
        rectSize = rectTransform.rect;
    }

    /// <summary>
    /// レイアウトの調整
    /// </summary>
    /// <param name="cardList">
    /// 複数枚のカード
    /// </param>
    public void AdjustmentLayout(Card[] cardList) {
        // カードサイズを取得
        var cardSize = cardList[0].size;

        // 横の枚数、縦の枚数を算出
        var row = cardList.Length / 2;
        var col = cardList.Length / row;

        // カードサイズの横：縦で調整
        var w = rectSize.width / row;
        var scale = w / cardSize.x;
        var h = cardSize.y * scale;

        // １枚ごとのセルサイズを設定
        layout.cellSize = new Vector2(w, h);

        // カードの拡大率を修正
        foreach(var card in cardList)
        {
            card.transform.localScale = Vector2.one * scale;
        }
        cardSize *= scale;

        // レイアウトのサイズや位置の修正
        var margin = rectSize.size - new Vector2(cardSize.x * row, cardSize.y * col);
        layout.padding.left = (int)margin.x / 4;
        layout.padding.top = (int)margin.y / 4;
        layout.spacing = new Vector2(0F, layout.padding.top);
    }
}
