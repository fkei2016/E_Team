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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var temp = new GameObject();
            temp.transform.position = Vector3.zero;
            ComboPopUp gen = temp.AddComponent<ComboPopUp>();
            gen.PopupString = "1combo";
            gen.PopupPosition = Input.mousePosition; 
            gen.PopupTextWidth = 40.0f;
            gen.TargetCanvas = this.TargetCanvas;
            gen.PopupTextObject = this.PopupText;
            gen.Popup();

            print(Input.mousePosition);
        }
    }
}