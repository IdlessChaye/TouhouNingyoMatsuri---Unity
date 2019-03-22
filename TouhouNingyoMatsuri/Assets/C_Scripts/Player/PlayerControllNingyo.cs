using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControllNingyo : NetworkBehaviour {
    public GameObject FX_release;
    public GameObject FX_swtich;

    void Update() {
        if(!hasAuthority)
            return;
        if(Input.GetKeyDown(KeyCode.Q)) {
            GetComponent<PlayerNingyoListManager>().SwitchCapturedNingyo();
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            GetComponent<PlayerNingyoListManager>().ReleaseFirstCapturedNingyo();
            GetComponent<UIPlayerManager>().ReqMinusAllNingyoCount();
            GetComponent<UIPlayerManager>().ReqMinusNowNingyoCount();
            GetComponent<UIPlayerManager>().ReqGetPlayerDictionary();
        }
    }
}
