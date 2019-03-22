using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager {
    public Dictionary<UIType, GameObject> UIDict = new Dictionary<UIType, GameObject>();

    private Transform canvas;

    private UIManager() {
        canvas = GameObject.Find("Canvas").transform;
        /*foreach(Transform tf in canvas) {
            GameObject.Destroy(tf.gameObject);
        }*/
    }

    public GameObject GetSingleUI(UIType uiType) {
        if(UIDict.ContainsKey(uiType) == false || UIDict[uiType] == null) {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(uiType.Path)) as GameObject;
            obj.transform.SetParent(canvas, false);
            obj.name = uiType.Name;
            AddOrReplaceOne(uiType, obj);
            return obj;
        }
        return UIDict[uiType];
    }

    public void DestroySingleUI(UIType uiType) {
        if(UIDict.ContainsKey(uiType) == false)
            return;
        if(UIDict[uiType] == null) {
            UIDict.Remove(uiType);
            return;
        }
        GameObject.Destroy(UIDict[uiType]);
        UIDict.Remove(uiType);
        return;
    }


    private void AddOrReplaceOne(UIType uiType, GameObject obj) {
        if(UIDict.ContainsKey(uiType) == false) {
            UIDict.Add(uiType, obj);
            return;
        }
        if(UIDict[uiType] == null) {
            UIDict[uiType] = obj;
            return;
        }
    }
}
