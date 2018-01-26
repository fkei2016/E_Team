using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroup : MonoBehaviour {

    [SerializeField]
    private GameObject[] prefabs;

    /// <summary>
    /// グループの作成
    /// </summary>
    /// <param name="maxPlayerNum">
    /// プレイヤー数
    /// </param>
    /// <returns>
    /// ユーザーの配列
    /// </returns>
    public Player[] Create(int maxPlayerNum/*,int[] _players*/) {
        // ユーザーの配列を生成
        var players = new Player[maxPlayerNum];
        // プレイヤーを個別に生成
        for (int i = 0; i < players.Length; i++)
        {
            var player = Instantiate(prefabs[Client.characterNumber]);
            players[i] = player.gameObject.AttachComponet<Player>();
            players[i].transform.Reset(transform);
        }
        return players;
    }
}
