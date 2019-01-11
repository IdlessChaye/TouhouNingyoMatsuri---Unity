using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerNetworkCustom : NetworkManager {
    public GameObject[] players;
    public int chosenCharacter = 0;
   
    public class NetworkMessage:MessageBase {
        public int chosenClass;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;
        Transform startPositionsTF = GameObject.Find("StartPositions").transform;
        Transform startPositionTF = startPositionsTF.GetChild(Random.Range(0,startPositionsTF.childCount)).transform;
        GameObject player = Instantiate(players[selectedClass], startPositionTF.position, startPositionTF.rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    public override void OnClientConnect(NetworkConnection conn) {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;
        ClientScene.AddPlayer(conn, 0,test);
    }
    public override void OnClientSceneChanged(NetworkConnection conn) {

    }

    public void SetPlayerReimu() {
        chosenCharacter = 0;
    }
    public void SetPlayerMarisa() {
        chosenCharacter = 1;
    }
    public void SetPlayerAlice() {
        chosenCharacter = 2;
    }
}
