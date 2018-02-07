// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    public int ClientNumber = 0;
    [SerializeField]
    private int maxClient = 4;

    [SerializeField, Range(2, 6)]
    private int pairNum = 3;
    [SerializeField]
    private float cardRotaSpeed = 1F;
    [SerializeField]
    private Image cardMask;

    private bool turnFinish;
    private int keepPairNum;
    private int completeNum;
    private Card[] useCards;
    private int remainingCards;
    private Stack<Card> pairCard;
    private CardGenerator generator;
    private CardSpacement spacement;
    private Vector3[] cardPositions;
    private BattleManager battle;
    private int openCardCount = 0;

    [Header("AttakParticle")]

    //藤井追加分
    [SerializeField]
    private GameObject attackEffeckPrefab;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject effectPearent;
    private List<AttackParticle> attackParticles;

    //山口追加
    private int[] usenum;
    private PhotonView view;

    //山口追加(2018/2/5)
    bool OpenOK;
    bool[] WaitList;
    private int[] prevusenum;

    int Opencnt;

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
        if(PhotonNetwork.isMasterClient)
        RemakeCards(false);

        // バトルの管理者と連携
        battle = BattleManager.instance;

        // 攻撃エフェクト用の配列を生成
        attackParticles = new List<AttackParticle>();

        //追加
        //NetworkManager.instance.photonview.ObservedComponents.Add(this);
        //view = PhotonView.Get(this);
        //debugcnt = 0;
        // ネットワークマネージャの取得
        var nMrg = NetworkManager.instance;
        if (nMrg)
        {
            // 通知者に自信を追加
            nMrg.photonview.ObservedComponents.Add(this);
            view = PhotonView.Get(this);
        }

        WaitList = new bool[] { false, false, false, false };
        OpenOK = true;
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {
        // [Debug]ブレイクポイント
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }

        if (useCards == null)
        {
            RemakeCards();
            return;
        }

        if (PhotonNetwork.player.ID == 1)
        {
            view.RPC("AllClientTurnChange", PhotonTargets.MasterClient);
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
            useCards[i].transform.position = Vector3.MoveTowards(useCards[i].transform.position, cardPositions[i], Time.deltaTime * 100F);
        }

        // [Debug]シングルプレイテスト用
        //if (Input.GetMouseButtonDown(0) && !cardMask.gameObject.activeSelf)
        //{
        //    foreach (var card in useCards)
        //    {
        //        if (card.gameObject.activeSelf)
        //            card.OnClick(Input.mousePosition);
        //    }
        //}

        // アクティブユーザーのクリック処理
        if (Client.click && openCardCount < 2 && battle.turnNumber + 1 == PhotonNetwork.player.ID)
        {
            view.RPC("ShareTouchPosition", PhotonTargets.AllViaServer, Client.clickPosition);
        }

        // ペア成立の処理
        if (turnFinish || pairCard.Count >= remainingCards)
        {
            while (pairCard.Count > 0)
            {
                // カードのフェードアウト
                var card = pairCard.Pop();
                StartCoroutine(card.FadeOut(2F));

                //  ペアの数だけ敵にダメージを与える
                if (--remainingCards % 2 == 0)
                {
                    var waitTime = (completeNum < useCards.Length) ? 1F : 4F;
                    StartCoroutine(battle.TakeDamageToEnemy(waitTime));
                    StartCoroutine(OpenCardCountInit(waitTime));
                }

                // スキル上昇
                battle.SkillUp(1F);
            }
        }

        // ターン終了時の攻撃
        if (turnFinish)
        {
            turnFinish = false;

            // 敵の攻撃でプレイヤーにダメージを与える
            battle.TakeDamageToPlayer();

            // ターンの切り替え
            //TurnChange(2.5F);
            view.RPC("ShareWait",PhotonTargets.MasterClient, PhotonNetwork.player.ID);

        }

        // 全消しの検知
        foreach (var card in useCards)
        {
            if(!card.isFinish) return;
        }

        // 再配布とターン切り替え
        if (!turnFinish && remainingCards <= 0)
        {

            view.RPC("ShareWait", PhotonTargets.MasterClient, PhotonNetwork.player.ID);

            if (PhotonNetwork.isMasterClient)
            {
                RemakeCards();
            }
            //TurnChange(2F);
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
        completeNum++;

        // スキル上昇
        battle.SkillUp(1F);

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
                StartCoroutine(OpenCardCountInit(1.5F));
                completeNum--;
                completeNum--;
            }
            else
            {
                // ペアパーティクルの発生
                PairParticle(card1, GetComponent<TouchCombo>());
                PairParticle(card2, GetComponent<TouchCombo>());
                // ペアカードを積む
                pairCard.Push(card1);
                pairCard.Push(card2);
                StartCoroutine(OpenCardCountInit(1.5F));
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


        //追加
        if (PhotonNetwork.player.ID != 1&& prevusenum != null)
        {
            if (usenum == prevusenum)
                return;
        }

        if (reset && useCards != null)
        {
            // 既存のカードを破棄
            foreach (var card in useCards)
            {
                Destroy(card.gameObject);
            }

            usenum.Initialize();//追加
            
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

        //追加
        if (PhotonNetwork.isMasterClient)
            usenum = array.ToArray();


        prevusenum = usenum;

        // カードに番号を割り振る
        generator.AppendPairList(useCards, usenum);
        remainingCards = useCards.Length;

        // カード配置の生成と調整
        spacement.AdjustmentLayout(useCards);
        cardPositions = spacement.MakePositions(useCards);

        // ターンを変更
        turnFinish = false;
        completeNum = 0;

    }



    /// <summary>
    /// ペアのパーティクル設定
    /// </summary>
    /// <param name="card">
    /// 対象カード
    /// </param>
    /// <param name="combo">
    /// コンボパーティクル
    /// </param>
    void PairParticle(Card card, TouchCombo combo) {
        // コンボ発生
        StartCoroutine(combo.Emission(card.transform));
        // 攻撃エフェクト発生
        var fx = Instantiate(attackEffeckPrefab, effectPearent.transform);
        var attack = fx.GetComponent<AttackParticle>();
        attack.Emission(card.size / Camera.main.farClipPlane, card.gameObject, enemy);
        attackParticles.Add(attack);
    }

    /// <summary>
    /// turnNumberをクライアント全員に共有します(山口追加)
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            //データの送信
            stream.SendNext(usenum);
            stream.SendNext(WaitList);
        }
        else
        {
            usenum = (int[])stream.ReceiveNext();
            WaitList = (bool[])stream.ReceiveNext();

            if(usenum!=prevusenum&& !PhotonNetwork.isMasterClient && remainingCards <= 0)
                RemakeCards();

        }
    }


    ///// <summary>
    ///// タッチ座標をマスタークライアントに送信します(山口追加)
    ///// </summary>
    ///// <param name="_touchposition"></param>
    //[PunRPC]
    //void ShareTouchPosition(Vector3 _position)
    //{
    //    if (clickposition == _position)
    //        return;
    //    clickcnt++;

    //    clickposition = _position;
    //}

    /// <summary>
    /// タッチ座標をマスタークライアントに送信します(山口追加)
    /// </summary>
    /// <param name="_touchposition"></param>
    [PunRPC]
    void ShareTouchPosition(Vector3 _position)
    {
        foreach (var card in useCards)
        {
            if (card.OnClick(_position)) ;//クライアント
            {
                openCardCount++;
            }
        }
    }

    /// <summary>
    /// ターン切り替えを共有
    /// </summary>
    [PunRPC]
    void AllClientTurnChange()
    {
        for (var i=0;i< PhotonNetwork.playerList.Length;i++)
        {
            if (WaitList[i] == false)
                return;
        }

        //TurnChange(2.5F);
        view.RPC("TurnChange", PhotonTargets.AllViaServer, 2.5F);

        for (var i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            WaitList[i] = false;
        }
    }

    /// <summary>
    /// 待機状態を共有
    /// </summary>
    [PunRPC]
    void ShareWait(int _id)
    {
        WaitList[_id - 1] = true;
    }


    /// <summary>
    /// ターン切り替え
    /// </summary>
    [PunRPC]
    void TurnChange(float waitTime = 1F)
    {
        StartCoroutine(battle.TurnChange(waitTime));
        foreach (var effect in attackParticles)
        {
            effect.Attack(true);
        }
    }

    public IEnumerator OpenCardCountInit(float waitTime = 1F) {
        yield return new WaitForSeconds(waitTime);
        openCardCount = 0;
    }
}
