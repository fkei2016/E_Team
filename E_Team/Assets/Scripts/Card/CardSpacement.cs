// ==================================================
// カードの並びを均等にするスクリプト
// ==================================================
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardSpacement : MonoBehaviour {

    [SerializeField, Range(3,5)]
    private int cardNumber = 3;

    private float width = 800F;
    private GridLayoutGroup layout;

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        // レイアウトの横幅を取得
        width = GetComponent<RectTransform>().sizeDelta.x;

        // 均等に並ぶようにレイアウトを設定
        layout = GetComponent<GridLayoutGroup>();
    }

    /// <summary>
    /// 更新時に実行
    /// </summary>
    private void Update() {
        // レイアウトの横幅から間隔を計算
        var spacing = (width - (PlayManager.instance.CardSize.x * cardNumber)) / cardNumber;

        // 均等に並ぶようにレイアウトを設定
        layout.padding.left = (int)spacing;
        layout.spacing = new Vector2(spacing / 2, 10);
    }
}
