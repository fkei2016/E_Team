// ==================================================
// ゲーム内でのカードの管理者クラス
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : SingletonMonoBehaviour<CardManager> {

    [SerializeField, Range(3,5)]
    private int pair = 3;
    [SerializeField]
    private Vector2 cardSize = new Vector2(150F, 250F);

    public int Pair { get { return pair; } private set { pair = value; } }
    public Vector2 CardSize { get{ return cardSize; } set { CardSize = value; } }

    private CardGenerator generator;
    private int keepPairNum;

    /// <summary>
    /// 開始時に処理
    /// </summary>
    private void Start() {
        // 生成機をシーンから取得
        generator = FindObjectOfType<CardGenerator>();

        // 初期のカードを生成
        generator.RemakeCards(pair * 2);
        keepPairNum = pair;
    }

    /// <summary>
    /// 更新時に処理
    /// </summary>
    private void Update() {
        // ペア数が変わったときに再生成
        if(keepPairNum != pair)
        {
            generator.RemakeCards(pair * 2);
            keepPairNum = pair;
        }

        // [Debug]全てのカードを非表示に
        if(Input.GetKeyDown(KeyCode.Space))
        {
            generator.ResetCardNumbers();
        }
    }
}
