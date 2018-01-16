using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hoge : Orderbace {

    private void Start()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        if(PhotonNetwork.isMasterClient)
        NetworkManager.instance.Order(this);


        this.gameObject.active = false;
    }
    
    override public void Order()
    {
        PhotonNetwork.LoadLevel("O_Test");
    }

}