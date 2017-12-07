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
    /// <returns></returns>
    public Card CreateCard(Vector2 cardSize) {
        var card = CreateCardDesign(cardSize);
        return card.AddComponent<Card>();
    }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <returns></returns>
    public Card[] CreateCards(int cardNum, Vector2 cardSize) {
        var cards = new Card[cardNum];
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i] = CreateCard(cardSize);
        }
        return cards;
    }

    /// <summary>
    /// カードの番号を変更
    /// </summary>
    /// <param name="number"></param>
    /// <param name="card"></param>
    /// <param name="cardSize"></param>
    public void ChangeCardNumber(int number, Card card, Vector2 cardSize) {
        var cardDesign = card.transform.GetChild(1).gameObject;
        AttachImage(cardDesign, designs[number], cardSize);
        card.number = number + 1;
    }


    /// <summary>
    /// カードのデザインを生成
    /// </summary>
    /// <param name="designTex"></param>
    private GameObject CreateCardDesign(Vector2 cardSize) {
        // 親オブジェクトの作成
        var card = CreateUI("Card", transform);

        // 「枠」の追加
        var frame = CreateUI("Frame", card.transform);
        AttachImage(frame, frameTex, cardSize);

        // 「柄」の追加
        var design = CreateUI("Design", card.transform);
        //AttachImage(design, designTex, cardSize);

        // 「背面」の追加
        var back = CreateUI("Back", card.transform);
        AttachImage(back, backTex, cardSize);

        return card;
    }
}
