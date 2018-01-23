using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    [SerializeField]
    private GameObject trigger;

    private void Update() {
        if (trigger.activeSelf)
        {
            GetComponent<JumpToScene>().Execute();
        }
    }
}
