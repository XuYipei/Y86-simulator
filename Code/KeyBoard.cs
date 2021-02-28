using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    KeyCode currentKey;
    void Start()
    {
        currentKey = KeyCode.Space;
    }
    void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                currentKey = e.keyCode;
                string s = currentKey.ToString();
                if (s == "F10") Next.F10();
                if (s == "F11") Step.F11();
                if (s == "F9") Run.F9();
                if (s == "F6") BreakControl.F6();
                if (s == "F5") Reset.F5();
            }
        }
    }
}
