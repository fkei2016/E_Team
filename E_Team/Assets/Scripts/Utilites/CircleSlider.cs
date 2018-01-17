using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour {

    [Range(0F, 1F)]
    public float value;

    [SerializeField]
    private Color backColor;
    [SerializeField]
    private Color fillColor;
    [SerializeField]
    private int minValue = 0;
    [SerializeField]
    private int maxValue = 1;

    private Image back;
    private Image fill;

    /// <summary>
    /// 開始時に実行
    /// </summary>
	void Start () {
        // 背面と前景を取得
        back = transform.GetChild(0).GetComponent<Image>();
        fill = transform.GetChild(1).GetComponent<Image>();
        // 色情報を設定
        back.color = backColor;
        fill.color = fillColor;
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    void Update () {
        // 値を更新
        fill.fillAmount = minValue + (maxValue * Mathf.Clamp01(value)) / maxValue;
	}
}
