using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelectContext : BaseContext {
    public GameModeSelectContext() : base(UIType.GameModeSelect) {

    }
}

public class GameModeSelectView : AnimatorView {
    public override void OnEnter() {
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

    public void HostCallBack() {
        FullDataManager.Instance.networkType = NetworkType.Host;
        Push(new GameInitialSetContext());
    }

    public void ClientCallBack() {
        FullDataManager.Instance.networkType = NetworkType.Client;
        Push(new GameInitialSetContext());
    }
}
