using UnityEngine;
using System.Collections;

public class TouchCombo : MonoBehaviour
{
    /// <summary>
    /// ポップアップするテキストオブジェクト
    /// NumberTextScript付き
    /// </summary>
    public GameObject PopupText;

    /// <summary>
    /// ポップアップする文字列
    /// </summary>
    public string PopupString = "Combo";

    ////コンボ数
    //public int ComboCount;

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    /// <param name="target">
    /// 表示するトランスフォーム
    /// </param>
    /// <returns></returns>
    public IEnumerator Emission(Transform target)
    {
        yield return new WaitForSeconds(1F);

        var temp = new GameObject("Combo");
        temp.transform.Reset(transform);
        //temp.transform.position = Vector3.zero;

        var gen = temp.AddComponent<ComboPopUp>();
        gen.PopupString = PopupString;
        //gen.PopupPosition = worldPosition;
        //gen.PopupTextWidth = 30.0f;
        gen.TargetCanvas = target.gameObject;
        gen.PopupTextObject = this.PopupText;
        gen.Popup();


        //音追加
        var audio = AudioManager.instance;
        if (audio) audio.PlaySE("PearSE");
    }
}