using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuContext : BaseContext {
    public MainMenuContext() : base(UIType.MainMenu) {

    }
}

public class MainMenuView : AnimatorView {
    
    public override void OnEnter() {
        base.OnEnter();
    }

    public override void OnExit() {
        throw new System.Exception("此动作无定义");
    }

    public override void OnPause(BaseContext nextContext) {
        base.OnPause(nextContext);
    }

    public override void OnResume() {
        base.OnResume();
    }

    public void GameModeSelectCallback() {
        Push(new GameModeSelectContext());
    }

    public void MusicRoomCallBack() {
        Push(new MusicRoomContext());
    }

    public void ResultCallBack() {
        Push(new ResultContext());
    }

    public void ConfigCallBack() {
        Push(new ConfigContext());
    }

    public void ExitGameCallBack() {
        Application.Quit();
    }
}
