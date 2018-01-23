using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Image skill;
    private GameObject basic;

    private float skillTmp;

    private float skillGauge
    {
        get { return skill.fillAmount; }
        set { skill.fillAmount = value; }
    }
    public bool active
    {
        get { return basic.activeSelf; }
        set { basic.SetActive(value); }
    }

    public int number;

    public bool click
    {
        get { return Input.GetMouseButtonDown(0); }
    }

    public Vector3 clickPosition
    {
        get { return Input.mousePosition; }
    }

    /// <summary>
    /// 開始時に実行
    /// </summary>
	void Start () {
        skill = transform.GetChild(0).GetComponent<Image>();
        basic = transform.GetChild(2).gameObject;

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
