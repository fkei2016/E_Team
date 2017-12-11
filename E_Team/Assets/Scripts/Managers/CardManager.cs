// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    [SerializeField, Range(3, 6)]
    private int pair = 3;
    [SerializeField]
    private Vector2 cardSize;

    private int pairCount = 0;
    private int missCount = 0;
    [SerializeField]
    private UnityEngine.UI.Text test;

    private int keepPairNum;
    private Card[] fieldCards;
    private Stack<Card> pairCard;
    private CardGenerator generator;
    private CardSpacement spacement;

    /// <summary>
    /// 送られてきたカードをペアにする
    /// </summary>
    /// <param name="card"></param>
    public void SendCard(Card card) {
        // カードを積む
        pairCard.Push(card);

        // ペアが組まれた場合
        if (pairCard.Count >= 2)
        {
            // 引いた回数
            pairCount++;

            // ２枚のカードを参照
            var c1 = pairCard.Pop();
            var c2 = pairCard.Pop();

            if (c1.number != c2.number)
            {
                missCount++;

                StartCoroutine(c1.Close());
                StartCoroutine(c2.Close());
            }
        }

        // 全てのカードが開いているか
        foreach(var c in fieldCards)
        {
            var back = c.transform.GetChild(2).gameObject;
            if (back.activeSelf) return;
        }
        // カードの再配布
        ResetFieldCards(true, 1F);
    }

    /// <summary>
    /// カードをすべて閉じる
    /// </summary>
    /// <param name="reset"></param>
    /// <param name="waitTime"></param>
    public void ResetFieldCards(bool reset, float waitTime) {
        // [Debug]回数の初期化
        pairCount = 0;
        missCount = 0;

        // 値を再配布してカードを閉じる
        if(reset) SetPairCards();
        foreach (var card in fieldCards)
            StartCoroutine(card.Close(1F));
    }

    /// <summary>
    /// 開始時に処理
    /// </summary>
    private void Start() {
        // ペアを組むスタック
        pairCard = new Stack<Card>();
        // シーンから「生成機」と「配置」を検索
        generator = FindObjectOfType<CardGenerator>();
        spacement = FindObjectOfType<CardSpacement>();
        // カードの生成
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
            ResetFieldCards(true, 0F);
        }
        // [Debug]ミス回数と引いた回数の表示を更新
        test.text = missCount.ToString() + " / " + pairCount.ToString();
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
        pairCard.Clear();
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
