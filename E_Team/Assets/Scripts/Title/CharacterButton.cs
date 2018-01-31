using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour {

    [SerializeField]
    private CharacterChenge characterChenge;

    [SerializeField]
    private int num;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPushButton()
    {
        AudioManager.instance.PlaySE("SelectSE");
        characterChenge.AddCharacterNum(num);
    }
}
