using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChenge : MonoBehaviour {

    [SerializeField]
    private GameObject[] characters;

    private Vector3 afterpos;

    [SerializeField]
    private int characterNum = 1;

    [SerializeField]
    private float time = 1.0f;

    [SerializeField]
    private bool moveFlag = false;

    [SerializeField]
    private float deltime = 0.0f;


	// Use this for initialization
	void Start () {

        afterpos = characters[characterNum].transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        if (moveFlag)
        {
            deltime += Time.deltaTime;
            if (deltime > 1)
            {
                deltime = 0.0f;
                moveFlag = false;
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
        }
    }


    void CharacterMove(int num)
    {

        if (characterNum == characters.Length)
        {
            characterNum = 0;
        }
        if (characterNum == -1)
        {
            characterNum = 3;
        }


        if (characterNum == 0)
        {
            if (num < 0)
            {
                Hashtable table = new Hashtable();
                table.Add("x", 0);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum], table);
                //table.Clear();
                //table.Add("x", -200);
                //table.Add("time", time);
                //iTween.MoveTo(characters[characterNum + characters.Length - 1], table);
                Vector3 pos = characters[characterNum + characters.Length - 1].transform.position;
                pos.x = -200;
                characters[characterNum + characters.Length - 1].transform.position = pos;
                table.Clear();
                table.Add("x", 200);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum + 1], table);
            }
            else
            {
                Hashtable table = new Hashtable();
                table.Add("x", 0);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum], table);
                table.Clear();
                table.Add("x", -200);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum + characters.Length - 1], table);
                //table.Clear();
                //table.Add("x", 200);
                //table.Add("time", time);
                //iTween.MoveTo(characters[characterNum + 1], table);
                Vector3 pos = characters[characterNum + 1].transform.position;
                pos.x = 200;
                characters[characterNum + 1].transform.position = pos;
            }
           
        }
        else if (characterNum == 3)
        {
            if (num < 0)
            {
                Hashtable table = new Hashtable();
                table.Add("x", 0);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum], table);
                //table.Clear();
                //table.Add("x", -200);
                //table.Add("time", time);
                //iTween.MoveTo(characters[characterNum - 1], table);
                Vector3 pos = characters[characterNum - 1].transform.position;
                pos.x = -200;
                characters[characterNum - 1].transform.position = pos;
                table.Clear();
                table.Add("x", 200);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum - characters.Length + 1], table);
            }
            else
            {
                Hashtable table = new Hashtable();
                table.Add("x", 0);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum], table);
                table.Clear();
                table.Add("x", -200);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum - 1], table);
                //table.Clear();
                //table.Add("x", 200);
                //table.Add("time", time);
                //iTween.MoveTo(characters[characterNum - characters.Length + 1], table);
                Vector3 pos = characters[characterNum - characters.Length + 1].transform.position;
                pos.x = 200;
                characters[characterNum - characters.Length + 1].transform.position = pos;
            }
        }
        else
        {
            if (num < 0)
            {
                Hashtable table = new Hashtable();
                
                table.Add("x", 0);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum], table);
                //table.Clear();
                //table.Add("x", -200);
                //table.Add("time", time);
                //iTween.MoveTo(characters[characterNum - 1], table);
                Vector3 pos = characters[characterNum - 1].transform.position;
                pos.x = -200;
                characters[characterNum - 1].transform.position = pos;
                table.Clear();
                table.Add("x", 200);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum + 1], table);
            }
            else
            {
                Hashtable table = new Hashtable();
                table.Add("x", 0);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum], table);
                table.Clear();
                table.Add("x", -200);
                table.Add("time", time);
                iTween.MoveTo(characters[characterNum - 1], table);
                //table.Clear();
                //table.Add("x", 200);
                //table.Add("time", time);
                //iTween.MoveTo(characters[characterNum + 1], table);
                Vector3 pos = characters[characterNum + 1].transform.position;
                pos.x = 200;
                characters[characterNum + 1].transform.position = pos;
            }
        }
    }
}
