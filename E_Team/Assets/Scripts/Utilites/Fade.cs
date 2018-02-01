// ==================================================
// イメージに対する透過処理のスクリプト
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    [SerializeField]
    private Image img;

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="fadeTime">
    /// 経過終了時間
    /// </param>
    /// <param name="method">
    /// ラムダ式
    /// </param>
    public void In(float fadeTime = 1F, System.Action method = null) {
        StartCoroutine(FadeUpdate(0F, 1F, fadeTime, method));
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="fadeTime">
    /// 経過終了時間
    /// </param>
    /// <param name="method">
    /// ラムダ式
    /// </param>
    public void Out(float fadeTime = 1F, System.Action method = null) {
        StartCoroutine(FadeUpdate(1F, 0F, fadeTime, method));
    }

    /// <summary>
    /// フェードオン
    /// </summary>
    /// <param name="outTime">
    /// フェードアウト時間
    /// </param>
    /// <param name="inTime">
    /// フェードイン時間
    /// </param>
    /// <param name="method">
    /// ラムダ式
    /// </param>
    public void On(float outTime, float inTime, System.Action method = null) {
        StartCoroutine(FadeOn(outTime, inTime, method));
    }

    /// <summary>
    /// フェードオン
    /// </summary>
    /// <param name="outTime">
    /// フェードアウト時間
    /// </param>
    /// <param name="inTime">
    /// フェードイン時間
    /// </param>
    /// <param name="method">
    /// ラムダ式
    /// </param>
    /// <returns>
    /// コルーチン
    /// </returns>
    public IEnumerator FadeOn(float outTime, float inTime, System.Action method = null) {
        // フェードアウト
        Out(outTime);
        yield return new WaitForSeconds(outTime);

        // ラムダ式
        method();

        // フェードイン
        yield return new WaitForSeconds(inTime);
        In(inTime);
    }

    /// <summary>
    /// 透過更新
    /// </summary>
    /// <param name="start">
    /// 開始する値[0 ~ 1]
    /// </param>
    /// <param name="end">
    /// 終了する値[1 ~ 0]
    /// </param>
    /// <param name="interval">
    /// 経過終了時間
    /// </param>
    /// <param name="method">
    /// ラムダ式
    /// </param>
    /// <returns>
    /// コルーチン
    /// </returns>
    private IEnumerator FadeUpdate(float start, float end, float interval, System.Action method = null) {
        // 0から1までクランプ
        start = Mathf.Clamp01(start);
        end = Mathf.Clamp01(end);

        // 経過終了まで透過度を補間する
        float time = 0F;
        while (time <= interval)
        {
            SetAlpha(img, Mathf.Lerp(start, end, time / interval));
            time += Time.deltaTime;
            yield return 0;
        }

        // フェード終了後の処理
        method();
    }

    /// <summary>
    /// 透過度の設定
    /// </summary>
    /// <param name="img">
    /// 透過画像
    /// </param>
    /// <param name="alpha">
    /// 透過度
    /// </param>
    private void SetAlpha(Image img, float alpha) {
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }
}
