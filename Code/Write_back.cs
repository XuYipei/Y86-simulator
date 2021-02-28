using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Write_back : MonoBehaviour
{
    static Control.Codes W_icode;
    static Control.States W_state;
    static Control.Registers W_dstE, W_dstM;
    static long W_valE, W_valM, W_ifun;

    static public Control.Registers Show_W_dstE() { return (W_dstE); }
    static public Control.Registers Show_W_dstM() { return (W_dstM); }
    static public long Show_W_valE() { return (W_valE); }
    static public long Show_W_valM() { return (W_valM); }
    static public long Show_W_ifun() { return (W_ifun); }
    static public Control.Codes Show_W_icode() { return (W_icode); }
    static public Control.States Show_W_state() { return (W_state); }

    static public void Updata(Control.States state, Control.Codes icode, long ifun, long valE, long valM, Control.Registers dstE, Control.Registers dstM)
    {
        W_state = state;
        W_icode = icode;
        W_ifun = ifun;
        W_valE = valE;
        W_valM = valM;
        W_dstE = dstE;
        W_dstM = dstM;
    }

    static public void Work()
    {
        if (W_state != Control.States.SAOK)
        {
            Program.Renew(W_state);
            return;
        }

        if (W_dstE != Control.Registers.RNONE && (W_icode == Control.Codes.IRRMOVQ || W_icode == Control.Codes.IIRMOVQ || W_icode == Control.Codes.IOPQ || W_icode == Control.Codes.ICALL || W_icode == Control.Codes.IRET || W_icode == Control.Codes.IPOPQ || W_icode == Control.Codes.IPUSHQ || W_icode == Control.Codes.IIOPQ))
            Decode.Write_reg((int)W_dstE, W_valE);
        if (W_dstM != Control.Registers.RNONE && (W_icode == Control.Codes.IMRMOVQ || W_icode == Control.Codes.IPOPQ))
            Decode.Write_reg((int)W_dstM, W_valM);
    }
}
