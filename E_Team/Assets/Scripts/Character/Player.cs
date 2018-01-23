using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [HideInInspector]
    public int number;

    private Image skill;
    private GameObject mask;

    private float skillTmp;

    private float skillGauge
    {
        get { return skill.fillAmount; }
        set { skill.fillAmount = value; }
    }
    public bool active
    {
        get { return mask.activeSelf; }
        set { mask.SetActive(value); }
    }

    /// <summary>
    /// 開始時に実行
    /// </summary>
	void Awake () {
        skill = transform.GetChild(1).GetComponent<Image>();
        mask = transform.GetChild(3).gameObject;

        // クリックでスキル発動処理の実行
        gameObject.AddEventTrigger(UnityEngine.EventSystems.EventTriggerType.PointerUp,
            data =>{ SkillActivation(); });

        skillGauge = 0F;
        skillTmp = 0F;

        number = PhotonNetwork.player.ID;
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
    /// スキル発動
    /// </summary>
    public void SkillActivation() {
        switch (number)
        {
            case 0:
                Debug.Log("特殊攻撃");
                break;
            case 1:
                Debug.Log("特殊回復");
                break;
            case 2:
                Debug.Log("攻撃力向上");
                break;
            case 3:
                Debug.Log("ダメージカット");
                break;
            default:
                break;
        }

        if (active && skillGauge >= 1F)
        {
            skillTmp = 0F;
            Debug.Log("Use Skill !!");
        }
        else
        {
            Debug.Log("Dont use Skill...");
        }
    }
}
