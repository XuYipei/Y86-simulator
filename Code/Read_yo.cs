using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Read_yo : MonoBehaviour
{
    static int Get_num(char c){
        if ('0' <= c && c <= '9') return (c - '0');
        if ('a' <= c && c <= 'f') return (c - 'a' + 10);
        return (c - 'A' + 10);
    }
    static long Get_long(string s, int p)
    {
        long rs = 0; int d = 0, l = s.Length;
        if (p + 1 < s.Length && s[p] == '0' && s[p + 1] == 'x') p += 2; d = p;
        for (; d < l && ('0' <= s[d] && s[d] <= '9') || ('a' <= s[d] && s[d] <= 'z'); d++) d++;
        for (int j = 0, i = d; i >= p; i -= 2, j++)
        {
            long x = Get_num(s[i]);
            if (i - 1 >= p) x = x + (Get_num(s[i - 1]) << 4);
            rs |= (x << (j * 8));
        }
        return (rs);
    }

    static void Get_code_yo(string path)
    {
        System.IO.StreamReader file = new System.IO.StreamReader(path);
        System.IO.StreamWriter sw = new System.IO.StreamWriter(@"./code.out");
        string line;
        while ((line = file.ReadLine()) != null)
        {
            int len = line.Length, p = 0, d = 0;
            sw.WriteLine(line);
            for (int i = 0; i < len; i++) if (line[i] == ':') { p = i; break; }
            p += 2; d = p;
            while (d < line.Length && ('0' <= line[d] && line[d] <= '9') || ('a' <= line[d] && line[d] <= 'f') || ('A' <= line[d] && line[d] <= 'F')) d++;
            if (p != 2)
            {
                int ps = (Get_num(line[2]) << 8) | (Get_num(line[3]) << 4) | Get_num(line[4]);
                for (int i = p; i < d; i += 2)
                {
                    int x = Get_num(line[i]);
                    int y = Get_num(line[i + 1]);
                    Memory.Write_Mem(ps, (x << 4) + y);
                    ps++;
                }

            }
        }
        file.Close();
        sw.Close();
    }

    static public void Work(string path)
    {
        Get_code_yo(path);
    }
}
