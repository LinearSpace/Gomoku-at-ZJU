using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using static 求是五子棋___Gomoku_at_ZJU.Form1;

namespace 求是五子棋___Gomoku_at_ZJU
{
    class Chess
    {
        // 舍弃原先的局部绘制，一律改为全局重绘

        #region 在Panel上直接重新聚焦绘制全部棋子，Paint方法专用
        public static void RefocusDrawChess(Panel p, Chess_State [,] BoardCells)
        {
            Graphics g = p.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 直接调用其重载函数在画布上绘制棋子
            RefocusDrawChess(g, BoardCells);

            g.Dispose();
            GC.Collect();
        }
        #endregion

        #region 重载的聚焦重绘，在一个Graphics对象上绘制棋子，鼠标移动事件专用
        public static void RefocusDrawChess(Graphics g, Chess_State[,] BoardCells)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 如果是AI模式，则给AI最后一颗落子打上标记
            if (Form1.GameMode == Game_Mode.AI && RetractList.Count >= 2)
            {
                // 先检测列表中的最后一颗棋子
                Retract LastChess = RetractList[RetractList.Count() - 1];
                // 如果这颗子是白子
                if (BoardCells[LastChess.X, LastChess.Y] == Chess_State.WHITE)
                {
                    // 获取绘图的精确位置
                    int LastChessX = LastChess.X * MainSize.ChessBoardGap + 1;
                    int LastChessY = LastChess.Y * MainSize.ChessBoardGap + 1;
                    g.DrawEllipse(new Pen(Color.OrangeRed, 2), LastChessX, LastChessY, 37, 37);
                }

                // 否则则检测列表的倒数第二颗棋子
                else
                {
                    LastChess = RetractList[RetractList.Count() - 2];
                    int LastChessX = LastChess.X * MainSize.ChessBoardGap + 1;
                    int LastChessY = LastChess.Y * MainSize.ChessBoardGap + 1;
                    g.DrawEllipse(new Pen(Color.OrangeRed, 2), LastChessX, LastChessY, 37, 37);
                }
            }

            // 如果是人工智能对战，则需要同时标记双方的最后一个棋子
            else if(Form1.GameMode == Game_Mode.AVA && RetractList.Count >= 2)
            {
                // 最后两颗棋子
                Retract LastChess = RetractList[RetractList.Count - 1];
                Retract NextChess = RetractList[RetractList.Count - 2];

                // 如果最后一颗棋子是黑子
                if(BoardCells[LastChess.X, LastChess.Y] == Chess_State.BLACK)
                {
                    int LastChessX = LastChess.X * MainSize.ChessBoardGap + 1;
                    int LastChessY = LastChess.Y * MainSize.ChessBoardGap + 1;
                    g.DrawEllipse(new Pen(Color.LightSkyBlue, 3), LastChessX, LastChessY, 37, 37);

                    int NextChessX = NextChess.X * MainSize.ChessBoardGap + 1;
                    int NextChessY = NextChess.Y * MainSize.ChessBoardGap + 1;
                    g.DrawEllipse(new Pen(Color.OrangeRed, 3), NextChessX, NextChessY, 37, 37);
                }

                // 如果最后一颗子是白子
                else
                {
                    int LastChessX = LastChess.X * MainSize.ChessBoardGap + 1;
                    int LastChessY = LastChess.Y * MainSize.ChessBoardGap + 1;
                    g.DrawEllipse(new Pen(Color.OrangeRed, 3), LastChessX, LastChessY, 37, 37);

                    int NextChessX = NextChess.X * MainSize.ChessBoardGap + 1;
                    int NextChessY = NextChess.Y * MainSize.ChessBoardGap + 1;
                    g.DrawEllipse(new Pen(Color.LightSkyBlue, 3), NextChessX, NextChessY, 37, 37);
                }
            }
            else if (Form1.GameMode == Game_Mode.AVA && RetractList.Count == 1)
            {
                Retract LastChess = RetractList[RetractList.Count - 1];
                int LastChessX = LastChess.X * MainSize.ChessBoardGap + 1;
                int LastChessY = LastChess.Y * MainSize.ChessBoardGap + 1;
                g.DrawEllipse(new Pen(Color.LightSkyBlue, 3), LastChessX, LastChessY, 37, 37);
            }

            for (int i = 0; i < BoardCells.GetLength(0); i++)
            {
                for (int j = 0; j < BoardCells.GetLength(1); j++)
                {
                    if (BoardCells[i, j] != Chess_State.NONE)
                    {
                        int AccurateX = i * MainSize.ChessBoardGap + 1;
                        int AccurateY = j * MainSize.ChessBoardGap + 1;

                        // 画黑子的方法
                        if (BoardCells[i, j] == Chess_State.BLACK)
                        {
                            // 使用深色棋盘时需要给黑色棋子描边
                            if(GameMode != Game_Mode.AVA && ChessBoards.isChessBoardDark == true)
                            {
                                g.DrawEllipse(new Pen(Color.Goldenrod, 2), AccurateX, AccurateY, 37, 37);
                            }
                            g.FillEllipse(
                                new LinearGradientBrush(new Point(20, 0), new Point(20, 40), Color.FromArgb(122, 122, 122), Color.FromArgb(0, 0, 0)),
                                new Rectangle(new Point(AccurateX, AccurateY), new Size(MainSize.ChessDiametre, MainSize.ChessDiametre))
                                );
                        }
                        // 画白子的方法
                        else
                        {
                            g.FillEllipse(
                                new LinearGradientBrush(new Point(20, 0), new Point(20, 40), Color.FromArgb(255, 255, 255), Color.FromArgb(204, 204, 204)),
                                new Rectangle(new Point(AccurateX, AccurateY), new Size(MainSize.ChessDiametre, MainSize.ChessDiametre))
                                );
                        }
                    }
                }
            }
        }
        #endregion

        #region 棋盘全局重绘
        public static void RefocusDrawChessBoard(Panel p, Image pBkgImage)
        {
            Bitmap Map = new Bitmap(600, 600);
            Graphics MapG = Graphics.FromImage(Map);
            MapG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Bitmap ResizedMap = ResizeBitmap(p.BackgroundImage, 1);
            MapG.DrawImage(ResizedMap, new Point(0, 0));
            RefocusDrawChess(MapG, BoardCells);
            Graphics g = p.CreateGraphics();
            g.DrawImage(Map, new Point(0, 0));
            Map.Dispose();
            MapG.Dispose();
            ResizedMap.Dispose();
            g.Dispose();
            GC.Collect();
        }
        #endregion
    }
}
