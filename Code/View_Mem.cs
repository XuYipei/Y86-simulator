using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

public class View_Mem : MonoBehaviour
{
    private void Start()
    {
        System.IO.StreamWriter sw = new System.IO.StreamWriter(@"./memory.out");
        sw.Write("");
        sw.Close();
    }
    private void Update()
    {
        Dropdown dw = GameObject.Find("Canvas/Memory_Panel/Page").GetComponent<Dropdown>();
        string s = "";
        int begin = dw.value * 16, end = begin + 47;
//        print(begin.ToString("X") + "To" + end.ToString("X"));
        for (int id = 0, i = begin; i <=end; i += 8, id++)
        {
            s = "";
            for (int j = 0; j < 8; j++)
            {
                if (i + j <= end)
                    s += Memory.Read_Mem(i + j).ToString("X2") + " ";
                else
                    s += "00 ";
            }
            GameObject.Find("Canvas/Memory_Panel/Array" + id.ToString() + "/Text").GetComponent<Text>().text = s;
//            print(s);
        }
    }

    void Show()
    {
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.StandardInput.WriteLine("write ./memory.out" + "&exit");
        p.StandardInput.AutoFlush = true;
        string strOuput = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        p.Close();
    }
    public void Click()
    {
        System.IO.StreamWriter sw = new System.IO.StreamWriter(@"./memory.out");
        
        int lm = (Display.Fmt==0) ? (16) : (10);
        for (int i = 0; i < 0x200; i++)
        {
            sw.Write(Memory.Read_Mem(i).ToString("X2") + " ");
            if ((i + 1) % lm == 0) sw.WriteLine();
        }
        sw.Close();

        Thread t = new Thread(Show);
        t.Start();
    }
}
