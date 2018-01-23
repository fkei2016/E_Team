using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : SingletonMonoBehaviour<Client> {

    static public bool click
    {
        get { return Input.GetMouseButtonDown(0); }
    }

    static public Vector3 clickPosition
    {
        get { return Input.mousePosition; }
    }

    static public int characterNumber = 0;
}
