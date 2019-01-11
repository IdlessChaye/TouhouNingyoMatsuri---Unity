//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;


public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject target;
    public float viewMoveSpeed;
    public float smooth = 3f;		// カメラモーションのスムーズ化用変数

    private SphericalVector sphericalVector = new SphericalVector(5f, 1f, 0.3f);
    Transform standardPos;			// the usual position for the camera, specified by a transform in the game
	Transform frontPos;			// Front Camera locater
	Transform jumpPos;          // Jump Camera locater
    GameObject lookAtGO;
    // スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
    bool bQuickSwitch = false;	//Change Camera Position Quickly
	

	
	void FixedUpdate ()	// このカメラ切り替えはFixedUpdate()内でないと正常に動かない
	{
        if(standardPos == null)
            return;

        if(Input.GetButton("Fire1"))	// left Ctlr
		{
            // Change Front Camera
            setCameraPositionFrontView();
		}else if(Input.GetButton("Fire2"))	//Alt
		{
            // Change Jump Camera
            setCameraPositionJumpView();
		}else 
		{
            float h = Input.GetAxis("Mouse X");//水平视角移动)
            float v = Input.GetAxis("Mouse Y");//垂直视角移动
            sphericalVector.azimuth += h * viewMoveSpeed;
            sphericalVector.zenith -= v * viewMoveSpeed;
            sphericalVector.zenith = Mathf.Clamp(sphericalVector.zenith, 0f, 1f);
            float s = Input.GetAxis("Mouse ScrollWheel");//滚轮拉近视角
            sphericalVector.length -= s * 10f;
            sphericalVector.length = Mathf.Clamp(sphericalVector.length, 2f, 25f);
            transform.position = standardPos.position + sphericalVector.Position;//设定摄像机位置
            transform.LookAt(target.transform);//摄像机视角
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
        target = go;
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
