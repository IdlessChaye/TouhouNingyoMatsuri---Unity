using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 实现所有场景的UI系统，以及场景切换对UI的影响
public class FullGameFlowManager : FullSingleton<FullGameFlowManager> {
    public override void Initial() {
        this.gameObject.name = "FullGameFlowManager";
        PrintSceneNumber(0);
        SceneZeroInitial();
    }

    private void OnLevelWasLoaded(int level) {
        PrintSceneNumber(level);
        Cursor.lockState = CursorLockMode.Confined;
        if(level == 1) {
            SceneOneInitial();
        } else if(level == 2) {
            SceneTwoInitial();
        } else if(level == 3) {
            SceneThreeInitial();
        } else if(level == 4) {
            SceneFourInitial();
        } else {
            throw new System.Exception("Error In FullGameFlowManager!");
        }
    }

    private void PrintSceneNumber(int level) {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(level + " Active scene's number is " + scene.buildIndex + ".");
    }

    // Start Scene
    private void SceneZeroInitial() {

    }

    // Menu Scene
    private void SceneOneInitial() {
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();
        Singleton<ContextManager>.Instance.FirstPush(new MainMenuBGContext());
        FullMusicManager.Instance.Play("趣味工房にんじんわいん - 添い寝人形");
        FullDataManager.Instance.playerNameList.Clear();
    }

    // Loading Scene
    private void SceneTwoInitial() {
        FullMusicManager.Instance.Stop();
    }

    // Main Scene
    private void SceneThreeInitial() {
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();

        Text textIPAddress = GameObject.Find("Text - IP").GetComponent<Text>();
        textIPAddress.text = "IP Address : " + Network.player.ipAddress;

        Invoke("RaiseComicBookChapterOne", 1);
    }

    private void RaiseComicBookChapterOne() {
        ComicBookManager.ComicBook.RaiseComicBook(ComicBookManager.chapter1);
        Invoke("RoyalFlareChapterOne", 0.1f);
    }

    private void RoyalFlareChapterOne() {
        ComicBookManager.ComicBook.RoyalFlare();
    }

    // Final Scene 
    private void SceneFourInitial() {
        FullMusicManager.Instance.Play("市松椿 - 童祭　~ Innocent Treasures");
    }

    private void GameStartConfirm() {
        if(FullDataManager.Instance.networkType == NetworkType.Host) {
            GameObject gameStartConfirm = Resources.Load<GameObject>("GameStartConfirm") as GameObject;
            gameStartConfirm = Instantiate(gameStartConfirm) as GameObject;
            gameStartConfirm.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
    }

    private void GameStartTrigger() {
        FullMusicManager.Instance.Play("ジャージと愉快な仲間たち - 今日ぞハレの日、われらが祭り");
    }

    private void GameOverTrigger() {
        FullMusicManager.Instance.Play("グーシャンダグー - 幽境奏楽");
        FinalScoreTableBuilder builder = GameObject.FindObjectOfType(typeof(FinalScoreTableBuilder)) as FinalScoreTableBuilder;
        builder.SendMessage("BuildTable");
    }

    private void MainSceneOverTrigger() {
        ComicBookManager.ComicBook.RaiseComicBook(ComicBookManager.chapter2);
        ComicBookManager.ComicBook.RoyalFlare();
    }

    private void MainSceneOver() {
        SceneManager.LoadScene("Final");
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }
}
