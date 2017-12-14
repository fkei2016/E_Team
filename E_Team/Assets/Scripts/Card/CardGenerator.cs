// ==================================================
// テクスチャからカードを生成するスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : Generator {

    [SerializeField]
    private Texture2D backTex;
    [SerializeField]
    private Texture2D frameTex;
    [SerializeField]
    private Texture2D[] designs;

    public int DesignLength { get { return designs.Length; } }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <returns>
    /// 単体のカード
    /// </returns>
    public Card CreateCard() {
        var card = CreateCardDesign();
        return card.AddComponent<Card>();
    }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <param name="cardNum">
    /// カード番号
    /// </param>
    /// <returns>
    /// 複数のカード
    /// </returns>
    public Card[] CreateCards(int cardNum) {
        var cards = new Card[cardNum];
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i] = CreateCard();
        }
        return cards;
    }

    /// <summary>
    /// カードの番号を変更
    /// </summary>
    /// <param name="number">
    /// カード番号
    /// </param>
    /// <param name="card">
    /// カード
    /// </param>
    public void ChangeCardNumber(int number, Card card) {
        // カード番号の割り当て
        card.number = number + 1;

        // カードの「柄」部分を変更
        var cardDesign = card.transform.GetChild(1).gameObject;
        AttachImage(cardDesign, designs[number]);
    }


    /// <summary>
    /// カードデザインの生成
    /// </summary>
    /// <returns>
    /// カードのオブジェクト
    /// </returns>
    private GameObject CreateCardDesign() {
        // 親オブジェクトの作成
        var card = CreateUI("Card", transform);

        // 「枠」の追加
        var frame = CreateUI("Frame", card.transform);
        AttachImage(frame, frameTex);

        // 「柄」の追加
        var design = CreateUI("Design", card.transform);
        //AttachImage(design, designTex, cardSize);

        // 「背面」の追加
        var back = CreateUI("Back", card.transform);
        AttachImage(back, backTex);

        return card;
    }
}
