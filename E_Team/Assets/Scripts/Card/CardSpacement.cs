// ==================================================
// カードの並びを均等にするスクリプト
// ==================================================
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardSpacement : MonoBehaviour {

    private Rect rectSize;
    private GridLayoutGroup layout;

    /// <summary>
    /// 生成時の処理
    /// </summary>
    private void Awake() {
        // レイアウトの矩形を取得
        rectSize = GetComponent<RectTransform>().rect;
        // 均等に並ぶようにレイアウトを設定
        layout = GetComponent<GridLayoutGroup>();
    }

    /// <summary>
    /// レイアウトの調整
    /// </summary>
    /// <param name="rowCardNum"></param>
    /// <param name="cardSize"></param>
    public void AdjustmentLayout(int rowCardNum, Vector2 cardSize) {
        //// レイヤーとカードの横幅から1枚ごとの間隔を計算
        //var cardsWidth = cardSize.x * rowCardNum;
        //var spacing = (rectWidth - cardsWidth) / rowCardNum;

        //// 均等に並ぶようにレイアウトを設定
        //layout.cellSize = cardSize;
        //layout.padding.left = (int)spacing;
        //layout.spacing = new Vector2(spacing / 2, layout.spacing.y);
    }

    public void AdjustmentLayout(Card[] cardList, Vector2 cardSize) {

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
    }
}
