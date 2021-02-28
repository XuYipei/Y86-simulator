using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    static public long MEMORY_LIMIT = 1000;
    static int[] mem = new int[100010];
    static public void Write_Mem(int x, int y) { mem[x] = y; }
    static public void Init_Mem() { Array.Clear(mem, 0, 100000); }
    static public void Write_Mem_Long(int x, long y)
    {
        ulong y_ = (ulong)y;
        for (int i = 0; i < 8; i++)
        {
            ulong nm = y_ % 256;
            Write_Mem(x + i, (int)nm);
            y_ /= 256;
        }
    }
    static public int Read_Mem(int x) { return (mem[x]); }

    static bool M_Cnd;
    static Control.Codes M_icode, m_icode;
    static Control.States M_state, m_state;
    static Control.Registers m_dstE, m_dstM, M_dstM, M_dstE;
    static long M_valE, M_valA, m_Addr, m_valM, m_valE, m_ifun, M_ifun;

    static public bool Show_M_Cnd() { return (M_Cnd); }
    static public long Show_m_ifun() { return (m_ifun); }
    static public long Show_M_ifun() { return (M_ifun); }
    static public long Show_m_valE() { return (m_valE); }
    static public long Show_m_valM() { return (m_valM); }
    static public long Show_M_valE() { return (M_valE); }
    static public long Show_M_valA() { return (M_valA); }
    static public Control.Registers Show_m_dstE() { return (m_dstE); }
    static public Control.Registers Show_m_dstM() { return (m_dstM); }
    static public Control.Registers Show_M_dstE() { return (M_dstE); }
    static public Control.Registers Show_M_dstM() { return (M_dstM); }
    static public Control.Codes Show_m_icode() { return (m_icode); }
    static public Control.Codes Show_M_icode() { return (M_icode); }
    static public Control.States Show_m_state() { return (m_state); }
    static public Control.States Show_M_state() { return (M_state); }

    static public void Updata(Control.States state, Control.Codes icode, long ifun, bool Cnd, long valE, long valA, Control.Registers dstE, Control.Registers dstM)
    {
        M_state = state;
        M_icode = icode;
        M_ifun = ifun;
        M_Cnd = Cnd;
        M_valE = valE;
        M_valA = valA;
        M_dstE = dstE;
        M_dstM = dstM;
    }

    static public void Work()
    {
//        if (M_state == Control.States.SHLT || M_state == Control.States.SINS) { m_state = M_state; return; }
        
        m_Addr = 0;
        if (M_icode == Control.Codes.IRET || M_icode == Control.Codes.IPOPQ)
            m_Addr = M_valA;
        if (M_icode == Control.Codes.IMRMOVQ || M_icode == Control.Codes.IRMMOVQ || M_icode == Control.Codes.ICALL || M_icode == Control.Codes.IPUSHQ)
            m_Addr = M_valE;
        if ((m_Addr > MEMORY_LIMIT || m_Addr < 0) && (M_icode == Control.Codes.IRMMOVQ || M_icode == Control.Codes.ICALL || M_icode == Control.Codes.IPUSHQ || M_icode == Control.Codes.IMRMOVQ || M_icode == Control.Codes.IRET || M_icode == Control.Codes.IPOPQ))
        {
            m_state = Control.States.SMEM;
            return;
        }

        if (M_icode == Control.Codes.IPOPQ)
        {
            int deb = 0;
            int nm = deb + 1;
        }

        m_valM = 0;
        if (M_icode == Control.Codes.IRMMOVQ || M_icode == Control.Codes.ICALL || M_icode == Control.Codes.IPUSHQ)
        {
            long nm = M_valA;
            for (int i = 0; i < 8; i++)
            {
                long x = nm % 256;
                mem[m_Addr + i] = (int)x;
                nm /= 256;
            }
        }
        if (M_icode == Control.Codes.IMRMOVQ || M_icode == Control.Codes.IRET || M_icode == Control.Codes.IPOPQ)
        {
            for (int i = 0; i < 8; i++)
            {
                long x = (mem[m_Addr + i]);
                x = x << (i * 8);
                m_valM |= x;
            }
        }

        m_ifun = M_ifun;
        m_dstM = M_dstM;
        m_dstE = M_dstE;
        m_state = M_state;
        m_icode = M_icode;
        m_valE = M_valE;
    }
}
