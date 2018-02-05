using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    [SerializeField]
    private GameObject trigger;

    private PhotonView view;

    bool[] standby;

    public void Start()
    {
        view = PhotonView.Get(this);
    }

    private void Update() {
        if (!PhotonNetwork.isMasterClient)
            return;

        if (trigger.activeSelf)
        {
            view.RPC("Execute", PhotonTargets.All);
        }
    }

    /// <summary>
    /// シーン遷移の処理を共有します（山口追加）
    /// </summary>
    [PunRPC]
    private void Execute()
    {
        NetworkManager.instance.photonview.ObservedComponents.Clear();
        PhotonNetwork.isMessageQueueRunning = false;
        GetComponent<JumpToScene>().Execute();
    }

    /// <summary>
    /// シーン遷移の決定を同期（山口追加）
    /// </summary>
    private void SwitchStandby(bool _flag,int _id)
    {
        standby[_id] = _flag;
    }

}