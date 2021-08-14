using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 开始游戏时的模式选择器
    class Selector: System.Windows.Forms.Form
    {
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;
        private PictureBox pictureBox4;
        new public int Select = 1;  // 隐藏与基类同名的字段

        public Selector()
        {
            this.Opacity = 0;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Selector));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.DarkCyan;
            this.label1.Location = new System.Drawing.Point(25, 514);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "浙江大学《课程综合实践》求是五子棋设计小组2021©版权所有";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.PVP01;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(101, 120);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 53);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AI01;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Location = new System.Drawing.Point(101, 217);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 53);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseHover += new System.EventHandler(this.pictureBox2_MouseHover);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Quit01;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Location = new System.Drawing.Point(101, 410);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(200, 53);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.MouseHover += new System.EventHandler(this.pictureBox3_MouseHover);
            this.pictureBox3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseMove);
            this.pictureBox3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Interval = 3;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 3;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AVA01;
            this.pictureBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox4.Location = new System.Drawing.Point(101, 314);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(200, 53);
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.MouseHover += new System.EventHandler(this.pictureBox4_MouseHover);
            this.pictureBox4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseMove);
            this.pictureBox4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseUp);
            // 
            // Selector
            // 
            this.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Selector;
            this.ClientSize = new System.Drawing.Size(402, 540);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Selector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Selector_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Selector_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Selector_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.2;
            if (this.Opacity >= 1)
            {
                timer1.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.2;
            if (this.Opacity <= 0)
            {
                timer2.Enabled = false;
                switch (Select)
                {
                    case 0:
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Dispose();
                        break;
                    case 1:
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                        this.Dispose();
                        break;
                    case 2:
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        this.Dispose();
                        break;
                    case 3:
                        this.DialogResult = System.Windows.Forms.DialogResult.No;
                        this.Dispose();
                        break;
                }
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.ToolTipTitle = "玩家对战";
            // 设置伴随的对象
            toolTip1.SetToolTip(pictureBox1, "在您的计算机上进行本地双人对战");
        }

        private void Selector_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.PVP01;
            pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AI01;
            pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Quit01;
            pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AVA01;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.PVP02;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Select = 0;
            timer2.Start();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AI02;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            Select = 1;
            timer2.Start();
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Quit02;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            Select = 2;
            timer2.Start();
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            // 设置显示样式
            toolTip2.AutoPopDelay = 10000;
            toolTip2.InitialDelay = 0;
            toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.ToolTipTitle = "人机对战";
            // 设置伴随的对象
            toolTip2.SetToolTip(pictureBox2, "和基于极大极小值搜索算法的超强人工智能一较高下");
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip3 = new ToolTip();
            // 设置显示样式
            toolTip3.AutoPopDelay = 10000;
            toolTip3.InitialDelay = 0;
            toolTip3.ReshowDelay = 500;
            toolTip3.ShowAlways = true;
            toolTip3.ToolTipTitle = "退出游戏";
            // 设置伴随的对象
            toolTip3.SetToolTip(pictureBox3, "关闭游戏并返回桌面");
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AVA02;
            ToolTip toolTip4 = new ToolTip();
            // 设置显示样式
            toolTip4.AutoPopDelay = 10000;
            toolTip4.InitialDelay = 0;
            toolTip4.ReshowDelay = 500;
            toolTip4.ShowAlways = true;
            toolTip4.ToolTipTitle = "神仙对决";
            // 设置伴随的对象
            toolTip4.SetToolTip(pictureBox4, "欣赏两位基于极大极小值搜索算法的超强人工智能间的巅峰对决");
        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.AVA02;
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            Select = 3;
            timer2.Start();
        }
    }
}
