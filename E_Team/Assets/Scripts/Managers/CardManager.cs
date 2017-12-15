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
        useCards = MakeCards(pair * 2);
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
            useCards = MakeCards(pair * 2);
        }

        // 回転速度の変更
        foreach(var card in useCards)
        {
            card.rotaSpd = cardRotaSpeed;
        }

        // カード配置の調整
        spacement.AdjustmentLayout(useCards);

        // [Debug]ミス回数と引いた回数の表示を更新
        debugText.text = missCount.ToString() + " / " + passCount.ToString();


        // 全てのカードが開いているか
        foreach (var card in useCards)
        {
            var back = card.transform.GetChild(2).gameObject;
            if (back.activeSelf) return;
        }

        foreach(var card in useCards)
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
}
