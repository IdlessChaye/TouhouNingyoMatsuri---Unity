using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetWorkTransformSync : NetworkBehaviour {
    [SyncVar]//当同步变量有变化时，服务器会自动发送他们的最新数据。不需要手工为同步变量设置任何的脏数据标志位。  
    private Vector3 v3_PlayerPos;

    [SyncVar]
    private Quaternion qua_PlayerRotate;

    [Command]
    public void CmdSendServerPos(Vector3 pos, Quaternion rotate) { //向服务端发送坐标并同步到客户端 
        v3_PlayerPos = pos;
        qua_PlayerRotate = rotate;
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (!hasAuthority) {
            transform.position = Vector3.Lerp(transform.position, v3_PlayerPos, 5 * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, qua_PlayerRotate, 5 * Time.fixedDeltaTime);
            return;
        }else {
            if(transform.position == Vector3.zero)
                return;
            if(isServer) {
                v3_PlayerPos = transform.position;
                qua_PlayerRotate = transform.rotation;
            }else if(isClient && !isServer){
                CmdSendServerPos(transform.position, transform.rotation);
            }
        }
    }
}
