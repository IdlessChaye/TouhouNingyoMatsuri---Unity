using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCaptureNingyo : NetworkBehaviour {
    public GameObject[] NingyoCaptured;
    public GameObject FX_fengyin;
    private float lastesrNingyoHP;

    void OnCollisionEnter(Collision collision) {
        if(!hasAuthority)
            return;
        GameObject ningyo = collision.gameObject;
        if(ningyo.tag == "NingyoUncaptured") {
            CaptureNingyo(ningyo);
        }
    }

    void CaptureNingyo(GameObject ningyo) {
        if(!hasAuthority)
            return;
        lastesrNingyoHP = ningyo.GetComponent<NingyoBoomManager>().GetNingyoHP();
        CmdReplaceClientNingyo(ningyo.GetComponent<NetworkIdentity>().netId);
    }

    [Command]
    void CmdReplaceClientNingyo(NetworkInstanceId netId) {
        GameObject ningyo = NetworkServer.FindLocalObject(netId);
        if(ningyo == null)
            return;
        string ningyoName;
        ningyoName = ningyo.name;
        Vector3 position = ningyo.transform.position;
        Quaternion rotation = ningyo.transform.rotation;
        Destroy(ningyo);
        foreach(GameObject go in NingyoCaptured) {
            if(ningyoName.Equals(go.name.Replace("Captured", ""))) {
                GameObject clientNingyo = Instantiate(go, position, rotation) as GameObject;
                NetworkServer.SpawnWithClientAuthority(clientNingyo, connectionToClient);
                GameObject GO_FX_fengyin = Instantiate(FX_fengyin, clientNingyo.transform.position, clientNingyo.transform.rotation);
                FXBoomFollowManager fxm = GO_FX_fengyin.GetComponent<FXBoomFollowManager>();
                if(fxm != null) {
                    fxm.SetTarget(clientNingyo);
                }
                NetworkServer.Spawn(GO_FX_fengyin);
                Destroy(GO_FX_fengyin, 0.5f);
                RpcAddCapturedNingyo(clientNingyo.GetComponent<NetworkIdentity>().netId);
                break;
            }
        }
    }

    [ClientRpc]
    void RpcAddCapturedNingyo(NetworkInstanceId netId) {
        if(!hasAuthority)
            return;
        GameObject ningyo = ClientScene.FindLocalObject(netId);
        ningyo.GetComponent<NingyoBoomManager>().SetNingyoHP(lastesrNingyoHP);
        GetComponent<PlayerNingyoListManager>().AddLastCapturedNingyo(netId);
        GetComponent<UIPlayerManager>().ReqAddAllNingyoCount();
        GetComponent<UIPlayerManager>().ReqAddNowNingyoCount();
        GetComponent<UIPlayerManager>().ReqGetPlayerDictionary();
    }
}
