using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCreate : MonoBehaviour
{

    [SerializeField]
    private GameObject _cardPrefab;

    [SerializeField]
    private int _maxCardNum;

    [SerializeField]
    private int _maxRange;

    private List<int> _number = new List<int>();

    private List<int> _rangeNumber = new List<int>();

    void Awake()
    {
        if (_maxCardNum % 2 == 1)
            _maxCardNum++;

        List<int> list = new List<int>();

        for (var i = 0; i < 13; i++)
        {
            list.Add(i);
        }

        //同じカード2つ生成
        for (var i = 0; i < _maxCardNum / 2; i++)
        {
            var card = Random.Range(0, list.Count);
            card = list[card];
            list.Remove(card);
            _rangeNumber.Add(card);
            _rangeNumber.Add(card);

        }

        //カード生成
        for (int i = 0; i < _maxCardNum; i++)
        {
            var card = Instantiate(_cardPrefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);

            var num = Random.Range(0, _rangeNumber.Count);
            num = _rangeNumber[num];
            _rangeNumber.Remove(num);

            //カードに番号を振る
            card.GetComponentInChildren<Text>().text = num.ToString();
            //_number.Add(num);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}