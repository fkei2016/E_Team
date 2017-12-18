// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    [SerializeField, Range(2, 6)]
    private int pair = 3;
    [SerializeField]
    private float cardRotaSpeed = 1F;

    private int passCount = 0;
    private int missCount = 0;

    [SerializeField]
    private UnityEngine.UI.Text debugText;

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

        // カード座標の再生成
        cardPositions = new Vector3[useCards.Length];
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {

        if (spacement.transform.childCount > 0)
        {
            if (spacement.transform.GetChild(0).position != cardPositions[0])
            {
                for (int i = 0; i < useCards.Length; i++)
                {
                    cardPositions[i] = spacement.GetComponent<RectTransform>().GetChild(i).position;
                }
            }
        }

        // カード配布の線形補間
        for(int i = 0; i < useCards.Length; i++)
        {
            useCards[i].transform.position = Vector2.MoveTowards(useCards[i].transform.position, cardPositions[i], Time.deltaTime * 500F);
        }

        // ペア数が変わったとき
        if (keepPairNum != pair)
        {
            // カードを再生成
            ReMakeCards();

            // カード座標の再生成
            cardPositions = new Vector3[useCards.Length];

            // カード配置の調整
            spacement.AdjustmentLayout(useCards);
            foreach(RectTransform child in spacement.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < useCards.Length; i++)
            {
                var obj = new GameObject("Pos(" + i + ")");
                var rect = obj.AddComponent<RectTransform>();
                rect.SetParent(spacement.GetComponent<RectTransform>());
            }
        }

        // 回転速度の変更
        foreach(var card in useCards)
        {
            card.rotaSpd = cardRotaSpeed;
        }

        // [Debug]ミス回数と引いた回数の表示を更新
        debugText.text = missCount.ToString() + " / " + passCount.ToString();

        // 全てのカードが開いているか
        foreach (var card in useCards)
        {
            var back = card.transform.GetChild(2).gameObject;
            if (back.activeSelf) return;
        }

        // 全て開いていたら再設定
        ReMakeCards();
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
                StartCoroutine(c1.Close(2F));
                StartCoroutine(c2.Close(2F));
            }
        }
    }

    /// <summary>
    /// カードの生成
    /// </summary>
    /// <param name="cardNum">
    /// カードの枚数
    /// </param>
    private Card[] MakeCards(int cardNum) {
        // ペア数を保存
        keepPairNum = cardNum / 2;
        // 使用するカードを生成
        return generator.CreateCards(cardNum);
    }

    /// <summary>
    /// カードの再生成
    /// </summary>
    private void ReMakeCards() {
        // 既存のカードを破棄
        foreach (var card in useCards)
            Destroy(card.gameObject);
        // カードを生成
        useCards = MakeCards(pair * 2);
    }
}
