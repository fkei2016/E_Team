using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChenge : MonoBehaviour {

    private int characterNum = 0;
    

    private bool moveFlag = false;

    [SerializeField]
    private float width;

    [SerializeField]
    private int maxcharacter = 4;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

      
    }

    public void AddCharacterNum(int num)
    {
        if (!moveFlag)
        {
            moveFlag = true;
            characterNum += num;
            CharacterMove();
        }
    }


    void CharacterMove()
    {

        characterNum = Mathf.Clamp(characterNum, 0, maxcharacter - 1);

        Move(-width * characterNum);

    }

    void MoveEnd()
    {
        moveFlag = false;
    }

    void Move(float direction,float time = 1F)
    {
        Hashtable table = new Hashtable();
        table.Add("x", direction);
        table.Add("time", time);
        table.Add("islocal", true);
        table.Add("oncomplete", "MoveEnd");
        iTween.MoveTo(gameObject, table);
    }
}
