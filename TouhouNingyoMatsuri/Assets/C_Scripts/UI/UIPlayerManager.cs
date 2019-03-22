using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIPlayerManager : NetworkBehaviour {
    Dictionary<NetworkInstanceId, List<int>> playerDictionary;
    private GameObject sm; // scoreManager
    private GameObject spawnerm; // ningyoSpawner
    private GameObject networkDatasync;
    private NetworkInstanceId playerNetId;

    private int allNingyoCount;
    private int nowNingyoCount;
    private Text textAllNingyoCount;
    private Text textNowNingyoCount;

    private int maxNingyoCount;
    private int remainNingyoCount;
    private Slider sliderRemainNingyo;
    private Text textRemainToAll;

    private float lastTime;
    private int textNum = 1;

    public int[] playerList;

    void Start() {
        if(!hasAuthority)
            return;
        FullDataManager.Instance.AddPlayerName(FullDataManager.Instance.name);
    }

    public void ReqAddPlayerWithNoNingyo() {
        if(!hasAuthority)
            return;
        CmdAddPlayerWithNoNingyo(playerNetId);
    }
    [Command]
    void CmdAddPlayerWithNoNingyo(NetworkInstanceId playerNetId) {
        sm.SendMessage("AddPlayerWithNoNingyo", playerNetId);
    }

    public void ReqAddAllNingyoCount() { // 向scoreManager的请求
        if(!hasAuthority)
            return;
        CmdReqAddAllNingyoCount();
    }
    [Command]
    void CmdReqAddAllNingyoCount() {
        sm.SendMessage("AddAllNingyoCount", playerNetId);
    }
    public void ReqMinusAllNingyoCount() { // 向scoreManager的请求
        if(!hasAuthority)
            return;
        CmdReqMinusAllNingyoCount();
    }
    [Command]
    void CmdReqMinusAllNingyoCount() {
        sm.SendMessage("MinusAllNingyoCount", playerNetId);
    }
    public void AnsSetAllNingyoCount(int count) { // scoreManager响应
        if(!hasAuthority)
            return;
        allNingyoCount = count;
    }

    public void ReqAddNowNingyoCount() {
        if(!hasAuthority)
            return;
        CmdReqAddNowNingyoCount();
    }
    [Command]
    void CmdReqAddNowNingyoCount() {
        sm.SendMessage("AddNowNingyoCount", playerNetId);
    }
    public void ReqMinusNowNingyoCount() {
        if(!hasAuthority)
            return;
        CmdReqMinusNowNingyoCount();
    }
    [Command]
    void CmdReqMinusNowNingyoCount() { 
        sm.SendMessage("MinusNowNingyoCount", playerNetId);
    }
    public void AnsSetNowNingyoCount(int count) {
        if(!hasAuthority)
            return;
        nowNingyoCount = count;
    }

    public void ReqGetPlayerDictionary() {
        if(!hasAuthority)
            return;
        CmdReqGetPlayerDictionary();
    }
    [Command]
    void CmdReqGetPlayerDictionary() {
        sm.SendMessage("GetPlayerDictionary",playerNetId);
    }
    public void AnsSetPlayerDictionary(int[] lists) {
        if(!hasAuthority)
            return;
        playerList = lists;
        ShowAllPlayerScore(lists);
    }
    void ShowAllPlayerScore(int[] lists) {
        int playerNum = lists.Length / 3;
        for(int i= 0;i<playerNum;i++) {
            int netIdInt = lists[i * 3];
            if(netIdInt == playerNetId.Value)
                continue;
            GameObject textGO;
            GameObject scorePanel;
            float posY;
            RectTransform rectTF;
            if(GameObject.Find(netIdInt.ToString() + " - AllNingyoCount") == null) {
                scorePanel = GameObject.Find("ScorePanel");
                posY = 22.8f + 60f * textNum;//posY = -86f + 60f * textNum;
                textNum++;

                textGO = Instantiate(Resources.Load("Text - AllNingyoCount") as GameObject);
                textGO.transform.parent = scorePanel.transform;
                rectTF = textGO.GetComponent<Text>().GetComponent<RectTransform>();
                rectTF.anchoredPosition3D = new Vector3(100f, posY - 35f, 0f);
                textGO.name = netIdInt.ToString()+" - AllNingyoCount";

                textGO = Instantiate(Resources.Load("Text - AllNingyoCount") as GameObject);
                textGO.transform.parent = scorePanel.transform;
                rectTF = textGO.GetComponent<Text>().GetComponent<RectTransform>();
                rectTF.anchoredPosition3D = new Vector3(100f, posY - 60f, 0f);
                textGO.name = netIdInt.ToString() + " - NowNingyoCount";
            }
            string playerName = ClientScene.FindLocalObject(new NetworkInstanceId((uint)netIdInt)).name;
            int allNingyoCount = lists[i * 3 + 1];
            int nowNingyoCount = lists[i * 3 + 2];
            textGO = GameObject.Find(netIdInt.ToString() + " - AllNingyoCount");
            textGO.GetComponent<Text>().text = playerName + "现持有人偶数 : " + nowNingyoCount;
            textGO = GameObject.Find(netIdInt.ToString() + " - NowNingyoCount");
            textGO.GetComponent<Text>().text = playerName + "已封印人偶数 : " + allNingyoCount;
        }
    }

    public void ReqGetMaxNingyoCount() {
        if(!hasAuthority)
            return;
        CmdGetMaxNingyoCount();
    }
    [Command]
    private void CmdGetMaxNingyoCount() {
        spawnerm.SendMessage("GetMaxNingyoCount",playerNetId);
    }
    public void AnsSetMaxNingyoCount(int maxNingyoCount) {
        if(!hasAuthority)
            return;
        this.maxNingyoCount = maxNingyoCount;
    }

    public void ReqGetRemainNingyoCount() {
        if(!hasAuthority)
            return;
        CmdGetRemainNingyoCount();
    }
    [Command]
    private void CmdGetRemainNingyoCount() {
        spawnerm.SendMessage("GetRemainNingyoCount",playerNetId);
    }
    public void AnsSetRemainNingyoCount(int remainNingyoCount) {
        if(!hasAuthority)
            return;
        this.remainNingyoCount = remainNingyoCount;
    }

    void Update() {
        if(sm == null) {
            sm = GameObject.FindWithTag("ScoreManager");
            spawnerm = GameObject.FindWithTag("NingyoSpawner");
            networkDatasync = GameObject.FindWithTag("NetWorkDatasync");
            playerNetId = GetComponent<NetworkIdentity>().netId;
            textAllNingyoCount = GameObject.Find("Text - AllNingyoCount").GetComponent<Text>();
            textNowNingyoCount = GameObject.Find("Text - NowNingyoCount").GetComponent<Text>();
            sliderRemainNingyo = GameObject.Find("Slider - RemainNingyoCount").GetComponent<Slider>();
            textRemainToAll = GameObject.Find("RemainToAll").GetComponent<Text>();
            if(!hasAuthority)
                return;
            ReqAddPlayerWithNoNingyo();
            ReqGetMaxNingyoCount();
            ReqNameSyncBoardCast();
        }
        if(!hasAuthority)
            return;
        if(Time.time - lastTime > 0.7f) {
            ReqGetPlayerDictionary();
            ReqGetRemainNingyoCount();
            lastTime = Time.time;

            textAllNingyoCount.text = "已封印人偶数 : " + allNingyoCount;
            textNowNingyoCount.text = "现持有人偶数 : " + nowNingyoCount;
            sliderRemainNingyo.maxValue = maxNingyoCount;
            sliderRemainNingyo.value = remainNingyoCount;
            textRemainToAll.text = remainNingyoCount.ToString()+" / "+maxNingyoCount.ToString();
        }
    }

    public void ReqSetName() {
        if(!hasAuthority)
            return;
        CmdSetName(FullDataManager.Instance.name);
    }
    [Command]
    void CmdSetName(string name) {
        if(!isServer)
            return;
        networkDatasync.GetComponent<NetWorkDataSync>().SetName(name, playerNetId);
    }

    public void ReqNameSyncBoardCast() {
        if(!hasAuthority)
            return;
        CmdReqNameSyncBoardCast();
    }
    [Command]
    void CmdReqNameSyncBoardCast() {
        if(!isServer)
            return;
        networkDatasync.SendMessage("ReqNameSyncBoardCast");
    }

    public void GetPlayerList() {
        if(!hasAuthority)
            return;
        FinalScoreTableBuilder builder = GameObject.FindObjectOfType(typeof(FinalScoreTableBuilder)) as FinalScoreTableBuilder;
        builder.SetPlayerList(playerList);
    }
}
