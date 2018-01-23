using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : SingletonMonoBehaviour<BattleManager> {

    [SerializeField]
    private Slider hp;
    
    [SerializeField]
    private Enemy[] target;
    [SerializeField]
    private Player[] users;

    private float tmpHP;
    private float damageSPD;
    private int turnNumber;
    public Text testtxt_yourID;
    public Text testtxt_activeID;
    public GameObject mask;

    public bool turnActive;
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
        tmpHP = hp.value;
        damageSPD = 2.5F;
        turnNumber = 0;
        testtxt_yourID.text ="yourID:"+ PhotonNetwork.player.ID.ToString();
        testtxt_activeID.text = "activeID:" + (turnNumber + 1);

        turnActive = (turnNumber + 1 == PhotonNetwork.player.ID);
        mask.active = !turnActive;
        NetworkManager.instance.photonview.ObservedComponents.Add(this);
        view = PhotonView.Get(this);
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        testtxt_activeID.text = "activeID:" + (turnNumber + 1);

        turnActive = (turnNumber + 1 == PhotonNetwork.player.ID);
        mask.active = !turnActive;

        if (hp.value >= tmpHP)
        {
            hp.value -= damageSPD;
        }

        // 指定の番号のみアクティブにする
        foreach (var user in users)
        {
            user.active = false;
        }
        users[turnNumber].active = true;

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
    public void TurnChange() {
       
        if(turnActive)
        view.RPC("turnchange", PhotonTargets.MasterClient);
    }

    /// <summary>
    /// 敵にダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamageToEnemy() {
        var takeDown = target[0].TakeDamage();
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
    private void turnchange()
    {
        // ターン番号を更新
        if (++turnNumber >= users.Length)
        {
            turnNumber = 0;
        }
    }

}