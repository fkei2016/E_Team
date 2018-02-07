using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//未接続時メッセージ
public class ConnectedMassage : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        this.gameObject.active = !(PhotonNetwork.connected);
	}
}
