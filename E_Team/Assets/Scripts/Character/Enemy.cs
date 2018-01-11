﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int maxAtk;

    private Slider hp;
    private Text atk;
    private Animation anim;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        hp = GetComponentInChildren<Slider>();
        atk = GetComponentInChildren<Text>();
        anim = GetComponentInChildren<Animation>();
        ResetATK();
    }

    /// <summary>
    /// 攻撃カウントをリセット
    /// </summary>
    private void ResetATK() {
        atk.text = (maxAtk).ToString();
    }

    /// <summary>
    ///  ダメージを与える
    /// </summary>
    /// <returns>
    /// 耐久値が０未満
    /// </returns>
    public bool TakeDamage() {
        return (hp.value-- > 0);
    }

    /// <summary>
    /// 攻撃のカウントダウン
    /// </summary>
    /// <returns>
    /// 攻撃を行う
    /// </returns>
    public bool AtkCountDown() {
        // カウントの数値を取得
        var count = int.Parse(atk.text);
        if(--count <= 0)
        {
            // 攻撃andリセット
            ResetATK();
            return true;
        }
        // カウントをテキスト表示
        atk.text = (count).ToString();
        return false;
    }

    /// <summary>
    /// 攻撃アニメーションの再生
    /// </summary>
    public void PlayAttackAnimation() {
        anim.Play("Attack");
    }

    /// <summary>
    /// ダメージアニメーションの再生
    /// </summary>
    public void PlayDamageAnimation() {
        anim.Play("TakeDamage");
    }
}
