﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using Core.Comm;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace Core
{
    static class Program
    {
        public static Win.WinInput MIme = null;
 
        public static string ProductVer = "3.1.8";//软件版本

        public class vvclase
        {
            public string txt { get; set; }
            public long num { get; set; }
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region 根据词频调整
            //Dictionary<string, long> dictcp = new Dictionary<string, long>();
            //Dictionary<string, long> blcp = new Dictionary<string, long>();
            //var cplist = File.ReadAllText(@"D:\soft\office\大量中文词库 词性 词频\词典360万.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //foreach(var item in cplist)
            //{
            //    if (item.Trim().Length == 0) continue;
            //    if (!dictcp.ContainsKey(item.Trim().Split('	')[0]))
            //        dictcp.Add(item.Trim().Split('	')[0], long.Parse(item.Trim().Split('	')[item.Trim().Split('	').Length-1]));
            //}
            //cplist = File.ReadAllText(@"D:\soft\office\词库\300万词库词频.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //foreach (var item in cplist)
            //{
            //    if (item.Trim().Length == 0) continue;
            //    if (!dictcp.ContainsKey(item.Trim().Split('	')[0]))
            //    {
            //        long cpnum = 0;
            //        long.TryParse(item.Trim().Split('	')[item.Trim().Split('	').Length - 1], out cpnum);
            //        if (cpnum > 0)
            //            dictcp.Add(item.Trim().Split('	')[0], cpnum);
            //    }
            //    else
            //    {
            //        long cpnum = 0;
            //        long.TryParse(item.Trim().Split('	')[item.Trim().Split('	').Length - 1], out cpnum);
            //        if (dictcp[item.Trim().Split('\t')[0]]==0)
            //            dictcp[item.Trim().Split('\t')[0]] = cpnum;
            //    }
            //}
            //cplist = File.ReadAllText(@"D:\work\srkmm\空明码6万词.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //foreach (var item in cplist)
            //{
            //    if (item.Trim().Length == 0) continue;
            //    if (dictcp.ContainsKey(item.Trim().Split('	')[0]))
            //    {
            //        dictcp[item.Trim().Split('	')[0]] = 10000000000;
            //    }
            //}
            //var bigc = File.ReadAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit.shp", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //StringBuilder ssb = new StringBuilder();
            //foreach (var item in bigc)
            //{
            //    if (item.Trim().Length == 0) continue;
            //    bool cl = true;
            //    string txt = item.Split(' ')[0].Trim();
            //    string txtc = item.Split(' ')[1].Trim();
            //    if (string.IsNullOrEmpty(txt) || txt.Trim().Length==0) continue;


            //    if (txt.Length < 4) cl = false;
            //    else if (item.Split(' ').Length == 2 && txt.Length < 5)
            //        cl = false;
            //    else if (txtc.Length == 1)
            //        cl = false;

            //    if (txt.Substring(0, 1).ToString() == "i" || txt.Substring(0, 1).ToString() == "v" || txt.Substring(0, 1).ToString() == "u"
            //        || txt.Substring(0, 1).ToString() == ";" || txt.Substring(0, 1).ToString() == "7" || txt.Substring(0, 1).ToString() == ":")
            //        cl = false;


            //    if (cl)
            //    {
            //        blcp = new Dictionary<string, long>();
            //        for (int i = 1; i < item.Split(' ').Length; i++)
            //        {
            //            string zstr = item.Split(' ')[i].Trim();
            //            if (zstr.Length == 0) continue;
            //            if (!blcp.ContainsKey(zstr))
            //            {
            //                if (dictcp.ContainsKey(zstr))
            //                {
            //                    blcp.Add(zstr, dictcp[zstr]);
            //                }
            //                else
            //                    blcp.Add(zstr, 0);
            //            }
            //        }
            //        if (blcp.Count > 0)
            //        {

            //            var ll = blcp.OrderByDescending(o => o.Value).ToList();
            //            string temp = "";

            //            for (int j = 0; j < ll.Count; j++)
            //            {
            //                temp += ll[j].Key + " ";

            //            }

            //            ssb.Append(txt + " " + temp.Trim() + "\n");
            //        }
            //    }
            //    else
            //        ssb.Append(item + "\n");

            //}
            //File.WriteAllText(@"D:\work\srkmm\MasterDit_cp.shp", ssb.ToString(), Encoding.UTF8);
            #endregion
            #region 多音字处理
            //var pyck = File.ReadAllText(@"D:\soft\office\词库\[雾凇拼音][20230725][简体 全拼]\合并词库.yaml", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //var dict = File.ReadAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit___多多格式.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //StringBuilder ssb = new StringBuilder();
            //Dictionary<string, string> pykdict = new Dictionary<string, string>();

            //Dictionary<string, string> dictk = new Dictionary<string, string>();
            //foreach (var item in pyck)
            //{
            //    if (item.Length > 1)
            //    {
            //        string str = item.Split('	')[0];
            //        string code = item.Replace(str + "	", "").Split('	')[0];
            //        if (!pykdict.ContainsKey(str) && code.Length > 0 && code.Replace("	", "") != str)
            //        {
            //            pykdict.Add(str, code);
            //        }
            //    }
            //}
            //foreach (var item in dict)
            //{
            //    if (item.Length > 1)
            //    {
            //        string str = item.Split('	')[0];
            //        string code = item.Split('	')[1];
            //        if (str.Length == 3 && str.IndexOf("什么") >= 0 && code.Length > 3)
            //        {
            //            if (str.StartsWith("什么"))
            //            {
            //                code = code.Substring(0, 2) + "m" + code.Substring(3);
            //            }
            //            else if (str.EndsWith("什么"))
            //            {
            //                code = code.Substring(0, code.Length - 1) + "m";
            //            }
            //        }
            //        else if (str.Length == 4 && str.IndexOf("什么") >= 0 && code.Length > 3)
            //        {
            //            if (str.StartsWith("什么"))
            //            {
            //                code = code.Substring(0, 1) + "m" + code.Substring(2);
            //            }
            //            else if (str.EndsWith("什么"))
            //            {
            //                code = code.Substring(0, code.Length - 1) + "m";
            //            }
            //            else
            //            {
            //                string code1 = "";
            //                for (int i = 0; i < str.Length; i++)
            //                {
            //                    if (str.Substring(i, 1) == "么")
            //                    {
            //                        code1 += "m";
            //                    }
            //                    else
            //                        code1 += code.Substring(i, 1);
            //                }
            //                code = code1;
            //            }
            //        }
            //        else if (str.Length > 4 && str.IndexOf("什么") >= 0 && code.Length == str.Length)
            //        {
            //            if (str.StartsWith("什么"))
            //            {
            //                code = code.Substring(0, 1) + "m" + code.Substring(2);
            //            }
            //            else if (str.EndsWith("什么"))
            //            {
            //                code = code.Substring(0, code.Length - 1) + "m";
            //            }
            //            else if (str.Length > 4 && code.Length > 4)
            //            {
            //                string code1 = "";
            //                for (int i = 0; i < str.Length; i++)
            //                {
            //                    if (str.Substring(i, 1) == "么")
            //                    {
            //                        code1 += "m";
            //                    }
            //                    else
            //                        code1 += code.Substring(i, 1);
            //                }
            //                code = code1;
            //            }
            //        }
            //        else if (str.Length > 4 && str.IndexOf("什么") >= 0 && code.Length != str.Length)
            //        {
            //            if (str.StartsWith("什么"))
            //            {
            //                code = code.Substring(0, 1) + "m" + code.Substring(2);
            //            }
            //            else if (str.EndsWith("什么"))
            //            {
            //                code = code.Substring(0, code.Length - 1) + "m";
            //            }
            //        }
            //        if (str.Length > 1 && code.Length > 3)
            //        {
            //            if (pykdict.ContainsKey(str))
            //            {
            //                try
            //                {
            //                    纠正读音
            //                    var py = pykdict[str].Split(' ');
            //                    string code1 = "";

            //                    for (int i = 0; i < str.Length; i++)
            //                    {
            //                        string ym = py[i];
            //                        string flag = "";
            //                        if (ym.StartsWith("a") || ym.StartsWith("o"))
            //                        {
            //                            flag = "a";
            //                        }
            //                        else if (ym.StartsWith("e"))
            //                        {
            //                            flag = "e";
            //                        }
            //                        else
            //                        {

            //                            string sym = "";
            //                            if (ym.Length > 2 &&
            //                                (ym.Substring(0, 2) == "sh" || ym.Substring(0, 2) == "ch" || ym.Substring(0, 2) == "zh"))
            //                            {
            //                                sym = ym.Substring(2, 1);
            //                            }
            //                            else
            //                            {
            //                                sym = ym.Substring(1, 1);
            //                            }

            //                            if (sym.StartsWith("a") || sym.StartsWith("o"))
            //                            {
            //                                flag = "a";
            //                            }
            //                            else if (sym.StartsWith("e"))
            //                            {
            //                                flag = "e";
            //                            }
            //                            else if (sym.StartsWith("i"))
            //                            {
            //                                flag = "i";
            //                            }
            //                            else
            //                                flag = "u";
            //                        }

            //                        if (str.Length == 2)
            //                        {
            //                            if (i == 0)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper() + code.Substring(1, 1).ToLower();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper() + code.Substring(1, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower() + code.Substring(1, 1).ToUpper();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower() + code.Substring(1, 1).ToLower();
            //                            }
            //                            else
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper() + code.Substring(3, 1).ToLower();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper() + code.Substring(3, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower() + code.Substring(3, 1).ToUpper();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower() + code.Substring(3, 1).ToLower();
            //                            }

            //                        }
            //                        else if (str.Length == 3)
            //                        {
            //                            if (i == 0)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper() + code.Substring(1, 1).ToLower();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper() + code.Substring(1, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower() + code.Substring(1, 1).ToUpper();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower() + code.Substring(1, 1).ToLower();
            //                            }
            //                            else if (i == 1)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                            else
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                        }
            //                        else if (str.Length == 4)
            //                        {
            //                            if (flag == "a" || flag == "o")
            //                            {
            //                                code1 += ym.Substring(0, 1).ToUpper();
            //                            }
            //                            else if (flag == "u")
            //                                code1 += ym.Substring(0, 1).ToUpper();
            //                            else if (flag == "e")
            //                                code1 += ym.Substring(0, 1).ToLower();
            //                            else
            //                                code1 += ym.Substring(0, 1).ToLower();
            //                        }
            //                        else if (str.Length > 4 && str.Length == code.Length)
            //                        {
            //                            if (flag == "a" || flag == "o")
            //                            {
            //                                code1 += ym.Substring(0, 1).ToUpper();
            //                            }
            //                            else if (flag == "u")
            //                                code1 += ym.Substring(0, 1).ToUpper();
            //                            else if (flag == "e")
            //                                code1 += ym.Substring(0, 1).ToLower();
            //                            else
            //                                code1 += ym.Substring(0, 1).ToLower();
            //                        }
            //                        else if (str.Length > 4 && code.Length == 4)
            //                        {
            //                            if (i == 0)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                            else if (i == 1)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                            else if (i == 2)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                            else if (i == str.Length - 1)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                        }
            //                        else if (str.Length > 4 && code.Length > 6)
            //                        {
            //                            if (i < 6)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(i, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(i, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(i, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(i, 1).ToLower();
            //                            }
            //                            else if (i == str.Length - 1)
            //                            {
            //                                if (flag == "a" || flag == "o")
            //                                {
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                }
            //                                else if (flag == "u")
            //                                    code1 += ym.Substring(0, 1).ToUpper();
            //                                else if (flag == "e")
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                                else
            //                                    code1 += ym.Substring(0, 1).ToLower();
            //                            }
            //                        }
            //                    }
            //                    if (code1 != "")
            //                        code = code1;
            //                }
            //                catch { }
            //            }
            //        }
            //        string sv = str + "	" + code;
            //        if (!dictk.ContainsKey(sv))
            //        {
            //            dictk.Add(sv, "");
            //            ssb.Append(sv + "\n");
            //        }
            //    }

            //}
            //File.WriteAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit_1.txt", ssb.ToString(), Encoding.UTF8);
            #endregion

            #region 缩减词库
            //Dictionary<string, long> blcp = new Dictionary<string, long>();

            //var blc = File.ReadAllText(@"D:\work\srkmm\10万词频表.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //foreach (var item in blc)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') <= 0) continue;

            //    string txt = item.Split('	')[0].Replace("？", "").Replace("。", "").Replace("！", "").Replace("；", "").Replace("，", "");
            //    if (txt.Length == 1) continue;
            //    if (!blcp.ContainsKey(txt))
            //    {
            //        blcp.Add(txt, 0);

            //    }
            //}
            //blc = File.ReadAllText(@"D:\soft\office\词库\矧码词.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //foreach (var item in blc)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') <= 0) continue;

            //    string txt = item.Split('	')[0].Replace("？", "").Replace("。", "").Replace("！", "").Replace("；", "").Replace("，", "");
            //    if (txt.Length == 1) continue;
            //    if (!blcp.ContainsKey(txt))
            //    {
            //        blcp.Add(txt, 0);
            //    }
            //}
            //var bigc = File.ReadAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit.shp", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //StringBuilder ssb = new StringBuilder();
            //StringBuilder ssb1 = new StringBuilder();
            //foreach (var item in bigc)
            //{
            //    bool cl = true;
            //    string txt = item.Split(' ')[0];
            //    if (string.IsNullOrEmpty(item) || item.Trim() == "" || txt.Length > 4) continue;


            //    if (txt.Length < 4) cl = false;
            //    if (item.Split(' ').Length == 2 && txt.Length < 5) cl = false;

            //    if (item.Split(' ')[1].Length == 1) cl = false;
            //    if (item.Split(' ').Length > 1 && item.Split(' ')[1].Length == 1) cl = false;
            //    if (txt.Substring(0, 1).ToString() == "i" || txt.Substring(0, 1).ToString() == "v" || txt.Substring(0, 1).ToString() == "u"
            //        || txt.Substring(0, 1).ToString() == ";" || txt.Substring(0, 1).ToString() == "7" || txt.Substring(0, 1).ToString() == ":")
            //        cl = false;


            //    if (cl)
            //    {
            //        string item1 = "";
            //        if (txt.Length > 4)
            //        {

            //            for (int i = 1; i < item.Split(' ').Length; i++)
            //            {
            //                if (blcp.ContainsKey(item.Split(' ')[i]))
            //                    item1 += " " + item.Split(' ')[i];
            //            }
            //        }
            //        else
            //        {
            //            item1 += " " + item.Split(' ')[1];
            //            for (int i = 2; i < item.Split(' ').Length; i++)
            //            {
            //                if (blcp.ContainsKey(item.Split(' ')[i]))
            //                    item1 += " " + item.Split(' ')[i];

            //            }
            //        }
            //        if (item1.Length > 0)
            //        {
            //            ssb.Append(txt + " " + item1 + "\n");
            //            ssb1.Append(txt + " " + item1 + "\n");
            //        }
            //    }
            //    else
            //    {
            //        ssb.Append(item + "\n");
            //        string item1 = "";

            //        for (int i = 1; i < item.Split(' ').Length; i++)
            //        {
            //            if (blcp.ContainsKey(item.Split(' ')[i]))
            //                item1 += " " + item.Split(' ')[i];
            //            else if (item.Split(' ')[i].Length == 1)
            //                item1 += " " + item.Split(' ')[i];

            //        }
            //        if (item1.Length > 0)
            //        {
            //            ssb1.Append(txt + " " + item1 + "\n");
            //        }
            //        else if(item.Split(' ').Length > 1)
            //        {
            //            ssb1.Append(txt + " " + item.Split(' ')[1] + "\n");
            //        }
            //    }
            //}
            //File.WriteAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit_s1.shp", ssb.ToString(), Encoding.UTF8);
            //File.WriteAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit_s2.shp", ssb1.ToString(), Encoding.UTF8);
            #endregion

            #region 筛选好词

            //Dictionary<string, long> mdict = new Dictionary<string, long>();
            //Dictionary<string, long> pydict = new Dictionary<string, long>();
            //var bigc = File.ReadAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit___多多格式.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');

            //foreach (var item in bigc)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') <= 0) continue;

            //    string txt = item.Split('	')[0];

            //    if (!mdict.ContainsKey(txt))
            //        mdict.Add(txt, 0);
            //}

            //List<string> newdict = new List<string>();
            //var pyk = File.ReadAllText(@"D:\soft\office\词库\[雾凇拼音][20230725][简体 全拼]\合并词库.yaml", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //StringBuilder ssb = new StringBuilder();
            //foreach (var item in pyk)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') <= 0) continue;

            //    string txt = item.Split('	')[0].Replace("#", "");

            //    if (!pydict.ContainsKey(txt))
            //    {
            //        pydict.Add(txt, 0);

            //        if (!mdict.ContainsKey(txt))
            //        {
            //            int ispy = 1;
            //            newdict.Add(txt + " " + ispy);
            //        }
            //    }
            //}

            //Dictionary<string, long> blcp = new Dictionary<string, long>();
            //var blc = File.ReadAllText(@"D:\soft\office\词库\矧码词.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');

            //foreach (var item in blc)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') <= 0) continue;

            //    string txt = item.Split('	')[0].Replace("？", "").Replace("。", "").Replace("！", "").Replace("；", "").Replace("，", "");
            //    if (txt.Length == 1) continue;
            //    if (!blcp.ContainsKey(txt))
            //    {
            //        blcp.Add(txt, 0);

            //        if (!mdict.ContainsKey(txt))
            //        {
            //            int ispy = pydict.ContainsKey(txt) ? 1 : 0;
            //            newdict.Add(txt + " " + ispy);
            //        }
            //    }
            //}
            //foreach (var item in newdict)
            //{
            //    ssb.Append(item + " " + (blcp.ContainsKey(item.Split(' ')[0]) ? 1 : 0) + "\n");
            //}
            //File.WriteAllText(@"D:\soft\office\词库\矧码词_未加入的词.txt", ssb.ToString(), Encoding.UTF8);
            #endregion

            #region rime词库排序
            //var bigc = File.ReadAllText(@"D:\soft\office\空明码并击_小狼豪\weasel-0.17.4\data\kongmingmas.dict.yaml", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //StringBuilder ssb = new StringBuilder();
            //StringBuilder ssbd = new StringBuilder();
            ////int pos = 100000000;
            //foreach (var item in bigc)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') > 0)
            //    {
            //        if (item.Split('	')[0].Length > 1)
            //            ssb.Append(item + "\r\n");
            //        else
            //        {
            //            string x = item.Split('	')[1];
            //            if (x.Length == 4 && x.EndsWith("="))
            //            {
            //                ssb.Append(item.Split('	')[0] + " " + item.Split('	')[1].Substring(2, 1) + "\t" + item.Split('	')[1].Substring(0, 2) + "\r\n");

            //            }
            //            ssb.Append(item + "\r\n");
            //        }
            //    }
            //    else
            //    {
            //        ssb.Append(item + "\r\n");

            //    }
            //}
            //File.WriteAllText(@"D:\soft\office\空明码并击_小狼豪\weasel-0.17.4\data\kongmingmascz.dict.yaml", ssb.ToString(), Encoding.UTF8);
            //////File.WriteAllText(@"D:\soft\office\空明码并击_小狼豪\weasel-0.17.4\data\kongmingmasdz.dict.yaml", ssbd.ToString(), Encoding.UTF8);
            #endregion

            #region 缩减词库
            //Dictionary<string, long> blcp = new Dictionary<string, long>();

            //var blc = File.ReadAllText(@"D:\soft\office\词库\木已成舟32万词库.txt", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //foreach (var item in blc)
            //{
            //    if (string.IsNullOrEmpty(item)) continue;
            //    if (item.IndexOf('	') <= 0) continue;
            
            //    string txt = item.Split('	')[0].Replace("？", "").Replace("。", "").Replace("！", "").Replace("；", "").Replace("，", "");
            //    if (txt.Length < 5) continue;
            //    if (!blcp.ContainsKey(txt))
            //    {
            //        blcp.Add(txt, 0);

            //    }
            //}
 
            //var bigc = File.ReadAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit.shp", Encoding.UTF8).Replace("\r\n", "\n").Split('\n');
            //StringBuilder ssb = new StringBuilder();

            //foreach (var item in bigc)
            //{
            //    bool cl = true;
            //    string txt = item.Split(' ')[0];
            //    if (string.IsNullOrEmpty(item) || item.Trim() == "") continue;


            //    if (txt.Length < 4) cl = false;
         
            //    if (item.Split(' ')[1].Length == 1) cl = false;
            //    if (item.Split(' ').Length > 1 && item.Split(' ')[1].Length == 1) cl = false;
            //    if (txt.Substring(0, 1).ToString() == "i" || txt.Substring(0, 1).ToString() == "v" || txt.Substring(0, 1).ToString() == "u"
            //        || txt.Substring(0, 1).ToString() == ";" || txt.Substring(0, 1).ToString() == "7" || txt.Substring(0, 1).ToString() == ":")
            //        cl = false;


            //    if (cl)
            //    {
            //        string item1 = "";

            //        for (int i = 1; i < item.Split(' ').Length; i++)
            //        {
            //            if (item.Split(' ')[i] == "哥哥在一起")
            //                ;
            //            if (item.Split(' ')[i].Length > 4)
            //            {
            //                if (blcp.ContainsKey(item.Split(' ')[i]))
            //                    item1 += " " + item.Split(' ')[i];
            //            }
            //            else
            //                item1 += " " + item.Split(' ')[i];
            //        }

            //        if (item1.Length > 0)
            //        {
            //            ssb.Append(txt + " " + item1 + "\n");

            //        }
            //    }
            //    else
            //    {
            //        ssb.Append(item + "\n");

            //    }
            //}
            //File.WriteAllText(@"D:\work\速录宝2.0\dict\空明码宏版\MasterDit_s1.shp", ssb.ToString(), Encoding.UTF8);

            #endregion
            Win.Login login = new Win.Login();
            login.BackgroundImage = System.Drawing.Image.FromFile(System.IO.Path.Combine(Application.StartupPath, "login.png"));
            login.Wait = true;
            login.Show();

            login.Wait = false;


            int iProcessNum = 0;
            foreach (Process singleProc in Process.GetProcesses())
            {
                if (singleProc.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
                    iProcessNum += 1;
                }
            }

            if (iProcessNum <= 1)
            {
                Task stask = new Task(() =>
                {
                    while (true)
                    {
                        System.Threading.Thread.Sleep(30 * 1000);
                        try
                        {
                            if (!string.IsNullOrEmpty(Base.InputMode.AppPath) && Base.InputMode.autodata)
                            {
                                File.WriteAllLines(System.IO.Path.Combine(Base.InputMode.AppPath, "dict", Base.InputMode.CDPath, "mapdatacount.txt")
                                    , new string[] { Win.WinInput.savemapdata() }, Encoding.UTF8);
                            }

                        }
                        catch { }

                        try
                        {
                            Win.DictMrg.savedict();
                        }
                        catch { }
                    }
                });

                stask.Start();
                //不要重复运行程序
                MIme = new Win.WinInput();
                Application.Run(MIme);

            }


        }
 
    }
}
