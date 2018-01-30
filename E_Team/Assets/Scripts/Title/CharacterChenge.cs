using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChenge : MonoBehaviour {

    private int characterNum = 0;

    private bool moveFlag = false;

    [SerializeField]
    private GameObject trigger;
    [SerializeField]
    private float width;

    [SerializeField]
    private TakeOverClient takeoverclient;

    /// <summary>
    /// 更新時に実行
    /// </summary>
	void Update () {
        Client.characterNumber = characterNum % (transform.childCount);
    }

    /// <summary>
    /// キャラクター番号の変更
    /// </summary>
    /// <param name="num">
    /// 番号の移動数
    /// </param>
    public void AddCharacterNum(int num) {
        if (!moveFlag && trigger.activeSelf)
        {
            moveFlag = true;
            characterNum += num;
            CharacterMove();
        }
    }

    /// <summary>
    /// キャラクターの移動処理
    /// </summary>
    void CharacterMove() {
        // 範囲内にクランプ
        characterNum = Mathf.Clamp(characterNum, 0, transform.childCount - 1);
        // 番号分だけ移動
        Move(-width * characterNum);

    }

    /// <summary>
    /// 移動の終了
    /// </summary>
    void MoveEnd() {
        // メゾット名で呼び出される
        moveFlag = false;

        takeoverclient.ChangeClientNum(PhotonNetwork.player.ID - 1, Client.characterNumber);
        //TakeOverClient.clientnums[PhotonNetwork.player.ID - 1] = Client.characterNumber;
    }

    /// <summary>
    ///  移動処理
    /// </summary>
    /// <param name="direction">
    /// X軸移動量
    /// </param>
    /// <param name="time">
    /// 移動時間
    /// </param>
    void Move(float direction,float time = 1F) {
        Hashtable table = new Hashtable();
        table.Add("x", direction);
        table.Add("time", time);
        table.Add("islocal", true);
        table.Add("oncomplete", "MoveEnd");
        iTween.MoveTo(gameObject, table);
    }
}
