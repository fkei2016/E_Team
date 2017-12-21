// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    [SerializeField, Range(2, 6)]
    private int pair = 3;
    [SerializeField]
    private float cardRotaSpeed = 1F;

    private int passCount = 0;
    private int missCount = 0;

    private int keepPairNum;
    private Card[] useCards;
    private Stack<Card> pairCard;
    private CardGenerator generator;
    private CardSpacement spacement;
    private Vector3[] cardPositions;


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
        useCards = MakeCards(pair * 2);
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {

        if (keepPairNum != pair)
        {
            // ペア数が変わったとき再生成
            foreach (var card in useCards)
            {
                Destroy(card.gameObject);
            }
            useCards = MakeCards(pair * 2);
        }

        // カード配置の再設定
        if (spacement.transform.GetChild(0).position != cardPositions[0])
        {
            var rect = spacement.GetComponent<RectTransform>();
            for (int i = 0; i < useCards.Length; i++)
            {
                cardPositions[i] = rect.GetChild(i).position;
                useCards[i].enabled = false;
            }
        }

        // カード移動時のタッチ無効処理
        if(useCards[0].transform.position == cardPositions[0])
        {
            if (!useCards[0].enabled)
            {
                foreach (var card in useCards)
                {
                    card.enabled = true;
                }
            }
        }

        for (int i = 0; i < useCards.Length; i++)
        {
            // カードの回転速度を変更
            useCards[i].rotaSpd = cardRotaSpeed;

            // カード配置までの線形補間
            useCards[i].transform.position = Vector2.MoveTowards(useCards[i].transform.position, cardPositions[i], Time.deltaTime * 500F);
        }

        // 全てのカードが開いているか
        foreach (var card in useCards)
        {
            var back = card.transform.GetChild(2).gameObject;
            if (back.activeSelf) return;
        }

        // 全て開いていたら再設定
        foreach (var card in useCards)
        {
            Destroy(card.gameObject);
        }
        useCards = MakeCards(pair * 2);
    }

    /// <summary>
    /// 送られてきたカードを受け取る
    /// </summary>
    /// <param name="card">
    /// </param>
    public void SendCard(Card card) {
        // カードを積む
        pairCard.Push(card);

        // ペアが組まれた場合
        if (pairCard.Count >= 2)
        {
            // 引いた回数
            passCount++;

            // ２枚のカードを参照
            var c1 = pairCard.Pop();
            var c2 = pairCard.Pop();

            if (c1.number != c2.number)
            {
                // ミスの回数
                missCount++;
                // カードを閉じる
                StartCoroutine(c1.Close(1F));
                StartCoroutine(c2.Close(1F));
            }
        }
    }

    /// <summary>
    /// カードの生成
    /// </summary>
    /// <param name="cardNum">
    /// カードの枚数
    /// </param>
    /// <returns></returns>
    private Card[] MakeCards(int cardNum) {
        // ペア数を保存
        keepPairNum = cardNum / 2;

        // カードを生成
        var cards = generator.CreateCards(cardNum);

        // カード座標の再生成
        cardPositions = new Vector3[cards.Length];

        // カード配置の生成と調整
        spacement.AdjustmentLayout(cards);
        MakeCardPositions(spacement.RectTransform, cards);

        // 生成したカードを返す
        return cards;
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
    private void MakeCardPositions(RectTransform transform, Card[] cards) {
        // 既にある子要素（座標）を破棄
        foreach (RectTransform child in transform)
        {
            Destroy(child.gameObject);
        }
        // カードの枚数分だけ座標を生成
        for (int i = 0; i < cards.Length; i++)
        {
            var obj = new GameObject("Pos(" + i + ")");
            var rect = obj.AddComponent<RectTransform>();
            rect.SetParent(transform);
        }
    }
}
