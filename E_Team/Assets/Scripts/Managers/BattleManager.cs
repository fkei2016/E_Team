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
    private int turnNumber;

    private Enemy[] target;
    private Player[] users;

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
    }

    /// <summary>
    /// 敵にダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public IEnumerator TakeDamageToEnemy(float damage, float waitTime = 1F) {
        yield return new WaitForSeconds(waitTime);

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
}
