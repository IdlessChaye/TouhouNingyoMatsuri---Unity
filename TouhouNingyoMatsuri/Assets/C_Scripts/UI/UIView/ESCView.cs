using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCContext : BaseContext {
    public ESCContext() : base(UIType.ESC) {

    }
}

public class ESCView : AnimatorView {
    private Text musicNameText;

    public override void OnEnter() {
        base.OnEnter();
        SetMusicName();
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

    public void LoadMainMenuCallBack() {
        SceneManager.LoadScene(1);
    }

    private void SetMusicName() {
        if(musicNameText == null) {
            musicNameText = GameObject.Find("BGMText").GetComponent<Text>();
        }
        musicNameText.text = " BGM : " + FullMusicManager.Instance.nowPlayingName;
    }
}
