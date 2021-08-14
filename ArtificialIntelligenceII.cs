using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 求是五子棋___Gomoku_at_ZJU.Form1;

namespace 求是五子棋___Gomoku_at_ZJU
{
    // 继承人工智能的人工智能2号，与父类的判断方向相反
    class ArtificialIntelligenceII : ArtificialIntelligence
    {
        #region 获取评价
        new protected static int Evaluate()
        {
            // 人工智能评分
            int c_score = 0;
            // 玩家评分
            int p_score = 0;

            // 行评分
            for (int i = 0; i < 15; i++)
            {
                List<Chess_State> Arr = new List<Chess_State>();
                for (int j = 0; j < 15; j++)
                {
                    Arr.Add(BoardCells[i, j]);
                }
                c_score += GetValue(Arr, Chess_State.BLACK);
                p_score += GetValue(Arr, Chess_State.WHITE);
                Arr.Clear();
            }

            // 列评分
            for (int j = 0; j < 15; j++)
            {
                List<Chess_State> Arr = new List<Chess_State>();
                for (int i = 0; i < 15; i++)
                {
                    Arr.Add(BoardCells[i, j]);
                }
                c_score += GetValue(Arr, Chess_State.BLACK);
                p_score += GetValue(Arr, Chess_State.WHITE);
                Arr.Clear();
            }

            // 上半正对角线
            for (int i = 0; i < 15; i++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for (x = i, y = 0; x < 15 && y < 15; x++, y++)
                {
                    Arr.Add(BoardCells[y, x]);
                }
                c_score += GetValue(Arr, Chess_State.BLACK);
                p_score += GetValue(Arr, Chess_State.WHITE);
                Arr.Clear();
            }

            // 下半正对角线
            for (int j = 1; j < 15; j++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for (x = 0, y = j; x < 15 && y < 15; x++, y++)
                {
                    Arr.Add(BoardCells[y, x]);
                }
                c_score += GetValue(Arr, Chess_State.BLACK);
                p_score += GetValue(Arr, Chess_State.WHITE);
                Arr.Clear();
            }

            // 上半反对角线
            for (int i = 1; i < 15; i++)
            {
                int x, y;
                List<Chess_State> Arr = new List<Chess_State>();
                for (y = i, x = 0; y >= 0 && x < 15; --y, x++)
                {
                    Arr.Add(BoardCells[y, x]);
                }
                c_score += GetValue(Arr, Chess_State.BLACK);
                p_score += GetValue(Arr, Chess_State.WHITE);
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
                c_score += GetValue(Arr, Chess_State.BLACK);
                p_score += GetValue(Arr, Chess_State.WHITE);
                Arr.Clear();
            }

            // 极大极小值法要选择朝着AI有利的方向（即最大值）选择，因此玩家得分采用减法
            return c_score - p_score;
        }
        #endregion

        #region 重写的玩家层的落子函数，极小-极大函数
        new protected static int Min_Max(int depth, int i1, int i2)
        {
            int value = Evaluate();
            if (GameControl.ChessJudgement(BoardCells, Chess_State.WHITE) || depth <= 0)
            {
                return value;
            }

            List<Pairint> v = GetPoint();
            int len = v.Count();
            int best = int.MaxValue;

            for (int i = 0; i < len; i++)
            {
                BoardCells[v[i].x, v[i].y] = Chess_State.WHITE;
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

        #region 重写的人工智能层的落子函数，极大-极小函数
        new protected static int Max_Min(int depth, int i1, int i2)
        {
            int value = Evaluate();
            if (GameControl.ChessJudgement(BoardCells, Chess_State.BLACK) || depth <= 0)
            {
                return value;
            }

            List<Pairint> v = GetPoint();
            int len = v.Count();
            int best = int.MinValue;

            for (int i = 0; i < len; i++)
            {
                BoardCells[v[i].x, v[i].y] = Chess_State.BLACK;
                int temp = (int)(Min_Max(depth - 1, v[i].x, v[i].y) * 0.5) + value;
                if (temp > best)
                {
                    best = temp;
                }
                BoardCells[v[i].x, v[i].y] = Chess_State.NONE;
            }

            return best;
        }
        #endregion

        // 重写的人工智能模块的接口函数，获得人工智能落子位置
        new public static Pairint Result()
        {
            int depth = 2;

            List<Pairint> v = GetPoint();

            int best = int.MinValue;
            int len = v.Count();

            Console.WriteLine(len);

            List<Pairint> u = new List<Pairint>();

            for (int i = 0; i < len; i++)
            {
                BoardCells[v[i].x, v[i].y] = Chess_State.BLACK;
                int temp = Min_Max(depth - 1, v[i].x, v[i].y);
                if (temp == best)
                {
                    u.Add(v[i]);
                }
                if (temp > best)
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
