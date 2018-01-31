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


    private float damageCut = 1F;
    private float attackBonus = 1F;

    private int turnNumber;
    private Enemy[] target;
    private Player[] users;

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

        target = FindObjectsOfType<Enemy>();
        var numbers = new int[] { 0, 1, 2 , 3};
        users = playerGroup.Create(numbers.Length, numbers);

        // 指定の番号のみアクティブにする
        foreach (var user in users)
        {
            user.active = true;
        }
        users[turnNumber].active = false;
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        if(Client.click)
        {
            if (activeUser.OnClick(Client.clickPosition))
            {
                var particle = skill.Emission(activeUser.number, activeUser.transform.position + Vector3.down, 5F);
                switch(activeUser.number)
                {
                    case 0:
                        //音追加
                        AudioManager.instance.PlaySE("SkillAttackSE");
                        var fire = particle.GetComponent<AttackParticle>().Emission(Vector2.zero, activeUser.gameObject, target[0].gameObject);
                        fire.Attack(true);
                        StartCoroutine(TakeDamageToEnemy(1F));
                        break;
                    case 1:
                        //音追加
                        AudioManager.instance.PlaySE("HeelSE");
                        playerGroup.ChangeHpValue(+100F);
                        break;
                    case 2:
                        //音追加
                        AudioManager.instance.PlaySE("AttackBufSE");
                        attackBonus = 2F;
                        break;
                    case 3:
                        //音追加
                        AudioManager.instance.PlaySE("DefBufSE");
                        damageCut = 0.5F;
                        break;
                    default:
                        break;
                }
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
        AudioManager.instance.PlaySE("PlayerAttackSE");

        // ダメージ計算とアニメーション
        target[0].TakeDamage(playerGroup.atkPower * attackBonus);
        //target.gameObject.SetActive(takeDown);
        target[0].PlayDamageAnimation();

        // 攻撃力向上の解除
        if (attackBonus >= 1F) attackBonus = 1F;
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
            AudioManager.instance.PlaySE("EnemyAttackSE");

            target[0].PlayAttackAnimation();
            playerGroup.ChangeHpValue(-target[0].atkPower * damageCut);

            if (damageCut <= 1F) damageCut = 1F;
        }
    }
}
