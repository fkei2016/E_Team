using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

    [SerializeField]
    GameObject CLICK_PARTICLE; // PS_TouchStarを割り当てること


    private GameObject m_ClickParticle;
   
    private ParticleSystem m_ClickParticleSystem;

    // Use this for initialization
    void Start()
    {
        // パーティクルを生成
        m_ClickParticle = (GameObject)Instantiate(CLICK_PARTICLE);

        // パーティクルの再生停止を制御するためにコンポーネントを取得
        m_ClickParticleSystem = m_ClickParticle.GetComponent<ParticleSystem>();
        m_ClickParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // パーティクルをマウスカーソルに追従させる
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 20f;  // ※Canvasよりは手前に位置させること
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // マウス操作
        if (Input.GetMouseButtonDown(0))
        {
            // 左ボタンダウンを検知したら、マウスカーソル位置から破裂エフェクトとキラキラエフェクトを再生する。
            m_ClickParticle.transform.position = mousePosition;
            m_ClickParticleSystem.Play();   // １回再生(ParticleSystemのLoopingがfalseだから)
           
        }
        if (Input.GetMouseButtonUp(0))
        {
            // 左ボタンアップを検知したら、Particleの放出を停止する
            m_ClickParticleSystem.Stop();
        }
    }
}