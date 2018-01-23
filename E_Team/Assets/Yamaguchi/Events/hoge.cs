using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hoge : Orderbace {

    // Update is called once per frame
    void Update ()
    {

        if (PhotonNetwork.isMasterClient)
            NetworkManager.instance.Order(this);

        this.gameObject.active = false;
    }

    [PunRPC]
    override public void Order()
    {

        Debug.Log("massage");
        //PhotonNetwork.LoadLevel("O_Test");
    }

}