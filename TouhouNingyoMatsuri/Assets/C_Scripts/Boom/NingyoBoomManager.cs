using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NingyoBoomManager : NetworkBehaviour {
    [SyncVar]
    public float hp;

    [Range(1f, 200f)]
    public float hp_min;
    [Range(100f, 1000f)]
    public float hp_max;

    public float factorHPReduceByTime;
    public float factorHPReduceByBoom;
    public float factorHPReduceByPlayerHit;
    //public float factorHPReduceByPlayerSkill;

    public GameObject FX_WhenStartAlert;
    public GameObject FX_WhenFengkuangAlert;
    public GameObject FX_Boom;

    private float lastTime;
    public bool wontLoseHP;

    void Update() {
        if(!hasAuthority)
            return;
        if(lastTime == 0) {
            bool gameOver = GameObject.Find("MainSceneGameOverManager").GetComponent<MainSceneGameOverManager>().gameOver;
            if(gameOver) {
                NingyoWontLoseHP();
            }
        }
        if(hp == 0 && lastTime == 0) { //游戏开始的情况
            wontLoseHP = false;
            hp = Random.Range(hp_min, hp_max);
            lastTime = Time.time;
        }

        float newhp = hp + factorHPReduceByTime * (lastTime - Time.time);
        if(hp != 0 && lastTime == 0) { // 人偶本地再生的情况
            newhp = hp;
            hp = 100f;
        }

        if(newhp <= 0f) {
            CmdBoom(GetComponent<NetworkIdentity>().netId);
        } else if(newhp <= 20f && hp > 50f) {
            CmdFengkuangAlert(GetComponent<NetworkIdentity>().netId);
        } else if(newhp <= 50f && hp > 50f) {
            CmdStartAlert(GetComponent<NetworkIdentity>().netId);
        } else if(newhp <= 20f && hp > 20f) {
            CmdFengkuangAlert(GetComponent<NetworkIdentity>().netId);
        }

        if(wontLoseHP == false) { 
            hp = newhp;
        } else {
            if(hp != 0 && lastTime == 0) {
                hp = newhp;
            }
        }
        lastTime = Time.time;
    }

    [Command]
    void CmdBoom(NetworkInstanceId netId) {
        GameObject ningyo = NetworkServer.FindLocalObject(netId);
        Vector3 boomPosition = ningyo.transform.position;
        Quaternion boomQuaternion = ningyo.transform.rotation;
        GameObject FX = Instantiate(FX_Boom,boomPosition,boomQuaternion);
        NetworkServer.Spawn(FX);
        Destroy(FX, 0.5f);
        RpcDestroySelf();
    }
    [Command]
    void CmdStartAlert(NetworkInstanceId netId) {
        GameObject ningyo = NetworkServer.FindLocalObject(netId);
        Vector3 boomPosition = ningyo.transform.position;
        Quaternion boomQuaternion = ningyo.transform.rotation;
        GameObject FX = Instantiate(FX_WhenStartAlert, boomPosition, boomQuaternion);
        NetworkServer.Spawn(FX);
        FXBoomFollowManager fxm = FX.GetComponent<FXBoomFollowManager>();
        if(fxm != null) {
            fxm.SetTarget(gameObject);
        }
    }
    [Command]
    void CmdFengkuangAlert(NetworkInstanceId netId) {
        GameObject ningyo = NetworkServer.FindLocalObject(netId);
        Vector3 boomPosition = ningyo.transform.position;
        Quaternion boomQuaternion = ningyo.transform.rotation;
        GameObject FX = Instantiate(FX_WhenFengkuangAlert, boomPosition, boomQuaternion);
        NetworkServer.Spawn(FX);
        FXBoomFollowManager fxm = FX.GetComponent<FXBoomFollowManager>();
        if(fxm != null) {
            fxm.SetTarget(gameObject);
        }
    }
    [ClientRpc]
    void RpcDestroySelf() {
        if(hasAuthority) { 
            if(GetComponent<NingyoSelfManager>() != null) { 
                GetComponent<NingyoSelfManager>().ningyoMaster.GetComponent<PlayerNingyoListManager>().RemoveCapturedNingyo(GetComponent<NetworkIdentity>().netId);
                GetComponent<NingyoSelfManager>().ningyoMaster.GetComponent<UIPlayerManager>().ReqMinusNowNingyoCount();
            }
        }
        CmdDestroy();
    }
    [Command]
    void CmdDestroy() {
        Destroy(gameObject);
    }

    // Boom
    public void TakeDamageByBoom() {
        if(!hasAuthority)
            return;
        hp = hp - factorHPReduceByBoom;
    }

    // PlayerHit
    void TakeDamageByPlayerHit(float damage) {
        if(!hasAuthority)
            return;
        hp = hp - damage;
    }

    public float GetNingyoHP() {
        return hp;
    }
    public void SetNingyoHP(float newhp) {
        if(!hasAuthority)
            return;
        hp = newhp;
        CmdSetNingyoHP(newhp);
    }
    [Command]
    void CmdSetNingyoHP(float newhp) {
        if(!hasAuthority)
            return;
        hp = newhp;
    }

    void NingyoWontLoseHP() {
        if(!hasAuthority)
            return;
        wontLoseHP = true;
    }
}
