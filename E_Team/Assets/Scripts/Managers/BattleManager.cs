using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : SingletonMonoBehaviour<BattleManager> {

    [SerializeField]
    private Slider playerHP;

    [SerializeField]
    private PlayerGroup playerGroup;

    private float tmpHP;
    private float damageSPD;
    public int turnNumber;//publibに変更しました（山口追加）

    private Enemy[] target;
    private Player[] users;

    //山口追加
    private PhotonView view;
    public GameObject mask;

    public Player activeUser
    {
        get
        {
            foreach(var user in users)
            {
                if(!user.active)
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
        tmpHP = playerHP.value;
        damageSPD = 2.5F;
        turnNumber = 0;

        target = FindObjectsOfType<Enemy>();
        users = playerGroup.Create(4);
        foreach(var user in users)
        {
            user.active = false;
        }

        //// 指定の番号のみアクティブにする
        //foreach (var user in users)
        //{
        //    user.active = true;
        //}
        //users[turnNumber].active = false;

        //山口追加
        view = PhotonView.Get(this);
        NetworkManager.instance.photonview.ObservedComponents.Add(this);
        mask.active = !(turnNumber + 1 == PhotonNetwork.player.ID);
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        if(playerHP.value >= tmpHP)
        {
            playerHP.value -= damageSPD;
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


    //↓タッチ終了時に実行
    /// <summary>
    /// ターン切り替え
    /// </summary>
    public void TurnChange() {

        // ターン番号を更新
        if (++turnNumber >= users.Length)
        {
            turnNumber = 0;
        }

        mask.active = !(turnNumber + 1 == PhotonNetwork.player.ID);

        // 指定の番号のみアクティブにする
        foreach (var user in users)
        {
            user.active = true;
        }
        users[turnNumber].active = false;
    }

    /// <summary>
    /// 敵にダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamageToEnemy(float damage) {
        var takeDown = target[0].TakeDamage(damage);
        //target.gameObject.SetActive(takeDown);
        target[0].PlayDamageAnimation();
    }

    /// <summary>
    /// プレイヤーにダメージを与える
    /// </summary>
    /// <param name="damage">
    /// ダメージ値
    /// </param>
    public void TakeDamageToPlayer(float damage) {
        if (target[0].AtkCountDown())
        {
            target[0].PlayAttackAnimation();
            tmpHP -= damage;
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

}
