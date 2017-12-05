using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    private Button[] button;

	// Use this for initialization
	void Start ()
    {

        button = GetComponentsInChildren<Button>();

        for (int i = 0; i < button.Length; i++)
        {
            //button[i].onClick.AddListener(ButtonClick);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ButtonClick()
    {
        var text = GetComponentInChildren<Text>();
        text.enabled = true;
        print(text.text);
    }
}
