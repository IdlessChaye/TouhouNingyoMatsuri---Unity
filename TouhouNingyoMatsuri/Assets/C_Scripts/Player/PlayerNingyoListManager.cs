using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNingyoListManager : NetworkBehaviour {
    public LinkedList<GameObject> ningyoLinkedList = new LinkedList<GameObject>();

    public void AddLastCapturedNingyo(NetworkInstanceId netId) {
        GameObject ningyo = ClientScene.FindLocalObject(netId);
        if(ningyoLinkedList.Count == 0) {
            ningyo.SendMessage("SetTargetTF", gameObject.transform);
        } else {
            ningyo.SendMessage("SetTargetTF", ningyoLinkedList.Last.Value.transform);
        }
        ningyoLinkedList.AddLast(ningyo);
        ningyo.GetComponent<NingyoSelfManager>().SetNingyoMaster(gameObject);
    }

    public void RemoveCapturedNingyo(NetworkInstanceId netId) {
        GameObject ningyo = ClientScene.FindLocalObject(netId);
        ningyoLinkedList.Remove(ningyo);
        GameObject[] gameObjectArray = new GameObject[ningyoLinkedList.Count];
        ningyoLinkedList.CopyTo(gameObjectArray, 0);
        if(gameObjectArray.Length == 0)
            return;
        gameObjectArray[0].SendMessage("SetTargetTF", gameObject.transform);
        for(int i = 1; i < gameObjectArray.Length; ++i) {
            gameObjectArray[i].SendMessage("SetTargetTF", gameObjectArray[i - 1].transform);
        }
    }

    public void SwitchCapturedNingyo() {
        int ningyoCount = ningyoLinkedList.Count;
        if(ningyoCount <= 1) {
            return;
        }
        GameObject lastNingyo = ningyoLinkedList.Last.Value;
        GameObject firstNingyo = ningyoLinkedList.First.Value;
        GameObject secondNingyo = ningyoLinkedList.First.Next.Value;
        ningyoLinkedList.Remove(firstNingyo);
        ningyoLinkedList.AddLast(firstNingyo);
        secondNingyo.SendMessage("SetTargetTF", gameObject.transform);
        firstNingyo.SendMessage("SetTargetTF", lastNingyo.transform);
        NetworkInstanceId ningyoNetId = firstNingyo.GetComponent<NetworkIdentity>().netId;
        CmdSwitchFirstNingyo(ningyoNetId);
    }
    [Command]
    void CmdSwitchFirstNingyo(NetworkInstanceId ningyoNetId) {
        GameObject ningyo = NetworkServer.FindLocalObject(ningyoNetId);
        GameObject FX_switch = GetComponent<PlayerControllNingyo>().FX_swtich;
        FX_switch = Instantiate(FX_switch, ningyo.transform.position, ningyo.transform.rotation);
        FXBoomFollowManager fxm = FX_switch.GetComponent<FXBoomFollowManager>();
        if(fxm != null) {
            fxm.SetTarget(ningyo);
        }
        NetworkServer.Spawn(FX_switch);
        Destroy(FX_switch, 0.5f);
    }

    public void ReleaseFirstCapturedNingyo() {
        int ningyoCount = ningyoLinkedList.Count;
        if(ningyoCount == 0)
            return;
        GameObject releaseNingyo = ningyoLinkedList.First.Value;
        NetworkInstanceId releaseNingyoNetId = releaseNingyo.GetComponent<NetworkIdentity>().netId;
        RemoveCapturedNingyo(releaseNingyoNetId);
        CmdSpawnNingyo(releaseNingyoNetId);
    }
    [Command]
    void CmdSpawnNingyo(NetworkInstanceId netId) {
        GameObject ningyo = NetworkServer.FindLocalObject(netId);
        string ningyoName = ningyo.name.Replace("Captured", "").Replace("(Clone)","");
        float hp = ningyo.GetComponent<NingyoBoomManager>().hp;
        Vector3 position = ningyo.transform.position;
        Quaternion rotation = ningyo.transform.rotation;
        Destroy(ningyo);
        GameObject ningyoSpawner = GameObject.Find("NingyoSpawner");
        NetworkInstanceId playerNetId = GetComponent<NetworkIdentity>().netId;
        ningyoSpawner.GetComponent<NingyoSpawner>().SpawnNingyoByRelease(ningyoName, hp, position, rotation, playerNetId);
    }

    void PlayerNingyoListWontLoseHP() {
        if(!hasAuthority)
            return;
        foreach(GameObject ningyo in ningyoLinkedList) {
            ningyo.SendMessage("NingyoWontLoseHP");
        }
    }
}
