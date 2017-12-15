// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    [SerializeField, Range(2, 6)]
    private int pair = 3;

    private int pairCount = 0;
    private int missCount = 0;

    [SerializeField]
    private UnityEngine.UI.Text debugText;

    private int keepPairNum;
    private Card[] useCards;
    private Vector2 cardSize;
    private Stack<Card> pairCard;
    private CardGenerator generator;
    private CardSpacement spacement;


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
        MakeCards(pair * 2);
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {
        // ペア数が変わったときに再生成
        if (keepPairNum != pair)
        {
            foreach (var card in useCards)
                Destroy(card.gameObject);
            MakeCards(pair * 2);
        }

        // カード配置の調整
        spacement.AdjustmentLayout(useCards, generator.CardSize);

        // [Debug]ペア番号を再設定
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCards(true, 0F);
        }
        // [Debug]ミス回数と引いた回数の表示を更新
        debugText.text = missCount.ToString() + " / " + pairCount.ToString();
    }

    /// <summary>
    /// 送られてきたカードを受け取る
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

                StartCoroutine(c1.Close(2F));
                StartCoroutine(c2.Close(2F));
            }
        }

        // 全てのカードが開いているか
        foreach(var c in useCards)
        {
            var back = c.transform.GetChild(2).gameObject;
            if (back.activeSelf) return;
        }
        // カードの再設定
        ResetCards(true, 1F);
    }



    /// <summary>
    /// カードの再設定
    /// </summary>
    /// <param name="resetPairs">
    /// 番号の再割り当て
    /// </param>
    /// <param name="waitTime">
    /// 遅延時間
    /// </param>
    public void ResetCards(bool resetPairs, float waitTime) {
        // [Debug]回数の初期化
        pairCount = 0;
        missCount = 0;

        // 値を再配布してカードを閉じる
        if(resetPairs) SetPairCards(useCards);
        foreach (var card in useCards)
            StartCoroutine(card.Close(waitTime));
    }

    /// <summary>
    /// ペア番号をカードに設定
    /// </summary>
    /// <param name="cards">
    /// 複数のカード
    /// </param>
    public void SetPairCards(Card[] cards) {
        // ペアリストの初期化
        pairCard.Clear();

        // ペア番号の生成
        var pairList = MakePairNumbers();
        for (int i = 0; i < cards.Length; i++)
        {
            // 重複しない値をカードに設定
            var number = GetNonOverlappingValue(pairList);
            generator.ChangeCardNumber(number, cards[i]);
        }
    }
    /// <summary>
    /// カードの再生成
    /// </summary>
    /// <param name="cardNum">
    /// カードの枚数
    /// </param>
    private void MakeCards(int cardNum) {
        // ペア数を保存
        keepPairNum = cardNum / 2;
        // 使用するカードを生成
        useCards = generator.CreateCards(cardNum);
        // カードにペア番号を割り当て
        SetPairCards(useCards);
    }

    /// <summary>
    /// ペアリストを作成
    /// </summary>
    /// <returns>
    /// ペアリスト
    /// </returns>
    private List<int> MakePairNumbers() {
        // 「柄の数」分の数列を作成
        var rangeList = new List<int>();
        for (int i = 0; i < generator.DesignLength; i++)
        {
            rangeList.Add(i);
        }

        // 数列からペアになる番号の抽選
        var pairNumbers = new List<int>();
        for (var i = 0; i < useCards.Length / 2; i++)
        {
            var rnd = GetNonOverlappingValue(rangeList);
            pairNumbers.Add(rnd);
            pairNumbers.Add(rnd);
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
        var rnd = Random.Range(0, list.Count);
        // リストから値を取り除く
        rnd = list[rnd];
        list.Remove(rnd);
        return rnd;
    }
}
