using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : SingletonMonoBehaviour<BattleManager> {

    [SerializeField]
    private Slider hp;
    
    private Enemy target;
    private Player[] users;

    private float tmpHP;
    private float damageSPD;
    private int turnNumber;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        target = FindObjectOfType<Enemy>();
        users = FindObjectsOfType<Player>();

        tmpHP = hp.value;
        damageSPD = 2.5F;
        turnNumber = 0;
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        if(hp.value >= tmpHP)
        {
            hp.value -= damageSPD;
        }
    }

    /// <summary>
    /// スキル上昇
    /// </summary>
    /// <param name="upper">
    /// 上昇値
    /// </param>
    public void SkillUp(float upper) {
        foreach(var user in users)
        {
            if (user.active)
                user.SkillUp(upper);
        }
    }

    /// <summary>
    /// 敵にダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamageToEnemy() {
        var takeDown = target.TakeDamage();
        //target.gameObject.SetActive(takeDown);
        target.PlayDamageAnimation();
    }

    /// <summary>
    /// プレイヤーにダメージを与える
    /// </summary>
    /// <param name="damage">
    /// ダメージ値
    /// </param>
    public void TakeDamageToPlayer(float damage) {
        if (target.AtkCountDown())
        {
            target.PlayAttackAnimation();
            tmpHP -= damage;
        }
    }
}
