using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIGameManager : MonoBehaviour {
    private Text textIPAddress;
    private Text textGameOver;
    private Text textBGM;

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        print("Awake!");
        OfflineUIset();
    }
    private void Start() {
        print("Strat!");
    }

    private void OnLevelWasLoaded(int level) {
        Cursor.lockState = CursorLockMode.Confined;
        if(level == 0) {
            OfflineUIset();
        } else {
            OnlineUIset();
        }
    }

    void OfflineUIset() {
        GameObject.Find("Button - CreateHost").GetComponent<Button>().onClick.AddListener(StartHost);
        GameObject.Find("Button - LinkTo").GetComponent<Button>().onClick.AddListener(StartClient);
        GameObject.Find("Button - GameExit").GetComponent<Button>().onClick.AddListener(ExitGame);
    }
    private void StartHost() {
        NetworkManager.singleton.StartHost();
    }
    private void StartClient() {
        string ipAddress = GameObject.Find("InputField - IPAddress").GetComponent<InputField>().text;
        if(ipAddress != "") {
            NetworkManager.singleton.networkAddress = ipAddress;
        }
        NetworkManager.singleton.StartClient();
    }
    private void ExitGame() {
        Application.Quit();
    }


    void OnlineUIset() {
        GameObject.Find("Button - StopLink").GetComponent<Button>().onClick.AddListener(StopHost);

        textIPAddress = GameObject.Find("Text - IP").GetComponent<Text>();
        textIPAddress.text = "IP地址： " + Network.player.ipAddress;

        textGameOver = GameObject.Find("Text - GameOver").GetComponent<Text>();
        textGameOver.text = "Game Over\n\r☆Thanks for playing☆";
        textGameOver.gameObject.SetActive(false);
    }
    private void StopHost() {
        NetworkManager.singleton.StopHost();
    }

    private void EnableGameOverText() {
        textGameOver.gameObject.SetActive(true);
    }

    private void SetBGMName(string name) {
        textBGM = GameObject.Find("Text - BGM").GetComponent<Text>();
        textBGM.text = "BGM: "+name;
    }
}
