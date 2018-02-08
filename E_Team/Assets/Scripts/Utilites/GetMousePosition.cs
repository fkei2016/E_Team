using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMousePosition : MonoBehaviour {

    public Text mousePositionText;

	// Use this for initialization
	void Start () {
        mousePositionText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
