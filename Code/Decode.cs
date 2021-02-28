using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Decode : MonoBehaviour
{
    static long[] reg = new long[16];
    static public void Write_reg(int x, long y) { reg[x] = y; }
    static public long Read_reg(int x) { return (reg[(int)x]); }

    static Control.Codes d_icode, D_icode;
    static Control.States D_state, d_state;
    static Control.Registers D_rA, D_rB, d_srcA, d_srcB, d_dstE, d_dstM;
    static long D_ifun, D_valC, D_valP, d_ifun, d_valC, d_valA, d_valB;

    static public Control.Codes Show_d_icode() { return (d_icode); }
    static public Control.Codes Show_D_icode() { return (D_icode); }
    static public Control.States Show_d_state() { return (d_state); }
    static public Control.States Show_D_state() { return (D_state); }
    static public Control.Registers Show_D_rA() { return (D_rA); }
    static public Control.Registers Show_D_rB() { return (D_rB); }
    static public Control.Registers Show_d_srcA() { return (d_srcA); }
    static public Control.Registers Show_d_srcB() { return (d_srcB); }
    static public Control.Registers Show_d_dstE() { return (d_dstE); }
    static public Control.Registers Show_d_dstM() { return (d_dstM); }
    static public long Show_d_ifun() { return (d_ifun); }
    static public long Show_d_valA() { return (d_valA); }
    static public long Show_d_valB() { return (d_valB); }
    static public long Show_d_valC() { return (d_valC); }
    static public long Show_D_ifun() { return (D_ifun); }
    static public long Show_D_valC() { return (D_valC); }
    static public long Show_D_valP() { return (D_valP); }

    static public void Updata(Control.States state, Control.Codes code, long ifun, Control.Registers rA, Control.Registers rB, long valC, long valP)
    {
        D_state = state;
        D_icode = code;
        D_ifun = ifun;
        D_rA = rA;
        D_rB = rB;
        D_valC = valC;
        D_valP = valP;
    }
    static public void Init_Reg() { Array.Clear(reg, 0, 16); }

    static public long Fwd(Control.Registers src, long rval)
    {
        if (src == Excute.Show_e_dstE()) return (Excute.Show_e_valE());
        if (src == Memory.Show_M_dstM()) return (Memory.Show_m_valM());
        if (src == Memory.Show_M_dstE()) return (Memory.Show_M_valE());
        if (src == Write_back.Show_W_dstM()) return (Write_back.Show_W_valM());
        if (src == Write_back.Show_W_dstE()) return (Write_back.Show_W_valE());
        return (rval);
    }
    static public void Work()
    {
//        if (D_state == Control.States.SINS || D_state == Control.States.SHLT) { d_state = D_state; return; }

        if (D_icode == Control.Codes.IPOPQ)
        {
            int deb = 0;
            int nm = deb + 1;
        }

        d_dstE = d_dstM = Control.Registers.RNONE;
        if (D_icode == Control.Codes.IRRMOVQ || D_icode == Control.Codes.IIRMOVQ || D_icode == Control.Codes.IOPQ || D_icode == Control.Codes.IIOPQ)
            d_dstE = D_rB;
        if (D_icode == Control.Codes.ICALL || D_icode == Control.Codes.IRET || D_icode == Control.Codes.IPUSHQ || D_icode == Control.Codes.IPOPQ)
            d_dstE = Control.Registers.RSP;
        if (D_icode == Control.Codes.IMRMOVQ || D_icode == Control.Codes.IPOPQ)
            d_dstM = D_rA;

        d_srcA = d_srcB = Control.Registers.RNONE;
        if (D_icode == Control.Codes.IRRMOVQ || D_icode == Control.Codes.IRMMOVQ || D_icode == Control.Codes.IOPQ || D_icode == Control.Codes.IPUSHQ)
            d_srcA = D_rA;
        if (D_icode == Control.Codes.IPOPQ || D_icode == Control.Codes.IRET)
            d_srcA = Control.Registers.RSP;
        if (D_icode == Control.Codes.IRMMOVQ || D_icode == Control.Codes.IMRMOVQ || D_icode == Control.Codes.IOPQ || D_icode == Control.Codes.IIOPQ)
            d_srcB = D_rB;
        if (D_icode == Control.Codes.IRET || D_icode == Control.Codes.IPUSHQ || D_icode == Control.Codes.IPOPQ || D_icode == Control.Codes.ICALL)
            d_srcB = Control.Registers.RSP;

        long d_rvalA = reg[(long)d_srcA];
        long d_rvalB = reg[(long)d_srcB];
        if (D_icode == Control.Codes.ICALL || D_icode == Control.Codes.IJXX)
            d_valA = D_valP;
        else
            d_valA = Fwd(d_srcA, d_rvalA);
        d_valB = Fwd(d_srcB, d_rvalB);

        d_state = D_state;
        d_icode = D_icode;
        d_ifun = D_ifun;
        d_valC = D_valC;
    }
}
