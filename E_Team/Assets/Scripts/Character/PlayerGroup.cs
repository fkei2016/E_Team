using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGroup : MonoBehaviour {

    public float atkPower;

    [Header("HP")]
    [SerializeField]
    private float damageSpeed = 1F;
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private float maxHp;
    private float tmpHP;

    [Space]
    [SerializeField]
    private GameObject[] prefabs;

    private Player[] players;

    /// <summary>
    /// グループの作成
    /// </summary>
    /// <param name="maxPlayerNum">
    /// プレイヤー数
    /// </param>
    /// <returns>
    /// ユーザーの配列
    /// </returns>
    public Player[] Create(int maxPlayerNum, int[] numbers) {
        // ユーザーの配列を生成
        players = new Player[maxPlayerNum];
        // プレイヤーを個別に生成
        for (int i = 0; i < players.Length; i++)
        {
            var prefab = prefabs[numbers[i]];
            var player = Instantiate(prefab);
            players[i] = player.gameObject.AttachComponet<Player>();
            players[i].number = numbers[i];
            players[i].transform.Reset(transform);
        }
        return players;
    }

    /// <summary>
    /// 体力値の変更
    /// </summary>
    /// <param name="value">
    /// 加減算する数値
    /// </param>
    public void ChangeHpValue(float value) {
        tmpHP += value;
    }

    /// <summary>
    /// プレイヤーの降参
    /// </summary>
    /// <returns>
    /// 体力値が０以下か
    /// </returns>
    public bool TakeDown() {
        return (hpBar.value <= 0F);
    }

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        hpBar.value = tmpHP = maxHp;
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        if (hpBar.value > tmpHP)
        {
            hpBar.value -= 5F;
        }
        else if(hpBar.value < tmpHP)
        {
            hpBar.value += 5F;
        }
    }
}
