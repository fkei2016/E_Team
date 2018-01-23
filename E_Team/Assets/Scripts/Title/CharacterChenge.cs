﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChenge : MonoBehaviour {

    [SerializeField]
    private GameObject[] characters;

    private Vector3 afterpos;

    [SerializeField]
    private int characterNum = 0;

    [SerializeField]
    private bool moveFlag = false;

    private float time = 1.0f;

    private float delTime = 0.0f;

	// Use this for initialization
	void Start () {

        afterpos = characters[characterNum].transform.position;
        var v1 = characters[0].transform.position;
        var v2 = characters[1].transform.position;
        var dir = v1 - v2;
	}
	
	// Update is called once per frame
	void Update () {

        if (moveFlag)
        {
            delTime += Time.deltaTime;
            if (delTime > 1.0f)
            {
                moveFlag = false;
                delTime = 0.0f;
            }
        }
    }

    public void AddCharacterNum(int num)
    {
        if (!moveFlag)
        {
            moveFlag = true;
            characterNum += num;
            CharacterMove(num);

            Client.characterNumber = characterNum;
        }
    }


    void CharacterMove(int num)
    {

        if (characterNum == characters.Length)
        {
            characterNum = characters.Length - 1;
            return;
        }
        if (characterNum == -1)
        {
            characterNum = 0;
            return;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (num == 1)
            {
                Move(-1.58f, characters[i]);
            }
            else
            {
                Move(1.58f, characters[i]);
            }
        }
    }

    void MoveEnd()
    {
        moveFlag = false;
    }

    void Move(float direction, GameObject obj, float time = 1F)
    {
        Hashtable table = new Hashtable();
        table.Add("x", direction);
        table.Add("time", time);
        iTween.MoveBy(obj, table);
    }
}