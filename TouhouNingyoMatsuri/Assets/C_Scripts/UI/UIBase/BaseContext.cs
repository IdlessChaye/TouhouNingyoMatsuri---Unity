using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContext {
    public UIType ViewType { get; private set; }

    public BaseContext(UIType viewType) {
        ViewType = viewType;
    }
}
