// ==================================================
// テクスチャからカードを生成するスクリプト
// ==================================================
using UnityEngine;

public class CardResourceGenerator : MonoBehaviour {

    [SerializeField]
    private Texture2D cardBack;
    [SerializeField]
    private Texture2D cardFrame;
    [SerializeField]
    private Texture2D[] cardDesigns;

    [HideInInspector]
    public GameObject[] cards;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        // カードのサイズを管理者から取得
        var rectSize = PlayManager.instance.CardSize;

        // デザイン数のカードを生成
        cards = new GameObject[cardDesigns.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            // UIオブジェクトの生成
            var card = CreateUI.Create("Card (" + i + ")", transform);
            card.AddComponent<Card>();

            // 枠画像の生成
            var frame = CreateUI.Create("Frame", card.transform);
            CreateUI.Attach(frame, cardFrame, rectSize);

            // デザイン画像の生成
            var design = CreateUI.Create("Design", card.transform);
            CreateUI.Attach(design, cardDesigns[i], rectSize);

            // 背面画像の生成
            var back = CreateUI.Create("Back", card.transform);
            CreateUI.Attach(back, cardBack, rectSize);
            back.SetActive(true);

            // カードを保存
            cards[i] = card;
        }
    }
}
