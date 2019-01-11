using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIPlayerManager : NetworkBehaviour {
    Dictionary<NetworkInstanceId, List<int>> playerDictionary;
    private GameObject sm; // scoreManager

    private NetworkInstanceId playerNetId;
    private int allNingyoCount;
    private int nowNingyoCount;

    private Text textAllNingyoCount;
    private Text textNowNingyoCount;

    private float lastTime;
    private int textNum = 1;

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
            Rect rect;
            if(GameObject.Find(netIdInt.ToString() + " - AllNingyoCount") == null) {
                scorePanel = GameObject.Find("ScorePanel");
                posY = -86f + 60f * textNum;
                textNum++;
                //scorePanel.transform.GetChild(0).gameObject.GetComponent<Text>().GetComponent<RectTransform>().rect.y;
                //for(int j = 1; j< scorePanel.transform.childCount;j++) {
                //    posY = Mathf.Min(posY, scorePanel.transform.GetChild(j).gameObject.GetComponent<Text>().GetComponent<RectTransform>().rect.y);
                //}
                textGO = Instantiate(Resources.Load("Text - AllNingyoCount") as GameObject);
                textGO.transform.parent = scorePanel.transform;
                rectTF = textGO.GetComponent<Text>().GetComponent<RectTransform>();
                rect = rectTF.rect;
                rectTF.anchoredPosition3D = new Vector3(33f, posY - 30f, 0f);
                textGO.name = netIdInt.ToString()+" - AllNingyoCount";
                textGO = Instantiate(Resources.Load("Text - AllNingyoCount") as GameObject);
                textGO.transform.parent = scorePanel.transform;
                rectTF = textGO.GetComponent<Text>().GetComponent<RectTransform>();
                rect = rectTF.rect;
                rectTF.anchoredPosition3D = new Vector3(33f, posY - 60f, 0f);
                textGO.name = netIdInt.ToString() + " - NowNingyoCount";
            }
            string playerName = ClientScene.FindLocalObject(new NetworkInstanceId((uint)netIdInt)).name.Replace("(Clone)","");
            int allNingyoCount = lists[i * 3 + 1];
            int nowNingyoCount = lists[i * 3 + 2];
            textGO = GameObject.Find(netIdInt.ToString() + " - AllNingyoCount");
            textGO.GetComponent<Text>().text = playerName + "现持有人偶数: " + nowNingyoCount;
            textGO = GameObject.Find(netIdInt.ToString() + " - NowNingyoCount");
            textGO.GetComponent<Text>().text = playerName + "已封印人偶数: " + allNingyoCount;
        }
    }

    void Update() {
        if(sm == null) {
            sm = GameObject.FindWithTag("ScoreManager");
            sm.SendMessage("SetDefaultPlayer", gameObject);
            playerNetId = GetComponent<NetworkIdentity>().netId;
            textAllNingyoCount = GameObject.Find("Text - AllNingyoCount").GetComponent<Text>();
            textNowNingyoCount = GameObject.Find("Text - NowNingyoCount").GetComponent<Text>();
            if(!hasAuthority)
                return;
        }
        if(!hasAuthority)
            return;
        if(Time.time - lastTime > 1f) {
            ReqGetPlayerDictionary();
            lastTime = Time.time;
        }
        textAllNingyoCount.text = "已封印人偶数: " + allNingyoCount;
        textNowNingyoCount.text = "现持有人偶数: " + nowNingyoCount;
    }
}
