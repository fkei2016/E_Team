// ==================================================
// Sliderの値をテキストに反映
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleText : MonoBehaviour {

    [SerializeField]
    private Text text;
    [SerializeField]
    private Slider slider;

    /// <summary>
    /// 実行時の処理
    /// </summary>
    void Start () {
        // "Slider"コンポーネントを取得
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    /// <summary>
    /// 更新時の処理
    /// </summary>
    void Update () {
        /// スライダーの値を文字にする
        text.text = slider.value.ToString();
	}
}
