using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 棋盘类
    class ChessBoards
    {
        // 默认颜色为对应浅色棋盘的深色秋麒麟
        public static Color ChessBoardColor = Color.DarkGoldenrod;
        public static bool isChessBoardDark = false;

        // 画棋盘的方法：只在生成棋盘素材的时候使用
        public static void DrawChessBoard(Graphics g)
        {
            // 棋格宽度
            int ChessCellWidth = MainSize.ChessBoardGap;
            // 棋格数量
            int ChessCellsNum = MainSize.ChessBoardHeight / ChessCellWidth - 1;
            
            Pen Pencil = new Pen(ChessBoardColor);
            Pencil.Width = 1;
            for(int i = 0; i < ChessCellsNum + 1; i ++)
            {
                if(i == 0 || i == ChessCellsNum)
                {
                    Pencil.Width = 2;
                }
                else
                {
                    Pencil.Width = 1;
                }
                g.DrawLine(Pencil, 
                    new Point(20, 20 + i * ChessCellWidth), 
                    new Point(20 + ChessCellsNum * ChessCellWidth, i * ChessCellWidth + 20));
                g.DrawLine(Pencil,
                    new Point(20 + i * ChessCellWidth, 20),
                    new Point(i * ChessCellWidth + 20, 20 + ChessCellsNum * ChessCellWidth));
            }

            // 绘制棋盘上的五个点
            Pencil.Width = 2;

            // 如果当前背景为浅色棋盘
            if (ChessBoardColor == Color.DarkGoldenrod)
            {
                g.FillEllipse(Brushes.DarkGoldenrod, 15 + 3 * ChessCellWidth, 15 + 3 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.DarkGoldenrod, 15 + 11 * ChessCellWidth, 15 + 11 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.DarkGoldenrod, 15 + 7 * ChessCellWidth, 15 + 7 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.DarkGoldenrod, 15 + 3 * ChessCellWidth, 15 + 11 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.DarkGoldenrod, 15 + 11 * ChessCellWidth, 15 + 3 * ChessCellWidth, 10, 10);
            }

            // 如果当前背景为深色棋盘
            else if (ChessBoardColor == Color.White)
            {
                g.FillEllipse(Brushes.White, 15 + 3 * ChessCellWidth, 15 + 3 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.White, 15 + 11 * ChessCellWidth, 15 + 11 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.White, 15 + 7 * ChessCellWidth, 15 + 7 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.White, 15 + 3 * ChessCellWidth, 15 + 11 * ChessCellWidth, 10, 10);
                g.FillEllipse(Brushes.White, 15 + 11 * ChessCellWidth, 15 + 3 * ChessCellWidth, 10, 10);
            }
        }
    }
}
