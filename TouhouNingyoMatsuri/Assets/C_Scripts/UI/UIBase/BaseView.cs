using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView : MonoBehaviour {
    public virtual void OnEnter() {

    }

    public virtual void OnExit() {

    }

    public virtual void OnPause(BaseContext nextContext) {

    }

    public virtual void OnResume() {

    }

    public void DestroySelf() {
        Destroy(this.gameObject);
    }
    
}
