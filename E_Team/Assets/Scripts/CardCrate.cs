using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardCrate : MonoBehaviour {

    [SerializeField]
    private GameObject _cardPrefab;

    [SerializeField]
    private int _cardNum;

    [SerializeField]
    private int _maxRange;

    private List<int> _number = new List<int>();
    // Use this for initialization
    void Start()
    {
        //カード生成
        for (int i = 0; i < _cardNum; i++)
        {
            var card = Instantiate(_cardPrefab, gameObject.transform.position,gameObject.transform.rotation,gameObject.transform);


            while (true)
            {
                //カードの番号をランダムで決める
                int num = Random.Range(1, _maxRange);

                //配列の中に同じ番号がいくつあるか検索
                int count = 0;
                foreach (var n in _number)
                    if (n == num)
                        count++;
                //同じ数字が２個以下なら値を入れる
                if (count < 2)
                {
                    //カードに番号を振る
                    card.GetComponentInChildren<Text>().text = num.ToString();
                    _number.Add(num);
                    break;
                }
            }



        }

        
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}