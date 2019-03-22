using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FXBoomFollowManager : NetworkBehaviour {
    private Transform targetTF;

    public void SetTarget(GameObject ningyo) {
        targetTF = ningyo.transform;
        transform.parent = targetTF;
    }
}
