using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hoge : Orderbace {

    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update ()
    {

        if(PhotonNetwork.isMasterClient)
        view.RPC("Order", PhotonTargets.All);

        this.gameObject.active = false;
    }

    [PunRPC]
    override public void Order()
    {
        PhotonNetwork.LoadLevel("O_Test");
    }

}