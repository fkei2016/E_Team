using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActivate : MonoBehaviour {

    [SerializeField]
    GameObject[] objects;

    int keepNumber = 0;

    AudioManager audio;
    PhotonView view;

    private void Start() {
        audio = AudioManager.instance;
        view = PhotonView.Get(this);
    }

    public void Execution() {
        //押されたら音を鳴らす
        if(audio) audio.PlaySE("DecisionSE");
        if(++keepNumber >= objects.Length)
        {
            keepNumber = 0;
        }


        view.RPC("ChangeActivate", PhotonTargets.MasterClient, PhotonNetwork.player.ID - 1, objects[0].GetActive());

        foreach(GameObject obj in objects)
        {
            obj.SetActive(false);
        }
        objects[keepNumber].SetActive(true);
    }

    [PunRPC]
    void ChangeActivate(int _id,bool _active)
    {
        SceneTransition.instance.checklist[_id] = _active;
    }
}
