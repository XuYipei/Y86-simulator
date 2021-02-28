using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    static public int Fmt;
    static public void Chg_Fmt() { Fmt = (Fmt + 1) % 3; }
    static string[] state = new string[] { "SAOK", "SHLT", "SINS", "SMEM" };
    static string[] reg_name = new string[] { "RAX", "RCX", "RDX", "RBX", "RSP", "RBP", "RSI", "RDI", "R8", "R9", "R10", "R11", "R12", "R13", "R14", "RNONE"};

    struct Instr
    {
        public string ins;
        public int icode, ifun;
    };
    static Instr[] instr = new Instr[40];
    private void Start()
    {
        Fmt = 0;
        instr[0].ins = "halt"; instr[0].icode = 0; instr[0].ifun = 0;
        instr[1].ins = "nop"; instr[1].icode = 1; instr[1].ifun = 0;
        instr[2].ins = "rrmovq"; instr[2].icode = 2; instr[2].ifun = 0;
        instr[3].ins = "cmovle"; instr[3].icode = 2; instr[3].ifun = 1;
        instr[4].ins = "cmovl"; instr[4].icode = 2; instr[4].ifun = 2;
        instr[5].ins = "cmove"; instr[5].icode = 2; instr[5].ifun = 3;
        instr[6].ins = "cmovne"; instr[6].icode = 2; instr[6].ifun = 4;
        instr[7].ins = "cmovge"; instr[7].icode = 2; instr[7].ifun = 5;
        instr[8].ins = "cmovg"; instr[8].icode = 2; instr[8].ifun = 6;
        instr[9].ins = "irmovq"; instr[9].icode = 3; instr[9].ifun = 0;
        instr[10].ins = "rmmovq"; instr[10].icode = 4; instr[10].ifun = 0;
        instr[11].ins = "mrmovq"; instr[11].icode = 5; instr[11].ifun = 0;
        instr[12].ins = "addq"; instr[12].icode = 6; instr[12].ifun = 0;
        instr[13].ins = "subq"; instr[13].icode = 6; instr[13].ifun = 1;
        instr[14].ins = "andq"; instr[14].icode = 6; instr[14].ifun = 2;
        instr[15].ins = "xorq"; instr[15].icode = 6; instr[15].ifun = 3;
        instr[16].ins = "jmp"; instr[16].icode = 7; instr[16].ifun = 0;
        instr[17].ins = "jle"; instr[17].icode = 7; instr[17].ifun = 1;
        instr[18].ins = "jl"; instr[18].icode = 7; instr[18].ifun = 2;
        instr[19].ins = "je"; instr[19].icode = 7; instr[19].ifun = 3;
        instr[20].ins = "jne"; instr[20].icode = 7; instr[20].ifun = 4;
        instr[21].ins = "jge"; instr[21].icode = 7; instr[21].ifun = 5;
        instr[22].ins = "jg"; instr[22].icode = 7; instr[22].ifun = 6;
        instr[23].ins = "call"; instr[23].icode = 8; instr[23].ifun = 0;
        instr[24].ins = "ret"; instr[24].icode = 9; instr[24].ifun = 0;
        instr[25].ins = "pushq"; instr[25].icode = 10; instr[25].ifun = 0;
        instr[26].ins = "popq"; instr[26].icode = 11; instr[26].ifun = 0;
        instr[27].ins = "iaddq"; instr[27].icode = 12; instr[27].ifun = 0;
        instr[28].ins = "isubq"; instr[28].icode = 12; instr[28].ifun = 1;
        instr[29].ins = "iandq"; instr[29].icode = 12; instr[29].ifun = 2;
        instr[30].ins = "ixorq"; instr[30].icode = 12; instr[30].ifun = 3;
    }
    static string Get_Ins(Control.Codes code, long fun)
    {
        long nm = (long)code;
        for (int i = 0; i < 31; i++) if (instr[i].icode == nm && instr[i].ifun == fun) return (instr[i].ins);
        return ("NOP");
    }
    static public string Get_Fmt(long x)
    {
        if (Fmt == 0) return (x.ToString("X2"));
        if (Fmt == 1) return (x.ToString("D3"));
        return (((ulong)x).ToString("D3"));
    }
    static string Get_State(Control.States st) { return (state[(long)st]); }
    static string Get_Reg(Control.Registers rg) { return (reg_name[(long)rg]); }

    static public void Show_Registers()
    {
        for (int i = 0; i < 15; i++)
        {
            Text txt = GameObject.Find("Canvas/Register_Panel/I"+reg_name[i]+"/N"+reg_name[i]).GetComponent<Text>();
            txt.text = Get_Fmt(Decode.Read_reg(i));
        }
    }

    static void Show_State()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/State/Value").GetComponent<Text>();
        txt.text = Get_State(Program.state);
    }
    static void Show_WriteBack()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/W_State/Value").GetComponent<Text>();
        txt.text = Get_State(Write_back.Show_W_state());
        txt = GameObject.Find("Canvas/PipeLine_Panel/W_Instr/Value").GetComponent<Text>();
        txt.text = Get_Ins(Write_back.Show_W_icode(), Write_back.Show_W_ifun());
        txt = GameObject.Find("Canvas/PipeLine_Panel/W_dstE/Value").GetComponent<Text>();
        txt.text = Get_Reg(Write_back.Show_W_dstE());
        txt = GameObject.Find("Canvas/PipeLine_Panel/W_dstM/Value").GetComponent<Text>();
        txt.text = Get_Reg(Write_back.Show_W_dstM());
        txt = GameObject.Find("Canvas/PipeLine_Panel/W_valE/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Write_back.Show_W_valE());
        txt = GameObject.Find("Canvas/PipeLine_Panel/W_valM/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Write_back.Show_W_valM());
    }
    static void Show_Memory()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/M_State/Value").GetComponent<Text>();
        txt.text = Get_State(Memory.Show_M_state());
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_Instr/Value").GetComponent<Text>();
        txt.text = Get_Ins(Memory.Show_M_icode(), Memory.Show_M_ifun());
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_dstE/Value").GetComponent<Text>();
        txt.text = Get_Reg(Memory.Show_M_dstE());
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_dstM/Value").GetComponent<Text>();
        txt.text = Get_Reg(Memory.Show_M_dstM());
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_valA/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Memory.Show_M_valA());
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_valE/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Memory.Show_M_valE());
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_Cnd/Value").GetComponent<Text>();
        txt.text = Memory.Show_M_Cnd().ToString();
    }
    static void Show_Excute()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/E_State/Value").GetComponent<Text>();
        txt.text = Get_State(Excute.Show_E_state());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_Instr/Value").GetComponent<Text>();
        txt.text = Get_Ins(Excute.Show_E_icode(), Excute.Show_E_ifun());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_dstE/Value").GetComponent<Text>();
        txt.text = Get_Reg(Excute.Show_E_dstE());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_dstM/Value").GetComponent<Text>();
        txt.text = Get_Reg(Excute.Show_E_dstM());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_srcA/Value").GetComponent<Text>();
        txt.text = Get_Reg(Excute.Show_E_srcA());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_srcB/Value").GetComponent<Text>();
        txt.text = Get_Reg(Excute.Show_E_srcB());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_valA/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Excute.Show_E_valA());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_valB/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Excute.Show_E_valB());
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_valC/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Excute.Show_E_valC());
    }
    static void Show_Decode()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/D_State/Value").GetComponent<Text>();
        txt.text = Get_State(Decode.Show_D_state());
        txt = GameObject.Find("Canvas/PipeLine_Panel/D_Instr/Value").GetComponent<Text>();
        txt.text = Get_Ins(Decode.Show_D_icode(), Decode.Show_D_ifun());
        txt = GameObject.Find("Canvas/PipeLine_Panel/D_rA/Value").GetComponent<Text>();
        txt.text = Get_Reg(Decode.Show_D_rA());
        txt = GameObject.Find("Canvas/PipeLine_Panel/D_rB/Value").GetComponent<Text>();
        txt.text = Get_Reg(Decode.Show_D_rB());
        txt = GameObject.Find("Canvas/PipeLine_Panel/D_valC/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Decode.Show_D_valC());
        txt = GameObject.Find("Canvas/PipeLine_Panel/D_valP/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Decode.Show_D_valP());
    }
    static void Show_Fetch()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/F_PredPC/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Fetch.Show_F_predPC());
        txt = GameObject.Find("Canvas/PipeLine_Panel/F_PC/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Fetch.SelectPC());
    }
    static void Show_Clock()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/Clock/Value").GetComponent<Text>();
        txt.text = Get_Fmt(Updata.Show_Clock());
        txt = GameObject.Find("Canvas/PipeLine_Panel/CPI/Value").GetComponent<Text>();
        double nm = 1.0;
        if (Updata.Clock > 0) nm += (double)(Updata.Nbub) / (double)(Updata.Clock - Updata.Nbub);
        txt.text = nm.ToString("N3");
    }
    static void Show_ConditionCode()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/IOF/Value").GetComponent<Text>();
        txt.text = Excute.Show_OF().ToString();
        txt = GameObject.Find("Canvas/PipeLine_Panel/ISF/Value").GetComponent<Text>();
        txt.text = Excute.Show_SF().ToString();
        txt = GameObject.Find("Canvas/PipeLine_Panel/IZF/Value").GetComponent<Text>();
        txt.text = Excute.Show_ZF().ToString();
    }
    static string Get_Logic(bool bub, bool stal)
    {
        if (bub) return ("SBUB");
        if (stal) return ("STAL");
        return ("SAOK");
    }
    static void Show_Logic()
    {
        Text txt = GameObject.Find("Canvas/PipeLine_Panel/W_Logic/Value").GetComponent<Text>();
        txt.text = Get_Logic(Control.Show_W_bubble(), false);
        txt = GameObject.Find("Canvas/PipeLine_Panel/M_Logic/Value").GetComponent<Text>();
        txt.text = Get_Logic(Control.Show_M_bubble(), false);
        txt = GameObject.Find("Canvas/PipeLine_Panel/E_Logic/Value").GetComponent<Text>();
        txt.text = Get_Logic(Control.Show_E_bubble(), Control.Show_E_stall());
        txt = GameObject.Find("Canvas/PipeLine_Panel/D_Logic/Value").GetComponent<Text>();
        txt.text = Get_Logic(Control.Show_D_bubble(), Control.Show_D_stall());
        txt = GameObject.Find("Canvas/PipeLine_Panel/F_Logic/Value").GetComponent<Text>();
        txt.text = Get_Logic(Control.Show_F_bubble(), Control.Show_F_stall());
    }

    static public int nlog = 0;
    static public void Shown_Log()
    {
        if (nlog == 10000) return;

        nlog++;
        System.IO.StreamWriter sw = new StreamWriter(@"./log.out", true);

        sw.WriteLine("Clock = " + Updata.Clock.ToString("X"));
        sw.WriteLine("Fetch:");
        sw.WriteLine("Pred_PC = " + Fetch.Show_F_predPC().ToString("X"));
        sw.WriteLine("Decode:");
        sw.Write("state = " + Decode.Show_D_state().ToString()+ " ; ");
        sw.Write("Instr = " + Get_Ins(Decode.Show_D_icode(), Decode.Show_D_ifun()) + " ; ");
        sw.Write("rA = " + Get_Reg(Decode.Show_D_rA()) + " ; ");
        sw.Write("rB = " + Get_Reg(Decode.Show_D_rB()) + " ; ");
        sw.WriteLine("");
        sw.Write("valC = " + Decode.Show_D_valC().ToString("X") + " ; ");
        sw.Write("valP = " + Decode.Show_D_valP().ToString("X") + " ; ");
        sw.WriteLine("");
        sw.WriteLine("Excute:");
        sw.Write("state = " + Excute.Show_E_state().ToString() + " ; ");
        sw.Write("Instr = " + Get_Ins(Excute.Show_E_icode(), Excute.Show_E_ifun()) + " ; ");
        sw.Write("dstE = " + Get_Reg(Excute.Show_E_dstE()) + " ; ");
        sw.Write("dstM = " + Get_Reg(Excute.Show_E_dstM()) + " ; ");
        sw.Write("srcA = " + Get_Reg(Excute.Show_E_srcA()) + " ; ");
        sw.Write("srcB = " + Get_Reg(Excute.Show_E_srcB()) + " ; ");
        sw.WriteLine("");
        sw.Write("valA = " + Excute.Show_E_valA().ToString("X") + " ; ");
        sw.Write("valB = " + Excute.Show_E_valB().ToString("X") + " ; ");
        sw.Write("valC = " + Excute.Show_E_valC().ToString("X") + " ; ");
        sw.WriteLine("");
        sw.Write("OF = " + Excute.Show_OF().ToString() + " ; " + "ZF = " + Excute.Show_ZF().ToString() + " ; ");
        sw.WriteLine("SF = " + Excute.Show_SF().ToString() + " ; ");
        sw.WriteLine("Memory:");
        sw.Write("state = " + Memory.Show_M_state().ToString() + " ; ");
        sw.Write("Instr = " + Get_Ins(Memory.Show_M_icode(), Memory.Show_M_ifun()) + " ; ");
        sw.Write("dstE = " + Get_Reg(Memory.Show_M_dstE()) + " ; ");
        sw.Write("dstM = " + Get_Reg(Memory.Show_M_dstM()) + " ; ");
        sw.WriteLine("");
        sw.Write("valA = " + Memory.Show_M_valA().ToString("X") + " ; ");
        sw.Write("valE = " + Memory.Show_M_valE().ToString("X") + " ; ");
        sw.Write("Cnd = " + Memory.Show_M_Cnd().ToString() + " ; ");
        sw.WriteLine("");
        sw.WriteLine("Write_Back:");
        sw.Write("state = " + Write_back.Show_W_state().ToString() + " ; ");
        sw.Write("Instr = " + Get_Ins(Write_back.Show_W_icode(), Write_back.Show_W_ifun()) + " ; ");
        sw.Write("dstE = " + Get_Reg(Write_back.Show_W_dstE()) + " ; ");
        sw.Write("dstM = " + Get_Reg(Write_back.Show_W_dstM()) + " ; ");
        sw.WriteLine("");
        sw.Write("valE = " + Write_back.Show_W_valE().ToString("X") + " ; ");
        sw.Write("valM = " + Write_back.Show_W_valM().ToString("X") + " ; ");
        sw.WriteLine("");
        for (int i = 0; i < 15; i++)
        {
            sw.Write(((Control.Registers)i).ToString() + " = " + Decode.Read_reg(i).ToString("X") + " ; ");
            if ((i + 1) % 3 == 0) sw.WriteLine("");
        }
        sw.WriteLine("");
        sw.Close();
    }

    static public void Show()
    {
        Show_Registers();
        Show_Fetch();
        Show_Decode();
        Show_Excute();
        Show_Memory();
        Show_WriteBack();
        Show_State();
        Show_Clock();
        Show_ConditionCode();
        Show_Logic();
    }

    private void Update()
    {
        Show();
    }
}

