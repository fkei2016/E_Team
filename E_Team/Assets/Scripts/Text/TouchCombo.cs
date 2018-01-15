using UnityEngine;
using System.Collections;

public class TouchCombo : MonoBehaviour
{

    /// <summary>
    /// 出力先キャンバス
    /// </summary>
    public GameObject TargetCanvas;

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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Emission(Input.mousePosition);
        }
    }

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    /// <param name="worldPosition">
    /// ワールド座標
    /// </param>
    public void Emission(Vector3 worldPosition)
    {
        var temp = new GameObject("Combo");
        temp.transform.position = Vector3.zero;

        var gen = temp.AddComponent<ComboPopUp>();
        gen.PopupString = PopupString;
        gen.PopupPosition = worldPosition;
        //gen.PopupTextWidth = 30.0f;
        gen.TargetCanvas = this.TargetCanvas;
        gen.PopupTextObject = this.PopupText;
        gen.Popup();
    }
}