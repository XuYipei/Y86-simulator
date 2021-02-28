using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    static public Control.States state = Control.States.SAOK;
    static public void Renew(Control.States state_)
    {
        state = state_;
        return;
    }
    public Control.States Show_State() { return (state); }
    static string Check_state()
    {
        if (state == Control.States.SMEM) return ("MEMORY OVERFLOW!");
        if (state == Control.States.SINS) return ("INVALID INSTRUCTION!");
        return ("RUN OVER SUCCESSIFULLY!");
    }

    static public void Step()
    {
//       print(Fetch.Show_F_predPC().ToString("X2"));
//        print(((int)Write_back.Show_W_state()).ToString("X2"));
        if (state == Control.States.SAOK)
        {
            Control.Work();
            Updata.Work();
            Write_back.Work();
            Memory.Work();
            Excute.Work();
            Decode.Work();
            Fetch.Work();
            Display.Shown_Log();
            BreakControl.Jmp_Break();
        }
//        print(Fetch.Show_F_predPC().ToString("X2"));
//        print(((int)Write_back.Show_W_state()).ToString("X2"));
//        print("-------------------------------------------");
    }
}
