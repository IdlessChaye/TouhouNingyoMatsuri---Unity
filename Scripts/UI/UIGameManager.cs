using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class UIGameManager : MonoBehaviour {
    private Text textIPAddress;
    private Text textGameOver;

    private void Start() {
        DontDestroyOnLoad(transform.gameObject);
        OfflineUIset();
    }
    void OfflineUIset() {
        GameObject.Find("Button - CreateHost").GetComponent<Button>().onClick.AddListener(StartHost);
        GameObject.Find("Button - LinkTo").GetComponent<Button>().onClick.AddListener(StartClient);
        GameObject.Find("Button - GameExit").GetComponent<Button>().onClick.AddListener(ExitGame);
    }
    public void StartHost() {
        NetworkManager.singleton.StartHost();
    }
    public void StartClient() {
        string ipAddress = GameObject.Find("InputField - IPAddress").GetComponent<InputField>().text;
        if(ipAddress != "") {
            NetworkManager.singleton.networkAddress = ipAddress;
        }
        NetworkManager.singleton.StartClient();
    }
    public void ExitGame() {
        Application.Quit();
    }

    private void OnLevelWasLoaded(int level) {
        if(level == 0) {
            OfflineUIset();
        } else {
            OnlineUIset();
        }
    }
    void OnlineUIset() {
        GameObject.Find("Button - StopLink").GetComponent<Button>().onClick.AddListener(StopHost);
        textIPAddress = GameObject.Find("Text - IP").GetComponent<Text>();
        textIPAddress.text = "IP地址： " + Network.player.ipAddress;
        textGameOver = GameObject.Find("Text - GameOver").GetComponent<Text>();
        textGameOver.text = "Game Over";
        textGameOver.gameObject.SetActive(false);
    }
    public void StopHost() {
        NetworkManager.singleton.StopHost();
    }

    public void EnableGameOverText() {
        textGameOver.gameObject.SetActive(true);
    }
}
