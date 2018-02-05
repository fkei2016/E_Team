using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviour {

    public static NetworkManager instance;

    public RoomOptions roomOptions
    {
        get; private set;
    }

    public PhotonView photonview
    {
        get; private set;
    }

    private Orderbace networkorder;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        PhotonNetwork.ConnectUsingSettings("ver2.0");
        PhotonNetwork.sendRate = 60;

        photonview = GetComponent<PhotonView>();
    }

    void OnJoinedLobby()
    {
        Debug.Log("ロビーに入室しました");
        PhotonNetwork.JoinRandomRoom();
    }
	
    void OnJoinedRoom()
    {
        PhotonPlayer[] player = PhotonNetwork.playerList;
        Debug.Log("ルームに入室しました");
        //プレイヤー名を指定
        if (string.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = "Player" + player.Length;
        }

    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("ルームを生成しました");

        //カスタムプロパティの設定
        roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom("pRoom2", roomOptions, null);//ルームを生成し名前を指定
    }

    //ルームから退室したプレイヤーが直前で行う処理
    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log(player.name + "が退室しました");
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log(newMasterClient.name + "さん が新しいホストになりました");
    }

    public void Order(Orderbace _order)
    {
        networkorder = _order;
        photonview.RPC("Networkorder", PhotonTargets.All);
    }

    [PunRPC]
    private void Networkorder()
    {
        PhotonNetwork.LoadLevel("O_Test");
    }

}