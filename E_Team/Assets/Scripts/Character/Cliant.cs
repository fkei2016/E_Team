using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cliant : MonoBehaviour {

    static public bool click
    {
        get { return Input.GetMouseButtonDown(0); }
    }

    static public Vector3 clickPosition
    {
        get { return Input.mousePosition; }
    }
}
