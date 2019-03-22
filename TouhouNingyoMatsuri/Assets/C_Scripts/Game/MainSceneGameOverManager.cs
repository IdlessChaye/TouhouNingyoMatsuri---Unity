using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainSceneGameOverManager : NetworkBehaviour {
    public bool gameOver;

    private NingyoSpawner ningyoSpawner;
    private float lastTime;

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
        if(maxNingyoCount == hasSpawnNingyoCounter && gameOver == false) {
            if(Time.time - lastTime > 1f) {
                GameObject ningyoUncaptured = GameObject.FindWithTag("NingyoUncaptured");
                if(ningyoUncaptured == null) {
                    gameOver = true;
                    RpcSetGameOverThings();
                    TellServerNingyoWontLoseHP();
                }
                lastTime = Time.time;
            }
        }
    }
    [ClientRpc]
    void RpcSetGameOverThings() {
        GameObject fullGameFlowManager = GameObject.Find("FullGameFlowManager");
        if(fullGameFlowManager == null)
            throw new System.Exception("Cant Find fullGameFlowManager!");
        fullGameFlowManager.SendMessage("GameOverTrigger");
        TellPlayerNingyoListWontLoseHP();
    }
    void TellPlayerNingyoListWontLoseHP() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
            player.SendMessage("PlayerNingyoListWontLoseHP");
    }
    void TellServerNingyoWontLoseHP() {
        if(!isServer)
            return;
        GameObject[] ningyos = GameObject.FindGameObjectsWithTag("NingyoUncaptured");
        foreach(GameObject ningyo in ningyos)
            ningyo.SendMessage("NingyoWontLoseHP");
    }
}
