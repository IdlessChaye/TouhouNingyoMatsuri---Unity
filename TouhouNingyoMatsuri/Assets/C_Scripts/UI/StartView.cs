using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartView : MonoBehaviour {

	public void MenuSceneCallBack() {
        SceneManager.LoadScene(1);
    }
}
