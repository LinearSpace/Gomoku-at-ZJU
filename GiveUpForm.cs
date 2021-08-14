using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 求是五子棋___Gomoku_at_ZJU
{
    class GiveUpForm: System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;

        private bool Dialog = false;

        public GiveUpForm(bool Side, int FatherWndX, int FatherWndY)
        {
            InitializeComponent();
            ResizeWnd();
            this.Opacity = 0;
            this.Location = new System.Drawing.Point(FatherWndX + MainFrmSize.FormWidth / 2 - 397 / 2, FatherWndY + MainFrmSize.FormHeight / 2 - 239 / 2);
            if(Side == true)
            {
                label2.Text = "您是黑方，您确定要主动放弃本场对局吗？";
            }
            else
            {
                label2.Text = "您是白方，您确定要主动放弃本场对局吗？";
            }
        }

        private void ResizeWnd()
        {
            float size = label1.Font.Size;
            this.label1.Font = new Font(label1.Font.Name, label1.Font.Size / Form1.ScaleConst, label1.Font.Style);
            size = label2.Font.Size;
            this.label2.Font = new Font(label2.Font.Name, label2.Font.Size / Form1.ScaleConst, label2.Font.Style);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GiveUpForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("汉仪行楷繁", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(141, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "认输";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("华文行楷", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Location = new System.Drawing.Point(31, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(338, 75);
            this.label2.TabIndex = 1;
            this.label2.Text = "您是双方，您确定要主动放弃本场对局吗？";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Yes01;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(60, 174);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.No01;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Location = new System.Drawing.Point(237, 174);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 50);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
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
            // GiveUpForm
            // 
            this.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Winner;
            this.ClientSize = new System.Drawing.Size(397, 239);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GiveUpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.GiveUpForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void GiveUpForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Yes02;
        }

        private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Yes01;
            Dialog = true;
            timer2.Start();
        }

        private void pictureBox2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.No02;
        }

        private void pictureBox2_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.No01;
            Dialog = false;
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.1;
            if (this.Opacity <= 0)
            {
                timer2.Enabled = false;
                if (Dialog == true)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05;
            if (this.Opacity >= 1)
            {
                timer1.Enabled = false;
            }
        }
    }
}
