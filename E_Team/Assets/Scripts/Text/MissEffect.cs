using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissEffect : MonoBehaviour {

    [SerializeField]
    private GameObject FadeText;

    /// <summary>
    /// パーティクルの発生
    /// </summary>
    /// <param name="target">
    /// 表示するトランスフォーム
    /// </param>
    /// <returns></returns>
    public IEnumerator Emission(Transform target) {
        // 発生の遅延
        yield return new WaitForSeconds(1F);

        // プレハブからエフェクトを生成
        var obj = Instantiate(FadeText);
        obj.transform.Reset(target);

        // 効果音の再生
        var audio = AudioManager.instance;
        if (audio) audio.PlaySE("MissSE");

        // エフェクトとして消滅
        yield return new WaitForSeconds(1F);
        Destroy(obj);
    }
}
