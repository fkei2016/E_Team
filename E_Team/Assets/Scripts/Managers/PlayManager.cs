// ==================================================
// プレイシーンの管理者クラス
// ==================================================1
using UnityEngine;

public class PlayManager : SingletonMonoBehaviour<PlayManager> {

    private Player[] players;

    protected override void Awake() {
        base.Awake();

        players = FindObjectsOfType<Player>();
    }

    private void Update() {
    }
}
