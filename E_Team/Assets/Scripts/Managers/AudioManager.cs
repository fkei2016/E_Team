// ==================================================
// オーディオの管理者クラス
// ==================================================1
using System.Collections.Generic;
using UnityEngine;

using AudioIndex = System.Collections.Generic.Dictionary<string, int>;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

    [SerializeField]
    private AudioClip[] bgmClips;
    [SerializeField]
    private AudioClip[] seClips;

    private AudioIndex bgmIndex;
    private AudioIndex seIndex;

    private AudioSource bgmPlayer;
    private AudioSource sePlayer;

    /// <summary>
    /// 生成時に実行
    /// </summary>
    protected override void Awake() {
        // シングルトン
        base.Awake();

        // BGMのオーディオを生成
        bgmPlayer = CreateAudioPlayer(true);
        bgmIndex = MakeAudioIndex(bgmClips);

        // SEのオーディオを生成
        sePlayer = CreateAudioPlayer();
        seIndex = MakeAudioIndex(seClips);
    }

    /// <summary>
    /// オーディオソースの生成
    /// </summary>
    /// <param name="loop">
    /// 繰り返し
    /// </param>
    /// <returns>
    /// オーディオソース
    /// </returns>
    AudioSource CreateAudioPlayer(bool loop = false) {
        var audio = gameObject.AddComponent<AudioSource>();
        audio.loop = loop;
        return audio;
    }

    /// <summary>
    /// オーディオ番号の設定
    /// </summary>
    /// <param name="clips">
    /// オーディオの配列
    /// </param>
    /// <returns>
    /// オーディオ番号
    /// </returns>
    AudioIndex MakeAudioIndex(AudioClip[] clips) {
        var index = new AudioIndex();
        for(int i = 0; i < clips.Length; i++)
        {
            index.Add(clips[i].name, i);
        }
        return index;
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="soundName">
    /// オーディオ名
    /// </param>
    public void PlayBGM(string soundName) {
        PlayBGM(bgmIndex[soundName]);
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="index">
    /// オーディオ番号
    /// </param>
    public void PlayBGM(int index) {
        bgmPlayer.clip = bgmClips[index];
        bgmPlayer.Play();
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="soundName">
    /// オーディオ名
    /// </param>
    public void PlaySE(string soundName) {
        PlaySE(seIndex[soundName]);
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="index">
    /// おーひど番号
    /// </param>
    public void PlaySE(int index) {
        sePlayer.PlayOneShot(seClips[index]);
    }
}
