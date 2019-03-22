using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetWorkDataSync : NetworkBehaviour {
    public void SetName(string name, NetworkInstanceId playerNetId) {
        if(!isServer)
            return;
        RpcSetName(name, playerNetId);
    }

    [ClientRpc]
    private void RpcSetName(string name, NetworkInstanceId playerNetId) {
        GameObject player = ClientScene.FindLocalObject(playerNetId);
        player.name = name;
        FullDataManager.Instance.AddPlayerName(name.Replace("(Clone)", "").Replace("（Clone）", ""));
    }

    private void ReqNameSyncBoardCast() {
        if(!isServer)
            return;
        RpcNameSyncBoardCast();
    }
    [ClientRpc]
    void RpcNameSyncBoardCast() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
            player.SendMessage("ReqSetName");
    }
}
