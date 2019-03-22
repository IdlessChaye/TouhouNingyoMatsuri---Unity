using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalView : MonoBehaviour {
    public GameObject thanks;
    public GameObject jiewubiao;
    public GameObject playerName;

    private bool isLocked;
    private int waitForZ;
    private Animator ani;
    // Use this for initialization
    void Start() {
        isLocked = true;
        waitForZ = 0;
        ani = GetComponent<Animator>();
        SetPLayerName();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Z)) {
            if(isLocked == false) {
                LoadSceneOne();
            }

            if(waitForZ == 1) {
                SetSadTrigger();
            }
        }
    }

    public void OnClear() {
        if(FullDataManager.Instance.clearCount == 0)
            UnlockFinalView();
        else
            WaitForZ();
    }

    public void WaitForZ() {
        waitForZ++;
    }

    public void UnlockFinalView() {
        isLocked = false;
    }

    public void SetSadTrigger() {
        waitForZ++;
        FullMusicManager.Instance.Stop();
        ani.SetTrigger("Sad");
    }

    public void LoadSceneOne() {
        FullGameFlowManager.Instance.LoadScene(1);
        FullDataManager.Instance.clearCount++;
    }
    
    private void SetPLayerName() {
        Text playerNameText = playerName.GetComponent<Text>();
        playerNameText.fontSize = (int)Mathf.Clamp((450f / (float)FullDataManager.Instance.playerNameList.Count),3f,45f);
        playerNameText.text = "Thank you again!\n\n";
        foreach(string playerName in FullDataManager.Instance.playerNameList) {
            playerNameText.text += playerName + "\n";
        }
        playerNameText.text +=  "\nAnd\n\n车万众们~" ;
    }

}
