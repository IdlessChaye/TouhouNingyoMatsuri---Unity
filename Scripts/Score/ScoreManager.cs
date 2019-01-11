using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {
    
    public Dictionary<NetworkInstanceId, List<int>> playerDictionary = new Dictionary<NetworkInstanceId, List<int>>();
    public GameObject defaultPlayer;
    void SetDefaultPlayer(GameObject go) {
        defaultPlayer = go;
    }

    void AddPlayerWithNoNingyo(NetworkInstanceId netId) {
        playerDictionary.Add(netId, new List<int> { 0, 0 });
        
    }

    void AddAllNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(!isServer)
            return;
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            playerDictionary[clientPlayerNetId][0]++;
            RpcAnsSetAllNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][0]);
        } else {
            playerDictionary.Add(clientPlayerNetId, new List<int> { 1, 0 }); // 先all，后now
            RpcAnsSetAllNingyoCount(clientPlayerNetId, 1);
        }
        
    }
    void MinusAllNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(!isServer)
            return;
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            if(playerDictionary[clientPlayerNetId][0] > 0 && playerDictionary[clientPlayerNetId][1] > 0)
                playerDictionary[clientPlayerNetId][0]--;
            RpcAnsSetAllNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][0]);
        } else {
            print("BugHere");
            playerDictionary.Add(clientPlayerNetId, new List<int> { 0, 0 }); // 先all，后now
            RpcAnsSetAllNingyoCount(clientPlayerNetId, 0);
        }
        
    }
    [ClientRpc]
    void RpcAnsSetAllNingyoCount(NetworkInstanceId clientPlayerNetId,int count) {
        GameObject player = ClientScene.FindLocalObject(clientPlayerNetId);
        player.SendMessage("AnsSetAllNingyoCount", count);
    }

    void AddNowNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            playerDictionary[clientPlayerNetId][1]++;
            RpcAnsSetNowNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][1]);
        } else {
            playerDictionary.Add(clientPlayerNetId, new List<int> { 0, 1 }); // 先all，后now
            RpcAnsSetNowNingyoCount(clientPlayerNetId, 1);
        }
        
    }
    void MinusNowNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            if(playerDictionary[clientPlayerNetId][1] > 0)
                playerDictionary[clientPlayerNetId][1]--;
            RpcAnsSetNowNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][1]);
        } else {
            print("BugHere");
            playerDictionary.Add(clientPlayerNetId, new List<int> { 0, 0 }); // 先all，后now
            RpcAnsSetNowNingyoCount(clientPlayerNetId, 0);
        }
        
    }
    [ClientRpc]
    void RpcAnsSetNowNingyoCount(NetworkInstanceId clientPlayerNetId, int count) {
        GameObject player = ClientScene.FindLocalObject(clientPlayerNetId);
        player.SendMessage("AnsSetNowNingyoCount", count);
    }

    void GetPlayerDictionary(NetworkInstanceId clientPlayerNetId) {
        int[] lists = new int[playerDictionary.Count*3];
        int i = 0;
        foreach(NetworkInstanceId netId in playerDictionary.Keys) {
            List<int> listInt = playerDictionary[netId];
            lists[i] = (int)netId.Value;
            i++;
            lists[i] = listInt[0];
            i++;
            lists[i] = listInt[1];
            i++;
        }
        RpcSetPlayerDictionary(clientPlayerNetId,lists);
    }
    [ClientRpc]
    void RpcSetPlayerDictionary(NetworkInstanceId clientPlayerNetId,int[] lists) {
        GameObject player = ClientScene.FindLocalObject(clientPlayerNetId);
        player.SendMessage("AnsSetPlayerDictionary", lists);
    }


    private NingyoSpawner ningyoSpawner;
    private float lastTime;
    public bool gameOver;
    void Start() {
        if(!isServer)
            return;
        ningyoSpawner = GameObject.Find("NingyoSpawner").GetComponent<NingyoSpawner>();
    }
    void Update() {
        if(!isServer)
            return;
        int hasSpawnNingyoCounter = ningyoSpawner.hasSpawnNingyoCounter;
        int maxNingyoCount = ningyoSpawner.maxNingyoCount;
        if(maxNingyoCount == hasSpawnNingyoCounter) {
            if(Time.time - lastTime > 1f) {
                GameObject ningyoUncaptured = GameObject.FindWithTag("NingyoUncaptured");
                if(ningyoUncaptured == null) {
                    gameOver = true;
                    RpcSetGameOverText();
                } else {
                    gameOver = false;
                }
                lastTime = Time.time;
            }
        }
    }
    [ClientRpc]
    void RpcSetGameOverText() {
        GameObject UIGameManager = GameObject.Find("UIGameManager");
        UIGameManager.GetComponent<UIGameManager>().EnableGameOverText();
    }


}
