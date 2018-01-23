// ==================================================
// カードの個別のスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour {

    [HideInInspector]
    public int number = 0;
    [HideInInspector]
    public float rotaSpd = 1F;
    [HideInInspector]
    public Vector2 size = Vector2.one;

    private GameObject back;
    private float toRotation;
    private bool rotateFlag;
    private bool isPlayFade;

    private Animation anim;

    public bool active
    {
        get { return back.activeSelf; }
    }

    public bool isFinish
    {
        get { return isPlayFade && !anim.IsPlaying("FadeOut"); }
    }
    
    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        // 背面の取得
        back = transform.GetChild(transform.childCount - 1).gameObject;
        toRotation = 0F;
        rotateFlag = false;
        isPlayFade = false;

        // フェードアウトのアニメーションを追加
        anim = gameObject.AddAnimationClip(new string[] { "FadeOut" });
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        // 回転処理をかける
        if (rotateFlag)
        {
            transform.Rotate(Vector3.up * rotaSpd);
            if (transform.localEulerAngles.y >= toRotation - rotaSpd)
            {
                rotateFlag = false;
            }
        }

        // 「柄」を表示
        if (back.activeSelf && transform.localEulerAngles.y >= 180F - 90F)
        {
            back.SetActive(false);
        }

        // 「背面」を表示
        if(!back.activeSelf && transform.localEulerAngles.y >= 360F - 90F)
        {
            back.SetActive(true);
        }
    }

    /// <summary>
    /// カードを開く
    /// </summary>
    public void Open() {
        CardManager.instance.SendCard(this);
        rotateFlag = true;
        toRotation = 180F;
    }

    /// <summary>
    /// カードを閉じる
    /// </summary>
    public IEnumerator Close(float waitTime = 0F) {
        yield return new WaitForSeconds(waitTime);
        rotateFlag = true;
        toRotation = 360F;
    }

    /// <summary>
    /// フェードアウトで消える
    /// </summary>
    public IEnumerator FadeOut(float waitTime = 0F) {
        yield return new WaitForSeconds(waitTime);
        anim.Play("FadeOut");
        isPlayFade = true;
    }

    public void OnClick(Vector3 position) {
        var worldPosition = Camera.main.WorldToScreenPoint(transform.position);

        // カードの矩形とクリック座標の交点で判定
        Rect rect = new Rect(worldPosition.x - size.x / 2, worldPosition.y - size.y / 2, size.x, size.y);
        if (rect.Contains(position))
        {
            if (back.activeSelf && !rotateFlag)
                Open();
        }

    }

}