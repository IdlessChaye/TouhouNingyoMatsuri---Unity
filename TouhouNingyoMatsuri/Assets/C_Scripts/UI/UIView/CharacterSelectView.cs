using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelectContext : BaseContext {
    public CharacterSelectContext() : base(UIType.CharacterSelect) {

    }
}

public class CharacterSelectView : AnimatorView {
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

    public void LoadLoadingScene() {
        SceneManager.LoadScene("Loading");
    }

}
