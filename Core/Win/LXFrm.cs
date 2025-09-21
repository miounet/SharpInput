using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Core.Win
{
    public partial class LXFrm : Form
    {
        public LXFrm()
        {
            InitializeComponent();

        }


        ~LXFrm()
        {
            GC.Collect();
        }
 
        
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private int linesed = 1;
        private int curp = 0;
        private int cwzs = 0;
        private int testzs = 0;
        private int oldtestzs = 0;
        private DateTime? startdate = null;
        private string flag = "";
        private string title = "";
        private int pagesize = 5000;
        private int page = 1;
        private string alltxt = "";
        private long bclxdczs = 0;
        private long dztcount = 0;
        private long daydzcount = 0;
        private long olddaydzcount = 0;

        private long dayddzcount = 0; //今日打单次数
        private long daydccount = 0;//今日打词次数
        private long ddzcount = 0;//共打单次数
        private long dccount = 0;//共打词次数
        private long daydczscount = 0;//今日打词字数
        private long dczscount = 0;//共打词字数
        private long bjtcount = 0;//并击总次数
        private DateTime dztjdate = DateTime.Now.Date;
        private void restsbut_Click(object sender, EventArgs e)
        {
            Win.WinInput.keybjnum = 0;
            Win.WinInput.inputdznum = 0;
            Win.WinInput.inputcznum = 0;
            Win.WinInput.inputczsnum = 0;
            this.richTextBox1.ForeColor = Color.Black;
            richTextBox1.Text = txt;

            richTextBox1.Select(0, richTextBox1.Text.Length);
            richTextBox1.SelectionBackColor = System.Drawing.Color.Transparent;
            linesed = 1;
            richTextBox2.Clear();
            curp = 0;
            cwzs = 0;
            testzs = 0;
            oldtestzs = 0;
            bclxdczs = 0;
            tsspeed.Text = "0.00";
            tszqltxt.Text = "0.00";
            mctxt.Text = "0.00";
            jstxt.Text = "0.00";
            toolStripButton2.Text = richTextBox1.Text.Length.ToString();
            startdate = null;
            timer1.Enabled = false;

            if (richTextBox1.Text.Length == 0)
            {
                toolStripButton10.Text = "没有了";
            }
            richTextBox2.ReadOnly = false;
            richTextBox1.ScrollToCaret();
            richTextBox2.Select();
            richTextBox2.Focus();

            if (dictcode.Count > 0)
            {
                start_ai();
                render();
            }
        }
        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        public static extern int GetScrollPos(IntPtr hwnd, int nBar);
        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                if (Core.Win.WinInput.Input.isActiveInput)
                {
                    if (!richTextBox2.Enabled)
                    {
                        richTextBox2.Enabled = true;
                        richTextBox2.Focus();
                    }
                }
                else
                {
                    richTextBox2.Enabled = false;
                }

                double dl = ((TimeSpan)(DateTime.Now - startdate)).TotalSeconds;

                tsspeed.Text = (Math.Round((richTextBox2.Text.Length / dl) * 60, 2)).ToString();
                tszqltxt.Text = Math.Round(((richTextBox1.Text.Length - (double)cwzs) / richTextBox1.Text.Length) * 100, 2).ToString();

                jstxt.Text = Math.Round(Win.WinInput.keybjnum / dl, 2).ToString();
                if (ddzs > 0)
                    mctxt.Text = Math.Round(Win.WinInput.keybjnum * 1.0 / ddzs * 1.0, 2).ToString();
                else mctxt.Text = "0.0";

                    dayddzcount += Win.WinInput.inputdznum;
                daydccount += Win.WinInput.inputcznum;
                daydczscount += Win.WinInput.inputczsnum;
                bclxdczs += Win.WinInput.inputczsnum;
                ddzcount += Win.WinInput.inputdznum;
                dccount += Win.WinInput.inputcznum;
                dczscount += Win.WinInput.inputczsnum;

                Win.WinInput.inputdznum = 0;
                Win.WinInput.inputcznum = 0;
                Win.WinInput.inputczsnum = 0;

                if (this.richTextBox1.Text.Length <= this.richTextBox2.Text.Length &&
                    this.richTextBox1.Text.Substring(this.richTextBox1.Text.Length - 1) == this.richTextBox2.Text.Substring(this.richTextBox2.Text.Length - 1))
                {
                    timer1.Enabled = false;
                    if (this.richTextBox1.Text == this.richTextBox2.Text.Substring(0, this.richTextBox1.Text.Length))
                        tszqltxt.Text = "100";
                    if (double.Parse(tszqltxt.Text) < 0) tszqltxt.Text = "0.00";
                    richTextBox2.ReadOnly = true;
                    bjtcount += Win.WinInput.keybjnum;
                    string bccjtxt = (flag.Length > 0 ? flag + " " : "")
                        + "速度" + (double.Parse(tszqltxt.Text) < 100 ? Math.Round(double.Parse(tsspeed.Text) * double.Parse(tszqltxt.Text) / 100.0, 2) + "/" : "") + tsspeed.Text
                        + " 击键" + jstxt.Text + " 码长" + mctxt.Text + " 字数" + this.richTextBox1.Text.Length
                        + " 回改" + (testzs - this.richTextBox1.Text.Length) + " 键数" + Win.WinInput.keybjnum
                        + " 键准" + Math.Round((1 - ((testzs - this.richTextBox1.Text.Length) * 1.0 / this.richTextBox1.Text.Length)) * 100, 2) + "%"
                        + " 准确率" + tszqltxt.Text + "%"
                        + " 打词" + Math.Round((1 - ((this.richTextBox1.Text.Length - bclxdczs) * 1.0 / this.richTextBox1.Text.Length)) * 100, 2) + "%"
                          + (toolStripTextBox3.Text.Trim().Length > 0 ? " 输入法:" + toolStripTextBox3.Text.Trim() : "");
                    bccjtxt += " 校检:" + Core.Comm.Security.MD5EncryptTo16(Core.Comm.Security.GetMD5(bccjtxt));
                    bccjtxt += " 速录宝v" + Program.ProductVer;
                    Win.WinInput.keybjnum = 0;
                    bclxdczs = 0;
                    Clipboard.SetText(bccjtxt);
                    timer1.Enabled = false;

                    //加成绩
                    string[] cjar = new string[9];
                    cjar[0] = DateTime.Now.ToString();
                    cjar[1] = (double.Parse(tszqltxt.Text) < 100 ? Math.Round(double.Parse(tsspeed.Text) * double.Parse(tszqltxt.Text) / 100.0, 2) + "/" : "") + tsspeed.Text;
                    cjar[2] = jstxt.Text;
                    cjar[3] = mctxt.Text;
                    cjar[4] = this.richTextBox1.Text.Length.ToString();
                    cjar[5] = (testzs - this.richTextBox1.Text.Length).ToString();
                    cjar[6] = tszqltxt.Text + "%";
                    cjar[7] = Math.Round((1 - ((this.richTextBox1.Text.Length - bclxdczs) * 1.0 / this.richTextBox1.Text.Length)) * 100, 2) + "%";
                    cjar[8] = jstxt.Text;
                    dataGridView1.Rows.Insert(0,cjar);
                    dataGridView1.Rows[0].Selected = true;
                }
               
            }
            catch { }
        }
        private string txt = string.Empty;
        private void LXFrm_Load(object sender, EventArgs e)
        {

            if (!File.Exists(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                , "testini.txt")))
            {
                File.WriteAllText(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                , "testini.txt"), @"25
并击並擊
5000
0

", Encoding.UTF8);
            }
            if (!File.Exists(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
    , "testdata.txt")))
            {
                File.WriteAllText(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                , "testdata.txt"), @"0_0_" + DateTime.Now.ToShortDateString() + "_0_0_0_0_0_0_0", Encoding.UTF8);
            }
            var setting = File.ReadAllLines(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                , "testini.txt"), Encoding.UTF8);//读配置
            var testdata = File.ReadAllLines(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
               , "testdata.txt"), Encoding.UTF8);//读 
            if (setting.Length > 4)
            {
                toolStripTextBox1.Text = setting[0].Trim();
                toolStripTextBox2.Text = setting[1].Trim();
                toolStripTextBox4.Text = setting[2].Trim();
                toolStripComboBox1.SelectedIndex = int.Parse(setting[3].Trim());
                toolStripTextBox3.Text = setting[4].Trim();
            }
            if (testdata.Length > 0)
            {
                if (testdata[0].Split('_').Length > 2)
                {
                    dztcount = long.Parse(testdata[0].Split('_')[1]);

                    if (DateTime.Parse(testdata[0].Split('_')[2]) == dztjdate)
                    {
                        daydzcount = long.Parse(testdata[0].Split('_')[0]);
                        olddaydzcount = daydzcount;
                        if (testdata[0].Split('_').Length > 9)
                        {
                            dayddzcount = long.Parse(testdata[0].Split('_')[3]);
                            ddzcount = long.Parse(testdata[0].Split('_')[4]);
                            daydccount = long.Parse(testdata[0].Split('_')[5]);
                            dccount = long.Parse(testdata[0].Split('_')[6]);
                            daydczscount = long.Parse(testdata[0].Split('_')[7]);
                            dczscount = long.Parse(testdata[0].Split('_')[8]);
                            bjtcount = long.Parse(testdata[0].Split('_')[9]);
                        }
                    }
                    else
                    {
                        daydzcount = 0;
                        olddaydzcount = 0;
                        dayddzcount = 0;
                        daydccount = 0;
                        daydczscount = 0;
                    }
                    toolStripLabel2.Text = "打字记录: " + daydzcount + "/" + dztcount;
                    toolStripLabel2.ToolTipText = "记录: " + daydzcount + "/" + dztcount + " 单字: " + dayddzcount + "/" + ddzcount + " 词组字数: " + daydczscount + "/" + dczscount + " 词组: " + daydccount + "/" + dccount + " 并击数:" + bjtcount;
                }
            }

            //SetLineSpace(richTextBox1, int.Parse(toolStripTextBox1.Text) + 48);
            clmode.SelectedIndex = 0;
            refreshQun();
        }
        private void saveTestini()
        {
            try
            {
                File.WriteAllText(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                   , "testini.txt"), String.Format(@"{0}
{1}
{2}
{3}
{4}
", toolStripTextBox1.Text, toolStripTextBox2.Text, toolStripTextBox4.Text, toolStripComboBox1.SelectedIndex, toolStripTextBox3.Text));
            }
            catch { }
        }
        private void txtlx()
        {
            if (toolStripComboBox1.SelectedIndex == 1)
            {
                int textLength = alltxt.Length;
                string str = "";
                if (textLength > 9)
                {
                    foreach (int num2 in this.GetRandomUnrepeatArray(0, textLength - 1, textLength))
                    {
                        str = str + alltxt.Substring(num2, 1);
                    }
                    alltxt = str;
                }
            }
        }
        private int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            Random random = new Random();
            int num = (maxValue - minValue) + 1;
            byte[] buffer = new byte[num];
            random.NextBytes(buffer);
            int[] items = new int[num];
            for (int i = 0; i < num; i++)
            {
                items[i] = i + minValue;
            }
            Array.Sort<byte, int>(buffer, items);
            int[] destinationArray = new int[count];
            Array.Copy(items, destinationArray, count);
            return destinationArray;
        }



        private void updatesorce_Tick(object sender, EventArgs e)
        {

            //System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
            //       {
            //           ApiClient.UpdataScore(opt, double.Parse(tszqltxt.Text), double.Parse(tsspeed.Text));
            //       });
            //task.Start();

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnbyjtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Clipboard.GetText()))
            {
                flag = "";
                txt = Clipboard.GetText().Replace("\r\n", "").Trim();
                if (txt.IndexOf("-----") > 0)
                {
                    flag = txt.Substring(txt.IndexOf("-----") + 5);
                    if (flag.IndexOf('-') > 0)
                        flag = flag.Split('-')[0];
                    else
                        flag = flag.Split(' ')[0];
                    txt = txt.Substring(0, txt.IndexOf("-----"));
                }
                alltxt = txt;
                page = 1;
                settxt();
                title = "剪贴板内容";
                restsbut_Click(null, e);
            }
        }

        private void btnopenfile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "选择练习文件(*.txt)|*.txt";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\TEST";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                CheckForIllegalCrossThreadCalls = false;

                txt = File.ReadAllText(openFileDialog1.FileName, Encoding.Default).Replace("\r\n", "");
                alltxt = txt;
                txtlx();
                page = 1;
                settxt();
                title = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1).Split('.')[0];
                restsbut_Click(null, e);
                flag = "第1段";
            }
        }
        public static Match getDuan;


        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            string qqtxt = getQQText();

            if (qqtxt.Length != 0)
            {
                string str3;
                string str4;

                str3 = "-----";
                str4 = @"第\d+段";
                getDuan = new Regex(@".+\s.+\s" + str3 + str4 + ".+", RegexOptions.RightToLeft).Match(qqtxt);
                if (getDuan.Length > 0)
                {

                    string getDuanAll = getDuan.ToString();
                    if (getDuanAll.Length > 0)
                    {
                        var txtlist = getDuanAll.Split('\n');
                        flag = "";
                        txt = getDuanAll.Trim();
                        if (txtlist.Length > 2)
                        {
                            title = txtlist[0].Trim();
                            flag = txtlist[2].Trim().Replace("-----", "");
                            if (flag.IndexOf('-') > 0)
                                flag = flag.Split('-')[0];
                            else
                                flag = flag.Split(' ')[0];

                            txt = txtlist[1].Trim();
                            //txt = txt.Substring(0, txt.IndexOf("-----"));
                        }
                        alltxt = txt;
                        page = 1;
                        int psize = pagesize;
                        pagesize = alltxt.Length * 2;
                        settxt();
                        pagesize = psize;
                        restsbut_Click(null, e);
                    }
                }
            }
        }

        private void settxt()
        {
            if (pagesize < alltxt.Length)
            {
                if ((page - 1) * pagesize + pagesize > alltxt.Length && (page - 1) * pagesize <= alltxt.Length)
                {
                    txt = alltxt.Substring((page - 1) * pagesize);
                    toolStripButton10.Text = "还剩0";
                }
                else if ((page - 1) * pagesize + pagesize <= alltxt.Length)
                {
                    txt = alltxt.Substring((page - 1) * pagesize, pagesize);
                    toolStripButton10.Text = "还剩" + (alltxt.Length - ((page - 1) * pagesize + pagesize)).ToString();
                }

            }
            else
            {
                txt = alltxt;
                toolStripButton10.Text = "还剩0";
            }
            richTextBox1.Text = txt;
        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.richTextBox1.Font = new Font("宋体", float.Parse(toolStripTextBox1.Text));
                saveTestini();
                if (dictcode.Count > 0)
                {
                    start_ai();
                    render();
                }
            }
            catch { }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        [DllImport("user32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        public bool Delay(int delayTime)
        {
            int totalMilliseconds;
            DateTime now = DateTime.Now;
            do
            {
                TimeSpan span = (TimeSpan)(DateTime.Now - now);
                totalMilliseconds = (int)span.TotalMilliseconds;
            }
            while (totalMilliseconds < delayTime);
            return true;
        }

        [DllImport("User32")]
        public static extern bool GetCursorPos(ref Point cPoint);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("User32")]
        public static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, IntPtr extraInfo);

        private enum MouseEventFlag : uint
        {
            Absolute = 0x8000,
            LeftDown = 2,
            LeftUp = 4,
            MiddleDown = 0x20,
            MiddleUp = 0x40,
            Move = 1,
            RightDown = 8,
            RightUp = 0x10,
            VirtualDesk = 0x4000,
            Wheel = 0x800,
            XDown = 0x80,
            XUp = 0x100
        }




        Random random1 = new Random();
        [DllImport("user32")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public string getQQText()
        {
            try
            {
                string text = toolStripTextBox2.Text;
                if (text.Length > 0)
                {
                    Point cPoint = new Point();
                    SwitchToThisWindow(FindWindow(null, "并击测速"), true);
                    IntPtr extraInfo = FindWindow(null, text);
                    if (extraInfo.ToString() != "0")
                    {
                        Clipboard.Clear();
                        SwitchToThisWindow(FindWindow(null, text), true);
                        Delay(50);

                        GetCursorPos(ref cPoint);
                        RECT lpRect = new RECT();
                        GetWindowRect(FindWindow("TXGuiFoundation", text), ref lpRect);
                        SetCursorPos(lpRect.Left + 20, lpRect.Top + 200);
                        Delay(50);
                        mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, extraInfo);
                        Delay(20);
                        mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, extraInfo);

                        Delay(80);
                        SendKeys.SendWait("^a");
                        //全选
                        keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                        Delay(30);
                        keybd_event((byte)Keys.A, 0, 0, 0);

                        Delay(30);
                        keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                        Delay(30);
                        keybd_event((byte)Keys.A, 0, 0x2, 0);

                        SendKeys.SendWait("^c");
                        //复制
                        Delay(80);
                        keybd_event((byte)Keys.ControlKey, 0, 0, 0);//按下
                        Delay(30);
                        keybd_event((byte)Keys.C, 0, 0, 0);
                        Delay(30);
                        keybd_event((byte)Keys.ControlKey, 0, 0x2, 0);//松开
                        Delay(30);
                        keybd_event((byte)Keys.C, 0, 0x2, 0);
                        Delay(50);
                        string input = "";
                        try
                        {
                            input = Clipboard.GetText();
                            Regex regex = new Regex("\r(?!\n)");
                            if (regex.IsMatch(input))
                            {
                                input = input.Replace("\r", "\r\n");
                            }
                        }
                        catch
                        {
                            base.Activate();
                            Delay(200);
                            SendKeys.SendWait("^s");

                            return "";
                        }
                        SetCursorPos(cPoint.X, cPoint.Y);
                        base.Activate();
                        return input;
                    }

                }
            }
            catch { }
            return "";
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            page++;
            if ((page - 1) * pagesize < alltxt.Length)
            {
                settxt();
                flag = "第" + page + "段";
                restsbut_Click(null, e);
            }
            else
            {
                page = 1;
                txtlx();
                settxt();
                flag = "第" + page + "段";
                restsbut_Click(null, e);
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                int hsnum = alltxt.Length - ((page - 1) * pagesize + pagesize);

                string sendtxt = (title.Length > 0 ? title : "跟打内容") + "\r\n" + txt + "\r\n-----第" + page + "段-" + (hsnum >= 0 ? "(还剩" + hsnum + "字)" : "") + " 速录宝 并击测速 群374971723";
                sendtext(sendtxt);
                base.Activate();
            }
            catch { base.Activate(); }
        }

        public void sendtext(string text)
        {
            if (text != "")
            {

                string lpWindowName = toolStripTextBox2.Text.ToString();
                if (lpWindowName.Length > 0)
                {
                    IntPtr qqptr = FindWindow(null, lpWindowName);
                    if (qqptr.ToInt64() > 0)
                    {
                        SwitchToThisWindow(FindWindow(null, this.Text), true);
                        Clipboard.Clear();
                        Delay(50);
                        Clipboard.SetDataObject(text);
                        SwitchToThisWindow(FindWindow(null, lpWindowName), true);
                        Delay(random1.Next(90, 120));
                        SendKeys.SendWait("^a^v");
                        Delay(random1.Next(150, 198));
                        SendKeys.SendWait("%s");
                        Delay(random1.Next(50, 99));
                    }
                }
            }
        }
 

        private void toolStripTextBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                pagesize = int.Parse(toolStripTextBox4.Text);
                saveTestini();
            }
            catch
            {

            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text.Length == this.richTextBox2.Text.Length)
            {
                string text = Clipboard.GetText();
                if (text != "")
                {

                    string lpWindowName = toolStripTextBox2.Text.ToString();
                    if (lpWindowName.Length > 0)
                    {
                        IntPtr qqptr = FindWindow(null, lpWindowName);
                        if (qqptr.ToInt64() > 0)
                        {
                            SwitchToThisWindow(FindWindow(null, this.Text), true);
                            Delay(random1.Next(30, 60));

                            SwitchToThisWindow(FindWindow(null, lpWindowName), true);
                            Delay(random1.Next(120, 160));
                            SendKeys.SendWait("^v");
                            Delay(random1.Next(150, 200));
                            SendKeys.SendWait("%s");
                            Delay(random1.Next(60, 110));
                        }

                        base.Activate();
                    }

                }
            }
        }


        private int ddzs = 0;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (oldtestzs == 0 || oldtestzs < richTextBox2.TextLength)
            {
                testzs++;
                daydzcount++;
                dztcount++;
            }

            try
            {
                if (richTextBox2.TextLength > 0)
                {
                    if (this.richTextBox2.Text.Length > ddzs)
                        ddzs = this.richTextBox2.Text.Length;

                    if (startdate == null)
                    {
                        startdate = DateTime.Now;

                        timer1.Enabled = true;
                    }
                    int cs = richTextBox2.TextLength - curp;
                    if (cs > 0)
                    {
                        for (int i = cs; i > 0; i--)
                        {
                            richTextBox1.Select(richTextBox2.TextLength - i, 1);
                            if (richTextBox2.Text.Substring(richTextBox2.TextLength - i, 1) == richTextBox1.SelectedText)
                            {
                                richTextBox1.SelectionBackColor = System.Drawing.Color.Gray;
                            }
                            else
                            {
                                richTextBox1.SelectionBackColor = System.Drawing.Color.Red;
                                cwzs = cwzs + 1;

                            }
                            if ((richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1) / linesed >= 6)
                            {
                                //一.得到垂直滚动条的位置
                                int pi = GetScrollPos(this.richTextBox1.Handle, 1);

                                //二.难点在这一步，如何取得listView1控件的滚动条区域的长度
                                int lb = 1;

                                //三.判断
                                if (pi != lb)
                                {
                                    richTextBox1.ScrollToCaret();
                                    linesed++;

                                    if (dictcode.Count > 0)
                                    {
                                        start_ai();
                                        render();
                                    }
                                }


                            }
                        }
                    }
                    else
                    {
                        richTextBox1.Select(richTextBox2.TextLength, 1);
                        if (richTextBox1.SelectionBackColor == System.Drawing.Color.Red)
                            cwzs = cwzs - 1;
                        richTextBox1.SelectionBackColor = System.Drawing.Color.Transparent;

                    }

                    curp = richTextBox2.TextLength;
                }
                else
                {
                    ddzs = 0;
                    richTextBox1.Select(0, 1);
                    if (richTextBox1.SelectionBackColor == System.Drawing.Color.Red)
                        cwzs = cwzs - 1;
                    richTextBox1.SelectionBackColor = System.Drawing.Color.Transparent;
                    curp = 0;
                }

                if (richTextBox2.TextLength >= richTextBox1.TextLength
                    && this.richTextBox1.Text.Substring(this.richTextBox1.Text.Length - 1) == this.richTextBox2.Text.Substring(this.richTextBox2.Text.Length - 1))
                {
                    richTextBox2.ReadOnly = true;
                }
                oldtestzs = richTextBox2.TextLength;
            }
            catch { }
        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            saveTestini();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveTestini();
        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            saveTestini();
        }

        private void LXFrm_Activated(object sender, EventArgs e)
        {
            if (startdate == null)
            {
                Win.WinInput.keybjnum = 0;
                Win.WinInput.inputdznum = 0;
                Win.WinInput.inputcznum = 0;
                Win.WinInput.inputczsnum = 0;
            }
            richTextBox2.Focus();
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P && e.Control)
            {
                //下一段
                toolStripButton15_Click(null, null);
            }
            if (e.KeyCode == Keys.E && e.Control)
            {
                //剪贴板载文
                btnbyjtn_Click(null, null);
            }
            if (e.KeyCode == Keys.L && e.Control)
            {
                //本段乱序
                int textLength = txt.Length;
                string str = "";
                if (textLength > 4)
                {
                    foreach (int num2 in this.GetRandomUnrepeatArray(0, textLength - 1, textLength))
                    {
                        str = str + txt.Substring(num2, 1);
                    }
                    txt = str;

                    restsbut_Click(null, null);
                }
            }
            else if (e.KeyCode == Keys.F3)
            {
                //重打
                restsbut_Click(null, null);
            }
            else if (e.KeyCode == Keys.F4)
            {
                //QQ载文
                toolStripButton6_Click(null, null);
            }
            else if (e.KeyCode == Keys.F9)
            {
                //发送成绩
                toolStripButton11_Click(null, null);
            }
            else if (e.KeyCode == Keys.F7)
            {
                //向Q群分享发文
                toolStripButton9_Click(null, null);
            }
        }

        private long timercount = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            try
            {
                timercount++;

                if (dztjdate != DateTime.Now.Date)
                    daydzcount = 0;

                if (daydzcount != olddaydzcount)
                {
                    olddaydzcount = daydzcount;
                    toolStripLabel2.Text = "打字记录: " + daydzcount + "/" + dztcount;
                    toolStripLabel2.ToolTipText = "记录: " + daydzcount + "/" + dztcount + " 单字: " + dayddzcount + "/" + ddzcount + " 词组字数: " + daydczscount + "/" + dczscount + " 词组: " + daydccount + "/" + dccount + " 并击数:" + bjtcount;
                    if (timercount > 9)
                    {
                        timercount = 0;
                        if (Base.InputMode.autodata)
                            File.WriteAllText(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                   , "testdata.txt"), String.Format(@"{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}"
, daydzcount, dztcount, DateTime.Now.ToShortDateString(), dayddzcount, ddzcount, daydccount, dccount, daydczscount, dczscount, bjtcount));
                    }
                }
            }
            catch { }
            timer2.Enabled = true;

        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            MapCount frm = new MapCount();
            frm.ShowDialog();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            SZCount frm = new SZCount();
            frm.ShowDialog();
        }

        private void LXFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timercount = 0;
                if (dztjdate != DateTime.Now.Date)
                    daydzcount = 0;

                File.WriteAllText(System.IO.Path.Combine(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
                          , "testdata.txt"), String.Format(@"{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}"
       , daydzcount, dztcount, DateTime.Now.ToShortDateString(), dayddzcount, ddzcount, daydccount, dccount, daydczscount, dczscount, bjtcount));
            }
            catch { }
        }
        string curtsdz = "";
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Core.Win.WinInput.Input.isActiveInput)
                {
                    if (!richTextBox2.Enabled)
                    {
                        richTextBox2.Enabled = true;
                    }
                }
                else
                {
                    richTextBox2.Enabled = false;
                }

                if (richTextBox1.Text.Length > 0 && richTextBox2.Text.Length < richTextBox1.Text.Length)
                {
                    lbbmtstxt.Visible = clmode.SelectedIndex == 2;
                    string viewword = richTextBox1.Text.Substring(richTextBox2.Text.Length, 1);
                    if (curtsdz != viewword)
                        if (dictcp.ContainsKey(viewword))
                        {
                            curtsdz = viewword;
                            var cp = dictcp[viewword];
                            var cpl = cp.dicts.Where(w => w.txt == viewword).ToList();
                            if (cpl.Count > 0) {
                                cpl = cpl.OrderBy(o => o.code.Length).ToList();
                                lbbmtstxt.Text = cpl.First().txt;
                                foreach (var item in cpl)
                                {
                                    lbbmtstxt.Text += "   " + item.code;
                                }
                            }
                        }
                }
                else
                    lbbmtstxt.Text="";
            }
            catch { }
        }

 
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT2 lParam);

 
        public const int PFM_LINESPACING = 256;
        public const int EM_SETPARAFORMAT = 1095;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PARAFORMAT2
        {
            public int cbSize;
            public uint dwMask;
            public Int16 wNumbering;
            public Int16 wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public Int16 wAlignment;
            public Int16 cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public Int16 sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public Int16 wShadingWeight;
            public Int16 wShadingStyle;
            public Int16 wNumberingStart;
            public Int16 wNumberingStyle;
            public Int16 wNumberingTab;
            public Int16 wBorderSpace;
            public Int16 wBorderWidth;
            public Int16 wBorders;
        }

        //height：要指定的行高像素
        private void SetLineSpace(Control ctl, int height)
        {
            //1像素=15缇。
            int dyLineSpacing = height * 15;
            //4:dylinespace成员以  缇。的形式指定从一行到下一行的间距。控件使用指定的精确间距，即使dylinespace指定的值小于单个间距。
            //3:dylinespace成员以  缇。的形式指定从一行到下一行的间隔。但是，如果dylinespace指定的值小于单间距，则控件将显示单间距文本。
            byte bLineSpacingRule = (byte)3;
            PARAFORMAT2 fmt = new PARAFORMAT2();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.bLineSpacingRule = bLineSpacingRule;
            fmt.dyLineSpacing = dyLineSpacing;
            fmt.dwMask = PFM_LINESPACING;
            try
            {
                SendMessage(new HandleRef(ctl, ctl.Handle), EM_SETPARAFORMAT, bLineSpacingRule, ref fmt);
            }
            catch
            { }
        }
        private void 删除空格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text.Replace(" ", "").Replace("　", "").Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            txt = richTextBox1.Text;
            alltxt = alltxt.Replace(" ", "").Replace("　", "").Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        }

        public string reText(string text)
        {
            Regex regex = new Regex("[a-zA-Z]");
            if (!regex.IsMatch(text))
            {
                string[] strArray = new string[] {
            "\"", "\"", "'", "'", ".", ",", ";", ":", "?", "!", "-", "~", "(", ")", "<", ">",
            @"\(", @"\)"
        };
                string[] strArray2 = new string[] {
            "“", "”", "‘", "’", "。", "，", "；", "：", "？", "！", "—", "～", "（", "）", "《", "》",
            "（", "）"
        };
                for (int i = 0; i < strArray.Length; i++)
                {
                    text = text.Replace(strArray[i], strArray2[i]);
                }
            }
            return text;
        }




        public string reToText(string text)
        {
            Regex regex = new Regex("[a-zA-Z]");
            if (!regex.IsMatch(text))
            {
                string[] strArray = new string[] {
            "\"", "\"", "'", "'", ".", ",", ";", ":", "?", "!", "-", "~", "(", ")", "<", ">",
            @"\(", @"\)"
        };
                string[] strArray2 = new string[] {
            "“", "”", "‘", "’", "。", "，", "；", "：", "？", "！", "—", "～", "（", "）", "《", "》",
            "（", "）"
        };
                for (int i = 0; i < strArray.Length; i++)
                {
                    text = text.Replace(strArray[i], strArray2[i]);
                }
            }
            return text;
        }



        private void 英文标点转中文标点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = reToText(richTextBox1.Text);
            txt = richTextBox1.Text;
            alltxt = reToText(alltxt);
        }

        private void 文件选文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnopenfile_Click(null, null);
            if (dictcode.Count > 0)
            {
                start_ai();
                render();
            }
        }

        private void 剪贴板载文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnbyjtn_Click(null, null);
            if (dictcode.Count > 0)
            {
                start_ai();
                render();
            }
        }

        #region  群相关
        public delegate bool CallBack(int hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hwnd, StringBuilder lpString, int cch);
        public static int GetWinC;
        public static string[,] GetWin;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int EnumWindows(CallBack x, int y);



        public static bool Report(int hwnd, int lParam)
        {
            Regex regex = new Regex(@"QQ20\d{2}");
            string[] source = new string[] { "TXMenuWindow", "FaceSelector", "TXFloatingWnd", "腾讯", "消息盒子", "来自", "分类推荐", "更换房间头像", "网络设置", "消息管理器", "QQ数据线", "骏伯网络科技" };
            StringBuilder lpString = new StringBuilder(0x200);
            GetWindowText(hwnd, lpString, lpString.Capacity);
            string input = lpString.ToString();
            if ((!regex.IsMatch(input) && !source.Contains<string>(input)) && !string.IsNullOrEmpty(input))
            {
                StringBuilder lpClassName = new StringBuilder(0x200);
                GetClassName(hwnd, lpClassName, 0x100);
                if (lpClassName.ToString() == "TXGuiFoundation")
                {
                    GetWin[GetWinC, 0] = input;
                    GetWin[GetWinC, 1] = hwnd.ToString();
                    GetWinC++;
                }
            }
            return true;
        }


        private void refreshQun2(object sender, EventArgs e)
        {
            this.refreshQun();
            this.TSMI2.ShowDropDown();
        }
        private void ChangeQun(object sender, EventArgs e)
        {
            this.toolStripTextBox2.Text = sender.ToString();
            toolStripButton6_Click(null, null);
        }



        private void refreshQun()
        {
            this.TSMI2.DropDownItems.Clear();
            this.TSMI2.DropDownItems.Add("刷新", null, new EventHandler(this.refreshQun2));
            try
            {
                GetWinC = 0;
                GetWin = new string[10, 2];
                CallBack x = new CallBack(Report);
                EnumWindows(x, 0);
                this.TSMI2.DropDownItems.Add("-");
                if (GetWinC > 0)
                {
                    ToolStripMenuItem[] toolStripItems = new ToolStripMenuItem[GetWinC];
                    for (int i = 0; i < GetWinC; i++)
                    {
                        toolStripItems[i] = new ToolStripMenuItem(GetWin[i, 0], null, new EventHandler(this.ChangeQun), Keys.Control | ((Keys)(0x31 + i)));
                    }
                    this.TSMI2.DropDownItems.AddRange(toolStripItems);
                }
                else
                {
                    this.TSMI2.DropDownItems.Add("未找到群");
                }
            }
            catch
            {
            }
        }



        #endregion

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshQun();
        }

        #region 词提相关

        private static Dictionary<string, dictpos> dictcp = new Dictionary<string, dictpos>();
        private static Dictionary<string, string> dictcode = new Dictionary<string, string>();
        private static Dictionary<string, bxinfo> dictbx = new Dictionary<string, bxinfo>();
        private void createindex()
        {
            if (dictcp.Count > 0) return;

    //        if (File.Exists(System.IO.Path.Combine(
    //Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
    //, "dictcp.bin")) && File.Exists(System.IO.Path.Combine(
    //Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
    //, "dictcode.bin")))
    //        {
    //            var inputStream = File.Open(System.IO.Path.Combine(
    //Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
    //, "dictcp.bin"), FileMode.Open, FileAccess.Read);
    //            using (var reader = new BinaryReader(inputStream))
    //            {

    //                dictcp = JsonConvert.DeserializeObject<Dictionary<string, dictpos>>(reader.ReadString());
    //            }

    //            inputStream = File.Open(System.IO.Path.Combine(
    //Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
    //, "dictcode.bin"), FileMode.Open, FileAccess.Read);
    //            using (var reader = new BinaryReader(inputStream))
    //            {

    //                dictcode = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadString());
    //            }
    //            return;
    //        }
            
            List<string> newlist = new List<string>();
            foreach (var item in WinInput.Input.MasterDit)
            {
                if (string.IsNullOrEmpty(item)) continue;
                if (!dictcode.ContainsKey(item.Split(' ')[0]))
                {
                    dictcode.Add(item.Split(' ')[0], item.Replace(item.Split(' ')[0] + " ", ""));
                }
                else
                    dictcode[item.Split(' ')[0]] += " " + item.Replace(item.Split(' ')[0] + " ", "");
                if (item.Split(' ').Length>1 && item.Split(' ')[0].Length > 1 && item.Split(' ')[1].Length ==1 
                    && !dictbx.ContainsKey(item.Split(' ')[0].Substring(0, 2)))
                {
                    bxinfo bxi = new bxinfo();
                    bxi.txt1 = item.Split(' ')[1];
                    bxi.txt2 = "";
                    bxi.txt3 = "";
                    dictbx.Add(item.Split(' ')[0].Substring(0, 2), bxi);
                }
                int pos = 1;

                string txts = "";
                for (int i = 1; i < item.Split(' ').Length; i++)
                {
                    string txt = item.Split(' ')[i];
                    if (txt.Length > 1)
                    {
                        txts += " " + txt;
                        continue;
                    }
                   
                    if (item.Split(' ')[0].Length > 1 && dictbx.ContainsKey(item.Split(' ')[0].Substring(0, 2)))
                    {
                        var bxobj = dictbx[(item.Split(' ')[0].Substring(0, 2))];
                        if (bxobj.txt2 == "" && bxobj.txt1 != txt)
                            bxobj.txt2 = txt;
                        else if (bxobj.txt3 == "" && bxobj.txt1 != txt)
                            bxobj.txt3 = txt;
                    }
                    if (!dictcp.ContainsKey(txt))
                    {
                        dictpos dp = new dictpos();

                        dp.dicts = new List<dictmodel>();
                        dictmodel dm = new dictmodel();
                        dm.code = item.Split(' ')[0];
                        dm.pos = pos;
                        dm.txt = txt;
                        if (dm.code.Length > 3)
                            pos++;
                        dp.dicts.Add(dm);
                        dictcp.Add(dm.txt, dp);
                    }
                    else
                    {
                        var dp = dictcp[txt];
                        dictmodel dm = new dictmodel();
                        dm.code = item.Split(' ')[0];
                        dm.pos = pos;
                        dm.txt = txt;
                        if (dm.code.Length > 3)
                            pos++;
                        dp.dicts.Add(dm);
                    }
                }
                if (txts.Length > 0)
                {
                    newlist.Add(item.Split(' ')[0] + txts);
                }


            }

            for (int j = 0; j < newlist.Count; j++)
            {
                string item = newlist[j];
                if (string.IsNullOrEmpty(item)) continue;
                for (int i = 1; i < item.Split(' ').Length; i++)
                {
                    if (dictcp.ContainsKey(item.Split(' ')[i].Substring(0, 1)))
                    {
                        string txt = item.Split(' ')[i];
                        dictmodel dm = new dictmodel();
                        dm.code = item.Split(' ')[0];
                        dm.txt = txt;
                        dictcp[item.Split(' ')[i].Substring(0, 1)].dicts.Add(dm);
                    }
                }
            }
 
            //var outputStream = File.Create(System.IO.Path.Combine(
            //    Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
            //    , "dictcp.bin"));
            //using (var writer = new BinaryWriter(outputStream))
            //{
            //    writer.Write(JsonConvert.SerializeObject(dictcp));
            //}
            //outputStream = File.Create(System.IO.Path.Combine(
            //    Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
            //    , "dictcode.bin"));
            //using (var writer = new BinaryWriter(outputStream))
            //{
            //    writer.Write(JsonConvert.SerializeObject(dictcode));
            //}
        }
        private txttipinfo ai_txt = new txttipinfo();
        private void start_ai()
        {
            ai_txt = new txttipinfo();
            ai_txt.txt = richTextBox1.Text;
            if (ai_txt.txt.Length == 0) return;
            if (dictcode.Count == 0) return;
            if (clmode.SelectedIndex == 0) return;
            ai_txt.words = new List<txtword>();
            for (int i = 0; i < ai_txt.txt.Length; i++)
            {
                string w = ai_txt.txt.Substring(i, 1);
                if (w == " ")
                {
                    txtword word = new txtword();
                    word.word = w;
                    dictmodel dm = new dictmodel();
                    dm.code = w;
                    word.codelist = new List<dictmodel>();
                    word.codelist.Add(dm);
                    ai_txt.words.Add(word);
                }
                else if (dictcp.ContainsKey(w))
                {
                    var dicts = dictcp[w];

                    var wordlist = dicts.dicts.FindAll(f => f.txt.Length > 1).OrderByDescending(o => o.txt.Length);

                    bool cz = false;
                    foreach (var item in wordlist)
                    {
                        string tptxt = "";

                        if (i + (item.txt.Length) < ai_txt.txt.Length)
                        {
                            tptxt = ai_txt.txt.Substring(i, item.txt.Length);

                        }
                        if (tptxt == item.txt)
                        {
                            txtword word = new txtword();
                            word.word = tptxt;
                            var wwlist = dicts.dicts.FindAll(f => f.txt == tptxt && f.code.Length < 5);
                            foreach (var ww in wwlist)
                            {
                                if (ww.code.Length > 4) continue;
                                ww.pos = 0;
                                if (dictcode.ContainsKey(ww.code))
                                {
                                    string codestr = dictcode[ww.code];
                                    for (int j = 0; j < codestr.Split(' ').Length; j++)
                                    {
                                        if (codestr.Split(' ')[j] == tptxt)
                                        {
                                            ww.pos = j + 1;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    ww.code += "???";
                                }
                            }
                            if (usebx)
                            {

                                if (wwlist.Where(wl => wl.pos < 4 && wl.txt.Length <= czmax).Count() > 0)
                                {
                                    word.codelist.AddRange(wwlist);
                                    ai_txt.words.Add(word);
                                    cz = true;
                                    i += tptxt.Length - 1;
                                    break;
                                }
                            }
                            else
                            {
                                if (wwlist.Where(wl => wl.pos < 2 && wl.txt.Length <= czmax).Count() > 0)
                                {
                                    word.codelist.AddRange(wwlist);
                                    ai_txt.words.Add(word);
                                    cz = true;
                                    i += tptxt.Length - 1;
                                    break;
                                }
                            }

                        }
                    }

                    if (!cz)
                    {
                        txtword word = new txtword();
                        word.word = w;
                        var wwlist = dicts.dicts.FindAll(f => f.txt == w);

                        foreach (var ww in wwlist)
                        {
                            ww.pos = 0;
                            if (dictcode.ContainsKey(ww.code))
                            {
                                string codestr = dictcode[ww.code];
                                for (int j = 0; j < codestr.Split(' ').Length; j++)
                                {
                                    if (codestr.Split(' ')[j] == w)
                                    {
                                        ww.pos = j + 1;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                ww.code += "???";
                            }
                        }
                        word.codelist.AddRange(wwlist);
                        ai_txt.words.Add(word);
                    }
                }
                else
                {
                    if ("　，。“”！（）()~\x00b7#￥%&*_[]{}‘’/\\<>,.《》？：；、—…1234567890１２３４５６７８９０abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ".Contains(w))
                    {
                        txtword word = new txtword();
                        word.word = w;
                        dictmodel dm = new dictmodel();
                        dm.code = w;
                        word.codelist = new List<dictmodel>();
                        word.codelist.Add(dm);
                        ai_txt.words.Add(word);
                    }
                    else
                    {
                        txtword word = new txtword();
                        word.word = "?";
                        dictmodel dm = new dictmodel();
                        dm.code = "????";
                        word.codelist = new List<dictmodel>();
                        word.codelist.Add(dm);
                        ai_txt.words.Add(word);
                    }
                }
            }
           
        }

        private void render()
        {
            ai_txt.minbjnum = 0;
            ai_txt.xcnum = 0;
            ai_txt.mincjnum = 0;
            ai_txt.mincjnum = 0;
            ai_txt.cznum = 0;
            this.richTextBox1.Controls.Clear();
            if (ai_txt.txt.Length == 0) return;
            if (clmode.SelectedIndex == 0) return;
            if (dictcode.Count == 0) return;
            SizeF _charSize = new SizeF();
            using (Graphics graphics = this.richTextBox1.CreateGraphics())
            {
                _charSize = graphics.MeasureString("测", this.richTextBox1.Font);
            }

            richTextBox1.ForeColor= System.Drawing.Color.Black;
            int start = 0;
            Color precolor= System.Drawing.Color.Black;
            int left = 0;
            short dzbx = 1;
            foreach (var word in ai_txt.words)
            {
                word.codelist = word.codelist.OrderBy(c => c.code.Length).ToList();
                if (word.word.Length > 1)
                {
                    ai_txt.cznum += word.word.Length;
                    richTextBox1.Select(start, word.word.Length);
                    Color curcolor = System.Drawing.Color.Gray;
                   
                    if (word.codelist.First().code.Length < 3)
                    {
                        //一击词
                        ai_txt.minbjnum += 1;
                        ai_txt.mincjnum += word.codelist.First().code.Length == 1 ? 2 : 3;
                        curcolor = System.Drawing.Color.Red;
                        if (precolor != curcolor)
                            richTextBox1.SelectionColor = curcolor;
                        else
                            richTextBox1.SelectionColor = System.Drawing.Color.DarkRed;
                    }
                    else if (word.codelist.First().code.Length < 4)
                    {
                        //2击字词
                        ai_txt.minbjnum += 2;
                        ai_txt.mincjnum += 4;
                        curcolor = System.Drawing.Color.Orange;
                        if (precolor != curcolor)
                            richTextBox1.SelectionColor = curcolor;
                        else
                            richTextBox1.SelectionColor = System.Drawing.Color.DarkOrange;
                    }
                    else
                    {
                        //并击选词
                        if (word.codelist.First().pos == 2)
                        {
                            dzbx = 2;
                            richTextBox1.SelectionColor = System.Drawing.Color.Brown;
                        }
                        else if (word.codelist.First().pos == 3)
                        {
                            dzbx = 3;
                            richTextBox1.SelectionColor = System.Drawing.Color.RosyBrown;
                        }
                        else
                        {
                            if (precolor != curcolor)
                                richTextBox1.SelectionColor = curcolor;
                            else
                                richTextBox1.SelectionColor = System.Drawing.Color.LightGray;
                        }
                        int add = 2;
                        bool xc = false;
                        bool cjxc = false;
                        foreach (var item in word.codelist)
                        {
                            if (item.pos > 1) cjxc = true;
                            if (item.code.Length == 4)
                            {
                                add = 2;
                                if (item.pos > 3)
                                    xc = true;

                            }
                            else
                            {
                                add = 3;
                                if (item.pos > 3)
                                    xc = true;
                            }
                            break;
                        }
                        if (xc) ai_txt.xcnum++;
                        if (cjxc) ai_txt.cjxcnum++;
                        ai_txt.minbjnum += add + (xc ? 1 : 0);
                        ai_txt.mincjnum += 4 + (cjxc ? 1 : 0);
                    }
                    precolor = richTextBox1.SelectionColor;
                }
                else
                {
                    if (word.codelist.First().code.Substring(0, 1) == "i")
                    {
                        if (word.codelist.Where(o => o.code.Substring(0, 1) != "i").Count() > 0)
                        {
                            word.codelist = word.codelist.Where(o => o.code.Substring(0, 1) != "i").OrderBy(o => o.code.Length).ToList();
                        }
                    }
                    var codes = word.codelist;
                    richTextBox1.Select(start, word.word.Length);
                    if (usebx)
                    {
                        if (",.，。、/　“”！!（）()~\x00b7#￥%&*_[]{}‘’?？:：;；\\<>《》：—…1234567890１２３４５６７８９０abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ".Contains(word.word)
|| word.word == " ")
                        {
                            ai_txt.minbjnum++;
                            ai_txt.mincjnum++;
                            Color curcolor = System.Drawing.Color.Black;
                            if (precolor != curcolor)
                                richTextBox1.SelectionColor = curcolor;
                            else
                                richTextBox1.SelectionColor = Color.DimGray;
                        }
                        else
                        {

                            if (codes.First().code.Length < 3)
                            {
                                ai_txt.minbjnum += 1;
                                ai_txt.mincjnum += codes.First().code.Length + 1;
                                if (codes.Count() == 1 || word.codelist.OrderByDescending(c => c.code.Length).First().code.Length < 3)
                                {
                                    //一击字
                                    Color curcolor = System.Drawing.Color.Green;
                                    if (precolor != curcolor)
                                        richTextBox1.SelectionColor = curcolor;
                                    else
                                        richTextBox1.SelectionColor = System.Drawing.Color.DarkGreen;
                                }
                                else
                                {
                                    Color curcolor = System.Drawing.Color.Black;
                                    if (precolor != curcolor)
                                        richTextBox1.SelectionColor = curcolor;
                                    else
                                        richTextBox1.SelectionColor = System.Drawing.Color.DimGray;
                                }
                            }
                            else
                            {
                                string code = codes.First().code;
                                if (dictbx.ContainsKey(code.Substring(0, 2))
                                    && (dictbx[code.Substring(0, 2)].txt2 == word.word || dictbx[code.Substring(0, 2)].txt3 == word.word))
                                {
                                    //并击选字
                                    if (dictbx[code.Substring(0, 2)].txt2 == word.word)
                                    {
                                        dzbx = 2;
                                        richTextBox1.SelectionColor = System.Drawing.Color.Brown;
                                    }
                                    else
                                    {
                                        dzbx = 3;
                                        richTextBox1.SelectionColor = System.Drawing.Color.RosyBrown;
                                    }

                                    bool cjxc = false;
                                    foreach (var item in codes)
                                    {
                                        if (item.pos > 1)
                                            cjxc = true;
                                        break;
                                    }

                                    if (cjxc) ai_txt.cjxcnum++;
                                    ai_txt.minbjnum++;
                                    ai_txt.mincjnum += 4 + (cjxc ? 1 : 0);
                                }
                                else
                                {
                                    //两击字
                                    Color curcolor = System.Drawing.Color.Black;
                                    if (precolor != curcolor)
                                        richTextBox1.SelectionColor = curcolor;
                                    else
                                        richTextBox1.SelectionColor = System.Drawing.Color.LightGray;

                                    int add = 2;
                                    bool xc = false;
                                    bool cjxc = false;
                                    foreach (var item in codes)
                                    {
                                        if (item.pos > 1)
                                            cjxc = true;

                                        if (item.pos > 3)
                                        {
                                            add = 3;
                                            xc = true;
                                        }
                                        break;
                                    }

                                    if (xc) ai_txt.xcnum++;
                                    if (cjxc) ai_txt.cjxcnum++;
                                    ai_txt.minbjnum += add + (xc ? 1 : 0);
                                    ai_txt.mincjnum += 4 + (cjxc ? 1 : 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ("　“”！!（）()~\x00b7#￥%&*_[]{}‘’?？:：;；\\<>《》：—…1234567890１２３４５６７８９０abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ".Contains(word.word)
|| word.word == " ")
                        {
                            ai_txt.minbjnum++;
                            ai_txt.mincjnum++;
                            Color curcolor = System.Drawing.Color.Black;
                            if (precolor != curcolor)
                                richTextBox1.SelectionColor = curcolor;
                            else
                                richTextBox1.SelectionColor = Color.DimGray;
                        }
                        else if (",.，。、/".Contains(word.word))
                        {
                            //ai_txt.minbjnum ++;
                            ai_txt.mincjnum++;
                            Color curcolor = System.Drawing.Color.Black;
                            if (precolor != curcolor)
                                richTextBox1.SelectionColor = curcolor;
                            else
                                richTextBox1.SelectionColor = Color.DimGray;
                        }
                        else
                        {
                            if (codes.First().code.Length < 3)
                            {
                                ai_txt.minbjnum += 1;
                                ai_txt.mincjnum += codes.First().code.Length + 1;
                                if (codes.Count() == 1 || word.codelist.OrderByDescending(c => c.code.Length).First().code.Length < 3)
                                {
                                    //一击字
                                    Color curcolor = System.Drawing.Color.Green;
                                    if (precolor != curcolor)
                                        richTextBox1.SelectionColor = curcolor;
                                    else
                                        richTextBox1.SelectionColor = System.Drawing.Color.DarkGreen;
                                }
                                else
                                {
                                    Color curcolor = System.Drawing.Color.Black;
                                    if (precolor != curcolor)
                                        richTextBox1.SelectionColor = curcolor;
                                    else
                                        richTextBox1.SelectionColor = System.Drawing.Color.DimGray;
                                }
                            }
                            else
                            {
                                string code = codes.First().code;
                                if (dictbx.ContainsKey(code.Substring(0, 2))
                                    && (dictbx[code.Substring(0, 2)].txt2 == word.word || dictbx[code.Substring(0, 2)].txt3 == word.word))
                                {
                                    //并击选字
                                    if (dictbx[code.Substring(0, 2)].txt2 == word.word)
                                    {
                                        dzbx = 2;
                                        richTextBox1.SelectionColor = System.Drawing.Color.Brown;
                                    }
                                    else
                                    {
                                        dzbx = 3;
                                        richTextBox1.SelectionColor = System.Drawing.Color.RosyBrown;
                                    }

                                    bool cjxc = false;
                                    foreach (var item in codes)
                                    {
                                        if (item.pos > 1)
                                            cjxc = true;
                                        break;
                                    }

                                    if (cjxc) ai_txt.cjxcnum++;
                                    ai_txt.minbjnum++;
                                    ai_txt.mincjnum += 4 + (cjxc ? 1 : 0);
                                }
                                else
                                {
                                    //两击字
                                    Color curcolor = System.Drawing.Color.Black;
                                    if (precolor != curcolor)
                                        richTextBox1.SelectionColor = curcolor;
                                    else
                                        richTextBox1.SelectionColor = System.Drawing.Color.SlateGray;

                                    int add = 2;
                                    bool xc = false;
                                    bool cjxc = false;
                                    foreach (var item in codes)
                                    {
                                        if (item.pos > 1)
                                            cjxc = true;

                                        if (item.pos > 3)
                                        {
                                            add = 3;
                                            xc = true;
                                        }
                                        break;
                                    }

                                    if (xc) ai_txt.xcnum++;
                                    if (cjxc) ai_txt.cjxcnum++;
                                    ai_txt.minbjnum += add + (xc ? 1 : 0);
                                    ai_txt.mincjnum += 4 + (cjxc ? 1 : 0);
                                }
                                
                            }
                        }
                    }

                    precolor = richTextBox1.SelectionColor;
                }

                #region 词提编码
                if (clmode.SelectedIndex == 2)
                {
                    try
                    {
                        Point positionFromCharIndex = this.richTextBox1.GetPositionFromCharIndex(start);
                        if (((positionFromCharIndex.X >= 0) && (positionFromCharIndex.Y >= 0))
                            && ((positionFromCharIndex.Y + _charSize.Height) <= this.richTextBox1.Height))
                        {
                            int x = positionFromCharIndex.X;
                            int num2 = (positionFromCharIndex.Y + ((int)_charSize.Height)) + 1;
                            int num3 = Math.Min((int)_charSize.Width, this.richTextBox1.ClientRectangle.Width - x);

                            if (start == 0)
                            {
                                x += 5;
                            }
                            else if ((start + word.word.Length) == this.richTextBox1.Text.Length)
                            {
                                x -= 5;
                            }
                            else
                            {
                                x -= 3;
                            }

                            Label freeFloatControl = new Label();
                            this.richTextBox1.Controls.Add(freeFloatControl);

                            freeFloatControl.Top = num2 - 9;
                            freeFloatControl.Left = x;
                            freeFloatControl.Height = 20;
                            freeFloatControl.Width = word.word.Length == 2 ? num3 + 3 : (word.word.Length > 2 ? num3 + 8 : num3);
                            freeFloatControl.BackColor = this.richTextBox1.BackColor;
                            freeFloatControl.ForeColor = precolor;
                            string codestr = codestr = word.codelist.First().code + (word.codelist.First().pos > 1 ? "" + word.codelist.First().pos : "");
                            freeFloatControl.Text = codestr;
                            freeFloatControl.TextAlign = ContentAlignment.TopCenter;
                            freeFloatControl.Font = new Font("宋体", 12);
                            left += word.left;
                        }
                    }
                    catch { }
                }
                #endregion
                start += word.word.Length;
            }

            if (ai_txt.minbjnum > 0)
            {
                lbllmc.Text = "理论码长:" + Math.Round((ai_txt.minbjnum * 1.0 / ai_txt.txt.Length), 2) + " 选重:" + ai_txt.xcnum + " 打词:" + Math.Round((ai_txt.cznum * 1.0 / ai_txt.txt.Length) * 100, 2) + "%";
                lbllmc.ToolTipText = "并击数:" + ai_txt.minbjnum + " 约等效串击码长:" + Math.Round((ai_txt.mincjnum * 1.0 / ai_txt.txt.Length), 2) + " 选重:" + ai_txt.cjxcnum;
            }
        }





        #endregion

        private int czmax = 10;
        private bool usebx = false;
        private void 智能词提分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
    
        }

        private void 清除缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(System.IO.Path.Combine(
Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
, "dictcp.bin")))
            {
                File.Delete(System.IO.Path.Combine(
Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
, "dictcp.bin"));
            }

            if (File.Exists(System.IO.Path.Combine(
Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
, "dictcode.bin")))
            {
                File.Delete(System.IO.Path.Combine(
Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"))
, "dictcode.bin"));
            }

            dictcode.Clear();
            dictcp.Clear();
         
            GC.Collect();
            MessageBox.Show("清除完毕");
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            if (dictcode.Count > 0)
            {
                start_ai();
                render();
            }
        }

        private void clmode_TextChanged(object sender, EventArgs e)
        {

            start_ai();
            render();

        }

        private void clmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbbmtstxt.Visible = clmode.SelectedIndex == 2;
            start_ai();
            render();
        }

        private void 中文数字全角转半角ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //１２３４５６７８９０
            
            richTextBox1.Text = richTextBox1.Text.Replace("１", "1")
                .Replace("２", "2")
                .Replace("３", "3")
                .Replace("４", "4")
                .Replace("５", "5")
                .Replace("６", "6")
                .Replace("７", "7")
                .Replace("８", "8")
                .Replace("９", "9")
                .Replace("０", "0");
            txt = richTextBox1.Text;
            alltxt= alltxt.Replace("１", "1")
                .Replace("２", "2")
                .Replace("３", "3")
                .Replace("４", "4")
                .Replace("５", "5")
                .Replace("６", "6")
                .Replace("７", "7")
                .Replace("８", "8")
                .Replace("９", "9")
                .Replace("０", "0");
        }

        private void 不选重词组最优ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            czmax = 100; usebx = false;
            clmode.SelectedIndex = 1;
            createindex();

            start_ai();

            render();
        }

        private void 不选重只用两字词ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            czmax = 2; usebx = false;
            clmode.SelectedIndex = 1;
            createindex();

            start_ai();

            render();
        }

        private void 使用并选词组最优ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            czmax = 100; usebx = true;
            clmode.SelectedIndex = 1;
            createindex();

            start_ai();

            render();
        }

        private void 单字并击模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            czmax = 1; 
            clmode.SelectedIndex = 1;
            createindex();

            start_ai();

            render();
        }
    }

    partial class dictmodel
    {
        public string txt { get; set; }
        public int pos = 0;

        public string code { get; set; }
    }

    partial class bxinfo
    {
        public string txt1 { get; set; }
        public string txt2 { get; set; }
        public string txt3 { get; set; }
    }
    partial class dictpos
    {
        public List<dictmodel> dicts { get; set; }
 
    }

    partial class txttipinfo
    {
        public string txt = "";
        public int minbjnum = 0;
        public int mincjnum = 0;
        public int cjxcnum = 0;
        public int xcnum = 0;
        public int cznum = 0;
        public List<txtword> words = new List<txtword>();
    }

    partial class txtword
    {
        public string word = "";
        public List<dictmodel> codelist = new List<dictmodel>();


        public Color color { get; set; }
        public int width { get; set; }
        public int top { get; set; }
        public int left { get; set; }
        public int len { get; set; }
    }
}
