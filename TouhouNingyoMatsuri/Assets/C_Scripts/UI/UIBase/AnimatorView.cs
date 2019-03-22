using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AnimatorView : BaseView {

    [SerializeField]
    protected Animator ani;

    private CanvasGroup canvasGroup;

    public override void OnEnter() {
        ani.SetTrigger("OnEnter");
    }

    public override void OnExit() {
        ani.SetTrigger("OnExit");
    }

    public override void OnPause(BaseContext nextContext) {
        ani.SetTrigger("OnPause");
    }

    public override void OnResume() {
        ani.SetTrigger("OnResume");
    }

    protected void Push(BaseContext nextContext) {
        Singleton<ContextManager>.Instance.Push(nextContext);
    }

    protected void Pop() {
        Singleton<ContextManager>.Instance.Pop();
    }

    public void SetCanvasGroupAlpha(float alpha) {
        if (canvasGroup == null) { 
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = alpha;
    }

    public void SetCanvasGroupRaycasts(int blocksRaycasts) {
        if(canvasGroup == null) {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.blocksRaycasts = blocksRaycasts == 1 ? true : false;
    }
}
