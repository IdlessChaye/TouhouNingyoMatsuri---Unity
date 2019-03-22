using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInitialSetContext : BaseContext {
    public GameInitialSetContext() : base(UIType.GameInitialSet) {

    }
}

public class GameInitialSetView : AnimatorView {
    public override void OnEnter() {
        GameObject.Find("Name-Placeholder").GetComponent<Text>().text = FullDataManager.Instance.name;
        if(FullDataManager.Instance.networkType == NetworkType.Host) {
            GameObject.Find("IPOrNingyoCount").GetComponent<Text>().text = "人偶总数";
            GameObject.Find("IPOrCount-Placeholder").GetComponent<Text>().text = FullDataManager.Instance.ningyoCount;
        } else if(FullDataManager.Instance.networkType == NetworkType.Client) {
            GameObject.Find("IPOrNingyoCount").GetComponent<Text>().text = "IP地址";
            GameObject.Find("IPOrCount-Placeholder").GetComponent<Text>().text = FullDataManager.Instance.ipAddress;
        } else {
            throw new System.Exception("Error In GameInitialSetView OnEnter!");
        }
        base.OnEnter();
    }
    public override void OnExit() {
        base.OnExit();
    }
    public override void OnPause(BaseContext nextContext) {
        base.OnPause(nextContext);
    }
    public override void OnResume() {
        base.OnResume();
    }

    public void BackCallBack() {
        Pop();
    }

    public void CharacterSelectCallBack() {
        SetGameInitialData();
        Push(new CharacterSelectContext());
    }

   private void SetGameInitialData() {
        string name = GameObject.Find("Name-InputField").GetComponent<InputField>().text;
        if(name.Equals(""))
            name = GameObject.Find("Name-Placeholder").GetComponent<Text>().text;
        FullDataManager.Instance.name = name;

        string IPOrNingyoCount = GameObject.Find("IPOrCount-InputField").GetComponent<InputField>().text;
        if(FullDataManager.Instance.networkType == NetworkType.Host) {
            if(IPOrNingyoCount.Equals(""))
                IPOrNingyoCount = GameObject.Find("IPOrCount-Placeholder").GetComponent<Text>().text;
            FullDataManager.Instance.ningyoCount = IPOrNingyoCount;
        } else if(FullDataManager.Instance.networkType == NetworkType.Client) {
            if(IPOrNingyoCount.Equals(""))
                IPOrNingyoCount = GameObject.Find("IPOrCount-Placeholder").GetComponent<Text>().text;
            FullDataManager.Instance.ipAddress = IPOrNingyoCount;
        } else {
            throw new System.Exception("Error In GameInitialSetView Set!");
        }
    }
    
}
