using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Run : MonoBehaviour
{

    static public int clk = 0;
    static public bool ck = false;
    // Update is called once per frame
    private void Update()
    {
        if (ck)
            GameObject.Find("Canvas/Button_Panel/Run/Text").GetComponent<Text>().text = "Stop";
        else
            GameObject.Find("Canvas/Button_Panel/Run/Text").GetComponent<Text>().text = "Run";

        int nm = 150 - (int)GameObject.Find("Canvas/Button_Panel/Run_Speed").GetComponent<Slider>().value + 1;
        if (ck && Program.state == Control.States.SAOK)
        {
            clk++;
            if (clk >= nm)
            {
                Program.Step();
                clk = 0;
            }
        }
    }

    static public void F9()
    {
        if (Program.state != Control.States.SAOK) return;
        ck = !ck;
    }
    public void Click()
    {
        if (Program.state != Control.States.SAOK) return;
        ck = !ck;
    }
}
