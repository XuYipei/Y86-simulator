using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public void Click()
    {
        Updata.Init_All();
        Read_code.Work(File_Input.path);
    }
    static public void F5()
    {
        Updata.Init_All();
        Read_code.Work(File_Input.path);
    }
}
