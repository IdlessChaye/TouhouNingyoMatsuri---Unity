using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableItemButtonManager : MonoBehaviour {

	void Start () {
        GetComponent<Button>().onClick.AddListener(transform.parent.GetComponent<FinalScoreTableBuilder>().MainSceneOverCallBack);
	}

}
