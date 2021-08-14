using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 欢迎界面窗口
    public class WelcomeForm: System.Windows.Forms.Form
    {
        public static bool isFirstTime = false;
        // 构造函数，在调用窗口时自动加载组件
        public WelcomeForm()
        {
            InitializeComponent();
            Install_Fonts();
            this.Opacity = 0;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 3;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 3000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 3;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label1.Location = new System.Drawing.Point(5, 318);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(524, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "首次运行需要安装游戏必需的字体，这可能需要花费您几秒钟的时间，安装完后会自动重启游戏。";
            this.label1.Visible = false;
            // 
            // WelcomeForm
            // 
            this.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Welcome;
            this.ClientSize = new System.Drawing.Size(600, 337);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.IContainer components;

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            isFirstTime = isFirstTimeRun();
            // 如果是第一次运行程序，延长timer2的时间保证字体能安装完成
            if(isFirstTime == true)
            {
                label1.Visible = true;
                timer2.Interval = 10000;
            }
            else
            {

            }
            timer1.Start();
        }

        // 读取Log文件判断用户是否第一次运行程序，如果是需要延长开始界面来保证完成安装字体
        private bool isFirstTimeRun()
        {
            string txt = "";
            StreamReader sr = new StreamReader(".\\Logs\\UserState.log");

            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                txt += str;
            }

            sr.Close();
            sr = null;

            if (txt[0] == '0')
            {
                StreamWriter sw = new StreamWriter(".\\Logs\\UserState.log");
                sw.WriteLine("1");
                sw.Close();
                sw = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 游戏字体控制
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        /// <summary>
        /// 安装字体
        /// </summary>
        /// <param name="fontFilePath">字体文件全路径</param>
        /// <returns>是否成功安装字体</returns>
        /// <exception cref="UnauthorizedAccessException">不是管理员运行程序</exception>
        /// <exception cref="Exception">字体安装失败</exception>
        public static bool InstallFont(string fontFilePath)
        {
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();

                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                //判断当前登录用户是否为管理员
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator) == false)
                {
                    throw new UnauthorizedAccessException("当前用户没有取得管理员权限，无法为游戏安装字体。");
                }
                //获取Windows字体文件夹路径
                string fontPath = Path.Combine(System.Environment.GetEnvironmentVariable("WINDIR"), "fonts", Path.GetFileName(fontFilePath));
                //检测系统是否已安装该字体
                if (!File.Exists(fontPath))
                {
                    // File.Copy(System.Windows.Forms.Application.StartupPath + "\\font\\" + FontFileName, FontPath); //font是程序目录下放字体的文件夹
                    //将某路径下的字体拷贝到系统字体文件夹下
                    File.Copy(fontFilePath, fontPath); //font是程序目录下放字体的文件夹
                    AddFontResource(fontPath);

                    //Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0); 
                    //WIN7下编译会出错，不清楚什么问题。注释就行了。  
                    //安装字体
                    WriteProfileString("fonts", Path.GetFileNameWithoutExtension(fontFilePath) + "(TrueType)", Path.GetFileName(fontFilePath));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"[{Path.GetFileNameWithoutExtension(fontFilePath)}] 字体安装失败！原因：{ex.Message}"));
            }
            return true;
        }

        public static void Install_Fonts()
        {
            InstallFont(".\\Data\\Font\\汉仪行楷繁.ttf");
            InstallFont(".\\Data\\Font\\悟空大字库.ttf");
            InstallFont(".\\Data\\Font\\华文行楷.ttf");
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05;
            if (this.Opacity >= 1)
            {
                timer1.Enabled = false;
                timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            timer3.Start();

            // 如果用户是第一次运行，则重启应用程序保证字体生效
            if(isFirstTime == true)
            {
                Application.Restart();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.08;
            if(this.Opacity <= 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Dispose();
            }
        }
    }
}
