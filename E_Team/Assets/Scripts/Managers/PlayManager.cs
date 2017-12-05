// ==================================================
// プレイシーンの管理者クラス
// ==================================================
using UnityEngine;

public class PlayManager : SingletonMonoBehaviour<PlayManager> {

    [SerializeField]
    private Vector2 cardSize = new Vector2(150F, 250F);
    public Vector2 CardSize { get { return cardSize; } }

    private Player[] players;

    protected override void Awake() {
        base.Awake();

        players = FindObjectsOfType<Player>();
    }
}
