using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour {
    Image m_Image;
    //Set this in the Inspector
    public Sprite m_Sprite;

    void Start() {
        //Fetch the Image from the GameObject
        m_Image = GetComponent<Image>();
        m_Sprite = Resources.Load<Sprite>("东方人形祭 ~ Emotional Echoes of Rebels light") as Sprite;
    }

    void Update() {
        //Press space to change the Sprite of the Image
        if(Input.GetKey(KeyCode.Space)) {
            m_Image.sprite = m_Sprite;
        }
    }
}