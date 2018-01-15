using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchParticle : MonoBehaviour {

    [SerializeField]
    GameObject CLICK_PARTICLE; // PS_TouchStarを割り当てること

    private GameObject m_ClickParticle;

    private ParticleSystem m_ClickParticleSystem;


    //追加--
    ExitGames.Client.Photon.Hashtable table;//ハッシュテーブル
   
    
    // Use this for initialization
    void Start()
    {
        // パーティクルを生成
        m_ClickParticle = (GameObject)Instantiate(CLICK_PARTICLE);

        // パーティクルの再生停止を制御するためにコンポーネントを取得
        m_ClickParticleSystem = m_ClickParticle.GetComponent<ParticleSystem>();

        table = new ExitGames.Client.Photon.Hashtable();
    }

    // Update is called once per frame
    void Update()
    {
        // マウス操作
        if (Input.GetMouseButtonDown(0))
        {
            Emission(Input.mousePosition, 30F);
        }
    }

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    /// <param name="worldPosition">
    /// ワールド座標
    /// </param>
    /// <param name="depth">
    /// ワールドの深度
    /// </param>
    public void Emission(Vector3 worldPosition, float depth = 0F) {
        // パーティクルをマウスカーソルに追従させる
        var mousePosition = Camera.main.ScreenToWorldPoint(worldPosition + Vector3.forward * depth);

        if (NetworkManager.instance)
        {
            // テーブルに"position"を再登録
            table.Remove("position");
            table.Add("position", mousePosition);

            //PhotonViewを通して全プレイヤーにPlayTouchEventを実行させる
            NetworkManager.instance.photonview.RPC("PlayTouchEvent", PhotonTargets.All, table);//変数同期の時はHashTableを追加してください
        }
        else
        {
            m_ClickParticleSystem.transform.position = mousePosition;
        }

        // 左ボタンダウンを検知したら、マウスカーソル位置から破裂エフェクトとキラキラエフェクトを再生する。 
        m_ClickParticleSystem.Play();   // １回再生(ParticleSystemのLoopingがfalseだから)
    }

    [PunRPC]
    private void PlayTouchEvent(ExitGames.Client.Photon.Hashtable _table)
    {
        object value = null;
        //HashTableに追加された変数を同期します
        if (_table.TryGetValue("position", out value))
        {
            m_ClickParticleSystem.transform.position = (Vector3)value;
        }
    }

}