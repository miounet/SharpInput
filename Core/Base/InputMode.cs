﻿using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Core.Base
{
    /// <summary>
    /// 输入法基础基类
    /// </summary>
    public class InputMode
    {
        #region public filed
        public string[] MasterDit = null;
        public string[] ProDit = null;
        public string[] UserDit = null;
        public string[] EnDit = null;
        public List<string> ClouddDit = new List<string>();
        public static string AppPath = string.Empty;
        public Dictionary<string,string> PinYi = new Dictionary<string, string>();//拼音注音
        public Dictionary<string, string> CfDict = new Dictionary<string, string>();//拆分数据
        public Dictionary<string, string> S2TDict = new Dictionary<string, string>();//繁体数据
        public string MasterDitPath = string.Empty;
        public string ProDitPath = string.Empty;
        public string UserDitPath = string.Empty;
        public string CloundDitPath = string.Empty;
        public string EnDitPath = string.Empty;
        public string SettingPath = string.Empty;
        public string mapstr1 = "~1qaz2wsx3edc4rfv5tgb6";
        public string mapstr2 = "yhn7ujm8ik,9ol.0p;/-['=]";
        public string[] qjzwdict;//全角中文按键字典
        public string[] bjzwdict;//半角中文按键字典
        public string[] qjywdict;//全角英文按键字典
        public string[] bjywdict;//半角英文按键字典
        public string[] onedict;//左右一简
     
        public Dictionary<string, List<string>> linkdictp=new Dictionary<string, List<string>>();//汉字联想的词库
        public int MaxLen = 40;//上屏编码最大长度
        public  List<MapintKey> mapsortkeys = new List<MapintKey>();
        public List<MapintKey> mapkeys = new List<MapintKey>();
        /// <summary>
        /// 索引建好
        /// </summary>
        public bool indexComplete = false;
        /// <summary>
        /// 1表示中文状态
        /// 0英文状态
        /// 2并击助手状态
        /// </summary>
        public ushort IsChinese = 1;
      
        /// <summary>
        /// 0为sendinput 发送
        /// 1为剪贴板方式发送
        /// </summary>
        public short OutType = 0;
        public bool SelfOut = false;
         
        /// <summary>
        /// 获取输入法状态栏背景文件名
        /// </summary>
        /// <returns></returns>
        public string GetSkinBKImg()
        {
            string skin = this.IsChinese == 2 ? "srzs.png" : (string.Format("{0}{1}{2}.png", (this.IsChinese == 1 ? "z" : "y")
                , (this.IsQJ ? "q" : "b"), (this.IsJT ? "j" : "f")));
            return skin;
        }
 
      
        /// <summary>
        /// true激活,false禁用/隐藏输入法
        /// </summary>
        public bool IsActive = true;

        /// <summary>
        /// true全角,false半角
        /// </summary>
        public bool IsQJ = false;
        /// <summary>
        /// 是否简体输出
        /// </summary>
        public bool IsJT = true;
        /// <summary>
        /// true中文标点,false英文标点
        /// </summary>
        public bool IsCnBd = true;
        /// <summary>
        /// 参与的编码
        /// </summary>
        public string mdcode = "qwertyuiopasdfghjkl;'zxcvbnm,./QWERTYUIOPASDFGHJKL:ZXCVBNM<>?!`~，。、；@*_#$%^&0123456789";
       
        public  bool isActiveInput = true;
        public  bool IsPressShift = false;
        public  bool IsPressAlt = false;
        public bool IsPressLAlt = false;
        public bool IsPressRAlt = false;
        public  bool IsPressCtrl = false;
        public  bool IsPressWin = false;
        public short IsPresAltPos = 0;
        public static string[] srleftdict;//速录左手一简
        public static string[] srrightdict;//速录右手一简
        /// <summary>
        /// 单字输出模式
        /// </summary>
        public static bool SingleInput = false;//单字模式输出
 
        #endregion

        #region static
      
        public static string CDPath = string.Empty;//当前词库目录
        public static string ZFPath = string.Empty;//当前指法目录
        public static string PFPath = string.Empty;//当前皮肤
        public static bool OpenCould=true;//云词库
        public static bool AutoUpdate = true;//自动升级 
        public static bool OpenLink = true;//智能联想
        public static bool AutoZJ = false;//自动
                                          //
        public static bool OpenAltSelect = false;//左alt选重
        public static bool AutoRun = true;//自动运行
        public static string SkinFontName = "宋体";//字体名
        public static int SkinFontSize = 13;//字体大小
        public static int SkinFontH = 20;//字体高度
        public static int SkinFontW = 20;//字体宽度
        public static int SkinFontJG = 30;//输入与候选汉字的间高
        public static int SkinWidth = 160;//汉字候选框宽度
        public static int SkinHeith = 46;//汉字候选框高度
        public static Color SkinBack = Color.FromArgb(22, 79, 141);//背景色
        public static Color Skinbordpen = Color.Gray;//边框色
        public static Color Skinbstring = Color.White;//字体颜色
        public static Color Skinbcstring = Color.Orange;//提示补码颜色
        public static Color Skinfbcstring = Color.Yellow;//第一候选框字体颜色
        public static int PageSize = 6; //候选框数量
        public static string txtla = string.Empty;//拇指左键输出
        public static string txtra = string.Empty;//拇指右键输出
        public static string txtlas = string.Empty;//拇指左右＋空格输出
        public static string txtras = string.Empty;//拇指右键＋空格输出
        public static string txtlra = string.Empty;//拇指左右键输出
        public static string txtlras = string.Empty;//拇指左右键＋空格输出
        public static bool right3_out = true;//开启右手第3码顶字
        public static bool pinyin = false;//显示拼音提示
        public static bool closebj = false;//关闭并击模式，只使用串击/连击
        public static bool autopos = false;//数字选码自动调频
        public static bool tautopos = true;//临时调频
        public static short autoup = 3;//几码强制上屏
        public static string stopup = "";//什么打头字母不自动上屏
        public static bool bjzckgsp = false;//速录助手时1、3码不自动上屏，需要空格确认上屏
        public static bool omeno = false;
        public static bool zsallmap = false;//速录助手完全使用指法映射配置map.shp
        public static int zsmode1 = 0;//速录助手按键输出间隔x毫秒
        public static bool imgsend = false;//以图片方式粘贴上屏
        public static bool zjsend = false;//以整句方式粘贴上屏
        public static int outtype = 0;//0默认,1剪贴板模式，2嵌入模式
        public static int SkinIndex=0;
        public static bool datacf = false;//显示汉字拆分
        public static string cffontname = "宋体";
        public static bool imghh = false;//图片换行
        public static bool oneoutbj = true;//一键输出标点,./;
        public static bool ftfzxs = false;//繁体辅助显示
        public static bool dcxz = true;//打词消字功能
        public static bool iselect = true;//数字+i选重
        public static bool onesp = true;//一码上屏
        public static bool select3 = false;//开启 空格＋第3码 右手选2，左手选3
        public static int spaceaout = 0;//0 上屏首选，1当1个重码时上屏次选，2强制上屏次选 3强制，顶上首之词
        public static bool outdatetime=true;
        public static bool autodata = true;
        public static bool useregular = false;
        public static bool smautoadd = true;// false;//自动添加缺失的三码词,词长<=4
        public IndexManger DictIndex = new IndexManger ();
        #endregion

        /// <summary>
        /// 按输入获取字词
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="dream"></param>
        /// <returns></returns>
        public virtual string[] GetInputValue(string inputstr, bool dream = false,int ncount=0,bool isright=false)
        {
            string valuestr = "";
            if (IsChinese == 1)
            {
                #region 中文处理
                if (inputstr.Length == 0) return null;
                if (!inputstr.StartsWith("'"))
                {
                    int count = 0;
                    int first = 0, last = MasterDit.Length - 1;
                    int pcount = inputstr.Length > 2 ? 188 : 66;// 30;// SingleInput == true ? 50 : 70;
                    if (ncount > 0) pcount = ncount;


                    #region 取字
                    string[] mdict = null;
                    if (indexComplete)
                    {
                        PosIndex poi = DictIndex.GetPos(inputstr, ref mdict);
                        if (poi != null)
                        {
                            first = poi.Start;
                            last = poi.End;
                        }
                        else
                        {
                            last = 0;
                        }
                    }
                    else last = 0;
                    if (mdict == null)
                        mdict = MasterDit;

                    for (int i = first; i <= last; i++)
                    {
                        if (mdict.Length > 0)
                        {
                            if (mdict[i].StartsWith(inputstr))
                            {
                                string strarr = mdict[i];
                                string fcode = strarr.Split(' ')[0];
                                string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                                int startint = fcode.IndexOf(inputstr) + inputstr.Length;
                                foreach (var hzvalue in fvalue.Split(' '))
                                {
                                    if (string.IsNullOrEmpty(hzvalue)) continue;
                                    fvalue = hzvalue;
                                    if (SingleInput)
                                    {
                                        if (fvalue.Length > 1 && fcode.Length > 3)
                                        {
                                            //单字
                                            continue;
                                        }
                                    }
                                    if (valuestr.IndexOf("|" + fvalue + "|") < 0)
                                    {
                                        //去重
                                        valuestr += i + "z|" + fvalue + "|" + (startint < fcode.Length ? fcode.Substring(startint, fcode.Length - inputstr.Length) : "") + "\n";
                                        count++;
                                    }
                                }


                            }
                        }
                        if (count > pcount) break;
                    }

                    #endregion

                    if (count < 28)
                    {
                        string prostr = GetProInputValue(inputstr, 20);
                        valuestr += prostr;
                    }
                    if (count < 28)
                        GetUserDict(inputstr, ref valuestr);

                }
                else
                {
                    GetCloudDict(inputstr, ref valuestr);
                }
                #endregion

            }
            else if (IsChinese == 2)
            {
                if (!bjzckgsp)
                    GetUserDict(inputstr, ref valuestr);
            }
            else
            {
                valuestr = string.Format("0z|{0}|", inputstr);
            }

            if (valuestr.Length > 0)
            {
                if (inputstr.Length == 1 && IsChinese == 1 && inputstr!="'")
                {
                    string onestr = GetLROne(inputstr, !isright);
                    if (!string.IsNullOrEmpty(onestr) && valuestr.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split('|')[1]!=onestr)
                    {
                        valuestr = "292184z|"+ onestr + "|\n" + valuestr;
                    }
                }
                valuestr = valuestr.Replace("^", "＾");
               
                if (!this.IsJT)
                {
                    //转繁体
                    try
                    {
                        valuestr = S2TConv(valuestr);
                    }
                    catch { }
                }
                string[] vsa = valuestr.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < vsa.Length; i++)
                    vsa[i] = Comm.Function.ReplaceSystem(vsa[i]);
                //vsa = vsa.OrderBy(v => v.Split('|')[1].Length).ToArray();
                return vsa;
            }

            return null;
        }


        public virtual string[] GetInputOneValue(string inputstr)
        {
            string valuestr = "";
            if (IsChinese == 1)
            {
                #region 中文处理
                if (inputstr.Length == 0 || inputstr.Length>1) return null;
                if (!inputstr.StartsWith("'"))
                {
                    int count = 0;
                    int first = 0, last = MasterDit.Length - 1;
                    int pcount = inputstr.Length > 2 ? 2 : 2;
      


                    #region 取字
                    string[] mdict = null;
                    if (indexComplete)
                    {
                        PosIndex poi = DictIndex.GetPos(inputstr, ref mdict);
                        if (poi != null)
                        {
                            first = poi.Start;
                            last = poi.End;
                        }
                        else
                        {
                            last = 0;
                        }
                    }
                    else last = 0;
                    if (mdict == null)
                        mdict = MasterDit;

                    for (int i = first; i <= last; i++)
                    {
                        if (mdict.Length > 0)
                        {
                            if (mdict[i].StartsWith(inputstr))
                            {
                                string strarr = mdict[i];
                                string fcode = strarr.Split(' ')[0];
                                string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                                int startint = fcode.IndexOf(inputstr) + inputstr.Length;
                                foreach (var hzvalue in fvalue.Split(' '))
                                {
                                    if (string.IsNullOrEmpty(hzvalue)) continue;
                                    fvalue = hzvalue;
                                    if (SingleInput)
                                    {
                                        if (fvalue.Length > 1 && fcode.Length > 3)
                                        {
                                            //单字
                                            continue;
                                        }
                                    }
                                    if (valuestr.IndexOf("|" + fvalue + "|") < 0)
                                    {
                                        //去重
                                        valuestr += i + "z|" + fvalue + "|" + (startint < fcode.Length ? fcode.Substring(startint, fcode.Length - inputstr.Length) : "") + "\n";
                                        count++;
                                    }
                                }


                            }
                        }
                        if (count > pcount) break;
                    }

                    #endregion

                    if (count < 28)
                    {
                        string prostr = GetProInputValue(inputstr, 20);
                        valuestr += prostr;
                    }
                    if (count < 28)
                        GetUserDict(inputstr, ref valuestr);

                }
                else
                {
                    GetCloudDict(inputstr, ref valuestr);
                }
                #endregion
                if (valuestr.Length > 0)
                {

                    if (!this.IsJT)
                    {
                        //转繁体
                        try
                        {
                            valuestr = S2TConv(valuestr);
                        }
                        catch { }
                    }
                    string[] vsa = valuestr.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < vsa.Length; i++)
                        vsa[i] = Comm.Function.ReplaceSystem(vsa[i]);
                    //vsa = vsa.OrderBy(v => v.Split('|')[1].Length).ToArray();
                    return vsa;
                }
            }
            
            return null;
        }

        public string S2TConv(string s)
        {
            if (S2TDict.Count <= 0)
            {
                return Microsoft.VisualBasic.Strings.StrConv(s, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
            }
            string ss = "";
            if (s.IndexOf("|") > 0)
            {
                string cs = "";
                for (int ii = 0; ii < s.Length; ii++)
                {
                    bool en = false;
                    char[] c = s.Substring(ii,1).ToCharArray();
                    for (int i = 0; i < s.Substring(ii, 1).Length; i++)
                    {
                        if ((int)c[i] <= 127)
                        {
                            en = true;
                            break;
                        }
                    }

                    if (en)
                    {
                        if (cs.Length > 0)
                        {


                            if (S2TDict.ContainsKey(cs))
                            {
                                ss += S2TDict[cs].Split(' ')[0];
                            }
                            else
                            {
                                if (cs.Length > 1)
                                {
                                    for (int i = 0; i < cs.Length; i++)
                                    {
                                        if (S2TDict.ContainsKey(cs.Substring(i,1)))
                                        {
                                            ss += S2TDict[cs.Substring(i, 1)].Split(' ')[0];
                                        }
                                        else
                                            ss += Microsoft.VisualBasic.Strings.StrConv(cs.Substring(i, 1), Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
                                    }
                                }
                                else
                                    ss += Microsoft.VisualBasic.Strings.StrConv(cs, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
                            }
                            cs = "";
                        }
                        ss += s.Substring(ii, 1);
                    }
                    else
                    {
                        cs += s.Substring(ii, 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (S2TDict.ContainsKey(s.Substring(i, 1)))
                    {
                        ss += S2TDict[s.Substring(i, 1)].Split(' ')[0];
                    }
                    else
                    {
                        ss += Microsoft.VisualBasic.Strings.StrConv(s.Substring(i, 1), Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
                    }
                }
            }
            return ss;
        }
        public bool UpdatePos(string inputstr, int pos,int page=1,int pagesize=9)
        {
            try
            {
                if (IsChinese == 1)
                {
                    #region 中文处理
                    //if (page > 1) pos = pos + ((page-1)*pagesize);
                    if (page > 1) return false;
                    if (inputstr.Length == 0) return false;
                    if (!inputstr.StartsWith("'"))
                    {

                        int first = 0, last = MasterDit.Length - 1;

                        #region 自动调频
                        string[] mdict = null;
                        if (indexComplete)
                        {
                            PosIndex poi = DictIndex.GetPos(inputstr, ref mdict, false);
                            if (poi == null) return false;
                            first = poi.Start;
                            last = poi.End;
                        }
                        else return false;
                        if (mdict == null) mdict = MasterDit;
                        int fpos = 0;
                        bool jpok = false;
                        int xtcount = 0;
                        for (int i = first; i <= last; i++)
                        {
                            if (fpos == 0 && mdict[i].Split(' ')[0].StartsWith(inputstr))
                            {
                                fpos = i;
                            }

                            if (mdict[i].Split(' ')[0] == inputstr)
                            {
                                xtcount++;
                                if (i != fpos && xtcount == pos && page == 1)
                                {
                                    string temp = mdict[i];
                                    int jhi = i;
                                    for (int j = i - 1; j >= fpos; j--)
                                    {
                                        if (mdict[j].Split(' ')[0] == inputstr)
                                        {
                                            mdict[jhi] = mdict[j];
                                            mdict[j] = temp;
                                            jhi = j;
                                        }
                                    }
                                    jpok = true;
                                }
                                else
                                {
                                    if (mdict[i].Split(' ').Length <= pos)
                                    {
                                        continue;
                                    }
                                    string strarr = mdict[i];
                                    string fcode = strarr.Split(' ')[0];
                                    string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                                    if (fvalue.Split(' ').Length < 2) return false;
                                    if (fvalue.Split(' ').Length < pos - 1) pos = fvalue.Split(' ').Length;
                                    string zdit = fvalue.Split(' ')[pos - 1];
                                    mdict[i] = fcode + " " + zdit + " ";
                                    foreach (var item in fvalue.Split(' '))
                                    {
                                        if (item.Length == 0) continue;
                                        if (item == zdit) continue;
                                        mdict[i] += "" + item + " ";
                                    }
                                    mdict[i] = mdict[i].TrimEnd();
                                    jpok = true;
                                }

                                break;

                            }


                        }
                        if (!jpok)
                        {
                            string temp = mdict[fpos];
                            mdict[fpos] = mdict[fpos + pos - 1];
                            mdict[fpos + pos - 1] = temp;
                        }
                        jpok = false;
                        if (indexComplete)
                        {
                            PosIndex poi = DictIndex.GetPos(inputstr, ref mdict, true);
                            if (poi == null) return false;
                            first = poi.Start;
                            last = poi.End;
                        }
                        else return false;
                        if (mdict == null) mdict = MasterDit;
                        fpos = 0;
                        xtcount = 0;
                        for (int i = first; i <= last; i++)
                        {
                            if (fpos == 0 && mdict[i].Split(' ')[0].StartsWith(inputstr))
                            {
                                fpos = i;
                            }
                            if (mdict[i].Split(' ')[0] == inputstr)
                            {
                                xtcount++;
                                if (i != fpos && xtcount == pos && page == 1)
                                {
                                    string temp = mdict[i];
                                    int jhi = i;
                                    for (int j = i - 1; j >= fpos; j--)
                                    {
                                        if (mdict[j].Split(' ')[0] == inputstr)
                                        {
                                            mdict[jhi] = mdict[j];
                                            mdict[j] = temp;
                                            jhi = j;
                                        }
                                    }
                                    jpok = true;
                                }
                                else
                                {
                                    if (mdict[i].Split(' ').Length <= pos)
                                    {
                                        continue;
                                    }
                                    string strarr = mdict[i];
                                    string fcode = strarr.Split(' ')[0];
                                    string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                                    if (fvalue.Split(' ').Length < 2) return false;
                                    if (fvalue.Split(' ').Length < pos - 1) pos = fvalue.Split(' ').Length;
                                    string zdit = fvalue.Split(' ')[pos - 1];
                                    mdict[i] = fcode + " " + zdit + " ";
                                    foreach (var item in fvalue.Split(' '))
                                    {
                                        if (item.Length == 0) continue;
                                        if (item == zdit) continue;
                                        mdict[i] += "" + item + " ";
                                    }
                                    mdict[i] = mdict[i].TrimEnd();
                                    jpok = true;
                                }

                                break;

                            }


                        }
                        if (!jpok)
                        {
                            string temp = mdict[fpos];
                            mdict[fpos] = mdict[fpos + pos - 1];
                            mdict[fpos + pos - 1] = temp;
                        }

                        #endregion

                    }

                    #endregion

                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 按输入获取专业词库字词
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="dream"></param>
        /// <returns></returns>
        public virtual string GetProInputValue(string inputstr, int ncount = 30)
        {
            string valuestr = "";
            if (IsChinese == 1)
            {
                #region 中文处理
                if (inputstr.Length == 0) return valuestr;
                if (ProDit == null) return valuestr;
                int count = 0;
                int first = 0, last = ProDit.Length - 1;
                int pcount = 30;// SingleInput == true ? 50 : 70;
                if (ncount > 0) pcount = ncount;
                #region 取字
                if (indexComplete)
                {
                    PosIndex poi = DictIndex.GetProPos(inputstr);
                    if (poi == null) return valuestr;
                    first = poi.Start;
                    last = poi.End;
                }
                else return null;

                for (int i = first; i <= last; i++)
                {
                    if (ProDit[i].StartsWith(inputstr))
                    {
                        string strarr = ProDit[i];
                        string fcode = strarr.Split(' ')[0];
                        string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                        int startint = fcode.IndexOf(inputstr) + inputstr.Length;
                        foreach (var hzvalue in fvalue.Split(' '))
                        {
                            if (string.IsNullOrEmpty(hzvalue)) continue;
                            fvalue = hzvalue;
                            if (SingleInput)
                            {
                                if (fvalue.Length > 1 && fcode.Length > 3)
                                {
                                    //单字
                                    continue;
                                }
                            }
                            if (valuestr.IndexOf("|" + fvalue + "|") < 0)
                            {
                                //去重
                                valuestr += i + "z|" + fvalue + "|" + (startint < fcode.Length ? fcode.Substring(startint, fcode.Length - inputstr.Length) : "") + "\n";
                                count++;
                            }
                        }


                    }

                    if (count > pcount) break;
                }

                #endregion
                #endregion
            }
            return valuestr;
        }

        /// <summary>
        /// 获取用户词组
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="valuestr"></param>
        private void GetUserDict(string inputstr, ref string valuestr)
        {
            int count = 0;
            foreach (var s in UserDit)
            {
                if (s.StartsWith(inputstr))
                {
                    if (valuestr.IndexOf("|" + s.Split(' ')[1] + "|") < 0)
                    {
                        //去重
                        valuestr += 100 + count + "z|" + s.Split(' ')[1] + "|" + s.Split(' ')[0].Replace(inputstr, "") + "\n";

                        count++;
                    }
                    if (count >= 20) break;
                }
            }
        }


        /// <summary>
        /// 获取Cloud词组/临时拼音
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="valuestr"></param>
        private void GetCloudDict(string inputstr, ref string valuestr)
        {
            if (inputstr.StartsWith("'"))
            {

                valuestr = "";
                int count = 0;
                if (inputstr.Length == 1) inputstr = "a";
                else inputstr = inputstr.Substring(1);


                foreach (var s in ClouddDit.FindAll(f => f.StartsWith(inputstr)))
                {
                    for (int i = 1; i < s.Split(' ').Length; i++)
                    {
                        if (valuestr.IndexOf("|" + s.Split(' ')[i] + "|") < 0)
                        {
                            valuestr += "|" + s.Split(' ')[i] + "|" + s.Split(' ')[0].Replace(inputstr, "") + "\n";

                            count++;
                        }
                        if (count >= 300) break;
                    }
                    if (count >= 300) break;
                }
            }

        }
        /// <summary>
        /// 按输入获取字词
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="dream"></param>
        /// <returns></returns>
        public virtual string[] GetEnInputValue(string inputstr)
        {
            string valuestr = "";

            #region 英文获取
            if (inputstr.Length == 0) return null;
            int count = 0;
            int first = 0, last = EnDit.Length - 1;
            int pcount = SingleInput == true ? 50 : 70;
            #region 取字
            if (indexComplete)
            {
                PosIndex poi = DictIndex.GetEnPos(inputstr.ToLower());
                if (poi == null) return null;
                first = poi.Start;
                last = poi.End;
            }
            else return null;

            for (int i = first; i <= last; i++)
            {
                if (EnDit[i].StartsWith(inputstr.ToLower()))
                {
                    string strarr = EnDit[i];
                    string fcode = strarr.Split(' ')[0];
                    string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取单词
                    int startint = fcode.IndexOf(inputstr) + inputstr.Length;
 
                    if (valuestr.IndexOf("|" + fvalue + "|") < 0)
                    {
                        //去重
                        valuestr += i + "z|" + fvalue + "|\n";
                        count++;
                    }
                }

                if (count > pcount) break;
            }

            #endregion
            #endregion


            if (valuestr.Length > 0)
            {

                string[] vsa = valuestr.Split(new string[1] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < vsa.Length; i++)
                    vsa[i] = Comm.Function.ReplaceSystem(vsa[i]);
                return vsa;
            }

            return null;
        }
 

        /// <summary>
        /// 获取左右一简
        /// </summary>
        /// <param name="input"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public virtual string GetLROne(string code, bool left,int pos=0)
        {
            string vv = string.Empty;
            pos = pos - 1;
            if (pos < 0) pos = 0;
            foreach (var dit in onedict)
            {
                if (dit.StartsWith(code + "="))
                {
                    try
                    {
                        if (left)
                        {
                            vv = dit.Split('=')[1].Split(' ')[0];
                            if (vv.IndexOf("_") > 0 && pos < vv.Split('_').Length)
                            {
                                vv = vv.Split('_')[pos];
                            }
                            else if (vv.IndexOf("_") > 0 && pos >= vv.Split('_').Length)
                            {
                                vv = vv.Split('_')[vv.Split('_').Length - 1];
                            }
                        }
                        else
                        {
                            vv = dit.Split('=')[1].Split(' ')[1];
                            if (vv.IndexOf("_") > 0 && pos < vv.Split('_').Length)
                            {
                                vv = vv.Split('_')[pos];
                            }
                            else if (vv.IndexOf("_") > 0 && pos >= vv.Split('_').Length)
                            {
                                vv = vv.Split('_')[vv.Split('_').Length - 1];
                            }
                        }
                        break;
                    }
                    catch { }
                }
            }
            vv = vv.Replace("~", " ");
            if (!this.IsJT)
            {
                try
                {
                    //转繁体
                    vv = S2TConv(vv);
                }
                catch { }
            }
            if (string.IsNullOrEmpty(vv))
            {
                //取词库本身的一级简码
                var v= GetInputOneValue(code);
                if(v!=null && v.Length > 0)
                {
                    vv = v[0].Split('|')[1];
                    if (left==false && v.Length >1 && v[1].Length>0 && v[1].Split('|')[2].Length==0)
                    {
                        vv = v[1].Split('|')[1];
                    }
                }
            }
            return vv;
        }

        public virtual void CreateIndex(string[] dict, ref List<PosIndex> posl, int p, int s, int e)
        {
            string ts = string.Empty;
            posl = new List<PosIndex>();
            if (e == 0) e = dict.Length;
            for (int i = s; i < e; i++)
            {
                if (!string.IsNullOrEmpty(dict[i]) && dict[i].Split(' ')[0].Length > p - 1)
                {
                    if (string.Compare(ts, dict[i].Substring(0, p), false) != 0)
                    {
                        PosIndex pos = new PosIndex();
                        ts = dict[i].Substring(0, p);
                        if (!string.IsNullOrEmpty(ts))
                        {
                            pos.Letter = ts;
                            pos.Start = i;
                            pos.End = i;
                            if (p==1 && dict[i].Split(' ')[0].Length > 1)
                                pos.subdict.Add(dict[i]);
                            posl.Add(pos);
                        }

                    }
                    else
                    {
                        posl[posl.Count - 1].End = i;
                        if (p == 1 && dict[i].Split(' ')[0].Length > 1)
                            posl[posl.Count - 1].subdict.Add(dict[i]);
                    }
                }
            }
            if (p == 1)
            {
                posl.AsParallel().ForAll(item =>
                {

                    item.mdict = (string[])item.subdict.OrderBy(o => o.Substring(0, 2)).ToArray();
                    item.subdict = null;
                    CreateIndex(item.mdict, ref item.SubIndex, 2, 0, item.mdict.Length);
                });
            }
            //if (p == 1)
            //    posl.AsParallel ().ForAll (item => {
            //        CreateIndex (dict, ref item.SubIndex, 2, item.Start, item.End);
            //    });
        }

        private short firstdh = 1;
        /// <summary>
        /// 全半角输出
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string CheckKeysString(Keys keys)
        {
            if ((this.IsChinese==0 && keys == Keys.Space) || (useregular && keys == Keys.Space)) return " ";
            string str = "";
            if (this.IsQJ)
            {
                #region
                if (this.IsCnBd)
                {
                    #region 不按shift键的情况
                    if (!this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = "　";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < qjzwdict.Length; i++)
                                {
                                    if (keys.ToString() == qjzwdict[i].Split('=')[0] && qjzwdict[i].Split(' ').Length > 1)
                                    {

                                        str = qjzwdict[i].Split('=')[1].Split(' ')[0];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                //if (firstdh == 1)
                                //{
                                //    str = "‘";
                                //    firstdh++;
                                //}
                                //else
                                //{
                                //    str = "’";
                                //    firstdh = 1;
                                //}

                                str = "'";
                            }
                        }
                    }
                    #endregion

                    #region 按shift键的情况
                    if (this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = "";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < qjzwdict.Length; i++)
                                {
                                    if (keys.ToString() == qjzwdict[i].Split('=')[0] && qjzwdict[i].Split(' ').Length > 1)
                                    {

                                        str = qjzwdict[i].Split('=')[1].Split(' ')[1];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                if (firstdh == 1)
                                {
                                    str = "“";
                                    firstdh++;
                                }
                                else
                                {
                                    str = "”";
                                    firstdh = 1;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 不按shift键的情况
                    if (!this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = "　";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < qjywdict.Length; i++)
                                {
                                    if (keys.ToString() == qjywdict[i].Split('=')[0] && qjywdict[i].Split(' ').Length > 1)
                                    {

                                        str = qjywdict[i].Split('=')[1].Split(' ')[0];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                //if (firstdh == 1)
                                //{
                                //    str = "‘";
                                //    firstdh++;
                                //}
                                //else
                                //{
                                //    str = "’";
                                //    firstdh = 1;
                                //}
                                str = "'";
                            }
                        }
                    }
                    #endregion

                    #region 按shift键的情况
                    if (this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = "";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < qjywdict.Length; i++)
                                {
                                    if (keys.ToString() == qjywdict[i].Split('=')[0] && qjywdict[i].Split(' ').Length > 1)
                                    {

                                        str = qjywdict[i].Split('=')[1].Split(' ')[1];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                if (firstdh == 1)
                                {
                                    str = "“";
                                    firstdh++;
                                }
                                else
                                {
                                    str = "”";
                                    firstdh = 1;
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                if (this.IsCnBd)
                {
                    #region 不按shift键的情况
                    if (!this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = "";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < bjzwdict.Length; i++)
                                {
                                    if (keys.ToString() == bjzwdict[i].Split('=')[0] && bjzwdict[i].Split(' ').Length > 1)
                                    {

                                        str = bjzwdict[i].Split('=')[1].Split(' ')[0];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                //if (firstdh == 1)
                                //{
                                //    str = "‘";
                                //    firstdh++;
                                //}
                                //else
                                //{
                                //    str = "’";
                                //    firstdh = 1;
                                //}
                                str = "'";
                            }
                        }
                    }
                    #endregion

                    #region 按shift键的情况
                    if (this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = "";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < bjzwdict.Length; i++)
                                {
                                    if (keys.ToString() == bjzwdict[i].Split('=')[0] && bjzwdict[i].Split(' ').Length > 1)
                                    {
                                        str = bjzwdict[i].Split('=')[1].Split(' ')[1];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                if (firstdh == 1)
                                {
                                    str = "“";
                                    firstdh++;
                                }
                                else
                                {
                                    str = "”";
                                    firstdh = 1;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 不按shift键的情况
                    if (!this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = " ";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < bjywdict.Length; i++)
                                {
                                    if (keys.ToString() == bjywdict[i].Split('=')[0] && bjywdict[i].Split(' ').Length > 1)
                                    {

                                        str = bjywdict[i].Split('=')[1].Split(' ')[0];
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                //if (firstdh == 1)
                                //{
                                //    str = "'";
                                //    firstdh++;
                                //}
                                //else
                                //{
                                //    str = "'";
                                //    firstdh = 1;
                                //}
                                str = "'";
                            }
                        }
                    }
                    #endregion

                    #region 按shift键的情况
                    if (this.IsPressShift)
                    {
                        if (keys == Keys.Space)
                            str = " ";
                        else
                        {
                            if (keys != Keys.Oem7)
                            {
                                for (int i = 0; i < qjywdict.Length; i++)
                                {
                                    if (keys.ToString() == bjywdict[i].Split('=')[0] && bjywdict[i].Split(' ').Length > 1)
                                    {
                                        if (bjywdict[i].IndexOf("== +")>0)
                                        {
                                            str = "+";
                                        }
                                        else
                                        {
                                            str = bjywdict[i].Split('=')[1].Split(' ')[1];
                                        }
                                        break;
                                    }
                                }
                            }
                            else if (keys == Keys.Oem7)
                            {
                                if (firstdh == 1)
                                {
                                    str = "\"";
                                    firstdh++;
                                }
                                else
                                {
                                    str = "\"";
                                    firstdh = 1;
                                }
                            }
                        }
                    }
                    #endregion
                }

            }
            if (InputMode.closebj)
            {
                str = str.Replace("，", ",").Replace("。", ".").Replace("、", "/").Replace("；", ";");
            }
            return str;
        }
        /// <summary>
        /// 判断输入的编码是否是参与的编码的字符
        /// </summary>
        /// <param name="codechar"></param>
        /// <returns></returns>
        public  bool CheckSRCode(string codechar)
        {
            if (codechar == "~") return false;
            bool valuebool = mapkeys.FindAll(f => f.ZM == codechar).Count > 0;

            return valuebool;
        }
        /// <summary>
        /// 判断输入的编码是否是参与的编码的字符
        /// </summary>
        /// <param name="codechar"></param>
        /// <returns></returns>
        public bool CheckCode(string codechar)
        {
             bool valuebool = false;
            for (int i = 0; i < mdcode.Length; i++)
            {
                if (mdcode.Substring(i,1) == codechar)
                {
                    valuebool = true;
                    break;
                }
            }
            if (!valuebool) valuebool = CheckSRCode(codechar);
            return valuebool;
        }
        public bool SRLeft(string s)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (mapstr1.IndexOf(s.Substring(i, 1)) >= 0) count++;
            }

            return count == s.Length;
        }
        public   bool SRRight(string s)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (mapstr2.IndexOf(s.Substring(i, 1)) >= 0) count++;
            }

            return count == s.Length;
        }
        /// <summary>
        /// 判断左右按键
        /// </summary>
        /// <param name="v"></param>
        /// <param name="hleft"></param>
        /// <param name="hright"></param>
        public void CheckLetRight(string v, out bool hleft, out bool hright)
        {
            hleft = false;
            hright = false;
            if (InputMode.closebj) return;

            string oldv = v;
            v = v.Replace("；", ";").Replace("，", ",").Replace("。", ".").Replace("、", "/").Replace("‘", "'").Replace("’", "'").Replace("~", "");
            if (v.Length == 1 && !CheckSRCode(v)) return;
            string s = string.Empty;

            mapsortkeys.FindAll(f => v.IndexOf(f.ZM) >= 0).OrderBy(o => o.Pos).ToList().ForEach(f => s += f.ZM);
            v = s;
            s = string.Empty;


        lbg:
            bool have = false;
            foreach (var m in mapkeys)
            {
                if (v.StartsWith(m.ZM))
                {
                    s += m.Map;
                    v = v.Replace(m.ZM, string.Empty);
                    have = true;
                    hleft = m.left;
                    hright = m.right;
                    //hleft = SRLeft(m.ZM);
                    //hright = SRRight(m.ZM);
                    break;
                }
                if (v.Length == 0) break;
            }
            if (v.Length > 0 && have) goto lbg;
            if (!hright) hleft = true;

        }

        /// <summary>
        /// 排序一下
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public  string SRSort(string v)
        {

            string s = string.Empty;

            mapsortkeys.FindAll(f => v.IndexOf(f.ZM) >= 0).OrderBy(o => o.Pos).ToList().ForEach(f => s += f.ZM);
            v = s;

            return v;
        }
        /// <summary>
        /// 不转换
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public string CovertNoStr(string v)
        {
            return v.Replace("；", ";").Replace("，", ",").Replace("。", ".").Replace("、", "/").Replace("‘", "'").Replace("’", "'");
        }

        /// <summary>
        /// 并击按键转换
        /// </summary>
        /// <param name="v"></param>
        /// <param name="px"></param>
        /// <returns></returns>
        public string CovertStr(string v, bool px = true)
        {
            if (InputMode.closebj && v.Length<4) return v;
            bool hleft = false;
            bool hright = false;
            string oldv = v;
            v = v.Replace("；", ";").Replace("，", ",").Replace("。", ".").Replace("、", "/").Replace("‘", "'").Replace("’", "'");
            v = v.Replace("，", ",").Replace("．", ".");
            if (v == " " && px)
            {

                return oldv;
            }
            if (v.Length == 1 && !CheckSRCode(v) && px) return oldv;
            string s = string.Empty;
            if (px)
            {
                mapsortkeys.FindAll(f => v.IndexOf(f.ZM) >= 0).OrderBy(o => o.Pos).ToList().ForEach(f => s += f.ZM);
                v = s;
                s = string.Empty;
            }
            bool havezh = false;

        lbg:
            bool have = false;
            foreach (var m in mapkeys)
            {
                if (v.StartsWith(m.ZM))
                {
                    s += m.Map;
                    v = v.Replace(m.ZM, string.Empty);
                    have = true;
                    if (m.ZM.Length > 1 && string.Compare(m.Map, m.ZM, true) != 0)
                        havezh = true;
                    if (mapstr1.IndexOf(m.ZM) >= 0) hleft = true;
                    if (mapstr2.IndexOf(m.ZM) >= 0) hright = true;
                    //hleft = m.left;
                    //hright = m.right;
                    m.keydown++;
                    break;
                }
                if (v.Length == 0) break;
            }
            if (v.Length > 0 && have) goto lbg;
            if (px && !havezh && !(hleft && hright))
            {
                return CovertStr(oldv, false);
            }
            return s + v;
        }

        //正则转按键
        public string CovertStrByReg(string v)
        {
            if (InputMode.closebj && v.Length < 4) return v;

            v = v.Replace("；", ";").Replace("，", ",").Replace("。", ".").Replace("、", "/").Replace("‘", "'").Replace("’", "'");
            v = v.Replace("，", ",").Replace("．", ".").Replace("－", "-").Replace("＝", "=");

            try
            {
                //排序
                var node = Win.WinInput.settingYaml.findNodeByKey("alphabet");
                if (node != null && node.name.Length > 0 && node.value.Length > 2)
                    v = Comm.RegHelp.RegexReplace(v, node.name + ":" + node.value);

                foreach (var m in mapkeys)
                {
                    if (v.Length == 0) break;
                    if (v.StartsWith(m.ZM))
                    {
                        //统计按键
                        m.keydown++;
                        break;
                    }
                }

                //正则替换 
                node = Win.WinInput.settingYaml.findNodeByKey("algebra");
                if (node != null)
                {
                    List<Core.Comm.YAMLHelp.Node> nodes = new List<Comm.YAMLHelp.Node>();
                    Win.WinInput.settingYaml.findChildren(node, nodes);
                    if (nodes.Count > 0)
                    {
                        foreach (var item in nodes)
                        {
                            v = Comm.RegHelp.RegexReplace(v, item.value);
                        }
                    }
                }

            }
            catch { }

            return v;
        }

        public static string CovertCodeStrByReg(string v)
        {
            if (InputMode.closebj && v.Length < 4) return v;
            if (v.Length == 0) return v;
            v = v.Replace("；", ";").Replace("，", ",").Replace("。", ".").Replace("、", "/").Replace("‘", "'").Replace("’", "'");
            v = v.Replace("，", ",").Replace("．", ".");

            try
            {
                //正则替换 
                var node = Win.WinInput.settingYaml.findNodeByKey("preedit_format");
                if (node != null)
                {
                    List<Core.Comm.YAMLHelp.Node> nodes = new List<Comm.YAMLHelp.Node>();
                    Win.WinInput.settingYaml.findChildren(node, nodes);
                    if (nodes.Count > 0)
                    {
                        foreach (var item in nodes)
                        {
                            v = Comm.RegHelp.RegexReplace(v, item.value);
                        }
                    }
                }

            }
            catch { }

            return v;
        }
        /// <summary>
        /// 是否关闭线程
        /// </summary>
        public bool Break = false;
        public int inT = 15;
        /// <summary>
        /// win8下的metro桌面
        /// </summary>
        public bool Metor = false;

        /// <summary>
        /// 要绘制的汉字
        /// </summary>
        private static string[] inputv = { "" };
        /// <summary>
        /// 要绘制的提示编码
        /// </summary>
        private static string[] inputc = { "" };
        public static Label[] lbinputv = new Label[1];
        public static Label[] lbinputc = new Label[1];
        public bool Show = false;
        public static int Top = 100;
        public static int Left = 100;
        public static int Width = 170;
        public static int Height = 100;
        public void DealView()
        {
            Top = Win.WinInput.InputStatus.Top;
            Left = Win.WinInput.InputStatus.Left;
            inputv = new string[InputStatusFrm.cachearry.Length];
            inputc = new string[InputStatusFrm.cachearry.Length];
            lbinputv = new Label[InputStatusFrm.cachearry.Length];
            lbinputc = new Label[InputStatusFrm.cachearry.Length];
            for (int i = 0; i < inputv.Length; i++)
            {
                if (!string.IsNullOrEmpty(InputStatusFrm.cachearry[i]))
                {
                    inputv[i] = InputStatusFrm.cachearry[i].Split('|')[1];
                    inputc[i] = InputStatusFrm.cachearry[i].Split('|')[2];

                    if (lbinputv[i] == null) { lbinputv[i] = new Label(); lbinputv[i].AutoSize = true; }
                    lbinputv[i].Font = new Font(InputMode.SkinFontName, InputMode.SkinFontSize);
                    lbinputv[i].Text = inputv[i];

                    if (lbinputc[i] == null) { lbinputc[i] = new Label(); lbinputc[i].AutoSize = true; }
                    lbinputc[i].Font = new Font("", InputMode.SkinFontSize);
                    lbinputc[i].Text = inputc[i];
                }
            }
        }

       
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        public static IntPtr deskDC = GetDC(IntPtr.Zero);
        private static BufferedGraphics grafx = null;
 
        private void ShowWork()
        {
            while (!Break)
            {
                //System.Threading.Thread.Sleep(inT);
                System.Threading.SpinWait.SpinUntil(() => !true, inT);
                try
                {
                    //if (InputMode.imgsend)
                    //    Win.WinInput.ImageInput.Refresh();
                    Win.WinInput.ImageInput.Refresh();
                    if (!Show)
                    {
                        Win.WinInput.InputStatus.Hide();
                        Win.WinInput.InputStatus.HideByApi();
                        continue;
                    }
                    if ((Win.WinInput.InputStatus.inputstr.Length > 0 || InputStatusFrm.Dream) && !Win.WinInput.InputStatus.Visible) { Show = true; Win.WinInput.InputStatus.ShowWindow(true); }
                   
                    
                    //要显示
                    if (InputStatusFrm.cachearry == null) continue;
                    DealView();


                    if (Win.WinInput.InputStatus.ViewType == 0)
                    {
                        //横排显示
                        Win.WinInput.InputStatus.Width = Width = MaxWidth();
                        Win.WinInput.InputStatus.Height = Height = InputMode.SkinHeith;
                        //Win.WinInput.ImageInput.Width = ImageInput.MaxWidth();
                    }
                    else
                    {
                        Win.WinInput.InputStatus.Height = Height = 166;// MaxHeight() + SkinFontJG + 4;
                        Win.WinInput.InputStatus.Width = Width = 200;// MaxWidth();
                    }
                    if (!this.Metor)
                    {
                        System.Threading.Thread.Sleep(50);
                        Win.WinInput.InputStatus.Refresh();
                        
                        continue;
                    }

     
                }
                catch { }
            }
        }
        public static int MaxWidth()
        {
            int vw = 0;
            int rcount = 0;
            for (int i = 0; i < inputv.Length; i++)
            {
                if (i == inputv.Length - 1) vw += 5;
                if (string.IsNullOrEmpty(inputv[i])) continue;
                rcount++;
                string v = InputStatusFrm.GetCutStr(inputv[i]);
                string pos = i == 9 ? "0." : (i + 1).ToString() + ".";
                lbinputv[i].Text = pos + v;
                vw += lbinputv[i].PreferredWidth - 6 + lbinputc[i].PreferredWidth;

            }
            if (vw < 240) vw = 240;
            else if (rcount > 6 && vw < 300) vw = 300;
            return vw;
        }
        public static int MaxHeight()
        {
            int vw = 0;
            for (int i = 0; i < inputv.Length; i++)
            {
                if (string.IsNullOrEmpty(inputv[i])) continue;
                vw += SkinFontH;
            }
            if (vw < 100) vw = 100;
     
            return vw;
        }
        public static int GetWidth(string str)
        {
            return 5 + str.Length * SkinFontW;
         
        }
       
        private static void SView(int inputy)
        {
            SolidBrush bstring = new SolidBrush(Skinbstring);
            SolidBrush bcstring = new SolidBrush(Skinbcstring);
            SolidBrush fbcstring = new SolidBrush(Skinfbcstring);
            string ins = InputStatusFrm.Dream ? "智能联想" : Win.WinInput.InputStatus.inputstr;
            grafx.Graphics.DrawString(ins, new Font(SkinFontName, SkinFontSize, FontStyle.Bold), bstring, new Point(Left + 3, Top + 4));
            if (Win.WinInput.InputStatus.valuearry != null && Win.WinInput.InputStatus.valuearry.Length > 0
                && !InputStatusFrm.Dream) //分页数显示
                grafx.Graphics.DrawString(string.Format("{0}/{1}", Win.WinInput.InputStatus.PageNum
                    , (Win.WinInput.InputStatus.valuearry.Length % Win.WinInput.InputStatus.PageSize == 0 ? Win.WinInput.InputStatus.valuearry.Length / Win.WinInput.InputStatus.PageSize : Win.WinInput.InputStatus.valuearry.Length / Win.WinInput.InputStatus.PageSize + 1))
                    , new Font("", 10F), bstring, new Point(Left+Width - 44, Top + 4));
            for (int i = 0; i < inputv.Length; i++)
            {
                if (string.IsNullOrEmpty(inputv[i])) break;
                string v =InputStatusFrm.GetCutStr(inputv[i]);
                string pos = i == 9 ? "0." : (i + 1).ToString() + ".";
                Font tfont = new Font(SkinFontName, SkinFontSize);
                int vw = GetWidth(inputv[i]);
                if (i == 0)
                    grafx.Graphics.DrawString(pos + v, tfont, fbcstring, new Point(Left + 3, Top + inputy + i * SkinFontH));
                else
                    grafx.Graphics.DrawString(pos + v, tfont, bstring, new Point(Left + 3, Top + inputy + i * SkinFontH));

                grafx.Graphics.DrawString(inputc[i], new Font("宋体", SkinFontSize - 1), bcstring, new Point(Left + 3 + vw, (Top + inputy + i * SkinFontH) - 1));
            }

            grafx.Render(deskDC);

        }

        private static void HView(int inputy)
        {
            SolidBrush bstring = new SolidBrush(Skinbstring);
            SolidBrush bcstring = new SolidBrush(Skinbcstring);
            SolidBrush fbcstring = new SolidBrush(Skinfbcstring);
            string ins = InputStatusFrm.Dream ? "智能联想" : Win.WinInput.InputStatus.inputstr;
            grafx.Graphics.DrawString(ins, new Font(SkinFontName, SkinFontSize, FontStyle.Bold), bstring, new Point(Left + 3, Top + 4));
            if (Win.WinInput.InputStatus.valuearry != null && Win.WinInput.InputStatus.valuearry.Length > 0
                && !InputStatusFrm.Dream) //分页数显示
                grafx.Graphics.DrawString(string.Format("{0}/{1}", Win.WinInput.InputStatus.PageNum
                    , (Win.WinInput.InputStatus.valuearry.Length % Win.WinInput.InputStatus.PageSize == 0 ? Win.WinInput.InputStatus.valuearry.Length / Win.WinInput.InputStatus.PageSize : Win.WinInput.InputStatus.valuearry.Length / Win.WinInput.InputStatus.PageSize + 1))
                    , new Font("", 10F), bstring, new Point(Left + Width - 44, Top + 4));

            int wx = 1;

            for (int i = 0; i < inputv.Length; i++)
            {
                if (string.IsNullOrEmpty(inputv[i])) break;
                string v = InputStatusFrm.GetCutStr(inputv[i]);
                string pos = i == 9 ? "0." : (i + 1).ToString() + ".";
                Font tfont = new Font(SkinFontName, SkinFontSize);
                 
                if (i == 0)
                    grafx.Graphics.DrawString(pos + v, tfont, fbcstring, new Point(Left + wx, Top + inputy));
                else
                    grafx.Graphics.DrawString(pos + v, tfont, bstring, new Point(Left + wx, Top + inputy));
                InputMode.lbinputv[i].Text = pos + v;
                if (InputMode.lbinputv[i].Text.Length > 3)
                    wx += InputMode.lbinputv[i].PreferredWidth - 10;
                else
                    wx += InputMode.lbinputv[i].PreferredWidth - 7;

                grafx.Graphics.DrawString(inputc[i], new Font("宋体", SkinFontSize - 1), bcstring, new Point(Left + wx, Top+inputy));
                if (string.IsNullOrEmpty(InputMode.lbinputc[i].Text))
                {
                    wx += 4;
                }
                else
                {
                    wx += InputMode.lbinputc[i].PreferredWidth;
                    if (InputMode.lbinputv[i].Text.Length > 3)
                        wx += -4;
                }
            }

            grafx.Render(deskDC);

        }

        /// <summary>
        /// 显示输入法状态栏
        /// </summary>
        private void ShowLog()
        {
            while (!Break)
            {
                //System.Threading.Thread.Sleep(inT);
                System.Threading.SpinWait.SpinUntil(() => !true, inT);
                try
                {
                    if (!this.isActiveInput)
                    {
                        //不在激活状态隐藏输入法
                        if (Program.MIme.Visible) Program.MIme.Hide();
                        continue;
                    }

                    if (!this.Metor)
                    {
                        System.Threading.Thread.Sleep(200);
                        continue;
                    }

                    //context = BufferedGraphicsManager.Current;

                    //Rectangle inputg = new Rectangle(Program.MIme.Left + 2, Program.MIme.Top + 3, 20, 19);
                    //bjgrafx = context.Allocate(deskDC, inputg);
                    //bjgrafx.Graphics.DrawImage(bglogimag, new Rectangle(Program.MIme.Left + 2, Program.MIme.Top + 3, 20, 19));

                    //bjgrafx.Render(deskDC);
                }
                catch { }
            }
        }


        public void ClearShow()
        {

            this.Show = false;
        }
        public System.Threading.Thread workth = null;
        public System.Threading.Thread sworkth = null;
        public void ShowInput(bool show)
        {
            //deskDC = GetDCEx(desk, IntPtr.Zero, 0x403);

            Show = show;

            if (workth == null)
            {
                workth = new System.Threading.Thread(new System.Threading.ThreadStart(ShowWork));

                workth.Start();
            }
            if (sworkth == null)
            {
                sworkth = new System.Threading.Thread(new System.Threading.ThreadStart(ShowLog));

                sworkth.Start();
            }

        }
    }
 
}

/// <summary>
/// 索引类
/// </summary>
public class PosIndex
{
    public string Letter = string.Empty;
    public int Start = 0;
    public int End = 0;

    public List<string> subdict = new List<string>();
    public string[] mdict = null;
    public List<PosIndex> SubIndex = new List<PosIndex>();

}
/// <summary>
    /// 索引管理类
    /// </summary>
    public class IndexManger
    {
        public List<PosIndex>  IndexList=new List<PosIndex>();
        public List<PosIndex> EnList = new List<PosIndex>();
        public List<PosIndex> ProList = new List<PosIndex>();
    /// <summary>
    /// 通过输入获取索引对象
    /// </summary>
    /// <param name="str">String.</param>
    public PosIndex GetPos(string str, ref string[] dict,bool f=true)
    {
        dict = null;
        if (str.Length == 1)
        {
            return IndexList.Find(i => i.Letter == str);
        }
        else if (str.Length > 1)
        {
            if (f)
            {
                var o = IndexList.Find(i => i.Letter == str.Substring(0, 1));
                if (o != null)
                {
                    dict = o.mdict;
                    return o.SubIndex.Find(i => i.Letter == str.Substring(0, 2));
                }
                else
                    return null;
            }
            else
            {
                var o = IndexList.Find(i => i.Letter == str.Substring(0, 1));
                return o;
            }
        }

        return null;
    }

        /// <summary>
        /// 通过输入获取索引对象
        /// </summary>
        /// <param name="str">String.</param>
        public PosIndex GetEnPos(string str)
        {
            if (str.Length == 1) {
                return EnList.Find (i => i.Letter == str);
            }
            else if (str.Length > 1)
            {
                var o = EnList.Find(i => i.Letter == str.Substring(0, 1));
                return o;
            }

            return null;
        }

        /// <summary>
        /// 通过输入获取索引对象
        /// </summary>
        /// <param name="str">String.</param>
        public PosIndex GetProPos(string str)
        {
            if (str.Length == 1)
            {
                return ProList.Find(i => i.Letter == str);
            }
            else if (str.Length > 1)
            {
                var o = ProList.Find(i => i.Letter == str.Substring(0, 1));
                return o;
            }

            return null;
        }
    }
public class MapintKey
{
    public string ZM = string.Empty;
    public string Map = string.Empty;
    public short Pos = 0;
    public bool zfzh = false;

    public Keys ZMK;
    public Keys MapK;

    public bool right = false;
    public bool left = false;
    public long keydown = 0;
}
 
