using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class File_Input : MonoBehaviour
{
    public void Start()
    {
        Updata.Init_All();
        Display.Show_Registers();
    }
    static public string path;
    public void Click()
    {
//        print("?");
        InputField NewText = GameObject.Find("Canvas/Button_Panel/Input_File").GetComponent<InputField>();
        path = NewText.text;
        while (path.Length > 0)
        {
            int l = path.Length;
            if (path[l - 1] == '\n' || path[l - 1] == ' ' || path[l - 1] == '\t')
                path = path.Substring(0, l - 1);
            else
                break;
        }
        while (path.Length > 0)
        {
            int l = path.Length;
            if (path[0] == '\n' || path[0] == ' ' || path[0] == '\t')
                path = path.Substring(1, l);
            else
                break;
        }
        Read_code.Work(path);
    }
}
