﻿using Core.Base;
using Core.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Win
{
    public partial class ConfigFrm : Form
    {
        Win.WinInput winput = null;
        public ConfigFrm(Win.WinInput input)
        {
            this.winput=input;
            InitializeComponent();
            //this.Icon = new Icon(System.IO.Path.Combine(Application.StartupPath, "log32.ico"));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            winput.LoadSettting();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            InputMode.OpenCould = this.ckOpenCould.Checked;
            InputMode.AutoRun = this.chkAutoRun.Checked;
            InputMode.AutoUpdate = this.ckAutoUpdate.Checked;
            InputMode.OpenLink = this.ckLink.Checked;
            InputMode.OpenAltSelect = this.ckalt.Checked;
            InputMode.SingleInput = this.SingleInput.Checked;
            InputMode.right3_out = this.ckright3out.Checked;
            winput.curTrac = this.tracchk.Checked;
            winput.curMouseTrac = this.mousetracchk.Checked;
            InputMode.PageSize = (int)this.selectnum.Value;
            InputMode.txtla = this.txtla.Text.Trim();
            InputMode.txtra = this.txtra.Text.Trim();
            InputMode.txtlas = this.txtlas.Text.Trim();
            InputMode.txtras = this.txtras.Text.Trim();
            InputMode.txtlra = this.txtlra.Text.Trim();
            InputMode.txtlras = this.txtlras.Text.Trim();
            InputMode.pinyin = this.ckpinyin.Checked;
            InputMode.closebj = this.chclosebj.Checked;
            InputMode.autopos = this.ckautopos.Checked;
            InputMode.tautopos = this.cktautopos.Checked;
            InputMode.bjzckgsp = this.chkbjzckgsp.Checked;
            InputMode.omeno = this.chkomeno.Checked;
            InputMode.zsallmap = this.chkzsallmap.Checked;
            InputMode.zsmode1 = ((int)this.nuzsmode2.Value);
            InputMode.outtype = this.cmouttype.SelectedIndex;
            InputMode.datacf = this.chedatacf.Checked;
            InputMode.imghh = this.chimghh.Checked;
            InputMode.oneoutbj = this.choneoutbj.Checked;
            InputMode.ftfzxs = this.chftfzxs.Checked;
            InputMode.dcxz = this.chkdcxz.Checked;
            InputMode.iselect = this.chkiselect.Checked;
            InputMode.onesp = this.chkonesp.Checked;
            InputMode.select3 = this.chkselect3.Checked;
            InputMode.spaceaout = this.cmspace.SelectedIndex;
            InputMode.autodata = this.chkautodata.Checked;
            InputMode.useregular = this.cheuseregular.Checked;
            InputMode.smautoadd = this.chksmautoadd.Checked;
            winput.SaveSetting();
            WinInput.InputStatus.bstring = new SolidBrush(InputMode.Skinbstring);
            WinInput.InputStatus.bcstring = new SolidBrush(InputMode.Skinbcstring);
            WinInput.InputStatus.fbcstring = new SolidBrush(InputMode.Skinfbcstring);
            WinInput.InputStatus.skinback = new SolidBrush(InputMode.SkinBack);
            if (InputMode.useregular && File.Exists(System.IO.Path.Combine(InputMode.AppPath, "dict", InputMode.CDPath, "setting.yaml")))
                WinInput.settingYaml = new YAMLHelp(System.IO.Path.Combine(InputMode.AppPath, "dict", InputMode.CDPath, "setting.yaml"));
            Comm.Function.RunWhenStart(InputMode.AutoRun);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            this.cmouttype.SelectedIndex = 0;
            this.cmspace.SelectedIndex = 0;
            this.ckOpenCould.Checked = InputMode.OpenCould;
            this.ckAutoUpdate.Checked = InputMode.AutoUpdate;
            this.chkAutoRun.Checked = InputMode.AutoRun;
            this.ckLink.Checked = InputMode.OpenLink;
            this.numSkinHeight.Value = InputMode.SkinHeith;
            this.selectnum.Value = InputMode.PageSize;
            this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
            this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
            this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            this.SkinBack.ForeColor = InputMode.SkinBack;
            this.btnSkinFontName.Font = new Font(InputMode.SkinFontName, InputMode.SkinFontSize);
            this.tracchk.Checked = winput.curTrac;
            this.mousetracchk.Checked = winput.curMouseTrac;
            this.ckalt.Checked = InputMode.OpenAltSelect;
            this.SingleInput.Checked = InputMode.SingleInput;
            this.txtla.Text = InputMode.txtla;
            this.txtra.Text = InputMode.txtra;
            this.txtlas.Text = InputMode.txtlas;
            this.txtras.Text = InputMode.txtras;
            this.txtlra.Text = InputMode.txtlra;
            this.txtlras.Text = InputMode.txtlras;
            this.ckright3out.Checked = InputMode.right3_out;
            this.ckpinyin.Checked = InputMode.pinyin;
            this.chclosebj.Checked = InputMode.closebj;
            this.ckautopos.Checked = InputMode.autopos;
            this.cktautopos.Checked = InputMode.tautopos;
            this.chkbjzckgsp.Checked = InputMode.bjzckgsp;
            this.chkomeno.Checked = InputMode.omeno;
            this.chkzsallmap.Checked = InputMode.zsallmap;
            this.nuzsmode2.Value = InputMode.zsmode1;
            this.cmouttype.SelectedIndex = InputMode.outtype;
            this.cmspace.SelectedIndex = InputMode.spaceaout;
            this.groupBox2.BackColor = InputMode.SkinBack;
            this.comboBox1.SelectedIndex = InputMode.SkinIndex;
            this.chedatacf.Checked = InputMode.datacf;
            this.chimghh.Checked = InputMode.imghh;
            this.choneoutbj.Checked = InputMode.oneoutbj;
            this.chftfzxs.Checked = InputMode.ftfzxs;

            this.chkdcxz.Checked = InputMode.dcxz;
            this.chkiselect.Checked = InputMode.iselect;
            this.chkonesp.Checked = InputMode.onesp;
            this.chkselect3.Checked = InputMode.select3;

            this.chkautodata.Checked = InputMode.autodata;

            this.cheuseregular.Checked = InputMode.useregular;
            this.chksmautoadd.Checked = InputMode.smautoadd;
            this.Text = "属性设置 ";
        }
 
        private void numSkinHeight_ValueChanged(object sender, EventArgs e)
        {
            InputMode.SkinHeith = int.Parse(this.numSkinHeight.Value.ToString());
        }

        private void btnSkinbstring_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.btnSkinbstring.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinbstring.ForeColor = this.colorDialog1.Color;
                InputMode.Skinbstring = this.btnSkinbstring.ForeColor;
            }
        }

        private void btnSkinbcstring_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.btnSkinbcstring.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinbcstring.ForeColor = this.colorDialog1.Color;
                InputMode.Skinbcstring = this.btnSkinbcstring.ForeColor;
            }
        }

        private void btnSkinfbcstring_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.btnSkinfbcstring.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinfbcstring.ForeColor = this.colorDialog1.Color;
                InputMode.Skinfbcstring = this.btnSkinfbcstring.ForeColor;
            }
        }

        private void btnSkinFontName_Click(object sender, EventArgs e)
        {
            this.fontDialog1.Font = new Font(InputMode.SkinFontName
                , InputMode.SkinFontSize);
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.btnSkinFontName.Font = this.fontDialog1.Font;
                InputMode.SkinFontName = this.btnSkinFontName.Font.Name;
                InputMode.SkinFontSize = (int)this.btnSkinFontName.Font.Size;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InputMode.SkinIndex = this.comboBox1.SelectedIndex;
            if (this.comboBox1.SelectedIndex == 0)
            {
                InputMode.SkinBack = Color.FromArgb(22, 79, 141);//背景色
                InputMode.Skinbordpen = Color.Gray;//边框色
                InputMode.Skinbstring = Color.White;//字体颜色
                InputMode.Skinbcstring = Color.Orange;//提示补码颜色
                InputMode.Skinfbcstring = Color.Yellow;//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
    
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                //清风
                InputMode.SkinBack = Color.FromArgb(241, 241, 245);//背景色
                InputMode.Skinbordpen = Color.FromArgb(205, 205, 250);//边框色
                InputMode.Skinbstring = Color.FromArgb(58, 0, 111);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(128, 0, 0);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(0, 145, 0);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 2)
            {
                //安卓
                InputMode.SkinBack = Color.FromArgb(162, 192, 83);//背景色
                InputMode.Skinbordpen = Color.FromArgb(162, 192, 83);//边框色
                InputMode.Skinbstring = Color.FromArgb(255, 255, 255);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(255, 255, 255);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 255, 255);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 3)
            {
                //星际争霸
                InputMode.SkinBack = Color.FromArgb(38, 38, 38);//背景色
                InputMode.Skinbordpen = Color.FromArgb(38, 38, 38);//边框色
                InputMode.Skinbstring = Color.FromArgb(85, 187, 48);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(168, 255, 96);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(168, 255, 96);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 4)
            {
                //小鹤
                InputMode.SkinBack = Color.FromArgb(244, 244, 244);//背景色
                InputMode.Skinbordpen = Color.FromArgb(205, 205, 250);//边框色
                InputMode.Skinbstring = Color.FromArgb(0, 128, 255);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(0, 128, 255);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(0, 128, 255);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 5)
            {
                //战争
                InputMode.SkinBack = Color.FromArgb(95, 108, 99);//背景色
                InputMode.Skinbordpen = Color.FromArgb(95, 108, 99);//边框色
                InputMode.Skinbstring = Color.FromArgb(220, 254, 171);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(220, 254, 171);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(220, 254, 171);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 6)
            {
                // 
                InputMode.SkinBack = Color.FromArgb(213, 63, 24);//背景色
                InputMode.Skinbordpen = Color.FromArgb(213, 63, 24);//边框色
                InputMode.Skinbstring = Color.FromArgb(255, 255, 255);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(255, 255, 255);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 255, 255);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 7)
            {
                // 
                InputMode.SkinBack = Color.FromArgb(65, 122, 208);//背景色
                InputMode.Skinbordpen = Color.FromArgb(65, 122, 208);//边框色
                InputMode.Skinbstring = Color.FromArgb(0, 0, 0);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(0, 0, 0);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 255, 255);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 7)
            {
                // 
                InputMode.SkinBack = Color.FromArgb(65, 122, 208);//背景色
                InputMode.Skinbordpen = Color.FromArgb(65, 122, 208);//边框色
                InputMode.Skinbstring = Color.FromArgb(0, 0, 0);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(0, 0, 0);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 255, 255);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 8)
            {
                // 
                InputMode.SkinBack = Color.FromArgb(37, 37, 37);//背景色
                InputMode.Skinbordpen = Color.FromArgb(131, 151, 62);//边框色
                InputMode.Skinbstring = Color.FromArgb(78, 183, 124);//字体颜色
                InputMode.Skinbcstring = Color.FromArgb(217, 217, 0);//提示补码颜色
                InputMode.Skinfbcstring = Color.FromArgb(255, 255, 255);//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
            else if (this.comboBox1.SelectedIndex == 9)
            {
                // 
                InputMode.SkinBack = Color.White;//背景色
                InputMode.Skinbordpen = Color.FromArgb(131, 151, 62);//边框色
                InputMode.Skinbstring = Color.Black;//字体颜色
                InputMode.Skinbcstring = Color.OrangeRed;//提示补码颜色
                InputMode.Skinfbcstring = Color.Blue;//第一候选框字体颜色
                this.groupBox2.BackColor = InputMode.SkinBack;
                this.btnSkinbstring.ForeColor = InputMode.Skinbstring;
                this.btnSkinbcstring.ForeColor = InputMode.Skinbcstring;
                this.btnSkinfbcstring.ForeColor = InputMode.Skinfbcstring;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LXFrm lxfrm = new LXFrm();
            lxfrm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清除后将重新统计,确定清除吗?？", "清除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                WinInput.Input.mapkeys.ForEach(f =>
                {
                    f.keydown = 0;
                });
            }
             
        }

        private void SkinBack_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.SkinBack.ForeColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.SkinBack.ForeColor = this.colorDialog1.Color;
                InputMode.SkinBack = this.SkinBack.ForeColor;
                this.groupBox2.BackColor = InputMode.SkinBack;
            }
        }
    }
}
