using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NingyoSpawner : NetworkBehaviour {
    public GameObject[] ningyoDaChi;
    public int maxNingyoCount;
    public float spawnInterval;
    public bool randomSpawnNingyo;
    public Transform[] whereToSpawn;
    public bool gameStart = false;

    public int hasSpawnNingyoCounter;
    private float lastSpawnTime;
	// Use this for initialization
	void Start () {
        if(!isServer)
            return;
        hasSpawnNingyoCounter = 0;
        lastSpawnTime = -spawnInterval;
        maxNingyoCount = int.Parse(FullDataManager.Instance.ningyoCount);
    }
	
	// Update is called once per frame
	void Update () {
        if(!isServer)
            return;
        if(!gameStart)
            return;
        if(hasSpawnNingyoCounter == maxNingyoCount)
            return;
        if(Time.time - lastSpawnTime > spawnInterval) {
            Vector3 spawnPosition;
            Quaternion spawnRotation = Quaternion.identity;
            if(randomSpawnNingyo) { 
                float xPosition = Random.Range(-20, 20);
                float zPosition = Random.Range(-20, 20);
                Ray ray = new Ray(new Vector3(xPosition, 20f, zPosition), -Vector3.up);
                Vector3 groundPoint = Vector3.zero;
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
                if(hits.Length != 0) {
                    for(int i = 0; i < hits.Length; ++i) {
                        if(hits[i].collider.tag == "AliceHouse") {
                            groundPoint = hits[i].point;
                            break;
                        }
                    }
                }
                spawnPosition = new Vector3(xPosition, Random.Range(1f, 2f) + groundPoint.y, zPosition);
            }else {
                Transform spawnTF = whereToSpawn[Random.Range(0, whereToSpawn.Length)];
                spawnPosition = spawnTF.position;
                spawnRotation = spawnTF.rotation;
            }
            GameObject ningyoKind = ningyoDaChi[Random.Range(0, ningyoDaChi.Length)];
            hasSpawnNingyoCounter = hasSpawnNingyoCounter + 1;
            GameObject ningyo = Instantiate(ningyoKind, spawnPosition, spawnRotation) as GameObject;
            ningyo.name = ningyoKind.name;
            NetworkServer.Spawn(ningyo);
            lastSpawnTime = Time.time;
        }
	}

    private void SetGameStart(bool isStart) {
        if(!isServer)
            return;
        gameStart = isStart;
        RpcGameStart();
    }
    [ClientRpc]
    private void RpcGameStart() {
        GameObject fullGameFlowManager = GameObject.Find("FullGameFlowManager");
        if(fullGameFlowManager == null)
            throw new System.Exception("Cant Find fullGameFlowManager!");
        fullGameFlowManager.SendMessage("GameStartTrigger");
    }

    private void GetMaxNingyoCount(NetworkInstanceId playerNetId) {
        if(!isServer)
            return;
        RpcSetMaxNingyoCount(playerNetId,maxNingyoCount);
    }
    [ClientRpc]
    private void RpcSetMaxNingyoCount(NetworkInstanceId playerNetId,int maxNingyoCount) {
        GameObject player = ClientScene.FindLocalObject(playerNetId);
        player.SendMessage("AnsSetMaxNingyoCount", maxNingyoCount);
    }

    private void GetRemainNingyoCount(NetworkInstanceId playerNetId) {
        if(!isServer)
            return;
        GameObject[] ningyoUncaptureds = GameObject.FindGameObjectsWithTag("NingyoUncaptured");
        int remainNingyoCount = maxNingyoCount - hasSpawnNingyoCounter + (ningyoUncaptureds == null?0:ningyoUncaptureds.Length);
        RpcSetRemainNingyoCount(playerNetId, remainNingyoCount);
    }
    [ClientRpc]
    private void RpcSetRemainNingyoCount(NetworkInstanceId playerNetId,int remainNingyoCount) {
        GameObject player = ClientScene.FindLocalObject(playerNetId);
        player.SendMessage("AnsSetRemainNingyoCount", remainNingyoCount);
    }

    public void SpawnNingyoByRelease(string ningyoName,float hp,Vector3 position,Quaternion rotation,NetworkInstanceId playerNetId) {
        foreach(GameObject GO in ningyoDaChi) {
            if(GO.name.Equals(ningyoName)) {
                GameObject ningyo = Instantiate(GO, position, rotation);
                ningyo.name = ningyoName;
                ningyo.GetComponent<NingyoBoomManager>().hp = hp;
                GameObject player = NetworkServer.FindLocalObject(playerNetId);
                GameObject FX_release = player.GetComponent<PlayerControllNingyo>().FX_release;
                FX_release = Instantiate(FX_release, position, rotation);
                FXBoomFollowManager fxm = FX_release.GetComponent<FXBoomFollowManager>();
                if(fxm != null) {
                    fxm.SetTarget(ningyo);
                }
                NetworkServer.Spawn(FX_release);
                Destroy(FX_release, 1f);
                NetworkServer.Spawn(ningyo);
            }
        }
    }
}
