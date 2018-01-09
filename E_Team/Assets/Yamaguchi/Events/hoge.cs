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
        //PhotonNetwork.LoadLevel("Title");
        //SceneManager.LoadScene("Title");

        
        NetworkManager.instance.Order(this);

        this.gameObject.active = false;
    }
    
    override public void Order()
    {
        Debug.Log("オーダー2");
        //SceneManager.LoadScene("O_Test");
        PhotonNetwork.LoadLevel("O_Test");
    }

}