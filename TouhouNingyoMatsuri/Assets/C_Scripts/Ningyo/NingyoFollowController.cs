using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NingyoFollowController : NetworkBehaviour {
    public float destinationSetInterval = 10f;

    private float lastDestSetTime;
    private NingyoAniDestController nadc;
    private GameObject GO_destinationNavigator;
    void Start() {
        if(hasAuthority) { 
            lastDestSetTime = -destinationSetInterval;
            nadc = GetComponent<NingyoAniDestController>();
            GO_destinationNavigator = new GameObject("NingyoDestinationNavigator");
            GO_destinationNavigator.transform.position = new Vector3(Random.Range(-20,20),5f, Random.Range(-20, 20));
            //print("NAVI"+GO_destinationNavigator.transform.position);
            nadc.targetTF = GO_destinationNavigator.transform;
        }
    }

    void Update() {
        if(hasAuthority) {
            if(Time.time - lastDestSetTime > destinationSetInterval) {
                GO_destinationNavigator.transform.position = new Vector3(Random.Range(-20, 20), 5f, Random.Range(-20, 20));
                lastDestSetTime = Time.time;
            }
        }
    }

    void OnDestroy() {
        Destroy(GO_destinationNavigator);
    }
}
