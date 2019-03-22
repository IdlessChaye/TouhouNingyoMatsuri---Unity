using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainSceneManager : MonoBehaviour {
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Singleton<ContextManager>.Instance.PeekOrNull() == null) {
                Singleton<ContextManager>.Instance.Push(new ESCContext());
            } else {
                Singleton<ContextManager>.Instance.Pop();
            }
        }
    }

}
