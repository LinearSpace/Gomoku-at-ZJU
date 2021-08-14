using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 继承Panel类，可以在此为NewPanel添加双缓冲性能
    public class NewPanel : Panel
    {    
        // 开启双缓冲特性
        public NewPanel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
        }
        public NewPanel(IContainer container)
        {
            container.Add(this);
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        // 禁用背景重绘
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014)
                return;
            base.WndProc(ref m);
        }
    }
}
