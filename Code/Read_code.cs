using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Read_code : MonoBehaviour
{
    static public void Work(string path)
    {
        if (!File.Exists(path))
        {
            GameObject.Find("Canvas/Button_Panel/Input_File").GetComponent<InputField>().text = "No Such File!";
            return;
        }
        int p = path.Length - 1;
        while (p >= 0 && path[p] == ' ') p--;
        if (path.Length <= 2 || (!(path[p] == 's' || path[p] == 'o') || path[p - 1] != 'y' || path[p - 2] != '.'))
        {
            GameObject.Find("Canvas/Button_Panel/Input_File").GetComponent<InputField>().text = "Invalid File!";
            return;
        }

        GameObject.Find("Canvas/Button_Panel/Input_File").GetComponent<InputField>().text = "Find the Target!";

        Updata.Init_All();
        Display.Show_Registers();
        if (path[p] == 's')
            Read_ys.Work(path);
        else
            Read_yo.Work(path);
    }
}
