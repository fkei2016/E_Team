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

    private bool turnFinish;
    private int keepPairNum;
    private Card[] useCards;
    private int remainingCards;
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
        RemakeCards(false);
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {

        // ペア数が変わったときに生成
        if (keepPairNum != pair)
        {
            RemakeCards();
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

        // ターン交代のタイミング
        if (turnFinish || pairCard.Count >= remainingCards)
        {
            // 成立したペアを消す
            turnFinish = false;
            while (pairCard.Count > 0)
            {
                var card = pairCard.Pop();
                StartCoroutine(card.FadeOut(2F));
                card.enabled = false;
                remainingCards--;
            }
        }

        if (!turnFinish && remainingCards <= 0)
        {
            // カードの再生成
            RemakeCards();
        }
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
        if (pairCard.Count % 2 == 0)
        {
            // ２枚のカードを参照
            var card1 = pairCard.Pop();
            var card2 = pairCard.Pop();

            // ミスしたらターン終了
            turnFinish = (card1.number != card2.number);

            if (turnFinish)
            {
                // カードを閉じる
                StartCoroutine(card1.Close(1.5F));
                StartCoroutine(card2.Close(1.5F));
            }
            else
            {
                // 成立したペアをスタック
                pairCard.Push(card1);
                pairCard.Push(card2);
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

        // ペアカードを初期化
        pairCard.Clear();

        // カードを生成
        var cards = generator.CreateCards(cardNum);

        // 生成したカードを返す
        return cards;
    }

    /// <summary>
    /// カードの再生成
    /// </summary>
    /// <param name="reset">
    /// 子要素の破棄
    /// </param>
    private void RemakeCards(bool reset = true) {
        if (reset)
        {
            // 既存のカードを破棄
            foreach (var card in useCards)
            {
                Destroy(card.gameObject);
            }
        }

        // カードの再生成
        useCards = MakeCards(pair * 2);
        remainingCards = useCards.Length;

        // カード配置の生成と調整
        spacement.AdjustmentLayout(useCards);
        cardPositions = spacement.MakePositions(useCards);

        // ターンを変更
        turnFinish = false;
    }
}
