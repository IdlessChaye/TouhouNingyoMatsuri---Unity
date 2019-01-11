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

    public int hasSpawnNingyoCounter;
    private float lastSpawnTime;
	// Use this for initialization
	void Start () {
        hasSpawnNingyoCounter = 0;
        lastSpawnTime = -spawnInterval;
	}
	
	// Update is called once per frame
	void Update () {
        if(!isServer)
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

    public void SpawnNingyoByRelease(string ningyoName,float hp,Vector3 position,Quaternion rotation) {
        foreach(GameObject GO in ningyoDaChi) {
            if(GO.name.Equals(ningyoName)) {
                GameObject ningyo = Instantiate(GO, position, rotation);
                ningyo.name = ningyoName;
                ningyo.GetComponent<NingyoBoomManager>().hp = hp;
                NetworkServer.Spawn(ningyo);
            }
        }
    }
}
