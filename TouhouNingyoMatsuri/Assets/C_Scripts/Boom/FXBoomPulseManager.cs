using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FXBoomPulseManager : NetworkBehaviour {
    public float boomRange = 1f;
    public float boomForce = 50f;

    private bool hasStarted;

    void Update() {
        if(!isServer)
            return;
        if(hasStarted == false) {
            Collider[] colliders;
            Vector3 explosionPosition = gameObject.transform.position;
            colliders = Physics.OverlapSphere(explosionPosition,boomRange);
            foreach(Collider collider in colliders) {
                switch(collider.tag) {
                    case "NingyoUncaptured":
                        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                        if(rb != null) {
                            NingyoAniDestController ningyoAniDestController = collider.gameObject.GetComponent<NingyoAniDestController>();
                            if(ningyoAniDestController != null)
                                ningyoAniDestController.DontWorkForTime(1f);
                            rb.AddExplosionForce(boomForce, explosionPosition, boomRange);
                            NingyoBoomManager ningyoBoomManager = collider.gameObject.GetComponent<NingyoBoomManager>();
                            if(ningyoBoomManager != null)
                                ningyoBoomManager.TakeDamageByBoom();
                        }
                        break;
                    case "NingyoCaptured":
                        RpcGetBoomPulse(collider.gameObject.GetComponent<NetworkIdentity>().netId, explosionPosition);
                        break;
                    case "Player":
                        float shakeRange = 10 - (transform.position - collider.gameObject.transform.position).magnitude;
                        shakeRange = Mathf.Clamp(shakeRange,1f,10f);
                        collider.gameObject.SendMessage("ReqRpcCameraShake", shakeRange);
                        break;
                }
            }
            hasStarted = true;
        }
    }

    [ClientRpc]
    void RpcGetBoomPulse(NetworkInstanceId netId,Vector3 explosionPosition) {
        if(!hasAuthority)
            return;
        GameObject ningyoCaptured = ClientScene.FindLocalObject(netId);
        Rigidbody rb = ningyoCaptured.GetComponent<Rigidbody>();
        if(rb != null) {
            ningyoCaptured.GetComponent<NingyoAniDestController>().DontWorkForTime(1f);
            rb.AddExplosionForce(boomForce, explosionPosition, boomRange);
            ningyoCaptured.GetComponent<NingyoBoomManager>().TakeDamageByBoom();
        }
    }


}
