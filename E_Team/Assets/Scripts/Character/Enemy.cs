using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float atkPower;

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int maxAtkCnt;

    private Animation anim;
    private Slider hp;
    private Text atk;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        anim = GetComponentInChildren<Animation>();
        hp = GetComponentInChildren<Slider>();
        atk = GetComponentInChildren<Text>();

        // 値を初期化
        ResetHP();
        ResetATK();
    }

    /// <summary>
    /// 体力値をリセット
    /// </summary>
    private void ResetHP() {
        hp.maxValue = maxHp;
        hp.value = hp.maxValue;
    }

    /// <summary>
    /// 攻撃カウントをリセット
    /// </summary>
    private void ResetATK() {
        atk.text = (maxAtkCnt).ToString();
    }

    /// <summary>
    ///  ダメージを与える
    /// </summary>
    /// <returns>
    /// 耐久値が０未満
    /// </returns>
    public void TakeDamage(float damage){
        hp.value -= damage;
    }

    /// <summary>
    /// エネミーの降参
    /// </summary>
    /// <returns>
    /// 体力値が０以下か
    /// </returns>
    public bool TakeDown() {
        return (hp.value <= 0F);
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
