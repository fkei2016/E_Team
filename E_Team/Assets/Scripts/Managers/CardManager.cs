// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    [SerializeField, Range(3, 5)]
    private int pair = 3;
    [SerializeField]
    private Vector2 cardSize = new Vector2(150F, 250F);

    private int keepPairNum;
    private Card[] fieldCards;
    private Stack<Card> pairCard;
    private CardGenerator generator;
    private CardSpacement spacement;

    /// <summary>
    /// 開始時に処理
    /// </summary>
    private void Start() {
        // シーンから「生成機」と「配置」を検索
        generator = FindObjectOfType<CardGenerator>();
        spacement = FindObjectOfType<CardSpacement>();

        RemakeCards(pair * 2);
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {
        // ペア数が変わったときに再生成
        if (keepPairNum != pair)
        {
            foreach (var card in fieldCards)
                Destroy(card.gameObject);
            RemakeCards(pair * 2);
        }

        // カード配置の調整
        spacement.AdjustmentLayout(pair, cardSize);

        // [Debug]ペア番号を再設定
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetPairCards();
        }
    }

    /// <summary>
    /// カードの再生成
    /// </summary>
    /// <param name="cardNum"></param>
    void RemakeCards(int cardNum) {
        keepPairNum = cardNum / 2;
        fieldCards = generator.CreateCards(cardNum, cardSize);
        SetPairCards();
    }

    /// <summary>
    /// カードをペア番号に設定
    /// </summary>
    public void SetPairCards() {
        var pairList = MakePairNumbers();
        for (int i = 0; i < fieldCards.Length; i++)
        {
            var number = GetRandomRange(pairList);
            generator.ChangeCardNumber(number, fieldCards[i], cardSize);
        }
    }

    /// <summary>
    /// ペア番号のリストを作成
    /// </summary>
    /// <returns></returns>
    private List<int> MakePairNumbers() {
        // 「柄の数」分の数列を作成
        var rangeList = new List<int>();
        for (int i = 0; i < generator.DesignLength; i++)
        {
            rangeList.Add(i);
        }

        // 数列からペアになる番号の抽選
        var pairNumbers = new List<int>();
        for (var i = 0; i < fieldCards.Length / 2; i++)
        {
            var rnd = GetRandomRange(rangeList);
            pairNumbers.Add(rnd);
            pairNumbers.Add(rnd);
        }
        return pairNumbers;
    }


    /// <summary>
    /// リストから重複しない番号を抽選
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private int GetRandomRange(List<int> list) {
        var rnd = Random.Range(0, list.Count);
        rnd = list[rnd];
        list.Remove(rnd);
        return rnd;
    }
}
