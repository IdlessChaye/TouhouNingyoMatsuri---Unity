using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadingScript : MonoBehaviour {
    public GameObject objProcessBar;
    public Text baifenbi;
    
    void Start() {
        StartCoroutine(StartLoading());
    }

    IEnumerator StartLoading() {
        float i = 0;
        while(i <= 100) {
            i++;
            objProcessBar.GetComponent<Slider>().value = i / 100;
            yield return new WaitForEndOfFrame();
            baifenbi.text = i.ToString() + "%";
        }
        LoadOnlineScene();
    }

    void LoadOnlineScene() {
        PlayerNetworkCustom playerNetworkCustom = GameObject.Find("PlayerNetworkCustom").GetComponent<PlayerNetworkCustom>();
        playerNetworkCustom.chosenCharacter = FullDataManager.Instance.chosenCharacter;

        NetworkServer.Reset();
        switch(FullDataManager.Instance.networkType) {
            case NetworkType.None:
                throw new System.Exception("Network Not Setted!");
                break;
            case NetworkType.Host:
                NetworkManager.singleton.StartHost();
                break;
            case NetworkType.Client:
                NetworkManager.singleton.networkAddress = FullDataManager.Instance.ipAddress;
                NetworkManager.singleton.StartClient();
                break;
        }

        
    }
}
