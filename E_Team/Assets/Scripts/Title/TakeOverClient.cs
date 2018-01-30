using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//山口追加(2018/01/26)
//TitleSceneで取得したClientのデータをPlaySceneに引き継ぎます

public class TakeOverClient : MonoBehaviour {

    public static int[] clientnums;
    PhotonView view;

	// Use this for initialization
	void Start () {
        clientnums = new int[] { 0, 0, 0, 0 };
        NetworkManager.instance.photonview.ObservedComponents.Clear();
        NetworkManager.instance.photonview.ObservedComponents.Add(this);
        view = PhotonView.Get(this);
    }

    public void ChangeClientNum(int _playerid, int _characternum)
    {
        view.RPC("changeclientnum", PhotonTargets.All, _playerid, _characternum);
    }

    [PunRPC]
    private void changeclientnum(int _playerid, int _characternum)
    {
        clientnums[_playerid] = _characternum;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(clientnums);
        }
        else
        {
            clientnums = (int[])stream.ReceiveNext();
        }
    }

}