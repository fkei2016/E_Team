using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActivate : MonoBehaviour {

    [SerializeField]
    GameObject[] objects;

    int keepNumber = 0;

    public void Execution() {
        //押されたら音を鳴らす
        AudioManager.instance.PlaySE("DecisionSE");
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
