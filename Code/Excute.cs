using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excute : MonoBehaviour
{
    static Control.Codes E_icode, e_icode;
    static bool OF, ZF, SF, e_SetCC, e_Cnd;
    static Control.States E_state, e_state;
    static Control.Registers E_dstE, E_dstM, E_srcA, E_srcB, e_dstM, e_dstE;
    static long E_ifun, E_valC, E_valA, E_valB, e_valA, e_valE, e_ifun;

    static public bool Show_e_Cnd() { return (e_Cnd); }
    static public int Show_OF() { return ((OF) ? (1) : (0)); }
    static public int Show_ZF() { return ((ZF) ? (1) : (0)); }
    static public int Show_SF() { return ((SF) ? (1) : (0)); }
    static public Control.States Show_E_state() { return (E_state); }
    static public Control.States Show_e_state() { return (e_state); }
    static public Control.Codes Show_E_icode() { return (E_icode); }
    static public Control.Codes Show_e_icode() { return (e_icode); }
    static public Control.Registers Show_e_dstE() { return (e_dstE); }
    static public Control.Registers Show_e_dstM() { return (e_dstM); }
    static public Control.Registers Show_E_dstM() { return (E_dstM); }
    static public Control.Registers Show_E_dstE() { return (E_dstE); }
    static public Control.Registers Show_E_srcA() { return (E_srcA); }
    static public Control.Registers Show_E_srcB() { return (E_srcB); }
    static public long Show_e_valE() { return (e_valE); }
    static public long Show_e_valA() { return (e_valA); }
    static public long Show_e_ifun() { return (e_ifun); }
    static public long Show_E_ifun() { return (E_ifun); }
    static public long Show_E_valA() { return (E_valA); }
    static public long Show_E_valC() { return (E_valC); }
    static public long Show_E_valB() { return (E_valB); }

    static public void Updata_CC(bool zf, bool sf, bool of) { OF = of; ZF = zf; SF = sf; }
    static public void Updata(Control.States state, Control.Codes icode, long ifun, long valC, long valA, long valB, Control.Registers dstE, Control.Registers dstM, Control.Registers srcA, Control.Registers srcB)
    {
        E_state = state;
        E_icode = icode;
        E_ifun = ifun;
        E_valC = valC;
        E_valA = valA;
        E_valB = valB;
        E_dstE = dstE;
        E_dstM = dstM;
        E_srcA = srcA;
        E_srcB = srcB;
    }

    static public void Work()
    {
        e_state = E_state;
        long e_ALUA = 0, e_ALUB = 0;
        if (E_icode == Control.Codes.IRRMOVQ || E_icode == Control.Codes.IOPQ) e_ALUA = E_valA;
        if (E_icode == Control.Codes.IIRMOVQ || E_icode == Control.Codes.IRMMOVQ || E_icode == Control.Codes.IMRMOVQ || E_icode == Control.Codes.IIOPQ) e_ALUA = E_valC;
        if (E_icode == Control.Codes.IRET || E_icode == Control.Codes.IPOPQ) e_ALUA = 8;
        if (E_icode == Control.Codes.ICALL || E_icode == Control.Codes.IPUSHQ) e_ALUA = -8;
        if (E_icode == Control.Codes.IRRMOVQ || E_icode == Control.Codes.IIRMOVQ) e_ALUB = 0;
        else e_ALUB = E_valB;

        switch (E_ifun)
        {
            case (0): e_valE = e_ALUB + e_ALUA; break;
            case (1): e_valE = e_ALUB - e_ALUA; break;
            case (2): e_valE = e_ALUB & e_ALUA; break;
            case (3): e_valE = e_ALUB ^ e_ALUA; break;
        }

        if ((E_icode == Control.Codes.IOPQ || E_icode == Control.Codes.IIOPQ) && Write_back.Show_W_state() == Control.States.SAOK && Memory.Show_m_state() == Control.States.SAOK && E_state == Control.States.SAOK) 
            e_SetCC = true;
        else
            e_SetCC = false;
        if (e_SetCC)
        {
            ZF = (e_valE == 0);
            SF = (e_valE < 0);
            if (E_ifun == 0) OF = (((e_ALUA < 0) == (e_ALUB < 0)) && ((e_valE < 0) != (e_ALUA < 0)));
            if (E_ifun == 1) OF = (((e_ALUA < 0) != (e_ALUB < 0)) && ((e_valE < 0) != (e_ALUB < 0)));
            if (E_ifun == 2 || E_ifun == 3) OF = false;
        }
        e_valA = E_valA;

        e_Cnd = false;
        switch (E_ifun)
        {
            case 0: e_Cnd = true; break;
            case 1: e_Cnd = SF | ZF; break;
            case 2: e_Cnd = SF; break;
            case 3: e_Cnd = ZF; break;
            case 4: e_Cnd = (!ZF); break;
            case 5: e_Cnd = (!SF); break;
            case 6: e_Cnd = (!SF && !ZF); break;
        }

        e_ifun = E_ifun;
        e_dstM = E_dstM;
        e_icode = E_icode;
        if (e_icode == Control.Codes.IRRMOVQ && !e_Cnd) e_dstE = Control.Registers.RNONE;
        else e_dstE = E_dstE;
    }
}
