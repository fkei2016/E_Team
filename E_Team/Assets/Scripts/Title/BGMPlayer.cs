using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour {

    [SerializeField]
    private string BGMname;


	// Use this for initialization
	void Start () {

        AudioManager.instance.PlayBGM(BGMname);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
