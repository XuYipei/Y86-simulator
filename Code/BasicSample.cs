using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class BasicSample : MonoBehaviour {
    private string _path;

    public void Click()
    {
        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
    }   

    public void WriteResult(string[] paths) {
        string res = "";
        if (paths.Length != 0)
        {
            foreach (var p in paths) res += p;
        }
        GameObject.Find("Canvas/Button_Panel/Input_File").GetComponent<InputField>().text = res;
    }

    public void WriteResult(string path) {
        _path = path;
    }
}
