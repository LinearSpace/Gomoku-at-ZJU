using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 求是五子棋___Gomoku_at_ZJU.Form1;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 游戏控制类
    class GameControl
    {
        // 判断当前判断玩家是否胜利
        public static bool ChessJudgement(Chess_State[,] BoardCells, Chess_State Judgement)
        {
            for(int i = 0; i < BoardCells.GetLength(0); i ++)
            {
                for(int j = 0; j < BoardCells.GetLength(1); j ++)
                {
                    if(BoardCells[i, j] == Judgement)
                    {
                        // 判断是否已经到达横向五连子的极限位置
                        if (i <= BoardSize - 5)
                        {
                            // 如果产生横向五连子
                            if (BoardCells[i + 1, j] == Judgement &&
                                BoardCells[i + 2, j] == Judgement &&
                                BoardCells[i + 3, j] == Judgement &&
                                BoardCells[i + 4, j] == Judgement)
                                return true;
                        }

                        // 判断是否已经到达纵向五连子的极限位置
                        if (j <= BoardSize - 5)
                        {
                            // 如果产生纵向五连子
                            if (BoardCells[i, j + 1] == Judgement &&
                                BoardCells[i, j + 2] == Judgement &&
                                BoardCells[i, j + 3] == Judgement &&
                                BoardCells[i, j + 4] == Judgement)
                                return true;
                        }

                        // 判断是否已经到达-45°方向五连子的极限位置
                        if (i <= BoardSize - 5 && j <= BoardSize - 5)
                        {
                            // 如果产生斜向五连子
                            if (BoardCells[i + 1, j + 1] == Judgement &&
                                BoardCells[i + 2, j + 2] == Judgement &&
                                BoardCells[i + 3, j + 3] == Judgement &&
                                BoardCells[i + 4, j + 4] == Judgement)
                                return true;
                        }

                        // 判断是否已经到达-45°方向五连子的极限位置
                        if (i >= 4 && j <= BoardSize - 5)
                        {
                            // 如果产生斜向五连子
                            if (BoardCells[i - 1, j + 1] == Judgement &&
                                BoardCells[i - 2, j + 2] == Judgement &&
                                BoardCells[i - 3, j + 3] == Judgement &&
                                BoardCells[i - 4, j + 4] == Judgement)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
