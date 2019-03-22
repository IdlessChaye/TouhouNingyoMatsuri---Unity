using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FinalScoreTableBuilder : NetworkBehaviour {
    public GameObject targetPanel;
    public float top = 18, down = 18, left = 20, right = 20;
    public float itemWidth = 165, itemHeight = 30;
    public float xPos_start = 60, yPos_start = 150;

    private GameObject tableItem;
    private GameObject tableItemButton;
    private int count;
    private float xPos, yPos;
    private float lerpInDis = 500f;
    private float lerpInterNum = 0.7f;

    private int[] playerList;
    private int[] playerScoreList;

    void Start() {
        xPos = xPos_start;
        yPos = yPos_start;
        tableItem = Resources.Load<GameObject>("Text-TableItem") as GameObject;
        tableItemButton = Resources.Load<GameObject>("Button-TableItem") as GameObject;
    }

    public void BuildTable() {
        ReqGetPlayerList();
        StartCoroutine(Building());
    }

    IEnumerator Building() {
        yield return new WaitForSeconds(1f);
        PlayerListsSort();
        count = playerList.Length / 3;
        SetInitialParas();
        Vector2 targetPosition;
        RectTransform rectTF;
        for(int i = 0; i < count; i++) {
            xPos += left;
            yPos -= top;
            targetPosition = new Vector2(xPos, yPos);
            GameObject newTableItem = Instantiate(tableItem);
            newTableItem.transform.SetParent(targetPanel.transform, false);
            rectTF = newTableItem.GetComponent<RectTransform>();
            rectTF.sizeDelta = new Vector2(itemWidth, itemHeight);
            rectTF.anchoredPosition3D = new Vector3(xPos + lerpInDis, yPos, 0);

            int netId = playerScoreList[i * 2];
            int playerScore = playerScoreList[i * 2 + 1];
            GameObject player = ClientScene.FindLocalObject(new NetworkInstanceId((uint)netId));
            Text newTableItemText = newTableItem.GetComponent<Text>();
            newTableItemText.text = (i + 1).ToString() + ". " + player.name + " ( " + playerScore + " Points )";
            while(rectTF.anchoredPosition.x - xPos > 1f) {
                rectTF.anchoredPosition = Vector2.Lerp(targetPosition, rectTF.anchoredPosition, lerpInterNum);
                yield return new WaitForEndOfFrame();
            }
            yPos -= down;
        }
        xPos += left;
        yPos -= top;
        targetPosition = new Vector2(xPos, yPos);
        GameObject newTableItemButton = Instantiate(tableItemButton);
        newTableItemButton.transform.SetParent(targetPanel.transform, false);
        rectTF = newTableItemButton.GetComponent<RectTransform>();
        rectTF.anchoredPosition3D = new Vector3(xPos + lerpInDis, yPos, 0);
        while(rectTF.anchoredPosition.x - xPos > 1f) {
            rectTF.anchoredPosition = Vector2.Lerp(targetPosition, rectTF.anchoredPosition, lerpInterNum);
            yield return new WaitForEndOfFrame();
        }
        yPos -= down;
    }

    private void SetInitialParas() {
        if(count <= 10)
            return;
        top = down = 350 / count / 2;
        left = right = 200 / count;
    }

    public void SetPlayerList(int[] playerList) {
        this.playerList = playerList;
    }

    private void ReqGetPlayerList() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
            player.GetComponent<UIPlayerManager>().GetPlayerList();
    }

    private void PlayerListsSort() {
        int nowIndex = 0;
        int score;
        int size = playerList.Length / 3;
        playerScoreList = new int[size * 2];
        for(int i = 0; i < size; i++) {
            playerScoreList[nowIndex] = playerList[3 * i];
            score = playerList[3 * i + 1] * FullDataManager.Instance.allScore + playerList[3 * i + 2] * FullDataManager.Instance.nowScore;
            playerScoreList[nowIndex + 1] = score;
            nowIndex = nowIndex + 2;
        }
        int maxScore, currentScore, maxIndex, tmp;
        for(int i = 0; i < playerScoreList.Length / 2 - 1; i++) {
            maxScore = playerScoreList[2 * i + 1];
            maxIndex = i;
            for(int j = i + 1; j < playerScoreList.Length / 2; j++) {
                currentScore = playerScoreList[2 * j + 1];
                if(currentScore > maxScore) {
                    maxScore = currentScore;
                    maxIndex = j;
                }
            }
            if(i != maxIndex) {
                playerScoreList[2 * maxIndex + 1] = playerScoreList[2 * i + 1];
                playerScoreList[2 * i + 1] = maxScore;
                tmp = playerScoreList[2 * maxIndex];
                playerScoreList[2 * maxIndex] = playerScoreList[2 * i];
                playerScoreList[2 * i] = tmp;
            }
        }
    }

    public void MainSceneOverCallBack() {
        FullGameFlowManager.Instance.SendMessage("MainSceneOverTrigger");
    }
}
