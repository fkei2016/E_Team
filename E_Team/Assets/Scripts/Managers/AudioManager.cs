// ==================================================
// オーディオの管理者クラス
// ==================================================1
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField]
    private AudioClip[] seClips;

    private Dictionary<string, int> bgmIndex;
    private Dictionary<string, int> seIndex;

    private AudioSource bgmPlayer;
    private AudioSource sePlayer;

    /// <summary>
    /// 生成時に実行
    /// </summary>
    protected override void Awake() {
        base.Awake();

        // BGMのオーディオを生成
        bgmPlayer = CreateAudioPlayer(true);
        bgmIndex = MakeAudioIndex(bgmClips);

        // SEのオーディオを生成
        sePlayer = CreateAudioPlayer();
        seIndex = MakeAudioIndex(seClips);
    }

    /// <summary>
    /// オーディオソースの設定
    /// </summary>
    /// <param name="loop"></param>
    /// <returns></returns>
    AudioSource CreateAudioPlayer(bool loop = false) {
        var audio = gameObject.AddComponent<AudioSource>();
        audio.loop = loop;
        return audio;
    }

    /// <summary>
    /// オーディオインデックスの設定
    /// </summary>
    /// <param name="clips"></param>
    /// <returns></returns>
    Dictionary<string, int> MakeAudioIndex(AudioClip[] clips) {
        var index = new Dictionary<string, int>();
        for(int i = 0; i < clips.Length; i++)
        {
            index.Add(clips[i].name, i);
        }
        return index;
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayBGM(string soundName) {
        // 名前からループ再生を行う
        var index = bgmIndex[soundName];
        bgmPlayer.clip = bgmClips[index];
        bgmPlayer.Play();
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySE(string soundName) {
        // 名前から単一再生を行う
        var index = seIndex[soundName];
        sePlayer.PlayOneShot(seClips[index]);
    }
}
