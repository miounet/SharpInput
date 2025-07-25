using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Base;
using Core.Config;

namespace Core.Win
{
    public partial class DictMrg : Form
    {
        public  static bool orderby = true;
        public static bool updatadict = false;
        public DictMrg()
        {

            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void bntfind_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim().Length == 0) return;
            finddict();
            if (this.listBox1.Items.Count == 0)
            {
                MessageBox.Show("未找到:" + this.textBox1.Text + " " + this.textBox2.Text);
            }
        }

        private void finddict()
        {
            int first = 0, last = Core.Win.WinInput.Input.MasterDit.Length - 1;

            string[] mdict = Core.Win.WinInput.Input.MasterDit;
            string inputstr = this.textBox1.Text.Trim();
            string inputvalue = this.textBox2.Text.Trim();
            int count = 0;
            this.listBox1.Items.Clear();
            for (int i = first; i <= last; i++)
            {

                if (mdict[i].Split(' ')[0].StartsWith(inputstr))
                {
                    if (inputvalue.Length > 0)
                    {
                        for (int j = 1; j < mdict[i].Split(' ').Length; j++)
                        {
                            if (inputvalue == mdict[i].Split(' ')[j].Trim())
                            {
                                string strarr = mdict[i];
                                string fcode = strarr.Split(' ')[0];
                                string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                                this.listBox1.Items.Add(i + "：" + fcode + " " + fvalue.Trim());
                                count = (int)numericUpDown1.Maximum;
                            }
                        }
                    }
                    else
                    {
                        string strarr = mdict[i];
                        string fcode = strarr.Split(' ')[0];
                        string fvalue = strarr.Substring(strarr.Split(' ')[0].Length).Trim();//获取汉字
                        this.listBox1.Items.Add(i + "：" + fcode + " " + fvalue.Trim());
                        count++;
                    }
                }
                if (count >= (int)numericUpDown1.Value) break;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 0) return;
            if (this.listBox1.SelectedIndex == 0) return;

            showhidebutton(false);
            try
            {
                int sint = this.listBox1.SelectedIndex;
                string txtf = this.listBox1.Items[sint - 1].ToString();
                string txtf2 = this.listBox1.Items[sint].ToString();
                if (replacepos(int.Parse(txtf.Split('：')[0]), int.Parse(txtf2.Split('：')[0])))
                {
                    //初始化索引
                    Core.Win.WinInput.Input.CreateIndex(Core.Win.WinInput.Input.MasterDit, ref Core.Win.WinInput.Input.DictIndex.IndexList, 1, 0, Core.Win.WinInput.Input.MasterDit.Length);
                    updatadict = true;

                    this.listBox1.Items[sint] = txtf2.Split('：')[0] + "：" + txtf.Split('：')[1];
                    this.listBox1.Items[sint - 1] = txtf.Split('：')[0] + "：" + txtf2.Split('：')[1];

                    this.listBox1.SelectedIndex = sint - 1;
                }
            }
            catch
            {
                MessageBox.Show("操作失败");
            }
            showhidebutton(true); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 0) return;
            if (this.listBox1.SelectedIndex == this.listBox1.Items.Count - 1) return;
            showhidebutton(false);
            try
            {
                int sint = this.listBox1.SelectedIndex;
                string txtf = this.listBox1.Items[sint + 1].ToString();
                string txtf2 = this.listBox1.Items[sint].ToString();
                if (replacepos(int.Parse(txtf.Split('：')[0]), int.Parse(txtf2.Split('：')[0])))
                {

                    //初始化索引
                    Core.Win.WinInput.Input.CreateIndex(Core.Win.WinInput.Input.MasterDit, ref Core.Win.WinInput.Input.DictIndex.IndexList, 1, 0, Core.Win.WinInput.Input.MasterDit.Length);
                    updatadict = true;
                    this.listBox1.Items[sint] = txtf2.Split('：')[0] + "：" + txtf.Split('：')[1];
                    this.listBox1.Items[sint + 1] = txtf.Split('：')[0] + "：" + txtf2.Split('：')[1];
                    this.listBox1.SelectedIndex = sint + 1;
                }
            }
            catch
            {
                MessageBox.Show("操作失败");
            }
            showhidebutton(true);

        }
        private bool replacepos(int pos1,int pos2)
        {
            try
            {
                string txt = Core.Win.WinInput.Input.MasterDit[pos1];
                Core.Win.WinInput.Input.MasterDit[pos1] = Core.Win.WinInput.Input.MasterDit[pos2];
                Core.Win.WinInput.Input.MasterDit[pos2] = txt;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 0) return;
            if (this.listBox1.SelectedIndex == 0) return;
            showhidebutton(false);
            try
            {
                int sint = this.listBox1.SelectedIndex;
                while (sint - 1 >= 0)
                {
                    string txtf = this.listBox1.Items[sint - 1].ToString();
                    string txtf2 = this.listBox1.Items[sint].ToString();
                    if (replacepos(int.Parse(txtf.Split('：')[0]), int.Parse(txtf2.Split('：')[0])))
                    {
                        this.listBox1.Items[sint] = txtf2.Split('：')[0] + "：" + txtf.Split('：')[1];
                        this.listBox1.Items[sint - 1] = txtf.Split('：')[0] + "：" + txtf2.Split('：')[1];

                        sint--;
                    }
                }
                //初始化索引
                Core.Win.WinInput.Input.CreateIndex(Core.Win.WinInput.Input.MasterDit, ref Core.Win.WinInput.Input.DictIndex.IndexList, 1, 0, Core.Win.WinInput.Input.MasterDit.Length);
                updatadict = true;
                this.listBox1.SelectedIndex = 0;
            }
            catch { MessageBox.Show("操作失败"); }

            showhidebutton(true);

        }

        private void showhidebutton(bool enable)
        {
            this.bntfind.Enabled = enable;
            this.button1.Enabled = enable;
            this.button2.Enabled = enable;
            this.button3.Enabled = enable;
            this.button4.Enabled = enable;
            ;this.button5.Enabled = enable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Items.Count == 0 || this.listBox1.SelectedItems.Count == 0) return;

            DialogResult result = MessageBox.Show("确认删除吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                showhidebutton(false);
                try
                {
                    string txtf = this.listBox1.Items[this.listBox1.SelectedIndex].ToString();
                    int delpos = int.Parse(txtf.Split('：')[0]);
                    var list = Core.Win.WinInput.Input.MasterDit.ToList();
                    list.RemoveAt(delpos);
                    Core.Win.WinInput.Input.MasterDit = list.ToArray();
                    //初始化索引
                    Core.Win.WinInput.Input.CreateIndex(Core.Win.WinInput.Input.MasterDit, ref Core.Win.WinInput.Input.DictIndex.IndexList, 1, 0, Core.Win.WinInput.Input.MasterDit.Length);
                    updatadict = true;
                    this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
                }
                catch { MessageBox.Show("操作失败"); }
                showhidebutton(true);
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim().Length == 0 || this.textBox2.Text.Trim().Length == 0) return;

            if (this.textBox1.Text.Trim().Length < 2)
            {
                MessageBox.Show("编码长度需大于1");
                return;
            }
            showhidebutton(false);
            try
            {
                int first = 0, last = Core.Win.WinInput.Input.MasterDit.Length - 1;

    
                string inputstr = this.textBox1.Text.Trim();
                string inputvalue = this.textBox2.Text.Trim();
                int pos = 0;
                bool newadd = false;
                string[] mdict = null;

                PosIndex poi = Core.Win.WinInput.Input.DictIndex.GetPos(inputstr, ref mdict, false);
                if (poi == null)
                {
                    showhidebutton(true); return;
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
                                MessageBox.Show("词条已存在");
                                showhidebutton(true);
                                return;
                            }
                        }
                        newadd = false;
                        pos = i;
                    }
                }
                if (pos == 0)
                {
                    newadd = true;
                    pos = last;
                }
                if (pos > 0)
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
                                pos = i;
                                if (posint == 0)
                                    posint++;
                            }
                            if (posint > 0)
                                posint++;

                            if (posint > 100) break;
                        }
                        if (inputvalue.Length == 1 && inputstr.Length > 1 && pos > poi.Start)
                            pos--;

                        var list = Core.Win.WinInput.Input.MasterDit.ToList();
                        list.Insert(pos + 1, inputstr + " " + inputvalue);
                        Core.Win.WinInput.Input.MasterDit = list.ToArray();
                    }
                    else
                    {

                        string txt = Core.Win.WinInput.Input.MasterDit[pos];
                        Core.Win.WinInput.Input.MasterDit[pos] = txt + " " + inputvalue;
                    }
                    //初始化索引
                    Core.Win.WinInput.Input.CreateIndex(Core.Win.WinInput.Input.MasterDit, ref Core.Win.WinInput.Input.DictIndex.IndexList, 1, 0, Core.Win.WinInput.Input.MasterDit.Length);
                    updatadict = true; 
                    finddict();
                }
            }
            catch
            {
                MessageBox.Show("操作失败");
            }
            showhidebutton(true);
        }

        public static void savedict()
        {
            if (updatadict == false) return;
            updatadict = false;
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

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listBox1.Items.Count == 0 || this.listBox1.SelectedItems.Count == 0) return;
            string txtf = this.listBox1.Items[this.listBox1.SelectedIndex].ToString();
            DictEdit frm = new DictEdit(txtf.Split('：')[1].Split(' ')[0]);
            frm.ShowDialog();
        }
    }
}
