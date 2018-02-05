using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class TouchParticle : MonoBehaviour {

    [SerializeField]
    GameObject CLICK_PARTICLE; // PS_TouchStarを割り当てること

    private ParticleSystem m_ClickParticleSystem;

    private PhotonView view;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    void Start() {
        // パーティクルを生成
        var particle = Instantiate(CLICK_PARTICLE, transform);

        // パーティクルの再生停止を制御するためにコンポーネントを取得
        m_ClickParticleSystem = particle.GetComponent<ParticleSystem>();
        m_ClickParticleSystem.Stop();

        NetworkManager.instance.photonview.ObservedComponents.Add(this);
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
      
        // クライアントのみ座標を変更する
        m_ClickParticleSystem.transform.position = Camera.main.ScreenToWorldPoint(touchPosition + Vector3.forward * 20F);

    }

    /// <summary>
    /// turnNumberをクライアント全員に共有します(山口追加)
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //データの送信
            stream.SendNext(m_ClickParticleSystem.transform.position);
        }
        else
        {
            m_ClickParticleSystem.transform.position = (Vector3)stream.ReceiveNext();
            // パーティクルの再生
            m_ClickParticleSystem.Play();
        }


    }

}