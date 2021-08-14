using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 获取当前运行状态下用户是否取得了管理员权限
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

            // 如果具有管理员权限，则直接运行程序
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                WelcomeForm welcome = new WelcomeForm();
                welcome.ShowDialog();

                // 如果开始界面正确关闭，就打开选择模式界面
                if (welcome.DialogResult == DialogResult.OK)
                {
                    Selector selector = new Selector();
                    selector.ShowDialog();

                    // DialogResult.OK对应玩家对战模式
                    if (selector.DialogResult == DialogResult.OK)
                    {
                        Application.Run(new Form1(Form1.Game_Mode.PVP));
                    }

                    // DialogResult.Yes对应人机对战模式
                    else if (selector.DialogResult == DialogResult.Yes)
                    {
                        Application.Run(new Form1(Form1.Game_Mode.AI));
                    }

                    // DialogResult.No对应人机对战模式
                    else if (selector.DialogResult == DialogResult.No)
                    {
                        Application.Run(new Form1(Form1.Game_Mode.AVA));
                    }
                    // 否则直接关闭环境
                    else
                    {
                        System.Environment.Exit(0);
                    }
                }
            }

            // 否则强制获取管理员权限，保证字体能够被正常按照
            else
            {
                //创建启动对象
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                //设置启动动作，确保以管理员身份运行
                startInfo.Verb = "runas";
                try
                {
                    System.Diagnostics.Process.Start(startInfo);
                }
                catch
                {
                    return;
                }
                Application.Exit();
            }
        }
    }
}
