// ==================================================
// シーン名で遷移できる簡易スクリプト
// ==================================================
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpToScene : MonoBehaviour {


    [SerializeField]
    private string sceneName;

    /// <summary>
    /// シーン遷移の実行
    /// </summary>
    public void Execute() {
        PhotonNetwork.LoadLevel(sceneName);
    }

}
