using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIPlayerManager : NetworkBehaviour { 
    private GameObject sm; // scoreManager

    private NetworkInstanceId playerNetId;
    public int allNingyoCount;
    public int nowNingyoCount;

    private Text textAllNingyoCount;
    private Text textNowNingyoCount;

    void Start() {
        
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

    void Update() {
        if(sm == null) {
            sm = GameObject.FindWithTag("ScoreManager");
            playerNetId = GetComponent<NetworkIdentity>().netId;
            textAllNingyoCount = GameObject.FindWithTag("AllNingyoCount").GetComponent<Text>();
            textNowNingyoCount = GameObject.FindWithTag("NowNingyoCount").GetComponent<Text>();
            if(!hasAuthority)
                return;
        }
        if(!hasAuthority)
            return;
        textAllNingyoCount.text = "已封印人偶数: " + allNingyoCount;
        textNowNingyoCount.text = "现持有人偶数: " + nowNingyoCount;
    }
}
