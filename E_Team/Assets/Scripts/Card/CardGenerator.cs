// ==================================================
// テクスチャからカードを生成するスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour {

    [SerializeField]
    private Texture2D backTex;
    [SerializeField]
    private Texture2D frameTex;
    [SerializeField]
    private Texture2D[] designs;

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
        // カードを複数枚の作成
        var cards = new Card[cardNum];
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i] = CreateCard();
        }
        return cards;
    }

    /// <summary>
    /// カードにペア番号を割り振る
    /// </summary>
    /// <param name="cards">
    /// カード配列
    /// </param>
    /// <param name="pairList">
    /// ペアリスト
    /// </param>
    public void AppendPairList(Card[] cards, int[] pairList) {
        for(int i = 0; i < cards.Length; i++)
        {
            var number = pairList[i];
            var design = cards[i].transform.GetChild(1).gameObject;
            Generator.AttachImage(design, designs[number]);
            cards[i].number = number;
        }
    }

    /// <summary>
    /// カードの生成
    /// </summary>
    /// <param name="number">
    /// カード番号
    /// </param>
    /// <returns>
    /// 単体のカード
    /// </returns>
    private Card CreateCard(int number = -1) {
        // 親オブジェクトの作成
        var obj = Generator.Create("Card", transform);

        // 「枠」の追加
        Generator.Create("Frame", obj.transform, frameTex);

        // 「柄」の追加
        var design = Generator.Create("Design", obj.transform);
        design.transform.rotation = Quaternion.Euler(Vector3.up * 180F);
        if(number != -1) Generator.AttachImage(design, designs[number]);

        // 「背面」の追加
        Generator.Create("Back", obj.transform, backTex);

        // カードに番号とサイズを割り振ってから返す
        var card = obj.AddComponent<Card>();
        card.number = number + 1;
        card.size = new Vector2(backTex.width, backTex.height);
        var cardRect = obj.GetComponent<RectTransform>();
        cardRect.sizeDelta = card.size;
        return card;
    }

    /// <summary>
    /// ペアリストの作成
    /// </summary>
    /// <param name="maxValue">
    /// リストの最大値
    /// </param>
    /// <param name="maxSize">
    /// リストの最大数
    /// </param>
    /// <returns>
    /// ペアリスト
    /// </returns>
    public List<int> MakePairList(int maxValue, int maxSize) {
        // 「柄の数」分の数列を作成
        var range = new List<int>();
        for (int i = 0; i < maxValue; i++)
        {
            range.Add(i);
        }

        // 数列からペアになる番号の抽選
        var pairList = new List<int>();
        for (var i = 0; i < maxSize / 2; i++)
        {
            var value = GetNonOverlappingValue(range);
            pairList.Add(value);
            pairList.Add(value);
        }
        return pairList;
    }


    /// <summary>
    /// リストから重複しない番号を抽選
    /// </summary>
    /// <param name="list">
    /// 整数リスト
    /// </param>
    /// <returns>
    /// 重複しない値
    /// </returns>
    public int GetNonOverlappingValue(List<int> list) {
        var value = Random.Range(0, list.Count);
        value = list[value];
        list.Remove(value);
        return value;
    }
}
