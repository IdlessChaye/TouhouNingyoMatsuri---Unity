using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFullManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FullGameFlowManager.Instance.Initial();
        FullDataManager.Instance.Initial();
        FullMusicManager.Instance.Initial();
    }

}
