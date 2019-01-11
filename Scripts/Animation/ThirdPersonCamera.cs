//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;


public class ThirdPersonCamera : MonoBehaviour
{
	public float smooth = 3f;		// カメラモーションのスムーズ化用変数
	Transform standardPos;			// the usual position for the camera, specified by a transform in the game
	Transform frontPos;			// Front Camera locater
	Transform jumpPos;          // Jump Camera locater
    GameObject lookAtGO;
    // スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
    bool bQuickSwitch = false;	//Change Camera Position Quickly
	

	
	void FixedUpdate ()	// このカメラ切り替えはFixedUpdate()内でないと正常に動かない
	{
        if (standardPos == null)
            return;
        //Transform playerTF = standardPos.parent;
        //RaycastHit hitInfo;
        //if(Physics.SphereCast(playerTF.position,0.2f,-playerTF.forward,out hitInfo,(transform.position-playerTF.position).magnitude,LayerMask.GetMask("Wall"))) {
        //    transform.position = Vector3.Lerp(transform.position, new Vector3(hitInfo.point.x + hitInfo.normal.x * 0.1f, transform.position.y, hitInfo.point.z + hitInfo.normal.z * 0.1f), Time.fixedDeltaTime * smooth / 5);
        //    lookAtGO.transform.LookAt(playerTF);
        //    transform.forward = Vector3.Lerp(transform.forward, lookAtGO.transform.forward, Time.fixedDeltaTime * smooth);
        //    return;
        //}
		if(Input.GetButton("Fire1"))	// left Ctlr
		{	
			// Change Front Camera
			setCameraPositionFrontView();
		}
		
		else if(Input.GetButton("Fire2"))	//Alt
		{	
			// Change Jump Camera
			setCameraPositionJumpView();
		}
		
		else
		{	
			// return the camera to standard position and direction
			setCameraPositionNormalView();
		}
	}

	void setCameraPositionNormalView()
	{
		if(bQuickSwitch == false){
		// the camera to standard position and direction
		    transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.fixedDeltaTime * smooth);	
		    transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
		}
		else{
			// the camera to standard position and direction / Quick Change
			transform.position = standardPos.position;	
			transform.forward = standardPos.forward;
			bQuickSwitch = false;
		}
	}

	
	void setCameraPositionFrontView()
	{
		// Change Front Camera
		bQuickSwitch = true;
		transform.position = frontPos.position;	
		transform.forward = frontPos.forward;
	}

	void setCameraPositionJumpView()
	{
		// Change Jump Camera
		bQuickSwitch = false;
		transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);	
		transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);		
	}

    void SetStandardPos(GameObject go)
    {
        standardPos = go.transform.GetChild(2).transform;
        frontPos = go.transform.GetChild(3).transform;
        jumpPos = go.transform.GetChild(4).transform;
        //lookAtGO = new GameObject("LookAt");
    }

    void OnDestroy() {
        if(lookAtGO!=null)
            Destroy(lookAtGO);
    }
}
