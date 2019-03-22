using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum NetworkType {
    None,
    Host,
    Client
}


public class FullDataManager : FullSingleton<FullDataManager> {
    public NetworkType networkType;
    public string name;
    public string ipAddress;
    public string ningyoCount;
    public int chosenCharacter;

    public int allScore;
    public int nowScore;

    public List<string> playerNameList = new List<string>();

    public int clearCount; // 通关次数


    public override void Initial() {
        this.gameObject.name = "FullDataManager";
        networkType = NetworkType.Host;
        name = "Alice";
        ipAddress = "127.0.0.1";
        ningyoCount = "30";
        chosenCharacter = 0;
        allScore = 100;
        nowScore = 200;
        clearCount = 0;
    }

    public void AddPlayerName(string playerName) {
        if(playerNameList.Contains(playerName))
            return;
        playerNameList.Add(playerName);
    }

}
