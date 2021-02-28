using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    static bool F_stall, D_stall, E_stall, F_bubble, D_bubble, E_bubble;
    static bool W_bubble, M_bubble;

    static public bool Show_F_stall() { return (F_stall); }
    static public bool Show_D_stall() { return (D_stall); }
    static public bool Show_E_stall() { return (E_stall); }
    static public bool Show_W_bubble() { return (W_bubble); }
    static public bool Show_M_bubble() { return (M_bubble); }
    static public bool Show_F_bubble() { return (F_bubble); }
    static public bool Show_D_bubble() { return (D_bubble); }
    static public bool Show_E_bubble() { return (E_bubble); }

    public enum Codes : long { IHALT, INOP, IRRMOVQ, IIRMOVQ, IRMMOVQ, IMRMOVQ, IOPQ, IJXX, ICALL, IRET, IPUSHQ, IPOPQ, IIOPQ, IWRNG0, IWRNG1, IWRNG2 };
    public enum Registers : long { RAX, RCX, RDX, RBX, RSP, RBP, RSI, RDI, R8, R9, R10, R11, R12, R13, R14, RNONE };
    public enum States : long { SAOK, SHLT, SINS, SMEM };

    static public void Work()
    {
        F_stall = D_stall = E_stall = false;
        //        F_bubble = D_bubble = E_bubble = false;
        W_bubble = M_bubble; M_bubble = E_bubble;
        E_bubble = D_bubble; D_bubble = F_bubble;
        F_bubble = false;

        bool ld = false, rt = false, wj = false;
        if ((Excute.Show_E_icode() == Codes.IMRMOVQ || Excute.Show_E_icode() == Codes.IPOPQ) && (Excute.Show_E_dstM() == Decode.Show_d_srcA() || Excute.Show_E_dstM() == Decode.Show_d_srcB())) ld = true;
        if ((Excute.Show_E_icode() == Codes.IRET) || (Memory.Show_M_icode() == Codes.IRET) || (Decode.Show_D_icode() == Codes.IRET)) rt = true;
        if (!Excute.Show_e_Cnd() && Excute.Show_e_icode() == Codes.IJXX) wj = true;

        if (ld && !rt) F_stall = D_stall = E_bubble = true;//LOAD_USE
        if (rt && !ld) F_stall = D_bubble = true;//RET
        if (wj) E_bubble = D_bubble = true;//WRONG_JXX
        if (ld && rt) E_bubble = D_stall = F_stall = true;//LOAD_USE && RET
    }

    static public void Init()
    {
        F_stall = false;
        D_stall = false;
        E_stall = false;
        F_bubble = false;
        D_bubble = false;
        E_bubble = false;
        W_bubble = false;
        M_bubble = false;
    }
}
