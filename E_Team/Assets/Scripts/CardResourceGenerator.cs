using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardResourceGenerator : MonoBehaviour {

    [SerializeField]
    private Vector2 rectSize = new Vector2(150F, 250F);

    [SerializeField]
    private Texture2D cardBack;
    [SerializeField]
    private Texture2D cardFrame;
    [SerializeField]
    private Texture2D[] cardDesigns;

    private void Start() {
        for (int i = 0; i < cardDesigns.Length; i++)
        {
            // カードの生成
            var card = CreateUI.Create("Card (" + i + ")", transform);
            card.AddComponent<Card>();

            // 枠画像の生成
            var frame = CreateUI.Create("Frame", card.transform);
            CreateUI.Attach(frame, cardFrame, rectSize);

            // デザイン画像の生成
            var design = CreateUI.Create("Design", card.transform);
            CreateUI.Attach(design, cardDesigns[i], rectSize);

            // 背面画像の生成
            var back = CreateUI.Create("Back", card.transform);
            CreateUI.Attach(back, cardBack, rectSize);
            back.SetActive(true);
        }
    }
}
