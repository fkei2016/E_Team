// ==================================================
// ジェネリック型のシングルトンパターン
// ==================================================
using UnityEngine;

public class SingletonMonoBehaviour<Type> :MonoBehaviour where Type : MonoBehaviour {

    [SerializeField]
    private bool dontDestory = true;

    private static Type singleton = null;
    public static Type instance
    {
        get
        {
            if(singleton == null)
            {
                // シーン内のシングルトンを検索
                singleton = FindObjectOfType<Type>();
            }
            return singleton;
        }
    }

    /// <summary>
    /// 生成時に実行
    /// </summary>
    protected virtual void Awake() {
        // 既存のインスタンスがシングルトンになる
        if(this != instance)
        {
            Destroy(this.gameObject);
            return;
        }

        // シーンを跨いで破棄されないオブジェクト
        if (dontDestory)
        {
            DontDestroyOnLoad(this);
        }
    }
}
