using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakControl : MonoBehaviour
{
    // Start is called before the first frame update
    static int[] p=new int[110];
    static int[] rg = new int[110];
    static int[] op = new int[110];
    static long[] vl = new long[110];
    static string[] cd = new string[] { "==", "<", ">", "<=", ">=", "!=" };
    static int MeetBreak()
    {
        for (int i = 0; i < 100; i++)
        {
            if (p[i] == Fetch.SelectPC())
            {
                if (rg[i] == -1) return (i);
                long nm = Decode.Read_reg(rg[i]);
                nm = Decode.Fwd((Control.Registers)rg[i], nm);
                switch (op[i])
                {
                    case (0):
                        if (nm == vl[i]) return (i);
                        break;
                    case (1):
                        if (nm < vl[i]) return (i);
                        break;
                    case (2):
                        if (nm > vl[i]) return (i);
                        break;
                    case (3):
                        if (nm <= vl[i]) return (i);
                        break;
                    case (4):
                        if (nm >= vl[i]) return (i);
                        break;
                    case (5):
                        if (nm != vl[i]) return (i);
                        break;
                }
            }
        }
        return (-1);
    }
    static void Display_Break(long x,int y,bool tg)//p[x], Break y
    {
        Text txt = GameObject.Find("Canvas/Button_Panel/Breaks/All/Break" + y.ToString()).GetComponent<Text>();
        Text value = GameObject.Find("Canvas/Button_Panel/Breaks/All/Break" + y.ToString() + "/Value").GetComponent<Text>();
        txt.text = Display.Get_Fmt(x + 1) + ":" + ((p[x] == -1) ? ("NONE") : ("0x" + p[x].ToString("X3")));
        if (rg[x] != -1)
        {
            txt.text += " " + ((Control.Registers)rg[x]).ToString();
            txt.text += " " + cd[op[x]];
            value.text = Display.Get_Fmt(vl[x]);
        }
        else
        {
            txt.text += " NONE nop" + '\n';
            value.text = "";
        }
        txt.color = new Color32((byte)((tg) ? (255) : (0)), 0, 0, 255);
        value.color = new Color32((byte)((tg) ? (255) : (0)), 0, 0, 255);
    }
    static void Disp(int id)
    {
        Slider sd = GameObject.Find("Canvas/Button_Panel/Breaks/All/Show").GetComponent<Slider>();
//        print("?");
        int begin = (int)(sd.value + 0.01);
        for (int i = 0; i < 4; i++)
        {
            Display_Break(i + begin, i + 1, (i + begin == id));
        }
    }
    static void Slider(int id)
    {
        int nm = id;
        if (nm > 96) nm = 96;
        GameObject.Find("Canvas/Button_Panel/Breaks/All/Show").GetComponent<Slider>().value = id;
    }
    static public void Jmp_Break()
    {
        int id = MeetBreak();
        if (id == -1) return;
        Slider(id);
    }

    void Start()
    {
        for (int i = 0; i < 100; i++) p[i] = -1;
    }
    // Update is called once per frame
    void Update()
    {
        Disp(MeetBreak());
    }

    static public void Value_Change()
    {
        Disp(MeetBreak());
    }
    static public void Init()
    {
        for (int i = 0; i < 110; i++) rg[i] = p[i] = -1;
    }
    static int GetNum(ref string txt)
    {
        if (txt == "") return(-1);
        while (txt.Length > 0 && txt[0] == ' ') txt = txt.Substring(1, txt.Length - 1);
        int nm = 0, l = txt.Length;
        if (l >= 2 && txt[0] == '0' && (txt[1] == 'X' || txt[1] == 'x'))
        {
            for (int i = 2; i < l; i++)
            {
                if ('0' <= txt[i] && txt[i] <= '9')
                    nm = nm * 16 + txt[i] - '0';
                else
                {
                    if ('a' <= txt[i] && txt[i] <= 'f')
                        nm = nm * 16 + txt[i] - 'a' + 10;
                    else
                    {
                        if ('A' <= txt[i] && txt[i] <= 'F') nm = nm * 16 + txt[i] - 'A' + 10;
                        else
                        {
                            if (txt[i] == ' ')
                            {
                                txt = txt.Substring(i, l - i);
                                return (nm);
                            }
                            return (-1);
                        }
                    }
                }
                if (nm > 0xFFF) return (-1);
            }
        }
        else
        {
            for (int i = 0; i < l; i++)
            {
                if ('0' <= txt[i] && txt[i] <= '9')
                    nm = nm * 10 + txt[i] - '0';
                else
                {
                    if (txt[i] == ' ')
                    {
                        txt = txt.Substring(i, l - i);
                        return (nm);
                    }
                    return (-1);
                }
                if (nm > 0xFFF) return (-1);
            }
        }
        txt = "";
        if (0 <= nm && nm <= 0xFFF) return (nm);
        else return (-1);
    }
    static long Get_Long(ref string txt)
    {
        long nm = 0, f = 1;
        int l = txt.Length;
        if (txt=="") { txt = "Fuck";return(-1); }
        if (txt[0] == '-')
        {
            f = -1;
            txt = txt.Substring(1, txt.Length - 1);
        }
        if (txt == "") { txt = "Fuck"; return (-1); }
        if (l >= 2 && txt[0] == '0' && (txt[1] == 'X' || txt[1] == 'x'))
        {
            for (int i = 2; i < l; i++)
            {
                if ('0' <= txt[i] && txt[i] <= '9')
                    nm = (nm << 4) + txt[i] - '0';
                else
                {
                    if ('a' <= txt[i] && txt[i] <= 'f')
                        nm = (nm << 4) + txt[i] - 'a' + 10;
                    else
                    {
                        if ('A' <= txt[i] && txt[i] <= 'F') nm = (nm << 4) + txt[i] - 'A' + 10;
                        else
                        {
                            txt = "Fuck";
                            return (-1);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < l; i++)
            {
                if ('0' <= txt[i] && txt[i] <= '9')
                    nm = nm * 10 + txt[i] - '0';
                else
                {
                    txt = "Fuck";
                    return (-1);
                }       
            }
        }
        return (nm * f);
    }
    public void AddClick()
    {
        string txt = GameObject.Find("Canvas/Button_Panel/Input_Continue").GetComponent<InputField>().text;
        while (txt.Length > 0 && txt[0] == ' ') txt = txt.Substring(1, txt.Length - 1);
        while (txt.Length > 0 && txt[txt.Length - 1] == ' ') txt = txt.Substring(0, txt.Length - 1);
        int nm = GetNum(ref txt), id = -1;
        if (nm == -1) return;
//        print(nm.ToString("X3"));
        
        for (int i = 0; i < 100; i++)
            if (p[i] == -1)
            {
                id = i;
                break;
            }
        if (id == -1) return;

        while (txt.Length > 0 && txt[0] == ' ') txt = txt.Substring(1, txt.Length - 1);
//        print(txt);
        if (!(txt.Length > 3 && txt[0] == 'i' && txt[1] == 'f' && txt[2] == ' '))
        {
            if (txt == "")
            {
                for (int j = 0; j < 100; j++) if (p[j] == nm && rg[j] == -1) return;
                p[id] = nm;
            }
            return;
        }
 //       print("if OK");

        txt = txt.Substring(3, txt.Length - 3);
        while (txt.Length > 0 && txt[0] == ' ') txt = txt.Substring(1, txt.Length - 1);
        if (txt.Length <= 3) return;
        int rg_ = -1;
        string sb = txt.Substring(0, 3);
        if (sb == "rax" || sb == "RAX") rg_ = 0; if (sb == "rcx" || sb == "RCX") rg_ = 1;
        if (sb == "rdx" || sb == "RDX") rg_ = 2; if (sb == "rbx" || sb == "RBX") rg_ = 3;
        if (sb == "rsp" || sb == "RSP") rg_ = 4; if (sb == "rbp" || sb == "RBP") rg_ = 5;
        if (sb == "rsi" || sb == "RSI") rg_ = 6; if (sb == "rdi" || sb == "RDI") rg_ = 7;
        if (sb == "r10" || sb == "R10") rg_ = 10; if (sb == "r11" || sb == "R11") rg_ = 11;
        if (sb == "r12" || sb == "R12") rg_ = 12; if (sb == "r13" || sb == "R13") rg_ = 13;
        if ((sb[0] == 'r' || sb[0] == 'R') && sb[1] == '8') rg_ = 8;
        if ((sb[0] == 'r' || sb[0] == 'R') && sb[1] == '9') rg_ = 9;
        if (sb == "r14" || sb == "R14") rg_ = 14;
        if (rg_ == -1) return;

//        print(((Control.Registers)rg_).ToString());

        if (rg_ != 8 && rg_ != 9)
            txt = txt.Substring(3, txt.Length - 3);
        else
            txt = txt.Substring(2, txt.Length - 2);
        while (txt.Length > 0 && txt[0] == ' ') txt = txt.Substring(1, txt.Length - 1);
        if (txt.Length < 3) return;
        sb = txt.Substring(0, 2);
        int op_ = -1;
//        print(sb);
        if (sb == "==") op_ = 0;
        if (sb[0] == '<') op_ = 1; if (sb[1] == '>') op_ = 2;
        if (sb == "<=") op_ = 3; if (sb == ">=") op_ = 4; if (sb == "!=") op_ = 5;
        if (op_ == -1) return;

//        print(cd[op_]);

        if (op_ == 1 || op_ == 2)
            txt = txt.Substring(1, txt.Length - 1);
        else
            txt = txt.Substring(2, txt.Length - 2);
        while (txt.Length > 0 && txt[0] == ' ') txt = txt.Substring(1, txt.Length - 1);
        long vl_ = 0;
        vl_ =Get_Long(ref txt);
        if (txt != "Fuck")
        {
            for (int j = 0; j < 100; j++) if (p[j] == nm && rg[j] == rg_ && vl[j] == vl_) return;
            p[id] = nm; rg[id] = rg_;
            op[id] = op_; vl[id] = vl_;
            Slider(id);
        }
//        print("ERROR");
    }
    public void DelClick()
    {
        string str= GameObject.Find("Canvas/Button_Panel/Input_Continue").GetComponent<InputField>().text;
        int nm = GetNum(ref str);
        if (nm <= 0 || nm > 100 || str != "") return;
        for (int i = nm - 1; i < 100; i++)
        {
            p[i] = p[i + 1]; vl[i] = vl[i + 1];
            rg[i] = rg[i + 1]; op[i] = op[i + 1];
        }
    }
    public void Click()
    {
        Program.Step();
        while (MeetBreak() == -1 && Program.state == Control.States.SAOK)
        {
            Program.Step();
//            print(Fetch.SelectPC().ToString("X3"));
        }
    }
    static public void F6()
    {
        Program.Step();
        while (MeetBreak() == -1 && Program.state == Control.States.SAOK)
        {
            Program.Step();
        }
    }
}
