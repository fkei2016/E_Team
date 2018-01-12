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
        m_ClickParticleSystem.Stop();

        table = new ExitGames.Client.Photon.Hashtable();

    }

    // Update is called once per frame
    void Update()
    {

        // マウス操作
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition;
            // パーティクルをマウスカーソルに追従させる
            mousePosition = Input.mousePosition;
            mousePosition.z = 20f;  // ※Canvasよりは手前に位置させること
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //追加--
            table.Remove("position");//テーブル内の"position"を消す
            table.Add("position", mousePosition);//テーブル内の"position"を追加する

            //PhotonViewを通して全プレイヤーにPlayTouchEventを実行させる
            this.GetComponent<PhotonView>().RPC("PlayTouchEvent", PhotonTargets.All,table);//変数同期の時はHashTableを追加してください

        }
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

        // 左ボタンダウンを検知したら、マウスカーソル位置から破裂エフェクトとキラキラエフェクトを再生する。 
        m_ClickParticleSystem.Play();   // １回再生(ParticleSystemのLoopingがfalseだから)
    }

}