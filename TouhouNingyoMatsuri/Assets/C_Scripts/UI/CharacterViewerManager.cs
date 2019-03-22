using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterViewerManager : MonoBehaviour {
    public Text characterName;

    private List<GameObject> charaList = new List<GameObject>();

    private int nowIndex;
    private GameObject nowShowingNode;

    private bool isLocked; // 解决快速双击UI导致此系统逻辑错误的Bug
    private float lastLockedTime;
    private float lockedTimeLong = 0.6f; // 锁1s

    void Start() {
        isLocked = false;
        lastLockedTime = -lockedTimeLong;

        foreach(Transform tf in transform) {
            if(tf.GetComponent<Animator>() != null) {
                charaList.Add(tf.gameObject);
            }
        }

        charaList[0].GetComponent<Animator>().SetTrigger("OnExitLeft");
        for(int i=2;i<charaList.Count;i++)
            charaList[i].GetComponent<Animator>().SetTrigger("OnExitRight");

        foreach(GameObject go in charaList)
            go.SetActive(false);

        SetNowIndex(1);
        nowShowingNode = charaList[nowIndex];
        nowShowingNode.SetActive(true);
        SetCharacterName();
    }

    public void RightSwitchCallBack() {
        if(Locked())
            return;
        int nextIndex = nowIndex - 1;
        if(nextIndex < 0)
            return;
        SetCenterPosition(charaList[nowIndex].GetComponent<RectTransform>());
        charaList[nowIndex].GetComponent<Animator>().SetTrigger("OnExitRight");
        GameObject nextShowingGO = charaList[nextIndex];
        nextShowingGO.SetActive(true);
        SetLeftPosition(nextShowingGO.GetComponent<RectTransform>());
        nextShowingGO.GetComponent<Animator>().SetTrigger("OnEnterLeft");
        SetNowIndex(nextIndex);
        SetCharacterName();
    }

    public void LeftSwitchCallBack() {
        if(Locked())
            return;
        int nextIndex = nowIndex + 1;
        if(nextIndex >= charaList.Count)
            return;
        SetCenterPosition(charaList[nowIndex].GetComponent<RectTransform>());
        charaList[nowIndex].GetComponent<Animator>().SetTrigger("OnExitLeft");
        GameObject nextShowingGO = charaList[nextIndex];
        nextShowingGO.SetActive(true);
        SetRightPosition(nextShowingGO.GetComponent<RectTransform>());
        nextShowingGO.GetComponent<Animator>().SetTrigger("OnEnterRight");
        SetNowIndex(nextIndex);
        SetCharacterName();
    }

    private void SetLeftPosition(RectTransform rectTF) {
        rectTF.anchoredPosition = new Vector2(-153.4f, -67.83f);
        rectTF.sizeDelta = new Vector2(97.5f,130f); 
    }

    private void SetCenterPosition(RectTransform rectTF) {
        rectTF.anchoredPosition = new Vector2(0f, 0f);
        rectTF.sizeDelta = new Vector2(200f, 260f);
    }

    private void SetRightPosition(RectTransform rectTF) {
        rectTF.anchoredPosition = new Vector2(153.4f, -67.83f);
        rectTF.sizeDelta = new Vector2(97.5f, 130f);
    }

    private bool Locked() {
        if(Time.time - lastLockedTime >= lockedTimeLong)
            isLocked = false;
        if(isLocked == true)
            return true;
        isLocked = true;
        lastLockedTime = Time.time;
        return false;
    }

    void SetCharacterName() {
        characterName.text = charaList[nowIndex].name;
    }

    void SetNowIndex(int num) {
        nowIndex = num;
        FullDataManager.Instance.chosenCharacter = num;
    }

}
