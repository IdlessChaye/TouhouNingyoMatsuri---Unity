using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NingyoSelfManager : NetworkBehaviour {

    public GameObject ningyoMaster;

    public void SetNingyoMaster(GameObject master) {
        ningyoMaster = master;
    }
}
