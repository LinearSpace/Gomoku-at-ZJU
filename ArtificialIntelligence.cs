using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 求是五子棋___Gomoku_at_ZJU.Form1;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 基于极大极小值搜索算法的人工智能
    class ArtificialIntelligence
    {
        // 为场上的一些常见局势赋予对AI有利度从高到低的分值，可以调整部分分值让AI呈现不同的特性
        enum ScoreTable
        {
            WinFive     = 100000,           // 五连
            AliveFour   = 10000,            // 活四
            DeadFour    = 2000,             // 死四
            AliveThere  = 2000,             // 活三
            DeadThree   = 100,              // 死三
            AliveTwo    = 100,              // 活二
            DeadTwo     = 10                // 死二
        };

        // 定义整数组类，实则类似C++的pair<int, int>用法，用于存储点
        public class Pairint
        {
            // 默认构造函数是一个不在棋盘上的点
            public Pairint()
            {

            }

            // 重载构造函数记录一个棋盘上的点
            public Pairint(int X, int Y)
            {
                this.x = X;
                this.y = Y;
            }

            public int x = -1;
            public int y = -1;
        }

        #region 获取不同情况对应的分数
        protected static int GetScore(int number, int empty)
        {
            // 五连
            if(number >= 5)
            {
                return (int)ScoreTable.WinFive;
            }

            // 四连
            else if(number == 4)
            {
                // 活四
                if(empty == 2)
                {
                    return (int)ScoreTable.AliveFour;
                }

                // 死四
                else if(empty == 1)
                {
                    return (int)ScoreTable.DeadFour;
                }
            }

            // 三连
            else if(number == 3)
            {
                // 活三
                if(empty == 2)
                {
                    return (int)ScoreTable.AliveThere;
                }

                // 死三
                else if(empty == 1)
                {
                    return (int)ScoreTable.DeadThree;
                }
            }

            // 二连
            else if(number == 2)
            {
                // 活二
                if(empty == 2)
                {
                    return (int)ScoreTable.AliveTwo;
                }

                // 死二
                else if(empty == 1)
                {
                    return (int)ScoreTable.DeadTwo;
                }
            }

            else if(number == 1 && empty == 2)
            {
                return 10;
            }

            // 不是上述情况则直接返回0分，表示无价值
            return 0;
        }
        #endregion

        #region 获取棋盘某一行、某一列或者某一斜线的判分
        protected static int GetValue(List<Chess_State> Arr, Chess_State Side)
        {
            int temp = 0;
            int len = Arr.Count();
            int empty = 0;
            int number = 0;
            if(Arr[0] == Chess_State.NONE)
            {
                empty++;
            }
            else if(Arr[0] == Side)
            {
                number++;
            }
            int i = 1;
            while (i < len)
            {
                if (Arr[i] == Side)
                {
                    number++;
                }
                else if (Arr[i] == Chess_State.NONE)
                {
                    if (number == 0)
                    {
                        empty = 1;
                    }
                    else
                    {
                        temp += GetScore(number, empty + 1);
                        empty = 1;
                        number = 0;
                    }
                }
                else
                {
                    temp += GetScore(number, empty);
                    empty = 0;
                    number = 0;
                }
                i++;
            }
            temp += GetScore(number, empty);
            return temp;
        }
        #endregion

        #region 获取评价
        protected static int Evaluate()
        {
            // 人工智能评分
            int c_score = 0;
            // 玩家评分
            int p_score = 0;

            // 行评分
            for(int i = 0; i < 15; i ++)
            {
                List<Chess_State> Arr = new List<Chess_State>();
                for(int j = 0; j < 15; j ++)
                {
                    Arr.Add(BoardCells[i, j]);
                }
                c_score += GetValue(Arr, Chess_State.WHITE);
                p_score += GetValue(Arr, Chess_State.BLACK);
                Arr.Clear();
            }

            // 列评分
            for(int j = 0; j < 15; j ++)
            {
                List<Chess_State> Arr = new List<Chess_State>();
                for(int i = 0; i < 15; i ++)
                {
                    Arr.Add(BoardCells[i, j]);
                }
                c_score += GetValue(Arr, Chess_State.WHITE);
                p_score += GetValue(Arr, Chess_State.BLACK);
                Arr.Clear();
            }

            // 上半正对角线
            for(int i = 0; i < 15; i ++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for(x = i, y = 0; x < 15 && y < 15; x ++, y ++)
                {
                    Arr.Add(BoardCells[y, x]);
                }
                c_score += GetValue(Arr, Chess_State.WHITE);
                p_score += GetValue(Arr, Chess_State.BLACK);
                Arr.Clear();
            }

            // 下半正对角线
            for(int j = 1; j < 15; j ++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for (x = 0, y = j; x < 15 && y < 15; x++, y++)
                {
                    Arr.Add(BoardCells[y, x]);
                }
                c_score += GetValue(Arr, Chess_State.WHITE);
                p_score += GetValue(Arr, Chess_State.BLACK);
                Arr.Clear();
            }

            // 上半反对角线
            for(int i = 1; i < 15; i ++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for (y = i, x = 0; y >= 0 && x < 15; --y, x++)
                {
                    Arr.Add(BoardCells[y, x]);
                }
                c_score += GetValue(Arr, Chess_State.WHITE);
                p_score += GetValue(Arr, Chess_State.BLACK);
                Arr.Clear();
            }

            // 下半反对角线
            for (int j = 1; j < 15; j++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for (y = j, x = 14; y < 15 && x >= 0; y++, --x)
                {
                    Arr.Add(BoardCells[x, y]);
                }
                c_score += GetValue(Arr, Chess_State.WHITE);
                p_score += GetValue(Arr, Chess_State.BLACK);
                Arr.Clear();
            }

            // 极大极小值法要选择朝着AI有利的方向（即最大值）选择，因此玩家得分采用减法
            return c_score - p_score;
        }
        #endregion

        #region 周围48格（7×7）内是否存在棋子
        protected static bool HasChess(int x, int y)
        {
            int i, j;
            for (i = (x - 3 > 0 ? x - 3 : 0); i <= x + 3 && i < 15; i++)
                for (j = (y - 3 > 0 ? y - 3 : 0); j <= y + 3 && j < 15; j++)
                    if (i != 0 || j != 0)
                        if (BoardCells[i, j] != Chess_State.NONE)
                            return true;
            return false;
        }
        #endregion

        #region 得到棋盘中可以落子的位置
        protected static List<Pairint> GetPoint()
        {
            List<Pairint> v = new List<Pairint>();
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    if (BoardCells[i, j] == Chess_State.NONE && HasChess(i, j) == true)
                    {
                        Pairint p = new Pairint(i, j);
                        v.Add(p);
                    }
            return v;
        }
        #endregion

        #region 玩家层的落子函数，极小-极大函数
        protected static int Min_Max(int depth, int i1, int i2)
        {
            int value = Evaluate();
            if(GameControl.ChessJudgement(BoardCells, Chess_State.BLACK) || depth <= 0)
            {
                return value;
            }

            List<Pairint> v = GetPoint();
            int len = v.Count();
            int best = int.MaxValue;

            for(int i = 0; i < len; i ++)
            {
                BoardCells[v[i].x, v[i].y] = Chess_State.BLACK;
                int temp = (int)(Max_Min(depth - 1, v[i].x, v[i].y) * 0.5) + value;
                if (temp < best)
                {
                    best = temp;
                }
                BoardCells[v[i].x, v[i].y] = Chess_State.NONE;
            }

            return best;
        }
        #endregion

        #region 人工智能层的落子函数，极大-极小函数
        protected static int Max_Min(int depth, int i1, int i2)
        {
            int value = Evaluate();
            if (GameControl.ChessJudgement(BoardCells, Chess_State.WHITE) || depth <= 0)
            {
                return value;
            }

            List<Pairint> v = GetPoint();
            int len = v.Count();
            int best = int.MinValue;

            for(int i = 0; i < len; i++)
            {
                BoardCells[v[i].x, v[i].y] = Chess_State.WHITE;
                int temp = (int)(Min_Max(depth - 1, v[i].x, v[i].y) * 0.5) + value;
                if(temp > best)
                {
                    best = temp;
                }
                BoardCells[v[i].x, v[i].y] = Chess_State.NONE;
            }

            return best;
        }
        #endregion

        // 人工智能模块的接口函数，获得人工智能落子位置
        public static Pairint Result()
        {
            int depth = 2;

            List<Pairint> v = GetPoint();

            int best = int.MinValue;
            int len = v.Count();

            Console.WriteLine(len);

            List<Pairint> u = new List<Pairint>();

            for(int i = 0; i < len; i ++)
            {
                BoardCells[v[i].x, v[i].y] = Chess_State.WHITE;
                int temp = Min_Max(depth - 1, v[i].x, v[i].y);
                if(temp == best)
                {
                    u.Add(v[i]);
                }
                if(temp > best)
                {
                    best = temp; 
                    u = new List<Pairint>();
                    u.Add(v[i]);
                }
                BoardCells[v[i].x, v[i].y] = Chess_State.NONE;
            }

            len = u.Count();
            Console.WriteLine(len);

            // 如果存在多个可取的点，则随机取一个点作为返回值
            Random random = new Random();
            int r = random.Next() % len;

            Console.WriteLine(u[r].x);
            Console.WriteLine(u[r].y);

            Pairint res = new Pairint(u[r].x, u[r].y);
            return res;
        }
    }
}
