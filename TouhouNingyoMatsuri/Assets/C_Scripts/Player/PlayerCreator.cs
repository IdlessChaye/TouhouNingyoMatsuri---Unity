using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCreator : NetworkBehaviour {
    public GameObject[] players;
    

    [SyncVar] private uint realPlayerNetId;
    private GameObject realPlayer;
    private ScoreManager sm;

    
    public override void OnStartServer() {
        sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        int playerNum = sm.playerDictionary.Count;
        GameObject player = players[playerNum];
        player = Instantiate(player, transform.position, transform.rotation);
        NetworkServer.SpawnWithClientAuthority(player, connectionToClient);
        NetworkInstanceId playerNetId = player.GetComponent<NetworkIdentity>().netId;
        realPlayerNetId = playerNetId.Value;
        sm.SendMessage("AddPlayerWithNoNingyo", playerNetId);
    }
    
    void Update() {
        if(realPlayer == null) {
            realPlayer = ClientScene.FindLocalObject( new NetworkInstanceId(realPlayerNetId));
        }
    }

    

}
