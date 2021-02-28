using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Read_ys : MonoBehaviour
{
    static bool Check_hex_num(char c)
    {
        if ('0' <= c && c <= '9') return (true);
        if ('a' <= c && c <= 'f') return (true);
        if ('A' <= c && c <= 'F') return (true);
        return (false);
    }
    static bool Check_dec_num(char c) { return ('0' <= c && c <= '9'); }
    static bool Check_char(char c)
    {
        if ('A' <= c && c <= 'Z') return (true);
        if ('a' <= c && c <= 'z') return (true);
        if (c == '_') return (true);
        return (false);
    }
    static int Get_num(char c)
    {
        if ('a' <= c && c <= 'f') return (c - 'a' + 10);
        if ('A' <= c && c <= 'F') return (c - 'A' + 10);
        return (c - '0');
    }
    static bool Compare(string s, int p, string t)
    {
        int l = t.Length;
        if (l + p > s.Length) return (false);
        for (int i = 0; i < l; i++) if (s[p + i] != t[i]) return (false);
        if (l + p == s.Length) return (true);
        if (s[l + p] == ' ' || s[l + p] == '\t') return (true);
        return (false);
    }

    struct Block
    {
        public string name;
        public int place;
    };

    struct Instr
    {
        public string ins;
        public int icode, ifun;
    };
    static Instr[] instr = new Instr[40];
    static void Get_instr()
    {
        instr[0].ins = "halt"; instr[0].icode = 0; instr[0].ifun = 0;
        instr[1].ins = "nop"; instr[1].icode = 1; instr[1].ifun = 0;
        instr[2].ins = "rrmovq"; instr[2].icode = 2; instr[2].ifun = 0;
        instr[3].ins = "cmovle"; instr[3].icode = 2; instr[3].ifun = 1;
        instr[4].ins = "cmovl"; instr[4].icode = 2; instr[4].ifun = 2;
        instr[5].ins = "cmove"; instr[5].icode = 2; instr[5].ifun = 3;
        instr[6].ins = "cmovne"; instr[6].icode = 2; instr[6].ifun = 4;
        instr[7].ins = "cmovge"; instr[7].icode = 2; instr[7].ifun = 5;
        instr[8].ins = "cmovg"; instr[8].icode = 2; instr[8].ifun = 6;
        instr[9].ins = "irmovq"; instr[9].icode = 3; instr[9].ifun = 0;
        instr[10].ins = "rmmovq"; instr[10].icode = 4; instr[10].ifun = 0;
        instr[11].ins = "mrmovq"; instr[11].icode = 5; instr[11].ifun = 0;
        instr[12].ins = "addq"; instr[12].icode = 6; instr[12].ifun = 0;
        instr[13].ins = "subq"; instr[13].icode = 6; instr[13].ifun = 1;
        instr[14].ins = "andq"; instr[14].icode = 6; instr[14].ifun = 2;
        instr[15].ins = "xorq"; instr[15].icode = 6; instr[15].ifun = 3;
        instr[16].ins = "jmp"; instr[16].icode = 7; instr[16].ifun = 0;
        instr[17].ins = "jle"; instr[17].icode = 7; instr[17].ifun = 1;
        instr[18].ins = "jl"; instr[18].icode = 7; instr[18].ifun = 2;
        instr[19].ins = "je"; instr[19].icode = 7; instr[19].ifun = 3;
        instr[20].ins = "jne"; instr[20].icode = 7; instr[20].ifun = 4;
        instr[21].ins = "jge"; instr[21].icode = 7; instr[21].ifun = 5;
        instr[22].ins = "jg"; instr[22].icode = 7; instr[22].ifun = 6;
        instr[23].ins = "call"; instr[23].icode = 8; instr[23].ifun = 0;
        instr[24].ins = "ret"; instr[24].icode = 9; instr[24].ifun = 0;
        instr[25].ins = "pushq"; instr[25].icode = 10; instr[25].ifun = 0;
        instr[26].ins = "popq"; instr[26].icode = 11; instr[26].ifun = 0;
        instr[27].ins = "iaddq"; instr[27].icode = 12; instr[27].ifun = 0;
        instr[28].ins = "isubq"; instr[28].icode = 12; instr[28].ifun = 1;
        instr[29].ins = "iandq"; instr[29].icode = 12; instr[29].ifun = 2;
        instr[30].ins = "ixorq"; instr[30].icode = 12; instr[30].ifun = 3;
    }

    static int Get_reg(string str, ref int p)
    {
        while (str[p] != '%') p++;

        if (str[p + 2] == '8') { p += 3; return (8); }
        if (str[p + 2] == '9') { p += 3; return (9); }
        if (str[p + 2] == '1' && str[p + 3] == '0') { p += 4; return (10); }
        if (str[p + 2] == '1' && str[p + 3] == '1') { p += 4; return (11); }
        if (str[p + 2] == '1' && str[p + 3] == '2') { p += 4; return (12); }
        if (str[p + 2] == '1' && str[p + 3] == '3') { p += 4; return (13); }
        if (str[p + 2] == '1' && str[p + 3] == '4') { p += 4; return (14); }

        if (str[p + 2] == 's' && str[p + 3] == 'p') { p += 4; return (4); }
        if (str[p + 2] == 'b' && str[p + 3] == 'p') { p += 4; return (5); }
        if (str[p + 2] == 's' && str[p + 3] == 'i') { p += 4; return (6); }
        if (str[p + 2] == 'd' && str[p + 3] == 'i') { p += 4; return (7); }

        if (str[p + 2] == 'a') { p += 4; return (0); }
        if (str[p + 2] == 'c') { p += 4; return (1); }
        if (str[p + 2] == 'd') { p += 4; return (2); }
        if (str[p + 2] == 'b') { p += 4; return (3); }

        return (15);
    }
    static public bool fun_name = false;
    static long Get_instant(string str, ref int p)
    {
        int p_ = p, p__ = p;
        long nm = 0, bs = 10, f = 1;
        while (p_ < str.Length && !Check_char(str[p_])) p_++;
        while (p__ < str.Length && str[p__] != '(' && !Check_dec_num(str[p__]))
        {
            if (str[p__] == '-') f = -1;
            p__++;
        }
        if (p_ < p__)
        {
            fun_name = true;
            p = p_;
            while (p < str.Length && Check_char(str[p])) p++;
            return (0);
        }
        else p = p__;
        if (str[p] == '(') return (0);
        if (p + 1 < str.Length && str[p] == '0' && str[p + 1] == 'x')
        {
            p += 2;
            bs = 16;
        }
        if (str[p] == '-') { p++; f = -1; }
        while (p < str.Length && (Check_dec_num(str[p]) || (Check_hex_num(str[p]) && bs == 16)))
        {
            nm = nm * bs + Get_num(str[p]);
            p++;
        }
        return (nm * f);
    }//找到一个立即数,没有fun_name=true

    static int N, M;
    static Block[] A = new Block[10010];
    static Block[] B = new Block[10010];

    static void Write_fun_name(string str, ref int p, int st)
    {
        int p_ = p;
        if (fun_name)
        {
            M++;
            B[M].name = "";
            while (p_ < str.Length && !Check_char(str[p_])) p_++;
            while (p_ < str.Length && Check_char(str[p_]))
            {
                B[M].name = B[M].name + str[p_];
                p_++;
            }
            B[M].place = st;
            fun_name = false;
        }
    }
    static void Get_rB_instant(string str, ref int p, ref int st)
    {
        long nm = 0;
        int p_ = p, p__ = p;
        nm = Get_instant(str, ref p);
        int rA = 15;
        int rB = Get_reg(str, ref p); p__ = p;
        Memory.Write_Mem(st, (rA << 4) + rB); st++;
        p = p_;
        Write_fun_name(str, ref p, st);
        Memory.Write_Mem_Long(st, nm); st += 8;
        p = p__;
    }

    static bool Check(string str, ref int p, int th, ref int st)
    {
        if (!Compare(str, p, instr[th].ins)) return (false);
        p += instr[th].ins.Length;
        Memory.Write_Mem(st, (instr[th].icode << 4) + instr[th].ifun);
        st++;

        long nm = 0;
        int rA = 15, rB = 15, p_ = 0, p__ = 0;
        switch (instr[th].icode)
        {
            case (0):
                break;
            case (1):
                break;
            case (2):
                rA = Get_reg(str, ref p);
                rB = Get_reg(str, ref p);
                Memory.Write_Mem(st, (rA << 4) + rB); st++;
                break;
            case (3):
                Get_rB_instant(str, ref p, ref st);
                break;
            case 4:
                rA = Get_reg(str, ref p); p_ = p;
                nm = Get_instant(str, ref p);
                rB = Get_reg(str, ref p); p__ = p;
                Memory.Write_Mem(st, (rA << 4) + rB); st++;
                p = p_;
                Write_fun_name(str, ref p, st);
                p = p__;
                Memory.Write_Mem_Long(st, nm); st += 8;
                break;
            case 5:
                p_ = p; nm = Get_instant(str, ref p);
                rB = Get_reg(str, ref p);
                rA = Get_reg(str, ref p); p__ = p;
                Memory.Write_Mem(st, (rA << 4) + rB); st++;
                p = p_;
                Write_fun_name(str, ref p, st);
                p = p__;
                Memory.Write_Mem_Long(st, nm); st += 8;
                break;
            case 6:
                rA = Get_reg(str, ref p);
                rB = Get_reg(str, ref p);
                Memory.Write_Mem(st, (rA << 4) + rB); st++;
                break;
            case 7:
                nm = 0;
                B[++M].name = "";
                while (' ' == str[p] || str[p] == '\t') p++;
                while (p < str.Length && ' ' != str[p] && str[p] != '\t')
                {
                    B[M].name = B[M].name + str[p];
                    p++;
                }
                B[M].place = st;
                Memory.Write_Mem_Long(st, nm); st += 8;
                break;
            case 8:
                nm = 0;
                B[++M].name = "";
                while (' ' == str[p] || str[p] == '\t') p++;
                while (p < str.Length && ' ' != str[p] && str[p] != '\t')
                {
                    B[M].name = B[M].name + str[p];
                    p++;
                }
                B[M].place = st;
                Memory.Write_Mem_Long(st, nm); st += 8;
                break;
            case 9:
                break;
            case 10:
                rA = Get_reg(str, ref p);
                rB = 15;
                Memory.Write_Mem(st, (rA << 4) + rB); st++;
                break;
            case 11:
                rA = Get_reg(str, ref p);
                rB = 15;
                Memory.Write_Mem(st, (rA << 4) + rB); st++;
                break;
            case 12:
                Get_rB_instant(str, ref p, ref st);
                break;
        }
        return (true);
    }

    static void Get_Num_Code(string str, ref int i, ref int p, int w)
    {
        int name = i;
        while (i < str.Length && ('0' > str[i] || str[i] > '9')) i++;
        while (name < str.Length && (str[name] != ' ' && str[name] != '\t')) name++;
        while (name < str.Length && (str[name] == ' ' || str[name] == '\t')) name++;
        if (name < i)
        {
            M++;
            B[M].name = "";
            while (name < str.Length && ' ' != str[name] && str[name] != '\t')
            {
                B[M].name = B[M].name + str[name];
                name++;
            }
            i = name;
            B[M].place = p;
            p += w;
            return;
        }

        i += 2;
        int i_ = i, i__ = i;
        while (i_ < str.Length && Check_hex_num(str[i_])) i_++;
        i__ = i_;
        i_--;
        for (int k = 0; k < w; k++)
        {
            long x = (i_ - 1 < i) ? (0) : (Get_num(str[i_ - 1]));
            long y = (i_ < i) ? (0) : (Get_num(str[i_]));
            long nm = (x << 4) + y;
            Memory.Write_Mem(p, (int)nm);
            p++;
            i_ -= 2;
        }
        i = i__;
    }

    static void Get_code_ys(string path)
    {
        System.IO.StreamReader file = new System.IO.StreamReader(path);

        View_Code.Init();

        string line, str = "";
        Get_instr();
        int p = 0, ck = 0, align = 0;
        while ((line = file.ReadLine()) != null)
        {
            str = "";
            int len = line.Length;
            bool sign = false;
            for (int i = 0; i < len; i++)
            {
                if (line[i] == '#') sign = true;
                if (i + 1 < len && line[i] == '/' && line[i + 1] == '*') ck++;
                if (ck == 0 && !sign) str = str + line[i];
                if (i + 1 < len && line[i] == '*' && line[i + 1] == '/') ck--;
            }

            len = str.Length;
            for (int j = 0, i = 0; i < len;)
            {
                for (; i < len && !(Check_char(str[i]) || Check_dec_num(str[i]) || '.' == str[i]); i++) ;
                for (j = i; j < len && str[j] != ' ' && str[j] != '\t'; j++) ;
                if (i >= len) break;

                if (str[i] == '.')
                {
                    int ii = i, pp = p;
                    bool fd = false, ary = true;
                    if (Compare(str, i, ".pos"))
                    {
                        i += 4;
                        p = (int)Get_instant(str, ref i);
                        fd = true;
                        ary = false;
                    }
                    if (Compare(str, i, ".align") && !fd)
                    {
                        i += 6;
                        align = (int)Get_instant(str, ref i);
                        p += (align - p % align) % align;
                        fd = true;
                        ary = false;
                    }
                    if (Compare(str, i, ".quad") && !fd)
                    {
                        Get_Num_Code(str, ref i, ref p, 8);
                        fd = true;
                    }
                    if (Compare(str, i, ".byte") && !fd)
                    {
                        Get_Num_Code(str, ref i, ref p, 1);
                        fd = true;
                    }
                    if (Compare(str, i, ".long") && !fd)
                    {
                        Get_Num_Code(str, ref i, ref p, 4);
                        fd = true;
                    }
                    if (Compare(str, i, ".value") && !fd)
                    {
                        Get_Num_Code(str, ref i, ref p, 2);
                        fd = true;
                    }
                    if (fd && ary)
                    {
                        string cd = "";
                        for (int ix = ii; ix < i; ix++) cd = cd + str[ix];
                        View_Code.Add(pp, p, cd);
                    }
                    if (!fd) break;
                    continue;
                }

                bool find = false;
                for (int k = 0; k < 31; k++)
                {
                    int ii = i, pp = p;
                    if (Check(str, ref i, k, ref p))
                    {
                        find = true;

                        string cd = "";
                        for (int ix = ii; ix < i; ix++) cd = cd + str[ix];
                        View_Code.Add(pp, p, cd);

                        break;
                    }
                }

                if (!find && str[j - 1] == ':')
                {
                    N++;
                    A[N].name = "";
                    for (int k = i; k < j - 1; k++) A[N].name = A[N].name + str[k];
                    A[N].place = p;
                    i = j;

                    View_Code.Add(-1, -1, A[N].name);
                }
                else break;
            }
        }

        for (int j = 1; j <= M; j++)
        {
            bool ps = false;
            for (int i = 1; i <= N; i++)
            {
                bool same = true;
                if (A[i].name.Length != B[j].name.Length) continue;
                for (int k = 0; k < A[i].name.Length; k++)
                    if (A[i].name[k] != B[j].name[k]) { same = false; break; }
                if (same)
                {
                    Memory.Write_Mem_Long(B[j].place, A[i].place);
                    ps = true;
                    break;
                }
            }
            if (!ps)
            {
                int ii = 0;
                long nm = Get_instant(B[j].name, ref ii);
                Memory.Write_Mem_Long(B[j].place, nm);
            }
        }
            

        file.Close();

        View_Code.Display();
    }
    static public void Work(string path)
    {
        Get_code_ys(path);
    }
    static public void Init()
    {
        N = M = 0;
    }
}
