using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{
    private void Update()
    {
        if (Display.Fmt == 0)
            GameObject.Find("Canvas/Button_Panel/Format/Text").GetComponent<Text>().text = "Hex";
        if (Display.Fmt == 1)
            GameObject.Find("Canvas/Button_Panel/Format/Text").GetComponent<Text>().text = "Dec";
        if (Display.Fmt == 2)
            GameObject.Find("Canvas/Button_Panel/Format/Text").GetComponent<Text>().text = "uDec";
    }
    public void Change()
    {
        Display.Chg_Fmt();
        if (Display.Fmt == 0) 
            GameObject.Find("Canvas/Button_Panel/Format/Text").GetComponent<Text>().text = "Hex";
        if (Display.Fmt == 1)
            GameObject.Find("Canvas/Button_Panel/Format/Text").GetComponent<Text>().text = "Dec";
        if (Display.Fmt == 2)
            GameObject.Find("Canvas/Button_Panel/Format/Text").GetComponent<Text>().text = "uDec";
    }
}
