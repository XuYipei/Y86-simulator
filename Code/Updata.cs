using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Updata : MonoBehaviour
{
    static bool Intl = false;
    static public int Clock = 0, Nbub = 0;
    static public int Show_Clock() { return ((Clock == -1) ? (0) : (Clock)); }
    static void Updata_Fetch()
    {
        if (!Control.Show_F_bubble() && !Control.Show_F_stall())
            Fetch.Updata(Fetch.Show_f_predPC());
    }
    static void Updata_Decode()
    {
        if (!Control.Show_D_bubble() && !Control.Show_D_stall())
            Decode.Updata(Fetch.Show_f_state(), Fetch.Show_f_icode(), Fetch.Show_f_ifun(), Fetch.Show_f_rA(), Fetch.Show_f_rB(), Fetch.Show_f_valC(), Fetch.Show_f_valP());
        if (Control.Show_D_bubble())
            Decode.Updata(Control.States.SAOK, Control.Codes.INOP, 0, Control.Registers.RNONE, Control.Registers.RNONE, 0, 0);
    }
    static void Updata_Excute()
    {
        if (!Control.Show_E_bubble() && !Control.Show_E_stall())
            Excute.Updata(Decode.Show_d_state(), Decode.Show_d_icode(), Decode.Show_d_ifun(), Decode.Show_d_valC(), Decode.Show_d_valA(), Decode.Show_d_valB(), Decode.Show_d_dstE(), Decode.Show_d_dstM(), Decode.Show_d_srcA(), Decode.Show_d_srcB());
        if (Control.Show_E_bubble())
            Excute.Updata(Control.States.SAOK, Control.Codes.INOP, 0, 0, 0, 0, Control.Registers.RNONE, Control.Registers.RNONE, Control.Registers.RNONE, Control.Registers.RNONE);
    }
    static void Updata_Memory()
    {
        Memory.Updata(Excute.Show_e_state(), Excute.Show_e_icode(), Excute.Show_e_ifun(), Excute.Show_e_Cnd(), Excute.Show_e_valE(), Excute.Show_e_valA(), Excute.Show_e_dstE(), Excute.Show_e_dstM());
    }
    static void Updata_Write_back()
    {
        if (Control.Show_W_bubble()) Nbub++;
        Write_back.Updata(Memory.Show_m_state(), Memory.Show_m_icode(), Memory.Show_m_ifun(), Memory.Show_m_valE(), Memory.Show_m_valM(), Memory.Show_m_dstE(), Memory.Show_m_dstM());
    }
    
    static public void Work()
    {
        Clock++;
        if (!Intl)
        {
            Updata_Fetch();
            Updata_Decode();
            Updata_Excute();
            Updata_Memory();
            Updata_Write_back();
        }
        Intl = false;
    }
    static public void Init_All()
    {
        Intl = true;
        Clock = Nbub = 0;
        Program.Renew(Control.States.SAOK);
        Excute.Updata_CC(true, false, false);
        Fetch.Updata(0);
        Decode.Updata(Control.States.SAOK, Control.Codes.INOP, 0, Control.Registers.RNONE, Control.Registers.RNONE, 0, 0);
        Excute.Updata(Control.States.SAOK, Control.Codes.INOP, 0, 0, 0, 0, Control.Registers.RNONE, Control.Registers.RNONE, Control.Registers.RNONE, Control.Registers.RNONE);
        Memory.Updata(Control.States.SAOK, Control.Codes.INOP, 0, false, 0, 0, Control.Registers.RNONE, Control.Registers.RNONE);
        Write_back.Updata(Control.States.SAOK, Control.Codes.INOP, 0, 0, 0, Control.Registers.RNONE, Control.Registers.RNONE);
        Program.Renew(Control.States.SAOK);

        Decode.Init_Reg();
        Memory.Init_Mem();

        Run.ck = false;
        Read_ys.Init();

        Control.Init();
        BreakControl.Init();

        Display.nlog++;
        StreamWriter sw =new StreamWriter(@"./log.out", false);
        sw.Close();
//        print("?");
    }
}
