using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class TouchParticle : MonoBehaviour {

    [SerializeField]
    GameObject CLICK_PARTICLE; // PS_TouchStarを割り当てること

    private ParticleSystem m_ClickParticleSystem;

    PhotonHashTable table;//ハッシュテーブル


    /// <summary>
    /// 開始時に実行
    /// </summary>
    void Start() {
        // パーティクルを生成
        var particle = Instantiate(CLICK_PARTICLE, transform);

        // パーティクルの再生停止を制御するためにコンポーネントを取得
        m_ClickParticleSystem = particle.GetComponent<ParticleSystem>();
        m_ClickParticleSystem.Stop();

        // 座標用のハッシュテーブルを作成
        table = new PhotonHashTable();
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    void Update() {
        // クリック処理
        if (Client.click)
        {
            Emission(Client.clickPosition);
        }
    }

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    /// <param name="touchPosition">
    /// タッチ座標
    /// </param>
    /// <param name="depth">
    /// ワールド座標に対する深度
    /// </param>
    public void Emission(Vector3 touchPosition, float depth = 0F) {
        // タッチされたスクリーン座標からワールド座標へ変換
        var position = Camera.main.ScreenToWorldPoint(touchPosition + Vector3.forward * 20F);

        if (NetworkManager.instance)
        {
            // テーブル内の座標を書き直す
            table.Remove("position");
            table.Add("position", position);

            //PhotonViewを通して全プレイヤーにPlayTouchEventを実行させる
            NetworkManager.instance.photonview.RPC("PlayTouchEvent", PhotonTargets.AllViaServer, table);//変数同期の時はHashTableを追加してください
        }
        else
        {
            // クライアントのみ座標を変更する
            m_ClickParticleSystem.transform.position = position;
        }

        // パーティクルの再生
        m_ClickParticleSystem.Play();
    }

    [PunRPC]
    private void PlayTouchEvent(PhotonHashTable _table) {
        object value = null;
        //HashTableに追加された変数を同期します
        if (_table.TryGetValue("position", out value))
        {
            m_ClickParticleSystem.transform.position = (Vector3)value;
        }
    }

}