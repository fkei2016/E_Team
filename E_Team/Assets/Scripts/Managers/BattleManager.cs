using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : SingletonMonoBehaviour<BattleManager> {

    [SerializeField]
    private PlayerGroup playerGroup;

    [SerializeField]
    private SkillParticle skill;

    [SerializeField]
    private GameObject gameover;
    [SerializeField]
    private GameObject gameclear;

    [SerializeField]
    private GameObject mask;

    public int turnNumber { get; private set; }

    private float damageCut = 1F;
    private float attackBonus = 1F;

    private Enemy[] target;
    private Player[] users;

    private PhotonView view;

    public Player activeUser
    {
        get
        {
            foreach(var user in users)
            {
                if(user.active)
                {
                    return user;
                }
            }
            return null;
        }
    }


    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        turnNumber = 0;

        // 仮り組み用の敵を検索
        target = FindObjectsOfType<Enemy>();

        // プレイヤーの生成
        var numbers = TakeOverClient.clientnums;
        if (numbers == null) numbers = new int[] { 0 };
        users = playerGroup.Create(PhotonNetwork.playerList.Length, numbers);//第一引数を変更しました(numbers.length →　Photonnetwork.Playerlist.Length)

        // 指定の番号のみアクティブにする
        foreach (var user in users)
        {
            user.active = true;
        }
        users[turnNumber].active = false;

        //マスクのアクティブ状態を変更する
        mask.active = (turnNumber + 1 != PhotonNetwork.player.ID);

        //山口追加
        view = PhotonView.Get(this);
        NetworkManager.instance.photonview.ObservedComponents.Add(this);
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        if(Client.click)
        {
            if (activeUser.OnClick(Client.clickPosition))
            {
                view.RPC("UseSkill", PhotonTargets.AllViaServer);
            }
        }

        // ゲームオーバー
        gameover.SetActive(playerGroup.TakeDown());
        // ゲームクリア
        gameclear.SetActive(target[0].TakeDown());

        // ゲームが終了した後はタイトルへ遷移
        if(gameover.activeSelf || gameclear.activeSelf)
        {
            if(Client.click)
            {
                GetComponent<JumpToScene>().Execute();
            }
        }
    }

    /// <summary>
    /// スキル上昇
    /// </summary>
    /// <param name="upper">
    /// 上昇値
    /// </param>
    public void SkillUp(float upper) {
        activeUser.SkillUp(upper);
    }

    /// <summary>
    /// ターン切り替え
    /// </summary>
    public IEnumerator TurnChange(float waitTime = 1F) {
        yield return new WaitForSeconds(waitTime);

        // ターン番号を更新
        if(++turnNumber >= users.Length)
        {
            turnNumber = 0;
        }

        // 指定の番号のみアクティブにする
        foreach(var user in users)
        {
            user.active = true;
        }
        users[turnNumber].active = false;


        //マスクのアクティブ状態を変更する
        mask.active = (turnNumber + 1 != PhotonNetwork.player.ID);

    }

    /// <summary>
    /// 敵にダメージを与える
    /// </summary>
    /// <param name="damage">
    /// ダメージ量
    /// </param>
    public IEnumerator TakeDamageToEnemy(float waitTime = 1F) {
        // 遅延演出
        yield return new WaitForSeconds(waitTime);

        //音追加
        var audio = AudioManager.instance;
        if (audio) audio.PlaySE("PlayerAttackSE");

        // ダメージ計算とアニメーション
        target[0].TakeDamage(playerGroup.atkPower * attackBonus);
        //target.gameObject.SetActive(takeDown);
        target[0].PlayDamageAnimation();

        // 攻撃力向上の解除
        if (attackBonus >= 1F) attackBonus = 1F;

        //カードマネージャーのカードカウントを初期化
        CardManager.instance.OpenCardCountInit();

    }

    /// <summary>
    /// プレイヤーにダメージを与える
    /// </summary>
    /// <param name="damage">
    /// ダメージ値
    /// </param>
    public void TakeDamageToPlayer() {
        if (target[0].AtkCountDown())
        {

            //音追加
            var audio = AudioManager.instance;
            if(audio) audio.PlaySE("EnemyAttackSE");

            target[0].PlayAttackAnimation();
            playerGroup.ChangeHpValue(-target[0].atkPower * damageCut);

            if (damageCut <= 1F) damageCut = 1F;
        }
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
            stream.SendNext(turnNumber);
        }
        else
        {
            //データの受信
            this.turnNumber = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void UseSkill()
    {
        var audio = AudioManager.instance;
        var particle = skill.Emission(activeUser.number, activeUser.transform.position + Vector3.down, 5F);
        switch (activeUser.number)
        {
            case 0:
                //音追加
                if(audio) audio.PlaySE("SkillAttackSE");
                var fire = particle.GetComponent<AttackParticle>().Emission(Vector2.zero, activeUser.gameObject, target[0].gameObject);
                fire.Attack(true);
                StartCoroutine(TakeDamageToEnemy(1F));
                break;
            case 1:
                //音追加
                if (audio) audio.PlaySE("HeelSE");
                playerGroup.ChangeHpValue(+100F);
                break;
            case 2:
                //音追加
                if (audio) audio.PlaySE("AttackBufSE");
                attackBonus = 2F;
                break;
            case 3:
                //音追加
                if (audio) audio.PlaySE("DefBufSE");
                damageCut = 0.5F;
                break;
            default:
                break;
        }
        activeUser.InitGauge(); 
    }

}
