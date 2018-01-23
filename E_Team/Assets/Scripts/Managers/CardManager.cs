﻿// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    public int cliantNumber = 0;
    [SerializeField]
    private int maxCliant = 4;

    [SerializeField, Range(2, 6)]
    private int pairNum = 3;
    [SerializeField]
    private float cardRotaSpeed = 1F;
    [SerializeField]
    private Image cardMask;//maskimg

    private bool turnFinish;
    private int keepPairNum;
    private Card[] useCards;
    private int remainingCards;
    private Stack<Card> pairCard;
    private CardGenerator generator;
    private CardSpacement spacement;
    private Vector3[] cardPositions;
    private BattleManager battle;

    private int[] usenumlist;
    private PhotonView view;

    /// <summary>
    /// 開始時に処理
    /// </summary>
    private void Start() {
        // ペアを組むスタック
        pairCard = new Stack<Card>();

        // シーンから「生成機」と「配置」を検索
        generator = FindObjectOfType<CardGenerator>();
        spacement = FindObjectOfType<CardSpacement>();

        //自身をphotonviewに追加します
        NetworkManager.instance.photonview.ObservedComponents.Add(this);

        // カードの生成
        if(PhotonNetwork.isMasterClient)
        RemakeCards(false);

        // バトルの管理者と連携
        battle = BattleManager.instance;
        view = PhotonView.Get(this);
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {

        if (useCards == null)
        {
            RemakeCards(false);
            return;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            RemakeCards(true);
            return;
        }

        // [Debug]ブレイクポイント
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }

        // ペア数が変わったときに生成
        if (keepPairNum != pairNum)
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
        if (useCards[0].transform.position == cardPositions[0])
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
            useCards[i].transform.position = Vector3.MoveTowards(useCards[i].transform.position, cardPositions[i], Time.deltaTime * 100F);
        }

        //// [Debug]シングルプレイテスト用
        //if (Input.GetMouseButtonDown(0) && !cardMask.gameObject.activeSelf)
        //{
        //    foreach (var card in useCards)
        //    {
        //        if(card.gameObject.activeSelf)
        //        card.OnClick(Input.mousePosition);
        //    }
        //}

        // アクティブユーザーのクリック処理
        if (battle.activeUser.click&&battle.turnActive)
        {
            view.RPC("onclick", PhotonTargets.All, battle.activeUser.clickPosition);
        }

        if (turnFinish)
        {
            // 敵の攻撃でプレイヤーにダメージを与える
            battle.TakeDamageToPlayer(100F);

            // ターンの切り替え
            battle.TurnChange();
        }

        // ターン交代のタイミング
        if (turnFinish || pairCard.Count >= remainingCards)
        {
            // 成立したペアを消す
            turnFinish = false;
            while (pairCard.Count > 0)
            {
                // カードのフェードアウト
                var card = pairCard.Pop();
                StartCoroutine(card.FadeOut(2F));
                --remainingCards;

                //  ペアの数だけ敵にダメージを与える
                if (remainingCards % 2 == 0)
                    battle.TakeDamageToEnemy();
                // スキル上昇
                battle.SkillUp(1F);
            }
        }

        foreach(var card in useCards)
        {
            if(!card.isFinish) return;
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
                // ペア不成立のエフェクト
                var missFX = GetComponent<MissEffect>();
                StartCoroutine(missFX.Emission(card1.transform));
                StartCoroutine(missFX.Emission(card2.transform));
                // カードを閉じる
                StartCoroutine(card1.Close(1.5F));
                StartCoroutine(card2.Close(1.5F));
            }
            else
            {
                // ペア成立のエフェクト
                var comboFX = GetComponent<TouchCombo>();
                StartCoroutine(comboFX.Emission(card1.transform));
                StartCoroutine(comboFX.Emission(card2.transform));
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

        // 生成したカードを返す
        return generator.CreateCards(cardNum);
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

            if (PhotonNetwork.isMasterClient)
            {//共有するカード番号を破棄
                usenumlist = new int[] { };
            }
        }

        //マスターの更新がされていない場合はスキップします
        if (!PhotonNetwork.isMasterClient&&usenumlist.Length==0)
        {
            return;
        }

        // カードの再生成
        useCards = MakeCards(pairNum * 2);
        // [Master]ペアリストの生成
        var list = generator.MakePairList(generator.maxDesign, useCards.Length);

        var array = new List<int>();

        for(int i = 0; i < useCards.Length; i++)
        {
            array.Add(generator.GetNonOverlappingValue(list));
        }

        if (PhotonNetwork.isMasterClient)
        {
            // カードに番号を割り振る
            generator.AppendPairList(useCards, array.ToArray());
            remainingCards = useCards.Length;
        }
        else if(!PhotonNetwork.isMasterClient)
        {
            // 割り振られたカードを受け取る
            generator.AppendPairList(useCards, usenumlist);
            remainingCards = useCards.Length;
        }

        // カード配置の生成と調整
        spacement.AdjustmentLayout(useCards);
        cardPositions = spacement.MakePositions(useCards);

        //クライアントのカード番号を共有
        if (PhotonNetwork.isMasterClient)
        {
            usenumlist = array.ToArray();
        }

        // ターンを変更
        turnFinish = false;
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
         if (stream.isWriting)
        {
            //データの送信
            stream.SendNext(usenumlist);
        }
        else
        {
            //データの受信
            this.usenumlist = (int[])stream.ReceiveNext();
        }
    }

    [PunRPC]
    void onclick(Vector3 _position)
    {
        foreach (var card in useCards)
        {
            card.OnClick(_position);
        }
    }

}