using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextManager{
    private Stack<BaseContext> stack = new Stack<BaseContext>();

    private static bool isLocked; // 解决快速双击UI导致此系统逻辑错误的Bug，Push有延迟性
    private static float lastLockedTime;
    private static float lockedTimeLong = 0.7f; // 锁1s

    private ContextManager() {
        isLocked = false;
        lastLockedTime = -lockedTimeLong;
    }

    public void FirstPush(BaseContext baseContext) {
        Push(baseContext);
    }

    public void Push(BaseContext nextContext) {
        if(Locked())
            return;
        if(stack.Count != 0) {
            BaseContext curContext = stack.Peek();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
            curView.OnPause(nextContext);
        }
        stack.Push(nextContext);
        BaseView nextView = Singleton<UIManager>.Instance.GetSingleUI(nextContext.ViewType).GetComponent<BaseView>();
        nextView.OnEnter();
    }

    public void Pop() {
        if(Locked())
            return;
        if(stack.Count != 0) {
            BaseContext curContext = stack.Peek();
            stack.Pop();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
            curView.OnExit();
        }
        if(stack.Count != 0) {
            BaseContext lastContext = stack.Peek();
            BaseView lastView = Singleton<UIManager>.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
            lastView.OnResume();
        }
    }

    public BaseContext PeekOrNull() {
        if(stack.Count != 0)
            return stack.Peek();
        return null;
    }

    private bool Locked() {
        if(Time.time - lastLockedTime >= lockedTimeLong)
            isLocked = false;
        if(isLocked == true)
            return true;
        isLocked = true;
        lastLockedTime = Time.time;
        return false;
    }

}
