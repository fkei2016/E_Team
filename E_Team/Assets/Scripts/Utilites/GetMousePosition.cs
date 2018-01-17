﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMousePosition : MonoBehaviour {

    private Text mousePositionText;

	// Use this for initialization
	void Start () {
        mousePositionText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        var text = Input.mousePosition.ToString();
        mousePositionText.text = text;
	}
}
