using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour {

    public bool IsFinish = false;

    public void OnAnimationFinish()
    {
        this.IsFinish = true;
    }
}
