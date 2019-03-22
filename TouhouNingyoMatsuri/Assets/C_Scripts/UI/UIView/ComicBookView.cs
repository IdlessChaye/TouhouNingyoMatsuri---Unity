using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBookView : MonoBehaviour {
    private CanvasGroup canvasGroup;

    public void SetCanvasGroupAlpha(float alpha) {
        if(canvasGroup == null) {
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
