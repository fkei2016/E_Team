using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActivate : MonoBehaviour {

    [SerializeField]
    GameObject[] objects;

    int keepNumber = 0;

    AudioManager audio;

    private void Start() {
        audio = AudioManager.instance;
    }

    public void Execution() {
        //押されたら音を鳴らす
        if(audio) audio.PlaySE("DecisionSE");
        if(++keepNumber >= objects.Length)
        {
            keepNumber = 0;
        }

        foreach(GameObject obj in objects)
        {
            obj.SetActive(false);
        }
        objects[keepNumber].SetActive(true);
    }
}
