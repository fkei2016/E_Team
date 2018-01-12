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
        // ペアリストの作成
        var pairList = MakePairNumbers(designs.Length, cardNum);

        // カードを複数枚の作成
        var cards = new Card[cardNum];
        for(int i = 0; i < cards.Length; i++)
        {
            var num = GetNonOverlappingValue(pairList);
            cards[i] = CreateCard(num);
        }
        return cards;
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
    private Card CreateCard(int number) {
        // 親オブジェクトの作成
        var obj = Generator.Create("Card", transform);

        // 「枠」の追加
        Generator.Create("Frame", obj.transform, frameTex);

        // 「柄」の追加
        Generator.Create("Design", obj.transform, designs[number]).transform.rotation = Quaternion.Euler(Vector3.up * 180F);

        // 「背面」の追加
        Generator.Create("Back", obj.transform, backTex);

        // カードに番号とサイズを割り振ってから返す
        var card = obj.AddComponent<Card>();
        card.number = number + 1;
        card.size = new Vector2(backTex.width, backTex.height);


        return card;
    }

    /// <summary>
    /// ペアリストの作成
    /// </summary>
    /// <param name="numLength">
    /// 数列の長さ
    /// </param>
    /// <param name="cardMaxSize">
    /// カードの最大数
    /// </param>
    /// <returns>
    /// ペアリスト
    /// </returns>
    private List<int> MakePairNumbers(int numLength, int cardMaxSize) {
        // 「柄の数」分の数列を作成
        var rangeList = new List<int>();
        for (int i = 0; i < numLength; i++)
        {
            rangeList.Add(i);
        }

        // 数列からペアになる番号の抽選
        var pairNumbers = new List<int>();
        for (var i = 0; i < cardMaxSize / 2; i++)
        {
            var num = GetNonOverlappingValue(rangeList);
            pairNumbers.Add(num);
            pairNumbers.Add(num);
        }
        return pairNumbers;
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
    private int GetNonOverlappingValue(List<int> list) {
        // ランダムに要素番号を選ぶ
        var element = Random.Range(0, list.Count);
        // リストから値を取り除く
        element = list[element];
        list.Remove(element);
        return element;
    }
}
