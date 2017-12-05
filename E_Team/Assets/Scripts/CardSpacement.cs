using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class CardSpacement : MonoBehaviour {

    [SerializeField, Range(3,5)]
    private int cardNumber = 3;
    [SerializeField]
    private Vector2 cardSize = new Vector2(150, 225);

    private GridLayoutGroup layout;
    private Transform[] cards;

    /// <summary>
    /// 生成時に実行
    /// </summary>
    private void Awake() {
        layout = GetComponent<GridLayoutGroup>();
        cards = new Transform[transform.childCount];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 開始時に実行
    /// </summary>
    private void Start() {
        var width = GetComponent<RectTransform>().sizeDelta.x;
        var spacing = (width - (cardSize.x * cardNumber)) / cardNumber;
        layout.padding.left = (int)spacing;
        layout.spacing = new Vector2(spacing / 2, 10);
    }
}
