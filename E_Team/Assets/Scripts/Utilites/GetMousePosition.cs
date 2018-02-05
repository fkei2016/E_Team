using System.Collections;
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

        Vector3 pos = CardManager.instance.clickposition;
        pos.z = CardManager.instance.clickcnt;
        var text = pos.ToString();
        mousePositionText.text = text;
	}
}
