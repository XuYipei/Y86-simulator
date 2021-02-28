using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next : MonoBehaviour
{
    public void Click()
    {
        long PC = Fetch.SelectPC();
        int icode= Memory.Read_Mem((int)PC) >> 4;
        if (icode == 0 || icode == 1 || icode == 9) PC++;
        if (icode == 2 || icode == 6 || icode == 0xA || icode == 0XB || icode==0xC ) PC += 2;
        if (icode == 3 || icode == 4 || icode == 5 || icode == 0XB) PC += 10;
        if (icode == 7 || icode == 8) PC += 9;
        while (Fetch.SelectPC() != PC && Program.state == Control.States.SAOK) 
        {
            Program.Step();
            Display.Show();
        }
    }
    static public void F10()
    {
        long PC = Fetch.SelectPC();
        int icode = Memory.Read_Mem((int)PC) >> 4;
        if (icode == 0 || icode == 1 || icode == 9) PC++;
        if (icode == 2 || icode == 6 || icode == 0xA || icode == 0XB || icode == 0xC) PC += 2;
        if (icode == 3 || icode == 4 || icode == 5 || icode == 0XB) PC += 10;
        if (icode == 7 || icode == 8) PC += 9;
        while (Fetch.SelectPC() != PC && Program.state == Control.States.SAOK)
        {
            Program.Step();
            Display.Show();
        }
    }
}

