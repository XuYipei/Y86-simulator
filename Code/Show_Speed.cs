using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_Speed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text txt = GameObject.Find("Canvas/Button_Panel/Show_Speed/Value").GetComponent<Text>();
        int nm = (int)GameObject.Find("Canvas/Button_Panel/Run_Speed").GetComponent<Slider>().value;
        txt.text = nm.ToString();
    }

    // Update is called once per frame
    public void Change()
    {
        Text txt = GameObject.Find("Canvas/Button_Panel/Show_Speed/Value").GetComponent<Text>();
        int nm = (int)GameObject.Find("Canvas/Button_Panel/Run_Speed").GetComponent<Slider>().value;
        txt.text = nm.ToString();
    }
}
