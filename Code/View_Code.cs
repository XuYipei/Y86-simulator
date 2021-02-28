using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

public class View_Code : MonoBehaviour
{
    private void Start()
    {
        System.IO.StreamWriter sw = new System.IO.StreamWriter(@"./code.out");
        sw.Write("");
        sw.Close();
    }

    static int N = 0;
    class Code
    {
        public int l, r;
        public string ins;
    };
    static Code[] cd = new Code[1010];
    static string[] cd0 = new string[10010];
    static int[] cd1 = new int[10010];
    static int[] cd2 = new int[10010];

    static public void Init()
    {
        N = 0;
    }
    static public void Add(int x, int y, string s)
    {
        N++;
        cd[N] = new Code();
//        print(N.ToString() + " " + x.ToString("X2") + " " + y.ToString("X2") + " " + s);
        cd[N].ins = s;
        cd[N].l = x;
        cd[N].r = y;
    }
    static public void Display()
    {
        System.IO.StreamWriter sw = new System.IO.StreamWriter(@"./code.out");
        for (int i = 1; i <= N; i++)
        {
            if (cd[i].l == -1)
            {
                sw.WriteLine(cd[i].ins);
                continue;
            }
            string ad = "0x" + cd[i].l.ToString("X3");
            for (int j = cd[i].l; j < cd[i].r; j++) ad = ad + " " + Memory.Read_Mem(j).ToString("X2");
            while (ad.Length < 50) ad = ad + " ";
            ad = ad + "|" + cd[i].ins;
            sw.WriteLine(ad);
        }
        sw.Close();
    }

    static void Show()
    {
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.StandardInput.WriteLine("write ./Code.out" + "&exit");
        p.StandardInput.AutoFlush = true;
        string strOuput = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        p.Close();
    }

    public void Click()
    {
        Thread t = new Thread(Show);
        t.Start();
    }
}
