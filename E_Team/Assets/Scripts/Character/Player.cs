using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [HideInInspector]
    public int number;

    private Image skill;
    private GameObject mask;

    [SerializeField, Range(0, 1)]
    private float skillTmp = 0F;

    private float skillGauge
    {
        get { return skill.fillAmount; }
        set { skill.fillAmount = value; }
    }
    public bool active
    {
        get { return !mask.activeSelf; }
        set { mask.SetActive(value); }
    }

    /// <summary>
    /// 生成時に実行
    /// </summary>
	void Awake () {
        skill = transform.GetChild(1).GetComponent<Image>();
        mask = transform.GetChild(3).gameObject;
    }

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        skillGauge = skillTmp;
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        if(skillGauge > skillTmp)
        {
            skillGauge -= 0.1F;
        }
        else if(skillGauge < skillTmp)
        {
            skillGauge += 0.1F;
        }
    }

    /// <summary>
    /// スキル上昇
    /// </summary>
    /// <param name="upper">
    /// 上昇値
    /// </param>
    public void SkillUp(float upper) {
        skillTmp += upper * 0.1F;
    }

    /// <summary>
    /// クリック処理
    /// </summary>
    /// <param name="position">
    /// クリック座標
    /// </param>
    public bool OnClick(Vector3 position) {
        var worldPosition = Camera.main.WorldToScreenPoint(transform.position);
        var size = GetComponent<RectTransform>().sizeDelta;

        // 矩形とクリック座標の交点で判定
        Rect rect = new Rect(worldPosition.x - size.x / 2, worldPosition.y - size.y / 2, size.x, size.y);
        if (rect.Contains(position) && skillGauge >= 1F)
        {
            // 値を初期化
            InitGauge();
            return true;
        }
        return false;
    }

    public void InitGauge()
    {
        // 値を初期化
        skillTmp = 0F;
    }
}
