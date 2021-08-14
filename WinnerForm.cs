using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 求是五子棋___Gomoku_at_ZJU
{
    class WinnerForm: System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;

        public WinnerForm(int WinnerSize, int FatherWndX, int FatherWndY)
        {
            InitializeComponent();
            ResizeWnd();
            this.Opacity = 0;

            // 黑方胜
            if(WinnerSize == 1)
            {
                label3.Location = new System.Drawing.Point(123, 18);
                label3.Text = "黑方胜";
            }

            // 白方胜
            else if (WinnerSize == 2)
            {
                label3.Location = new System.Drawing.Point(123, 18);
                label3.Text = "白方胜";
            }

            // 和棋
            else if(WinnerSize == 3)
            {
                label3.Location = new System.Drawing.Point(149, 18);
                label3.Text = "和棋";
                label1.Visible = true;
                label2.Visible = true;
            }

            this.Location = new System.Drawing.Point(FatherWndX + MainFrmSize.FormWidth / 2 - 397 / 2, FatherWndY + MainFrmSize.FormHeight / 2 - 239 / 2);
        }

        private void ResizeWnd()
        {
            float size = label1.Font.Size;
            this.label1.Font = new Font(label1.Font.Name, label1.Font.Size / Form1.ScaleConst, label1.Font.Style);
            size = label2.Font.Size;
            this.label2.Font = new Font(label2.Font.Name, label2.Font.Size / Form1.ScaleConst, label2.Font.Style);
            size = label3.Font.Size;
            this.label3.Font = new Font(label3.Font.Name, label3.Font.Size / Form1.ScaleConst, label3.Font.Style);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinnerForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("汉仪行楷繁", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(84, 81);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(240, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "莫为输赢扰清梦";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("汉仪行楷繁", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(84, 116);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(244, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "一局残棋寄深情";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("汉仪行楷繁", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(123, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 48);
            this.label3.TabIndex = 2;
            this.label3.Text = "双方胜";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Back01;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(139, 172);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(146, 67);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
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
            // WinnerForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Winner;
            this.ClientSize = new System.Drawing.Size(397, 239);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WinnerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.WinnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Back02;
        }

        private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Back01;
            timer2.Start();
        }

        private void WinnerForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05;
            if (this.Opacity >= 1)
            {
                timer1.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;
            if (this.Opacity <= 0)
            {
                timer2.Enabled = false;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                // this.Dispose();
            }
        }
    }
}
