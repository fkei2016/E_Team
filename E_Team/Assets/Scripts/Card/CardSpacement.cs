// ==================================================
// カードの並びを均等にするスクリプト
// ==================================================
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardSpacement : MonoBehaviour {

    [SerializeField]
    private Texture2D baseTex;
    [SerializeField]
    private RectOffset offset;

    private Rect rect;
    private GridLayoutGroup layout;

    /// <summary>
    /// 生成時の処理
    /// </summary>
    private void Awake() {
        // レイアウトの矩形を取得
        rect = GetComponent<RectTransform>().rect;
        // 均等に並ぶようにレイアウトを設定
        layout = GetComponent<GridLayoutGroup>();
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
        var w = rect.width / row;
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
        var margin = rect.size - new Vector2(cardSize.x * row, cardSize.y * col);
        layout.padding.left = offset.left + (int)margin.x / 4;
        layout.padding.top = offset.top + (int)margin.y / 4;
        layout.spacing = new Vector2(0F, layout.padding.top / 4F);
    }

    /// <summary>
    /// カード座標の生成
    /// </summary>
    /// <param name="transform">
    /// 親のトランスフォーム
    /// </param>
    /// <param name="cards">
    /// 使用されるカード
    /// </param>
    public Vector3[] MakePositions(Card[] cards) {
        // 既にある子要素（座標）を破棄
        foreach (RectTransform child in transform)
        {
            Destroy(child.gameObject);
        }

        // カードの枚数分だけ座標を生成
        var positions = new Vector3[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            var obj = Generator.Create("Pos(" + i + ")", transform, baseTex);
            positions[i] = obj.transform.position;
        }
        return positions;
    }
}
