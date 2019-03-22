using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBGContext : BaseContext {
    public MainMenuBGContext() : base(UIType.MainMenuBG) {

    }
}

public class MainMenuBGView : AnimatorView {
    //[SerializeField]
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
        SetMusicName();
    }

    public override void OnResume() {
        base.OnResume();
    }

    public void MainMenuCallBack() {
        Push(new MainMenuContext());
    }

    private void SetMusicName() {
        if(musicNameText == null) {
            musicNameText = GameObject.Find("BGBGMText").GetComponent<Text>();
        }
        musicNameText.text = " BGM : " + FullMusicManager.Instance.nowPlayingName;
    }
}
