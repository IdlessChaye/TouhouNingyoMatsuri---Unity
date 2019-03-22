using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartConfirmManager : MonoBehaviour {
    GameObject ningyoSpawner;

	// Update is called once per frame
	void Update () {
        if(ningyoSpawner == null) {
            ningyoSpawner = GameObject.FindGameObjectWithTag("NingyoSpawner");
            if(ningyoSpawner == null)
                return;
        }
		if(Input.GetKeyDown(KeyCode.Z)) {
            ningyoSpawner.SendMessage("SetGameStart",true);
            this.gameObject.SetActive(false);
        }
	}
}
