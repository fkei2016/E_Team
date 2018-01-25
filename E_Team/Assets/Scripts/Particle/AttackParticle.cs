using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackParticle : MonoBehaviour {


    private Vector2[] fourCorners = {
        Vector2.up + Vector2.left,
        Vector2.up + Vector2.right,
        Vector2.down + Vector2.right,
        Vector2.down + Vector2.left,
    };

    private int patrol = 0;
    private bool attackFlag = false;
    private bool moveFlag = false;
    private Vector2 rectSize;
    private GameObject shooter;
    private GameObject target;


    [SerializeField]
    private iTween.EaseType easetype;
    [SerializeField]
    private ParticleSystem wildfire;
    [SerializeField]
    private ParticleSystem explosion;


    //パーティクルのサイズ
    [SerializeField]
    private float particleSize = 1.0f;


    /// <summary>
    /// 開始時に実行
    /// </summary>
    void Start () {
        GetComponent<ParticleSystem>().startSize = particleSize;
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    void Update () {
        if (!attackFlag)
        {
            Move();
        }
        else
        {
            Attack();
        }
    }

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    /// <param name="size">
    /// 矩形のサイズ
    /// </param>
    /// <param name="shooter">
    /// 発射するオブジェクト
    /// </param>
    /// <param name="target">
    /// 被弾するオブジェクト
    /// </param>
    /// <returns>
    /// リストへ追加される
    /// </returns>
    public AttackParticle Emission(Vector2 size, GameObject shooter, GameObject target) {
        // 初期値の設定
        this.rectSize = size;
        this.shooter = shooter;
        this.target = target;
        return this;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move() {
        if (!moveFlag)
        {
            moveFlag = true;
            var x = shooter.transform.position.x - rectSize.x * fourCorners[patrol].x;
            var y = shooter.transform.position.y + rectSize.y * fourCorners[patrol].y;
            SetHash(new Vector2(x, y), "ChangePatrol");
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void Attack() {
        wildfire.gameObject.SetActive(true);
        SetHash(target.transform.position, "attackDestroy");
    }

    /// <summary>
    ///  攻撃命令
    /// </summary>
    /// <param name="flag">
    /// 攻撃の有無
    /// </param>
    public void Attack(bool flag) {
        attackFlag = flag;
    }


    /// <summary>
    /// ハッシュテーブルへの設定
    /// </summary>
    /// <param name="position">
    /// ２次元座標
    /// </param>
    /// <param name="method">
    /// 関数名
    /// </param>
    /// <param name="time">
    /// 時間
    /// </param>
    void SetHash(Vector2 position, string method, float time = 1F) {
        var table = new Hashtable();
        table.Add("easeType", easetype);
        table.Add("x", position.x);
        table.Add("y", position.y);
        table.Add("time", time);
        table.Add("oncomplete", method);
        iTween.MoveTo(gameObject, table);
    }

    /// <summary>
    /// 巡回番号の変更
    /// </summary>
    void ChangePatrol() {
        if (++patrol == 4) patrol = 0;
        moveFlag = false;
    }

    /// <summary>
    /// オブジェクト破棄
    /// </summary>
    void attackDestroy() {
        explosion.gameObject.SetActive(true);
        Destroy(this.gameObject,1.0f);
    }
}
