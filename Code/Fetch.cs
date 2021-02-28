using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : MonoBehaviour
{
    static Control.Codes f_icode;
    static Control.States f_state;
    static Control.Registers f_rA, f_rB;
    static long F_predPC, f_PC, f_ifun, f_predPC, f_valC, f_valP;

    public static Control.States Show_f_state() { return (f_state); }
    public static Control.Codes Show_f_icode() { return (f_icode); }
    public static Control.Registers Show_f_rA() { return (f_rA); }
    public static Control.Registers Show_f_rB() { return (f_rB); }
    public static long Show_f_PC() { return (f_PC); }
    public static long Show_f_valC() { return (f_valC); }
    public static long Show_f_valP() { return (f_valP); }
    public static long Show_f_ifun() { return (f_ifun); }
    public static long Show_f_predPC() { return (f_predPC); }
    public static long Show_F_predPC() { return (F_predPC); }

    public static void Updata(long F_predPC_) { F_predPC = F_predPC_; }

    static void Read_b(ref long a, ref long b)
    {
        a = Memory.Read_Mem((int)f_PC) >> 4;
        b = Memory.Read_Mem((int)f_PC) % 16;
        f_PC++;
    }
    static void Read_d(ref long a)
    {
        for (int i = 0; i < 8; i++)
        {
            long x = Memory.Read_Mem((int)f_PC + i);
            x <<= (i * 8);
            a |= x;
        }
        f_PC += 8;
    }
    static long PredPC()
    {
        if (f_icode == Control.Codes.IJXX || f_icode == Control.Codes.ICALL) return (f_valC);
        return (f_valP);
    }
    public static long SelectPC()
    {
        if (Memory.Show_M_icode() == Control.Codes.IJXX && !Memory.Show_M_Cnd()) return (Memory.Show_M_valA());
        if (Write_back.Show_W_icode() == Control.Codes.IRET) return (Write_back.Show_W_valM());
        return (F_predPC);
    }

    static public void Work()
    {
        long r0 = 0, r1 = 0;
        long valC = 0;

        f_PC = SelectPC();
        Read_b(ref r0, ref r1);
        f_icode = (Control.Codes)r0;
        f_ifun = r1;

        r0 = r1 = 15;
        f_state = Control.States.SAOK;
        switch (f_icode)
        {
            case Control.Codes.IHALT: f_state = Control.States.SHLT; break;
            case Control.Codes.INOP: break;
            case Control.Codes.IRRMOVQ:
                Read_b(ref r0, ref r1);
                break;
            case Control.Codes.IIRMOVQ:
                Read_b(ref r0, ref r1);
                Read_d(ref valC);
                break;
            case Control.Codes.IRMMOVQ:
                Read_b(ref r0, ref r1);
                Read_d(ref valC);
                break;
            case Control.Codes.IMRMOVQ:
                Read_b(ref r0, ref r1);
                Read_d(ref valC);
                break;
            case Control.Codes.IOPQ:
                Read_b(ref r0, ref r1);
                break;
            case Control.Codes.IJXX:
                Read_d(ref valC);
                break;
            case Control.Codes.ICALL:
                Read_d(ref valC);
                break;
            case Control.Codes.IRET:
                break;
            case Control.Codes.IPUSHQ:
                Read_b(ref r0, ref r1);
                break;
            case Control.Codes.IPOPQ:
                Read_b(ref r0, ref r1);
                break;
            case Control.Codes.IIOPQ:
                Read_b(ref r0, ref r1);
                Read_d(ref valC);
                break;
            default:
                f_state = Control.States.SINS;
                f_icode = Control.Codes.IHALT;
                f_ifun = 0;
                break;
        }
        f_rA = (Control.Registers)r0;
        f_rB = (Control.Registers)r1;
        f_valC = valC;

        f_valP = f_PC;
        f_predPC = PredPC();

        if (f_icode != Control.Codes.IOPQ && f_icode != Control.Codes.IJXX && f_icode != Control.Codes.IRRMOVQ && f_icode != Control.Codes.IIOPQ)
        {
            if (f_ifun != 0) f_state = Control.States.SINS;
        }
        else
        {
            if ((f_icode == Control.Codes.IOPQ || f_icode == Control.Codes.IIOPQ) && f_ifun > 3)
                f_state = Control.States.SINS;
            else
                if (f_ifun > 6)
                f_state = Control.States.SINS;
        }
        if ((f_icode == Control.Codes.IRRMOVQ || f_icode == Control.Codes.IRMMOVQ || f_icode == Control.Codes.IMRMOVQ || f_icode == Control.Codes.IOPQ) && (f_rA == Control.Registers.RNONE || f_rB == Control.Registers.RNONE))
            f_state = Control.States.SINS;
        if ((f_icode == Control.Codes.IIRMOVQ || f_icode == Control.Codes.IIOPQ) && (f_rA != Control.Registers.RNONE || f_rB == Control.Registers.RNONE))
            f_state = Control.States.SINS;
        if ((f_icode == Control.Codes.IPUSHQ || f_icode == Control.Codes.IPOPQ) && (f_rA == Control.Registers.RNONE || f_rB != Control.Registers.RNONE))
            f_state = Control.States.SINS;
    }
}
