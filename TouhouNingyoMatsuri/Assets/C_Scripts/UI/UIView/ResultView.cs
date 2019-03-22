using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultContext : BaseContext {
    public ResultContext() : base(UIType.Result) {

    }
}

public class ResultView : AnimatorView {
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

}
