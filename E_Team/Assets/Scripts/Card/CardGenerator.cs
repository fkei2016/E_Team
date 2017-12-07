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

    private Card[] cards;

    /// <summary>
    /// カードの再生成
    /// </summary>
    /// <param name="cardNumMax"></param>
    public void RemakeCards(int cardNumMax) {

        // 既存のカードをすべて破棄
        foreach (Transform card in transform)
        {
            Destroy(card.gameObject);
        }

        // カードの生成
        cards = new Card[cardNumMax];
        for (int i = 0; i < cards.Length; i++)
        {
            var card = CreateCardDesign();
            cards[i] = card.AddComponent<Card>();
        }

        // カードの再設定
        ResetCardNumbers();
    }

    /// <summary>
    /// カードの再設定
    /// </summary>
    public void ResetCardNumbers() {

        // 全てのカードを閉じる
        foreach (var card in cards) card.Close();

        // カードの番号を設定
        var pairList = MakePairNumbers(designs.Length);
        for (int i = 0; i < cards.Length; i++)
        {
            var design = cards[i].transform.GetChild(1).gameObject;
            cards[i].number = ChangedCardNumber(design, RandomRange(pairList));
        }
    }

    /// <summary>
    /// カードの番号を変更する
    /// </summary>
    /// <param name="card"></param>
    /// <param name="num"></param>
    private int ChangedCardNumber(GameObject obj, int num) {
        var rectSize = CardManager.instance.CardSize;
        AttachImage(obj, designs[num], rectSize);
        return num + 1;
    }

    /// <summary>
    /// 組になる数字を作る
    /// </summary>
    /// <param name="maximum"></param>
    /// <returns></returns>
    private List<int> MakePairNumbers(int maximum) {
        // 最大数までの数列を作成
        var rangeList = new List<int>();
        for (var i = 0; i < maximum; i++)
        {
            rangeList.Add(i);
        }

        // ペアになる番号の抽選
        var pairNumbers = new List<int>();
        for (var i = 0; i < cards.Length / 2; i++)
        {
            var rnd = RandomRange(rangeList);
            pairNumbers.Add(rnd);
            pairNumbers.Add(rnd);
        }
        return pairNumbers;
    }

    /// <summary>
    /// カードのデザインを生成
    /// </summary>
    /// <param name="designTex"></param>
    private GameObject CreateCardDesign() {
        Vector2 size = new Vector2(150F, 250F);

        // 親オブジェクトの作成
        var card = CreateUI("Card", transform);

        // 「枠」の追加
        var frame = CreateUI("Frame", card.transform);
        AttachImage(frame, frameTex, size);

        // 「柄」の追加
        var design = CreateUI("Design", card.transform);
        //AttachImage(design, designTex, size);

        // 「背面」の追加
        var back = CreateUI("Back", card.transform);
        AttachImage(back, backTex, size);

        return card;
    }

    /// <summary>
    /// リストから重複しない番号を抽選
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private int RandomRange(List<int> list) {
        var rnd = Random.Range(0, list.Count);
        rnd = list[rnd];
        list.Remove(rnd);
        return rnd;
    }
}
