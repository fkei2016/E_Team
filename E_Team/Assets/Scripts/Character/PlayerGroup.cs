using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroup : MonoBehaviour {

    public float atkPower;

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
            var player = Instantiate(prefabs[numbers[i]]);
            players[i] = player.gameObject.AttachComponet<Player>();
            players[i].number = Client.characterNumber;
            players[i].transform.Reset(transform);
        }
        return players;
    }
}
