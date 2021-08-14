using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    class GetMonitorCentre
    {
        public static int CenterX = SystemInformation.PrimaryMonitorSize.Width / 2 - MainFrmSize.FormWidth / 2;
        public static int CenterY = SystemInformation.PrimaryMonitorSize.Height / 2 - MainFrmSize.FormHeight / 2;
    }
}
