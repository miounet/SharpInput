﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Config;
using Core.Win;

namespace Core.Base
{
    public class InputStatusFrm:System.Windows.Forms.Form
    {
        public string inputstr = string.Empty;//当前的
        public string pinputstr = string.Empty;//前一次
        public string input = string.Empty;//本次输入的码元
        public static string zdzjstr = string.Empty;//
                                                    //句
        
        private static InputMode Input = null;
        /// <summary>
        /// 在联想状态
        /// true联想状态
        /// false录入状态
        /// </summary>
        public static bool Dream = false;
        public int ViewType = 0;
        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize
        {
            get { return InputMode.PageSize; }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNum = 1;

   
        /// <summary>
        /// 最近汉字数组
        /// </summary>
        public string[] valuearry;

        /// <summary>
        /// 本屏显示的汉字
        /// </summary>
        public static string[] cachearry;

        [DllImport("user32")]
        private static extern UInt32 SendInput(UInt32 nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("user32")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        /// <summary>
        /// 获取字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            bool en = false;
            char[] c = inputString.ToCharArray();
            for (int i = 0; i < inputString.Length; i++)
            {
                if ((int)c[i] <= 127)
                {
                    en = true;
                    break;
                }


            }

            if (en)
                return inputString.Length;
            else
                return System.Text.Encoding.Default.GetBytes(inputString).Length / 2;
        }
        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="message"></param>
        /// <param name="keysend"></param>
        public static void SendText(string message, string mcode,bool keysend = false)
        {
 
            if (string.IsNullOrEmpty(message)) return;
            Input.IsPressLAlt = false;
            Input.IsPressRAlt = false;
            Input.IsPresAltPos =0;
            if (keysend || message.StartsWith("sendkey"))
            {
                try
                {
                    if (Input.IsChinese == 2 && (message.IndexOf("}") < 0 || message.IndexOf("{") < 0))
                    {

                        if (!InputMode.omeno)
                        {
                            if (message.IndexOf(",") >= 0)
                                message = message.Replace(",", "") + ",";
                            else if (message.IndexOf(".") >= 0)
                                message = message.Replace(".", "") + ".";
                            else if (message.IndexOf("/") >= 0)
                                message = message.Replace("/", "") + "/";
                            else if (message.IndexOf(";") >= 0)
                                message = message.Replace(";", "") + ";";
                            else if (message.IndexOf("'") >= 0)
                                message = message.Replace("'", "") + "'";
                        }

                        for (int i = 0; i < message.Length; i++)
                        {
                            if (message.Substring(i, 1) == " ")
                            {
                                keybd_event((byte)Keys.Space, 0, 0, 0);
                                keybd_event((byte)Keys.Space, 0, 0x2, 0);
                            }
                            else if ("qwertyuiopassdfghjklzxcvbnm".IndexOf(message.Substring(i, 1)) >= 0)
                            {
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), message.Substring(i, 1).ToUpper()))), 0, 0, 0);
                                if (InputMode.zsmode1 > 0 && "qwertyuiopassdfghjklzxcvbnm".IndexOf(message.Substring(i, 1)) >= 0)
                                {
                                    System.Threading.Thread.Sleep(InputMode.zsmode1);
                                }
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), message.Substring(i, 1).ToUpper()))), 0, 0x2, 0);
                            }
                            else if ("qwertyuiopassdfghjklzxcvbnm".ToUpper().IndexOf(message.Substring(i, 1)) >= 0)
                            {
                                if (!Win.WinInput.Input.IsPressShift)
                                    keybd_event((byte)Keys.LShiftKey, 0, 0, 0);
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), message.Substring(i, 1).ToUpper()))), 0, 0, 0);
                                if (InputMode.zsmode1 > 0 && "qwertyuiopassdfghjklzxcvbnm".IndexOf(message.Substring(i, 1)) >= 0)
                                {
                                    System.Threading.Thread.Sleep(InputMode.zsmode1);
                                }
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), message.Substring(i, 1).ToUpper()))), 0, 0x2, 0);
                                if (!Win.WinInput.Input.IsPressShift)
                                    keybd_event((byte)Keys.LShiftKey, 0, 0x2, 0);



                            }
                            else if ("123456789".IndexOf(message.Substring(i, 1)) >= 0)
                            {
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), "D" + message.Substring(i, 1)))), 0, 0, 0);
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), "D" + message.Substring(i, 1)))), 0, 0x2, 0);
                            }
                            else if ("!@#$%^&*()".IndexOf(message.Substring(i, 1)) >= 0)
                            {
                                string ms = message.Substring(i, 1).Replace("!", "1").Replace("@", "2").Replace("#", "3").Replace("$", "4")
                                    .Replace("%", "5").Replace("^", "6").Replace("&", "7").Replace("*", "8").Replace("(", "9").Replace(")", "0");
                                if (!Win.WinInput.Input.IsPressShift)
                                    keybd_event((byte)Keys.LShiftKey, 0, 0, 0);
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), "D" + ms))), 0, 0, 0);
                                keybd_event(((byte)((Keys)Enum.Parse(typeof(Keys), "D" + ms))), 0, 0x2, 0);
                                if (!Win.WinInput.Input.IsPressShift)
                                    keybd_event((byte)Keys.LShiftKey, 0, 0x2, 0);
                            }
                            else if (",<>./?\\|;:'-_=+[]\"`~}{".IndexOf(message.Substring(i, 1)) >= 0)
                            {
                                if (!Win.WinInput.Input.IsPressShift &&
                                    (message.Substring(i, 1) == ">"
                                    || message.Substring(i, 1) == "<"
                                    || message.Substring(i, 1) == ":"
                                    || message.Substring(i, 1) == "_"
                                    || message.Substring(i, 1) == "+"
                                    || message.Substring(i, 1) == "?"
                                    || message.Substring(i, 1) == "{"
                                    || message.Substring(i, 1) == "}"
                                    || message.Substring(i, 1) == "|"
                                    || message.Substring(i, 1) == "~"
                                    ))
                                {
                                    keybd_event((byte)Keys.LShiftKey, 0, 0, 0);

                                }


                                if (message.Substring(i, 1) == "." || message.Substring(i, 1) == ">") keybd_event(190, 0, 0, 0);
                                else if (message.Substring(i, 1) == "," || message.Substring(i, 1) == "<") keybd_event(0xBC, 0, 0, 0);
                                else if (message.Substring(i, 1) == ";" || message.Substring(i, 1) == ":") keybd_event(186, 0, 0, 0);
                                else if (message.Substring(i, 1) == "-" || message.Substring(i, 1) == "_") keybd_event(189, 0, 0, 0);
                                else if (message.Substring(i, 1) == "=" || message.Substring(i, 1) == "+") keybd_event(187, 0, 0, 0);
                                else if (message.Substring(i, 1) == "/" || message.Substring(i, 1) == "?") keybd_event(191, 0, 0, 0);
                                else if (message.Substring(i, 1) == "'" || message.Substring(i, 1) == "\"") keybd_event((byte)Keys.Oem7, 0, 0, 0);
                                else if (message.Substring(i, 1) == "[" || message.Substring(i, 1) == "{") keybd_event(219, 0, 0, 0);
                                else if (message.Substring(i, 1) == "]" || message.Substring(i, 1) == "}") keybd_event(221, 0, 0, 0);
                                else if (message.Substring(i, 1) == "\\" || message.Substring(i, 1) == "|") keybd_event((byte)Keys.Oem5, 0, 0, 0);
                                else if (message.Substring(i, 1) == "`" || message.Substring(i, 1) == "~") keybd_event((byte)Keys.Oemtilde, 0, 0, 0);


                                if (message.Substring(i, 1) == "." || message.Substring(i, 1) == ">") keybd_event(190, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "," || message.Substring(i, 1) == "<") keybd_event(0xBC, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == ";" || message.Substring(i, 1) == ":") keybd_event(186, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "-" || message.Substring(i, 1) == "_") keybd_event(189, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "=" || message.Substring(i, 1) == "+") keybd_event(187, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "/" || message.Substring(i, 1) == "?") keybd_event(191, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "'" || message.Substring(i, 1) == "\"") keybd_event((byte)Keys.Oem7, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "[" || message.Substring(i, 1) == "{") keybd_event(219, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "]" || message.Substring(i, 1) == "}") keybd_event(221, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "\\" || message.Substring(i, 1) == "|") keybd_event((byte)Keys.Oem5, 0, 0x2, 0);
                                else if (message.Substring(i, 1) == "`" || message.Substring(i, 1) == "~") keybd_event((byte)Keys.Oemtilde, 0, 0x2, 0);

                                if (!Win.WinInput.Input.IsPressShift &&
                                     (message.Substring(i, 1) == ">"
                                            || message.Substring(i, 1) == "<"
                                            || message.Substring(i, 1) == ":"
                                            || message.Substring(i, 1) == "_"
                                            || message.Substring(i, 1) == "+"
                                            || message.Substring(i, 1) == "?"
                                            || message.Substring(i, 1) == "{"
                                            || message.Substring(i, 1) == "}"
                                            || message.Substring(i, 1) == "|"
                                            || message.Substring(i, 1) == "~"
                                     ))
                                {
                                    keybd_event((byte)Keys.LShiftKey, 0, 0x2, 0);
                                }
                            }
                            else
                                SendKeys.Send(message.Substring(i, 1));
                        }

                    }
                    else if (Input.IsChinese == 2 && message.IndexOf("}") >= 0)
                    {
                        if (message.ToLower().IndexOf("backspace") > 0)
                        {
                            keybd_event((byte)Keys.Back, 0, 0, 0);
                            keybd_event((byte)Keys.Back, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("home") > 0)
                        {
                            keybd_event((byte)Keys.Home, 0, 0, 0);
                            keybd_event((byte)Keys.Home, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("escape") > 0)
                        {
                            keybd_event((byte)Keys.Escape, 0, 0, 0);
                            keybd_event((byte)Keys.Escape, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("tab") > 0)
                        {
                            keybd_event((byte)Keys.Tab, 0, 0, 0);
                            keybd_event((byte)Keys.Tab, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("left") > 0)
                        {
                            keybd_event((byte)Keys.Left, 0, 0, 0);
                            keybd_event((byte)Keys.Left, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("right") > 0)
                        {
                            keybd_event((byte)Keys.Right, 0, 0, 0);
                            keybd_event((byte)Keys.Right, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("up") > 0)
                        {
                            keybd_event((byte)Keys.Up, 0, 0, 0);
                            keybd_event((byte)Keys.Up, 0, 0x2, 0);
                        }
                        else if (message.ToLower().IndexOf("down") > 0)
                        {
                            keybd_event((byte)Keys.Down, 0, 0, 0);
                            keybd_event((byte)Keys.Down, 0, 0x2, 0);
                        }
                        else
                            SendKeys.Send(message.StartsWith("sendkey") ? message.Split(':')[1] : message);
                    }
                    else
                    {
                        if (message.StartsWith("sendkey"))
                        {
                            SendKeys.Send(message.Split(':')[1]);
                        }
                        else if (message.IndexOf("}") > 0 && message.IndexOf("{") >= 0)
                        {
                            SendKeys.Send(message);
                        }
                        else
                        {
                            if (InputMode.imgsend)
                            {

                                ImageInput.imgstr += message;

                                if (message == " ")
                                {
                                    if (InputMode.zjsend)
                                    {
                                        SendKeys.Send(ImageInput.imgstr.Trim());
                                    }
                                    else
                                    {
                                        var img = MTextToBitmap(ImageInput.imgstr, new Font(InputMode.SkinFontName, InputMode.SkinFontSize < 16 ? 16 : InputMode.SkinFontSize)
                                    , Color.White, GetColor());

                                        Clipboard.SetImage(img);

                                        //发送ctrl+v 进行粘贴
                                        keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                                        keybd_event((byte)Keys.V, 0, 0, 0);
                                        keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                                        keybd_event((byte)Keys.V, 0, 0x2, 0);
                                    }


                                    ImageInput.imgstr = String.Empty;
                                }

                            }
                            else
                            {
                                if (message == " ")
                                {
                                    Win.WinInput.zzspace = true;

                                    //发送空格
                                    keybd_event((byte)Keys.Space, 0, 0, 0);//按下
                                    keybd_event((byte)Keys.Space, 0, 0x2, 0);//松开

                                    Win.WinInput.zzspace = false;
                                }
                                else
                                {
                                    SendKeys.Send(message);
                                }
                            }
                        }

                    }
                }
                catch { }
                return;
            }
            if (Input.OutType == 0)
            {
                if (message.Trim().Length == 1)
                    Win.WinInput.inputdznum++;
                else if (message.Trim().Length > 1)
                {
                    Win.WinInput.inputcznum++;
                    Win.WinInput.inputczsnum += message.Trim().Length;
                }

                if (InputMode.imgsend)
                {
                    ImageInput.imgstr += message;

                    if (message == " ")
                    {
                        //图片输出
                        if (InputMode.zjsend)
                        {
                            if (InputMode.outtype == 0)
                            {
                                SendKeys.Send(ImageInput.imgstr.Trim());
                            }
                            else if (InputMode.outtype == 1)
                            {
                                try
                                {
                                    Input.SelfOut = true;
                                    Clipboard.SetText(ImageInput.imgstr.Trim());
                                    //发送ctrl+v 进行粘贴
                                    keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                                    keybd_event((byte)Keys.V, 0, 0, 0);
                                    keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                                    keybd_event((byte)Keys.V, 0, 0x2, 0);

                                }
                                catch { }
                                finally { Input.SelfOut = false; }
                            }
                            else
                            {
                                try
                                {

                                    Input.SelfOut = true;
                                    SendKeys.Send(ImageInput.imgstr.Trim());

                                }
                                catch { }
                                finally { Input.SelfOut = false; }

                            }
                           
                        }
                        else
                        {
                            var img = MTextToBitmap(ImageInput.imgstr, new Font(InputMode.SkinFontName, InputMode.SkinFontSize < 16 ? 16 : InputMode.SkinFontSize)
                        , Color.White, GetColor());

                            Clipboard.SetImage(img);
                            //发送ctrl+v 进行粘贴
                            keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                            keybd_event((byte)Keys.V, 0, 0, 0);
                            keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                            keybd_event((byte)Keys.V, 0, 0x2, 0);
                        }
                      

                        ImageInput.imgstr = String.Empty;
                    }

                }
                else
                {
                    if (InputMode.outtype == 0)
                    {
                        if (message == " ")
                        {
                            Win.WinInput.zzspace = true;

                            //发送空格
                            keybd_event((byte)Keys.Space, 0, 0, 0);//按下
                            keybd_event((byte)Keys.Space, 0, 0x2, 0);//松开

                            Win.WinInput.zzspace = false;
                        }
                        else
                        {
                            INPUT[] input_down = new INPUT[message.Length];
                            INPUT[] input_up = new INPUT[message.Length];
                            for (int i = 0; i < message.Length; i++)
                            {
                                input_down[i].type = (int)InputType.INPUT_KEYBOARD;
                                input_down[i].ki.dwFlags = (int)KEYEVENTF.UNICODE;
                                input_down[i].ki.wScan = (ushort)message[i];
                                input_down[i].ki.wVk = 0;
                                input_up[i].type = input_down[i].type;
                                input_up[i].ki.wScan = input_down[i].ki.wScan;
                                input_up[i].ki.wVk = 0;
                                input_up[i].ki.dwFlags = (int)(KEYEVENTF.KEYUP | KEYEVENTF.UNICODE);
                            }
 
                            for (int i = 0; i < input_down.Length; i++)
                            {
                                SendInput(1, ref input_down[i], Marshal.SizeOf(input_down[i]));//keydown 
                                SendInput(1, ref input_up[i], Marshal.SizeOf(input_up[i]));//keyup    
                            }
                        }
                    }
                    else if (InputMode.outtype == 1)
                    {
                        try
                        {
                            Input.SelfOut = true;
                            Clipboard.SetText(message);
                            //发送ctrl+v 进行粘贴
                            keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                            keybd_event((byte)Keys.V, 0, 0, 0);
                            keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                            keybd_event((byte)Keys.V, 0, 0x2, 0);

                        }
                        catch { }
                        finally { Input.SelfOut = false; }
                    }
                    else
                    {
                        try
                        {

                            Input.SelfOut = true;
                            SendKeys.Send(message);

                        }
                        catch { }
                        finally { Input.SelfOut = false; }

                    }
                }
            }
 
            
            LastSPValue = message;
            LastLinkString += message;
            LastLinkCodeString += mcode;
            if (Input.IsChinese == 1 && !CheckChinese(message, true))
            {
                LastLinkString = string.Empty;
                if (Dream)
                {
                    Input.Show = false;
                    Dream = false;
                }

                AutoZJ();
            }
            else if (Input.IsChinese == 0 && !IsLowerLetter(message) && !IsUpperLetter(message))
            {
                LastLinkString = string.Empty;
                if (Dream)
                {
                    Dream = false;
                    Input.Show = false;
                }
            }
            if (Input.IsChinese == 1 && LastLinkString.Length > 4) LastLinkString = LastLinkString.Substring(LastLinkString.Length-4);
            if (Input.IsChinese == 1 && LastLinkCodeString.Length > 4) LastLinkCodeString = LastLinkCodeString.Substring(LastLinkCodeString.Length - 4);

        }

        //自动造4字以上的句，词作为联想字库，输入重请后消失。
        public static void AutoZJ()
        {
            if (InputMode.AutoZJ)
            {
                zdzjstr = zdzjstr.Trim();
                if (zdzjstr.Length > 3)
                {
                    if (!Input.linkdictp.ContainsKey(zdzjstr.Substring(0, 3)))
                    {
                        Input.linkdictp.Add(zdzjstr.Substring(0, 3), new List<string>() {  "#"+zdzjstr});
                    }
                    
                }
            }
            zdzjstr = string.Empty;
        }
        /// <summary>
        /// 判定是否为汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckChinese(string str, bool sp = false)
        {
            bool vv = Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
            if (sp) zdzjstr += str;
            return vv;
        }
        /// <summary>
        /// 判断输入的编码是否是26个小写字母
        /// </summary>
        /// <param name="codechar"></param>
        /// <returns></returns>
        public static bool IsLowerLetter(string codechar)
        {
           
            return System.Text.RegularExpressions.Regex.IsMatch(codechar, "[a-z]");
        }

        /// <summary>
        /// 判断输入的编码是否是26个大写字母
        /// </summary>
        /// <param name="codechar"></param>
        /// <returns></returns>
        public static bool IsUpperLetter(string codechar)
        {
          
            return System.Text.RegularExpressions.Regex.IsMatch(codechar, "[A-Z]");
        }
        /// <summary>
        /// 上屏
        /// </summary>像
        /// <param name="pos"></param>
        public void ShangPing(int pos, int index = 0, bool clear = true,string end="")
        {
            pinyipos = 0;
            this.pinputstr = this.inputstr;
            int tpos = pos != 0 ? pos - 1 : pos;
            if (InputStatusFrm.cachearry == null ||
                (InputStatusFrm.cachearry.Length > 0 && string.IsNullOrEmpty(InputStatusFrm.cachearry[0])))
            {
                Clear();
                return;
            }

            if (InputStatusFrm.Dream && tpos > 0 && !string.IsNullOrEmpty(InputStatusFrm.cachearry[tpos]) && InputStatusFrm.cachearry[tpos].Split('|')[1].StartsWith("#"))
            {
                SendText(InputStatusFrm.cachearry[tpos].Split('|')[1].Substring(4), input);
                LSView = false;
                inputstr = string.Empty;
                input = string.Empty;
                Clear();
                return;
            }
            for (int i = 0; i < InputStatusFrm.cachearry.Length; i++)
            {
                if (pos == 0)
                {
                    if (string.IsNullOrEmpty(InputStatusFrm.cachearry[i]))
                    {
                        if (LSView)
                        {
                            if (!InputMode.imgsend)
                                for (int j = 0; j < StrLength(InputStatusFrm.cachearry[0].Split('|')[1]); j++)
                                    InputStatusFrm.SendText("{BACKSPACE}", "", true);
                            else
                            {

                                for (int j = 0; j < InputStatusFrm.cachearry[0].Split('|')[1].Length; j++)
                                {
                                    if (ImageInput.imgstr.Length > 0)
                                    {
                                        if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                                        else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                                    }
                                }

                            }
                        }
                        SendText(InputStatusFrm.cachearry[i - 1].Split('|')[1].Substring(index) + end, input);
                        break;
                    }
                    else if (i == PageSize - 1)
                    {
                        if (LSView)
                        {
                            if (!InputMode.imgsend)
                                for (int j = 0; j < StrLength(InputStatusFrm.cachearry[0].Split('|')[1]); j++)
                                    InputStatusFrm.SendText("{BACKSPACE}", "", true);
                            else
                            {

                                for (int j = 0; j < InputStatusFrm.cachearry[0].Split('|')[1].Length; j++)
                                {
                                    if (ImageInput.imgstr.Length > 0)
                                    {
                                        if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                                        else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                                    }
                                }

                            }
                        }
                        SendText(InputStatusFrm.cachearry[i].Split('|')[1].Substring(index) + end, input);
                        break;
                    }
                }
                else if (tpos >= PageSize)
                {
                    if (!string.IsNullOrEmpty(InputStatusFrm.cachearry[PageSize - 1 - i]))
                    {
                        if (LSView)
                        {
                            if (!InputMode.imgsend)
                                for (int j = 0; j < StrLength(InputStatusFrm.cachearry[0].Split('|')[1]); j++)
                                    InputStatusFrm.SendText("{BACKSPACE}", "", true);
                            else
                            {

                                for (int j = 0; j < InputStatusFrm.cachearry[0].Split('|')[1].Length; j++)
                                {
                                    if (ImageInput.imgstr.Length > 0)
                                    {
                                        if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                                        else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                                    }
                                }

                            }
                        }
                        SendText(InputStatusFrm.cachearry[PageSize - 1 - i].Split('|')[1].Substring(index) + end, input);
                        break;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(InputStatusFrm.cachearry[tpos]))
                    {
                        if (LSView)
                        {
                            if (!InputMode.imgsend)
                            {
                                if (InputMode.select3)
                                {
                                    for (int j = 0; j < StrLength(InputStatusFrm.LastSPValue); j++)
                                        InputStatusFrm.SendText("{BACKSPACE}", "", true);
                                }
                                else
                                {
                                    for (int j = 0; j < StrLength(InputStatusFrm.cachearry[0].Split('|')[1]); j++)
                                        InputStatusFrm.SendText("{BACKSPACE}", "", true);
                                }
                            }
                            else
                            {

                                for (int j = 0; j < InputStatusFrm.cachearry[0].Split('|')[1].Length; j++)
                                {
                                    if (ImageInput.imgstr.Length > 0)
                                    {
                                        if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                                        else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                                    }
                                }

                            }
                        }
                        if (InputStatusFrm.Dream)
                            index = 0;
                        SendText(InputStatusFrm.cachearry[tpos].Split('|')[1].Substring(index)+end, input);
                        break;
                    }
                    else if (!string.IsNullOrEmpty(InputStatusFrm.cachearry[PageSize - 1 - i]))
                    {
                        if (LSView)
                        {
                            if (!InputMode.imgsend)
                                for (int j = 0; j < StrLength(InputStatusFrm.cachearry[0].Split('|')[1]); j++)
                                    InputStatusFrm.SendText("{BACKSPACE}", "", true);
                            else
                            {

                                for (int j = 0; j < InputStatusFrm.cachearry[0].Split('|')[1].Length; j++)
                                {
                                    if (ImageInput.imgstr.Length > 0)
                                    {
                                        if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                                        else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                                    }
                                }

                            }
                        }
                        SendText(InputStatusFrm.cachearry[PageSize - 1 - i].Split('|')[1].Substring(index)+end, input);
                        break;
                    }
                }

            }
            if (clear)
            {
                if (InputMode.smautoadd 
                    && inputstr.Length==0 && pinputstr.Length==4 && tpos > 0 
                    && InputStatusFrm.cachearry.Length> tpos
                    && !string.IsNullOrEmpty(InputStatusFrm.cachearry[tpos])
                    && InputStatusFrm.cachearry[tpos].Split('|')[1].Length > 0
                    && InputStatusFrm.cachearry[tpos].Split('|')[1].Length <=4)
                {
                    //自动添加第三码空缺词
                   var  tvar = Input.GetInputValue(this.pinputstr.Substring(0,3), false, 1);
                    if (tvar != null && tvar.Length > 0 && tvar[0].Split('|')[1].Length > 0
                        && tvar[0].Split('|')[2].Length > 0 && LastSPValue.Length > 1)
                    {
                        Task autoadd = new Task(() =>
                        {
                            try
                            {
                                int first = 0, last = Core.Win.WinInput.Input.MasterDit.Length - 1;


                                string inputstr = this.pinputstr.Substring(0, 3);
                                string inputvalue = LastSPValue;
                                int pos1 = 0;
                                bool newadd = false;
                                string[] mdict = null;

                                PosIndex poi = Core.Win.WinInput.Input.DictIndex.GetPos(inputstr, ref mdict, false);
                                if (poi == null)
                                {
                                    return;
                                }
                                first = poi.Start;
                                last = poi.End;

                                if (mdict == null) mdict = Core.Win.WinInput.Input.MasterDit;

                                for (int i = first; i <= last; i++)
                                {

                                    if (mdict[i].Split(' ')[0] == inputstr)
                                    {
                                        for (int j = 1; j < mdict[i].Split(' ').Length; j++)
                                        {
                                            if (inputvalue == mdict[i].Split(' ')[j].Trim())
                                            {

                                                return;
                                            }
                                        }
                                        newadd = false;
                                        pos1 = i;
                                    }
                                }
                                if (pos1 == 0)
                                {
                                    newadd = true;
                                    pos1 = last;
                                }
                                if (pos1 > 0)
                                {
                                    if (newadd)
                                    {

                                        if (inputvalue.Length == 1 && inputstr.Length > 1)
                                            poi = Core.Win.WinInput.Input.DictIndex.GetPos(inputstr.Substring(0, 2), ref mdict, false);//单字情况
                                        if (mdict == null) mdict = Core.Win.WinInput.Input.MasterDit;
                                        int posint = 0;
                                        for (int i = poi.Start; i <= poi.End; i++)
                                        {
                                            if (mdict[i].Split(' ')[1].Length == 1 && mdict[i].Split(' ')[0].Length > 1
                                                && mdict[i].Split(' ')[0].Substring(0, 2) == inputstr.Substring(0, 2))
                                            {
                                                pos1 = i;
                                                if (posint == 0)
                                                    posint++;
                                            }
                                            if (posint > 0)
                                                posint++;

                                            if (posint > 100) break;
                                        }
                                        if (inputvalue.Length == 1 && inputstr.Length > 1 && pos1 > poi.Start)
                                            pos1--;

                                        var list = Core.Win.WinInput.Input.MasterDit.ToList();
                                        list.Insert(pos1 + 1, inputstr + " " + inputvalue);
                                        Core.Win.WinInput.Input.MasterDit = list.ToArray();

                                        //初始化索引
                                        List<PosIndex> posl = new List<PosIndex>();
                                        Core.Win.WinInput.Input.CreateIndex(Core.Win.WinInput.Input.MasterDit, ref posl, 1, 0, Core.Win.WinInput.Input.MasterDit.Length);
                                        Core.Win.WinInput.Input.DictIndex.IndexList = posl;

                                        if (DictMrg.orderby && File.Exists(System.IO.Path.Combine(InputMode.AppPath, "dict", InputMode.CDPath, "Setting.shp")))
                                        {
                                            var setting = File.ReadAllLines(System.IO.Path.Combine(InputMode.AppPath, "dict", InputMode.CDPath, "Setting.shp"), Encoding.UTF8);//读配置

                                            DictMrg.orderby = string.IsNullOrEmpty(SetInfo.GetValue("orderby", setting)) ? true : bool.Parse(SetInfo.GetValue("orderby", setting));

                                        }
                                        if (DictMrg.orderby)
                                        {
                                            //保存词库
                                            List<string> dlist = new List<string>();
                                            var iteml = Core.Win.WinInput.Input.MasterDit.ToList();

                                            foreach (var ind in Core.Win.WinInput.Input.DictIndex.IndexList)
                                            {
                                                var item = iteml.FindAll(f => f.Split(' ')[0] == ind.Letter);
                                                if (item != null) dlist.AddRange(item);
                                                if (ind.mdict != null && ind.mdict.Length > 0)
                                                    dlist.AddRange(ind.mdict.ToList());
                                            }

                                            File.WriteAllLines(Core.Win.WinInput.Input.MasterDitPath, dlist.Where(t => t.Length > 0).ToArray(), Encoding.UTF8);
                                            DictMrg.orderby = false;
                                        }
                                        else
                                        {
                                            File.WriteAllLines(Core.Win.WinInput.Input.MasterDitPath, Core.Win.WinInput.Input.MasterDit.Where(t => t.Length > 0), Encoding.UTF8);
                                        }
                                    }


                                }
                            }
                            catch
                            {

                            }
                        });
                        autoadd.Start();
                    }
                }
                LSView = false;
                inputstr = string.Empty;
                input = string.Empty;
                Clear();
            }
            else
                LSView = true;

            if (index > 0)
            {
                Dream = false;
                LastLinkString = string.Empty;
            }
            else if ((LastLinkString.Length > 2 || (Input.IsChinese == 0 && LastLinkString.Length > 0)))
                GetDreamValue(LastLinkString);
        }

        public void Clear()
        {
             inputstr = string.Empty;
            input = string.Empty;
            HideWindow();
        }
        public void ClearOnly()
        {
            inputstr = string.Empty;
            input = string.Empty;
 
        }
        public InputStatusFrm(InputMode input)
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Text = string.Empty;
            this.TopLevel = true;
            this.TopMost = true;
            this.Width = 175;
            this.Height = 133;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ControlBox = true;
           
            Input = input;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            UpdateStyles();

            //this.Show();
        }
        private const int WM_MOUSEACTIVATE = 0x21;
        private const int MA_NOACTIVATE = 3;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = new IntPtr(MA_NOACTIVATE);
                return;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 删除最后上屏的字词
        /// </summary>
        public void DelLastInput()
        {
            if (!InputMode.imgsend || ImageInput.imgstr.Length==0)
            {
                if (LastSPValue.Length > 0)
                    for (int j = 0; j < StrLength(LastSPValue); j++)
                        SendText("{BACKSPACE}", "", true);
                else
                    SendText("{BACKSPACE}", "", true);
            }
            else
            {
                if (LastSPValue.Length > 0)
                {
                    for (int j = 0; j < LastSPValue.Length; j++)
                        if (ImageInput.imgstr.Length > 0)
                        {
                            if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                            else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                        }
                }
                else
                {
                    if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                    else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                }
            }
            LastSPValue = string.Empty;
            LastLinkString = string.Empty;
        }
        /// <summary>
        /// 删除指定字数
        /// </summary>
        public void DelLastInput(int num)
        {
            if (!InputMode.imgsend)
            {
                for (int j = 0; j < num; j++)
                    SendText("{BACKSPACE}", "", true);
            }
            else
            {
                for (int j = 0; j < num; j++)
                    if (ImageInput.imgstr.Length > 0)
                    {
                        if (ImageInput.imgstr.Length - 1 <= 0) ImageInput.imgstr = String.Empty;
                        else ImageInput.imgstr = ImageInput.imgstr.Substring(0, ImageInput.imgstr.Length - 1);
                    }
            }
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hWnd, short cmdShow);
        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        private const int SW_SHOWNOACTIVATE = 4;
        const int SW_HIDE = 0;  
        private const int SWP_NoActiveWINDOW = 0x10;
        private const int HWND_TOPMOST = -1;
        /// <summary>
        /// view input
        /// </summary>
        public void ShowWindow(bool f)
        {
            Win.WinInput.InputStatus.Left = this.Left;
            Win.WinInput.InputStatus.Top = this.Top;
            Win.WinInput.InputStatus.Left = this.Left;
            Win.WinInput.ImageInput.Top = Win.WinInput.InputStatus.Top - this.Height;
            Win.WinInput.ImageInput.Left = this.Left;
            Input.ShowInput(f);
            if (!this.Visible && f)
            {
                ShowWindow(this.Handle, SW_SHOWNOACTIVATE);
                SetWindowPos(this.Handle, HWND_TOPMOST, this.Left, this.Top, this.Width, this.Height, SWP_NoActiveWINDOW);
            }
            else if (!f)
            {

                this.Hide();
                ShowWindow(this.Handle, SW_HIDE);

            }
        }

        public void HideByApi()
        {
            //this.Hide();
            //ShowWindow(this.Handle, SW_HIDE);
        }
        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public void HideWindow()
        {

            Input.ClearShow();

            this.ShowWindow(false);
        }
        /// <summary>
        /// 上一次的字词
        /// </summary>
        public static string PreFirstValue = string.Empty;
        /// <summary>
        /// 最后一次上屏的字词
        /// </summary>
        public static string LastSPValue = string.Empty;
        /// <summary>
        /// 最后上屏的四个字
        /// </summary>
        public static string LastLinkString = string.Empty;
        /// <summary>
        /// 最后上屏的四个编码
        /// </summary>
        public static string LastLinkCodeString = string.Empty;
        public static string LastLinkNum = string.Empty;
        public static bool LSView = false;
        public static bool three_no2=false;
        /// <summary>
        /// 显示候选框的汉字
        /// smspace  
        /// </summary>
        public void ShowInput(bool sp, bool clear = true, int ncount = 0, bool smspace = false, bool alt = false, int spp = 1, bool isright = false
            , int selectpos = 0)//可上屏
        {
            if (LSView)
            {
                LSView = false;
            }
            this.HideByApi();
            Dream = false;
            PageNum = 1;
            valuearry = Input.GetInputValue(this.inputstr,false,0,isright);
            int count = 0;
            PreFirstValue = string.Empty;
            if (valuearry != null)
            {
                cachearry = new string[PageSize];
                for (int i = 0; i < PageSize && i < valuearry.Length; i++)
                {
                    try
                    {
                        if (Input.IsChinese==1 && (spp==1 || spp==3) && InputMode.spaceaout == 3 && !string.IsNullOrEmpty(valuearry[i]))
                        {
                            if(this.inputstr.Length>2 && selectpos == 3)
                            {
                                Input.IsPresAltPos = 2;
                                //selectpos = 2;
                            }
                            else if (this.inputstr.Length > 2 || (this.inputstr.Length <= 2 && valuearry[i].Split('|')[1].Length > 1 && valuearry.Length<5))
                            {

                                if (selectpos == 2)
                                {
                                    valuearry[i] = valuearry[i].Replace("|" + valuearry[i].Split('|')[1] + "|", "|" + valuearry[i].Split('|')[1] + "。|");
                                    if (this.inputstr.Length > 2 || valuearry.Length == 1)
                                    {
                                        Input.IsPresAltPos = 0;
                                        if (valuearry.Length == 1)
                                            alt = false;
                                        else if (this.inputstr.Length == 3)
                                            sp = true;
                                    }
                                    else if (valuearry[i].Length >= selectpos && valuearry[selectpos - 1].Split('|')[1].Length > 1)
                                    {
                                        Input.IsPresAltPos = 0;
                                    }
                                    spp = 1;
                                }
                                else if (selectpos == 4)
                                {

                                    //valuearry[i] = valuearry[i].Replace("|" + valuearry[i].Split('|')[1] + "|", "|" + valuearry[i].Split('|')[1] + "、|");
                                    valuearry[i] = valuearry[i].Replace("|" + valuearry[i].Split('|')[1] + "|", "|" + valuearry[i].Split('|')[1] + "，|");
                                    Input.IsPresAltPos = 0;
                                    alt = false;
                                    if (this.inputstr.Length == 3)
                                        sp = true;
                                }
                                else if (smspace && !alt)
                                {
                                    valuearry[i] = valuearry[i].Replace("|" + valuearry[i].Split('|')[1] + "|", "|" + valuearry[i].Split('|')[1] + "，|");
                                    Input.IsPresAltPos = 0;
                                    spp = 1;
                                }
                            }
                            else if (this.inputstr.Length <= 2 && valuearry[i].Split('|')[1].Length == 1)
                            {
                                if (selectpos == 4)
                                {
                                    Input.IsPresAltPos = 0; spp = 1;
                                    if (valuearry[0].Split('|')[1].Length == 1)
                                        sp = true;
                                    valuearry[i] = valuearry[i].Replace("|" + valuearry[i].Split('|')[1] + "|", "|" + valuearry[i].Split('|')[1] + "，|");

                                }
                            }
                        }
                    }
                    catch { }
                    cachearry[i] = valuearry[i];
                    count++;
                }
                if (count == 1 && !alt)
                {
                    //无重码直接上屏
                    if (!InputMode.useregular)
                    {
                        ShangPing(1,0,true);
                    }
                    else
                    {
                        if (cachearry[0].Split('|')[2].Length == 0)
                        {

                            ShangPing(1, 0, true);
                        }
                        else
                            this.ShowWindow(true);
                    }
                }
                else
                {
                    if (sp && count > 1)
                    {
                        clear = false;
                    }
                    if (sp)
                    {
                        if (smspace && InputMode.spaceaout > 0 && !InputMode.select3)
                        {
                            if (InputMode.spaceaout == 1 && valuearry.Length == 2)
                                ShangPing(2, 0, false);
                            else if (InputMode.spaceaout == 1 && valuearry.Length > 2)
                                ShangPing(1, 0, false);
                            else
                                ShangPing(2, 0, false);
                        }
                        else
                        {
                            this.ShowWindow(true);
                            if (three_no2 && this.inputstr.Length == 3 && isright)
                            {
                                if (cachearry[1].Split('|')[2].Length == 0)
                                {
                                    ShangPing(spp, 0, clear);
                                }
                                else
                                    ShangPing(1, 0, clear);
                            }
                            else if (smspace && InputMode.spaceaout > 0 && valuearry.Length >= 2)
                            {

                                if (InputMode.spaceaout == 1 && valuearry.Length == 2)
                                    ShangPing(2, 0, false);
                                else if (InputMode.spaceaout == 2 && spp < 2)
                                    ShangPing(2, 0, false);
                                else if (InputMode.spaceaout == 3 && this.inputstr.Length == 3)
                                    ShangPing(1, 0, false);
                                else
                                    ShangPing(spp, 0, clear);

                            }
                            else
                                ShangPing(spp, 0, clear);
                            //if(!clear)
                            //    this.ShowWindow(true);
                        }
                    }
                    else if (smspace && InputMode.spaceaout > 0)
                    {
                        if (InputMode.spaceaout == 1 && valuearry.Length == 2)
                            ShangPing(2, 0, false);
                        else if (InputMode.spaceaout == 2)
                            ShangPing(2, 0, false);
                        else if (InputMode.spaceaout == 3 && this.inputstr.Length >= 3)
                            ShangPing(1, 0, false);
                        else
                            this.ShowWindow(true);
                    }
                    else if (selectpos == 4 && this.inputstr.Length==2 && InputMode.spaceaout == 3 && valuearry.Length >= 2 && InputStatusFrm.cachearry[0].Split('|')[1].Replace("，", "").Length > 1)
                    {
                        ShangPing(2, 0, false);
                    }
                    else
                    {
                        this.ShowWindow(true);
                    }
                }
            }
            else
            {
                if (cachearry != null && cachearry.Length > 0)
                {
                    if (!string.IsNullOrEmpty(cachearry[0])) PreFirstValue = cachearry[0].Split('|')[1];
                }
                if (cachearry == null) cachearry = new string[PageSize];
                if (PreFirstValue.Length > 0 && this.inputstr != this.input && this.inputstr.Length >= 2)
                {
                    //错码上屏
                    if (InputMode.closebj)
                    {
                        if (this.input.Length > 1)
                        {
                            this.inputstr = this.inputstr.Substring(0, this.inputstr.Length - 1);
                            cachearry = Input.GetInputValue(this.inputstr);
                            string oPreFirstValue = PreFirstValue;
                            PreFirstValue = string.Empty;
                            if (cachearry != null && cachearry.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(cachearry[0]))
                                {
                                    PreFirstValue = cachearry[0].Split('|')[1];
                                    SendText(PreFirstValue, this.inputstr);
                                    this.inputstr = this.input.Substring(this.input.Length - 1, 1);
                                }
                            }
                            else
                            {
                                SendText(oPreFirstValue, this.inputstr);
                                this.inputstr = this.input;

                            }
                        }
                        else
                        {
                            SendText(PreFirstValue, this.inputstr);
                            this.inputstr = this.input;
                        }
                    }
                    else
                    {
                        if (this.input.Length > 1)
                        {
                            SendText(PreFirstValue, this.inputstr.Length > 2 ? this.inputstr.Substring(0, 2) : this.inputstr);
                            this.inputstr = this.input;
                        }
                        else
                        {
                            SendText(PreFirstValue, this.inputstr);
                            string outstr = Input.GetLROne(this.input + (smspace ? "~" : ""), !isright, Input.IsPresAltPos);
                            if (!string.IsNullOrEmpty(outstr))
                            {
                                Clear();
                                InputStatusFrm.SendText(outstr, "");
                            }
                            else
                            {
                                this.inputstr = this.input;
                            }
                        }

                    }
                    if (smspace)
                    {
                        if (this.inputstr.Length > 0)
                        {
                            ShowInput(true,true,0, smspace,alt,spp,isright,selectpos);
                            Clear();
                        }

                    }
                    else
                    {
                        if (this.inputstr.Length > 0)
                        {
                            if (this.inputstr.Length == 1 && InputMode.closebj
                                && ",./;，。、；，．／；＇‘’　".IndexOf(this.inputstr) >= 0)
                            {
                                SendText(this.inputstr.Replace(",", "，").Replace(".", "。").Replace("/", "、").Replace(";", "；"), "");
                                this.Clear();
                            }
                            else
                                ShowInput(false,true,0,false,alt,spp,isright,selectpos);

                        }
                    }
                }
                else
                {
                    this.Clear();
                }
            }
        }
        public  void GetDreamValue(string v)
        {
 

            if (!InputMode.OpenLink) return;

            int count = 0;
            string valuestr = string.Empty;
            string tinput = v.Length > 3 ? v.Substring(1) : v;
            if (Input.IsChinese == 1)
            {
                if (Input.linkdictp.ContainsKey(tinput))
                {
                    count++;
                    valuestr += count + "z|" + v + "|\n";
                    foreach (var item in Input.linkdictp[tinput])
                    {
                        if (count >= this.PageSize) break;
                        count++;
                        valuestr += count + "z|" + item + "|\n";
                    }
                }
                //for (int i = 0; i < Input.linkdict.Length; i++)
                //{
                //    if (count >= this.PageSize) break;

                //    if (Input.linkdict[i].StartsWith(v))
                //    {
                //        count++;
                //        valuestr += count + "z|" + Input.linkdict[i] + "|\n";
                //    }
                //}


                if (valuestr.Length > 0)
                {
                    valuestr = valuestr.Replace("^", "＾");
                    if (Input.IsChinese == 1 && !Input.IsJT)
                    {
                        //转繁体
                        try
                        {
                            valuestr = Microsoft.VisualBasic.Strings.StrConv(valuestr, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
                        }
                        catch { }
                    }
                    valuearry = valuestr.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < valuearry.Length; i++)
                        valuearry[i] = Comm.Function.ReplaceSystem(valuearry[i]);
                    int pagesi = PageSize;
                    cachearry = new string[pagesi];
                    int postcount = 0;
                    for (int i = 0; i < pagesi && i < valuearry.Length; i++)
                    {

                        cachearry[postcount++] = valuearry[i];

                    }
                    if (postcount == 0)
                    {
                        Dream = false;
                        LastLinkString = string.Empty;
                        this.ShowWindow(false);
                    }
                    else
                    {
                        Dream = true;
                        LSView = true;
                        this.ShowWindow(true);
                    }
                }
                else
                {
                    Dream = false;
                    LastLinkString = string.Empty;
                     
                    //this.ShowWindow(false);
                }
            }
            else
            {
                valuearry = Input.GetEnInputValue(LastLinkString);
                if (valuearry != null)
                {
                    int pagesi = PageSize;
                    cachearry = new string[pagesi];
                    int postcount = 0;
                    for (int i = 0; i < pagesi && i < valuearry.Length; i++)
                    {
                        if (string.Compare(LastLinkString , valuearry[i].Split('|')[1],true)!=0)
                            cachearry[postcount++] = valuearry[i];
                        else
                            pagesi++;
                    }
                    if (postcount == 0)
                    {
                        Dream = false;
                        LastLinkString = string.Empty;
                        this.ShowWindow(false);
                    }
                    else
                    {
                        Dream = true;
                        LSView = true;
                        this.ShowWindow(true);
                    }
                }
                else
                {
                    Dream = false;
                    LastLinkString = string.Empty;
                    this.ShowWindow(false);
                }
            }
        }
 
        public void NextPage()
        {
            PageNum++;
            if ((PageNum - 1) * PageSize >= valuearry.Length) { PageNum--; return; }
     
            valuearry = Input.GetInputValue(this.inputstr);
            if (valuearry != null)
            {
                int pos = 0;
                cachearry = new string[PageSize];
                
                for (int i = (PageNum - 1) * PageSize; i < valuearry.Length; i++)
                {
                    if (pos >= cachearry.Length) break;
                    cachearry[pos] = valuearry[i];
                    pos++;
                }
                HideByApi();
            }

        }
        public void PrePage()
        {
            PageNum--;
            if (PageNum == 0) { PageNum = 1; return; }
            valuearry = Input.GetInputValue(this.inputstr);
            if (valuearry != null)
            {
                int pos = 0;
                cachearry = new string[PageSize];
                for (int i = (PageNum - 1) * PageSize; i < valuearry.Length; i++)
                {
                    if (pos >= cachearry.Length) break;
                    cachearry[pos] = valuearry[i];
                    pos++;
                }
                HideByApi();
            }

        }



        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // InputStatusFrm
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(343, 31);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InputStatusFrm";
            this.Load += new System.EventHandler(this.InputStatusFrm_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.InputStatusFrm_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.InputStatusFrm_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        private void InputStatusFrm_Load(object sender, EventArgs e)
        {
            this.Width = 175;
            this.Height = 133;
        }

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static string GetCutStr(string v)
        {
            if (string.IsNullOrEmpty(v)) return v;

            if (Input.IsChinese == 1)
                if (v.Length > 8)
                {
                    string vv = v;
                    v = vv.Substring(0, 4) + ".." + vv.Substring(vv.Length - 4, 4);
                }
                else
                    if (v.Length > 14)
                    {
                        string vv = v;
                        v = vv.Substring(0, 9) + ".." + vv.Substring(vv.Length - 4, 4);
                    }

            return v;
        }
        public int pinyipos = 0;
        string pys = "";
        string cfs = "";
        public string s2t = "";
      
        BufferedGraphicsContext context = BufferedGraphicsManager.Current;
 
        Pen bordpen = new Pen(InputMode.Skinbordpen);
        public SolidBrush bstring = new SolidBrush(InputMode.Skinbstring);
        public SolidBrush bcstring = new SolidBrush(InputMode.Skinbcstring);
        public SolidBrush fbcstring = new SolidBrush(InputMode.Skinfbcstring);
        public SolidBrush skinback = new SolidBrush(InputMode.SkinBack);

        /// <summary>
        /// 绘制候选框
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (cachearry == null) return;

            if (this.inputstr.Length == 0 && !Dream) return;

            try
            {
                using (BufferedGraphics grafx = context.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
                {
                    grafx.Graphics.FillRectangle(skinback, new Rectangle(0, 0, Width, Height));

                    if (InputMode.pinyin || InputMode.datacf || InputMode.ftfzxs || !Input.IsJT)
                    {
                        if (valuearry != null && valuearry.Length <= pinyipos || PageSize <= pinyipos) pinyipos = 0;

                        if (InputMode.pinyin)
                        {
                            if (valuearry != null && valuearry.Length > pinyipos
                                && cachearry[pinyipos].Split('|').Length > 1
                                && Input.PinYi.ContainsKey(cachearry[pinyipos].Split('|')[1]))
                            {
                                pys = Input.PinYi[cachearry[pinyipos].Split('|')[1]];
                            }
                            else pys = String.Empty;

                        }
                        else pys = String.Empty;

                        if (InputMode.datacf)
                        {
                            if (valuearry != null && valuearry.Length > pinyipos
                            && cachearry[pinyipos].Split('|').Length > 1
                            && Input.CfDict.ContainsKey(cachearry[pinyipos].Split('|')[1]))
                            {
                                cfs = Input.CfDict[cachearry[pinyipos].Split('|')[1]];
                            }
                            else cfs = String.Empty;
                        }
                        else cfs = String.Empty;

                        if (!Input.IsJT || InputMode.ftfzxs)
                        {
                            if (valuearry != null && valuearry.Length > pinyipos
                            && cachearry[pinyipos].Split('|').Length > 1)
                            {
                                string osz = cachearry[pinyipos].Split('|')[1];
                                string sz = Input.IsJT ? cachearry[pinyipos].Split('|')[1]
                                    : Microsoft.VisualBasic.Strings.StrConv(cachearry[pinyipos].Split('|')[1], Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
                                if (sz.Length == 1)
                                {
                                    if (Input.S2TDict.ContainsKey(sz))
                                    {
                                        s2t = "";
                                        sz = Input.S2TDict[sz].Replace(osz, "").Trim();
                                        if (sz.Length > 0)
                                        {
                                            for (int i = 0; i < sz.Split(' ').Length; i++)
                                            {
                                                s2t += (i + 1) + "." + sz.Split(' ')[i] + " ";
                                            }
                                        }
                                        else
                                        {
                                            s2t = String.Empty;
                                        }
                                    }
                                    else
                                    {
                                        s2t = String.Empty;
                                    }
                                }
                                else if (Input.IsJT)
                                {
                                    s2t = "";
                                    bool haveft = false;
                                    for (int i = 0; i < sz.Length; i++)
                                    {
                                        if (Input.S2TDict.ContainsKey(sz.Substring(i, 1)))
                                        {
                                            string s2t1 = Input.S2TDict[sz.Substring(i, 1)].Split(' ')[0];
                                            if (sz.Substring(i, 1) == s2t1 && Input.S2TDict[sz.Substring(i, 1)].Split(' ').Length > 1)
                                            {
                                                s2t1 = Input.S2TDict[sz.Substring(i, 1)].Split(' ')[1];
                                            }
                                            s2t += s2t1;
                                            haveft = true;
                                        }
                                        else
                                            s2t += sz.Substring(i, 1);
                                    }
                                    if (!haveft) s2t = String.Empty;
                                }
                                else s2t = String.Empty;
                            }
                            else
                            {
                                s2t = String.Empty;
                            }
                        }
                        else { s2t = String.Empty; }
                    }
                    else
                    {
                        pys = String.Empty;
                        cfs = String.Empty;
                        s2t = string.Empty;
                        pinyipos = 0;
                    }


                    Rectangle hzrec = new Rectangle(0, 0, Width - 1, Height - 1);
                    grafx.Graphics.DrawRectangle(bordpen, hzrec);

                    int inputy = InputMode.SkinFontJG;
                    string ins = "";
                    if (InputMode.useregular)
                        ins = InputStatusFrm.Dream ? "智能联想" : InputMode.CovertCodeStrByReg(this.inputstr) + (s2t.Length > 0 ? " " + s2t : "") + (pys.Length > 0 || cfs.Length > 0 ? "  " + (pinyipos + 1) + "." + pys + " " + cfs : "");
                    else
                        ins = InputStatusFrm.Dream ? "智能联想" : this.inputstr + (s2t.Length > 0 ? " " + s2t : "") + (pys.Length > 0 || cfs.Length > 0 ? "  " + (pinyipos + 1) + "." + pys + " " + cfs : "");
                    int fontsize = InputMode.SkinFontSize;
                    grafx.Graphics.DrawString(ins, new Font(InputMode.cffontname, fontsize > 18 ? 18 : fontsize), bstring, new Point(0 + 3, 0 + 3));

                    if (valuearry != null && valuearry.Length > 0 && !InputStatusFrm.Dream && valuearry.Length > PageSize) //分页数显示
                        grafx.Graphics.DrawString(string.Format("{0}/{1}", PageNum, valuearry.Length / PageSize + 1), new Font("", 11F), bstring, new Point(Width - 44, 0 + 3));


                    if (ViewType == 0)
                    {
                        //横排显示
                        int wx = 1;
                        for (int i = 0; i < cachearry.Length; i++)
                        {
                            try
                            {
                                if (InputMode.lbinputc[i] == null) break;
                                if (string.IsNullOrEmpty(cachearry[i])) break; ;
                                string v = GetCutStr(cachearry[i].Split('|')[1]);

                                string pos = i == 9 ? "0." : (i + 1).ToString() + ".";


                                Font tfont = new Font(InputMode.SkinFontName, fontsize);

                                if (i == 0)
                                    grafx.Graphics.DrawString(pos + v, tfont, fbcstring, new PointF(wx, inputy));
                                else
                                    grafx.Graphics.DrawString(pos + v, tfont, bstring, new PointF(wx, inputy));

                                if (InputMode.lbinputv == null || InputMode.lbinputv[i] == null) return;

                                InputMode.lbinputv[i].Text = pos + v;

                                wx += InputMode.lbinputv[i].PreferredWidth - 7;

                                grafx.Graphics.DrawString(cachearry[i].Split('|')[2], new Font("", fontsize - 1), bcstring, new Point(wx, inputy - 2));

                                if (InputMode.lbinputc[i] == null || string.IsNullOrEmpty(InputMode.lbinputc[i].Text))
                                {
                                    wx += 1;
                                }
                                else
                                {
                                    wx += InputMode.lbinputc[i].PreferredWidth + 2;
                                    if (InputMode.lbinputv[i].Text.Length > 3)
                                        wx += -4;
                                }
                            }
                            catch { }
                        }

                    }
                    else
                    {
                        //竖排显示
                        for (int i = 0; i < cachearry.Length; i++)
                        {
                            if (string.IsNullOrEmpty(cachearry[i])) break;
                            string v = GetCutStr(cachearry[i].Split('|')[1]);
                            string pos = i == 9 ? "0." : (i + 1).ToString() + ".";
                            Font tfont = new Font(InputMode.SkinFontName, InputMode.SkinFontSize);
                            int vw = InputMode.GetWidth(cachearry[i].Split('|')[1]);
                            if (i == 0)
                                grafx.Graphics.DrawString(pos + v, tfont, fbcstring, new Point(3, inputy + i * InputMode.SkinFontH));
                            else
                                grafx.Graphics.DrawString(pos + v, tfont, bstring, new Point(3, inputy + i * InputMode.SkinFontH));

                            grafx.Graphics.DrawString(cachearry[i].Split('|')[2], new Font("宋体", InputMode.SkinFontSize - 1), bcstring, new Point(3 + vw, (inputy + i * InputMode.SkinFontH) - 1));
                        }
                    }

                    grafx.Render(e.Graphics);
                    grafx.Dispose();

                }
            }
            catch
            {
            }

        }

        //图片输出
        /// <summary>
        /// 把文字转换才Bitmap
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="rect">用于输出的矩形，文字在这个矩形内显示，为空时自动计算</param>
        /// <param name="fontcolor">字体颜色</param>
        /// <param name="backColor">背景颜色</param>
        /// <returns></returns>
        public static Bitmap TextToBitmap(string text, Font font, Color fontcolor, Color backColor)
        {
            Graphics g;
            Bitmap bmp;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            
            bmp = new Bitmap(1, 1);
            g = Graphics.FromImage(bmp);
            //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);

            int width = (int)(sizef.Width+1);
            int height = (int)(sizef.Height-2);
            var rect = new Rectangle(0,0, width, height);
            bmp.Dispose();

            bmp = new Bitmap(width, height);


            g = Graphics.FromImage(bmp);

            //使用ClearType字体功能
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString(text, font, new SolidBrush(fontcolor), rect, format);
            return bmp;
        }

        /// <summary>
        /// 图片输出 ,QQ,wx
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="fontcolor"></param>
        /// <param name="backColor"></param>
        /// <returns></returns>
        public static Bitmap MTextToBitmap(string text, Font font, Color fontcolor, Color backColor)
        {
            if (!InputMode.imghh || text.Length < 5) return TextToBitmap(text, font, fontcolor, backColor);

            Graphics g;
            Bitmap bmp;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);

            bmp = new Bitmap(1, 1);
            g = Graphics.FromImage(bmp);
            //计算绘制文字所需的区域大小（根据宽度计算长度），重新创建矩形区域绘图
            SizeF sizef = g.MeasureString("我我我我我我我我", font, PointF.Empty, format);

            int mh = (int)sizef.Height * 2 + (int)(text.Length / 10 * sizef.Height * 1.5);

            int width = (int)(sizef.Width + 7);
            int height = mh + 7;
            var rect = new Rectangle(0, 0, width, height);
            bmp.Dispose();

            bmp = new Bitmap(width, height);

            g = Graphics.FromImage(bmp);

            //使用ClearType字体功能
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(new SolidBrush(backColor), rect);
            g.DrawString("\n" + text, font, new SolidBrush(fontcolor), rect, format);
            return bmp;
        }
        public static short colorpos = 0;
        /// <summary>
        /// 随机获取颜色
        /// </summary>
        /// <returns></returns>
        
        public static Color GetColor()
        {
            switch (colorpos)
            {
                case 0: { colorpos++; return Color.FromArgb(218, 138, 70); }
                case 1: { colorpos++; return Color.FromArgb(58, 29, 79); }
                case 2: { colorpos++; return Color.FromArgb(150, 59, 25); }
                case 3: { colorpos++; return Color.FromArgb(181, 173, 145); }
                case 4: { colorpos++; return Color.FromArgb(230, 110, 180); }
                case 5: { colorpos++; return Color.FromArgb(32, 156, 219); }
                case 6: { colorpos++; return Color.FromArgb(112, 63, 168); }
                case 7: { colorpos++; return Color.FromArgb(215, 59, 45); }
                case 8: { colorpos++; return Color.FromArgb(22, 148, 73); }
                case 9: { colorpos++; return Color.FromArgb(131, 114, 73); }
                default: { colorpos = 0; return Color.FromArgb(22, 79, 142); }
            }
            
        }

        public void SendSelfImg()
        {

        }

        private void InputStatusFrm_MouseClick(object sender, MouseEventArgs e)
        {
            if (Win.WinInput.InputStatus.Visible)
            {
                Control control = sender as Control;
                if (control == null) return;
                //图片输出

                Bitmap img = new Bitmap(control.Width, control.Height);
                // 绘制控件内容到位图
                control.DrawToBitmap(img, new Rectangle(0, 0, control.Width, control.Height));
                //Graphics g = Graphics.FromImage(img);
                //g.CompositingQuality = CompositingQuality.HighQuality;
                //g.CopyFromScreen(control.Left, control.Top, 0, 0, new Size(control.Width, control.Height));
                Clipboard.SetImage(img);
            }
        }

        private void InputStatusFrm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Win.WinInput.InputStatus.Visible)
            {
                //图片输出
                Control control = sender as Control;
                if (control == null) return;
                //图片输出

                Bitmap img = new Bitmap(control.Width, control.Height);
                // 绘制控件内容到位图
                control.DrawToBitmap(img, new Rectangle(0, 0, control.Width, control.Height));
                //Graphics g = Graphics.FromImage(img);
                //g.CompositingQuality = CompositingQuality.HighQuality;
                //g.CopyFromScreen(control.Left, control.Top, 0, 0, new Size(control.Width, control.Height));
                Clipboard.SetImage(img);
                SendKeys.Send("^v"); //发送ctrl+v 进行粘贴
            }
        }
    }

    public enum InputType
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1,
        INPUT_HARDWARE = 2,
    }

    [Flags()]
    public enum KEYEVENTF
    {
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        UNICODE = 0x0004,
        SCANCODE = 0x0008,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public Int16 wVk;
        public ushort wScan;
        public Int32 dwFlags;
        public Int32 time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public Int32 dx;
        public Int32 dy;
        public Int32 mouseData;
        public Int32 dwFlags;
        public Int32 time;
        public IntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public Int32 type;//0-MOUSEINPUT;1-KEYBDINPUT;2-HARDWAREINPUT   
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public Int32 uMsg;
        public Int16 wParamL;
        public Int16 wParamH;
    }
}
