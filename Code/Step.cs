using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public void Click()
    {
        if (!Run.ck&&Program.state==Control.States.SAOK)
        {
            Program.Step();
        }
            
    }
    static public void F11()
    {
        if (!Run.ck && Program.state == Control.States.SAOK)
        {
            Program.Step();
        }

    }
}
