using Microsoft.VisualBasic;
using Shell32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace 求是五子棋___Gomoku_at_ZJU
{
    public partial class Form1 : Form
    {
        // 窗口的缩放系数
        public static float ScaleConst = 0;

        #region 悔棋列表使用的点位类
        public class Retract
        {
            // 构造函数，传入坐标实例化一个悔棋点位
            public Retract(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }

            // 默认悔棋点位坐标无效
            public int X = -1;
            public int Y = -1;
        }
        #endregion

        #region 游戏相关参数
        //游戏状态类型与游戏状态变量
        public enum Game_State
        {
            WAITING,
            GAMING
        };
        private Game_State GameState;

        // 游戏模式类型与游戏模式变量
        public enum Game_Mode
        {
            PVP,
            AI,
            AVA
        };
        public static Game_Mode GameMode = Game_Mode.AI;

        // 记录游戏当前执子玩家，黑子回合: true, 白子回合: false
        public static bool Sides = true;
        
        // 棋盘尺寸
        public const int BoardSize = 15;

        // 棋盘着子状态类型与棋盘着子点位数组变量
        public enum Chess_State        
        {
            NONE,
            BLACK,
            WHITE
        };
        public static Chess_State[,] BoardCells = new Chess_State[BoardSize, BoardSize]; 

        // 悔棋记录列表
        public static List<Retract> RetractList = new List<Retract>();

        // 已弃用：游戏控件列表
        public static List<Label> RowsList = new List<Label>();
        public static List<Label> ColumnsList = new List<Label>();
        #endregion

        // 由模式选择器决定游戏的模式
        public Form1(Game_Mode argGameMode)
        {
            //AllocConsole();  创建一个控制台

            this.Opacity = 0.0;
            GameMode = argGameMode;
            InitializeComponent();

            #region 主窗体本身开启双缓冲
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            DoubleBuffer(this);
            #endregion
            
            // 将窗体上方的两个按钮设置为初始状态不可见
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;

            #region 设置主窗体的所有控件在DPI变化时维持原大小
            int count = this.Controls.Count * 2 + 2;
            float[] factor = new float[count];
            int i = 0;
            factor[i++] = Size.Width;
            factor[i++] = Size.Height;
            foreach (Control ctrl in this.Controls)
            {
                factor[i++] = ctrl.Location.X / (float)Size.Width;
                factor[i++] = ctrl.Location.Y / (float)Size.Height;
                ctrl.Tag = ctrl.Size;
            }
            Tag = factor;
            #endregion
        }

        #region 为主窗体以及内部所有控件开启双缓冲
        private void DoubleBuffer(Control control)
        {
            Control.ControlCollection sonControls = control.Controls; 
            foreach (Control con in sonControls)
            {
                Type dgvType = con.GetType();
                if (dgvType.Name.Equals("Button") || dgvType.Name.Equals("TextBox") || dgvType.Name.Equals("Label") || dgvType.Name.Equals("DateTimePicker") || dgvType.Name.Equals("Panel") || dgvType.Name.Equals("DataGridView") || dgvType.Name.Equals("SplitterPanel") || dgvType.Name.Equals("TabControl") || dgvType.Name.Equals("Label") || dgvType.Name.Equals("SplitContainer"))
                {
                    PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                    pi.SetValue(con, true, null);
                    if (dgvType.Name.Equals("Panel") || dgvType.Name.Equals("SplitterPanel") || dgvType.Name.Equals("TabControl") || dgvType.Name.Equals("SplitContainer"))
                    {
                        DoubleBuffer(con);
                    }
                }
                else if (dgvType.Name.Equals("TabPage"))
                {
                    DoubleBuffer(con);
                }
            }
        }
        #endregion

        #region 开启控制台
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();
        #endregion

        #region 获取当前活动窗口状态

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        // IntPtr myPtr=GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                            //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                          //最下坐标
        }

        #endregion

        // 窗体加载
        private void Form1_Load(object sender, EventArgs e)
        {
            // 获取当前窗口被缩放比例
            ScaleConst = GetWndScaling();

            ResizeWnd(ScaleConst);

            // 初始化游戏
            InitializeGame();
            // 初始化游戏主控计时器
            InitTimer();

            // 获取游戏背景音乐目录
            GetMusicDirectories();

            ChessBoardSound_t.Start();
            // 自动播放背景音乐
            PlayMusic();
            this.Width = MainFrmSize.FormWidth;
            this.Height = MainFrmSize.FormHeight;
            // 窗体自动居中显示
            this.Location = new Point(GetMonitorCentre.CenterX, GetMonitorCentre.CenterY);
            timer1.Enabled = true;

            if(GameMode == Game_Mode.AVA)
            {
                timer3.Enabled = true;
            }
        }

        #region 缩放自适应：获取当前的缩放比率
        private float GetWndScaling()
        {
            int dpiX;
            Graphics graphics = this.CreateGraphics();
            dpiX = (Int32)graphics.DpiX;

            if (dpiX == 96) 
            { 
                return 1; 
            }
            else if (dpiX == 120) 
            {
                return 1.25f; 
            }
            else if (dpiX == 144) 
            {
                return 1.5f; 
            }
            else if (dpiX == 192) 
            {
                return 2f; 
            }
            else 
            {
                return 1; 
            }
        }
        #endregion

        // 调整会随着缩放比例变化的控件的位置和尺寸
        private void ResizeWnd(float Ratio)
        {
            // 自适应游戏计时器标签的字体尺寸
            float size = label1.Font.Size;
            label1.Font = new Font(label1.Font.Name, label1.Font.Size / Ratio, label1.Font.Style);

            // 自适应对局时间标签的字体尺寸
            size = label2.Font.Size;
            label2.Font = new Font(label2.Font.Name, label2.Font.Size / Ratio, label2.Font.Style);
        
            if(Ratio != 1f)
            {
                ChessBoard.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        // 初始化游戏
        private void InitializeGame()
        {
            GetLabelList();
            for (int i = 0; i < BoardSize; i ++)
            {
                for(int j = 0; j < BoardSize; j ++)
                {
                    BoardCells[i, j] = Chess_State.NONE;
                }
            }
            GameState = Game_State.WAITING;

            // 清空悔棋列表
            RetractList.Clear();

            #region 重置窗体的控件状态
            pictureBox5.Visible = true;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            label1.Text = "00:00";
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            // 已弃用：重置边界标签的可见性
            for (int i = 0; i < 15; i++)
            {
                RowsList[i].Visible = false;
                ColumnsList[i].Visible = false;
            }
            #endregion

            ChessBoard.Cursor = Cursors.Arrow;

            // 清空棋盘
            ChessBoard.Invalidate(); 

            Console.WriteLine(ScaleConst);
            Console.WriteLine(ChessBoard.Size.Width);
            Console.WriteLine(ChessBoard.Size.Height);
        }

        // 棋盘面板的绘制事件
        private void ChessBoard_Paint(object sender, PaintEventArgs e)
        {
            Chess.RefocusDrawChess(ChessBoard, BoardCells);
        }

        #region 已弃用：游戏内按钮的点击事件
        /* 已弃用
        // 开始游戏按钮的点击事件
        private void BT_StartGame_Click(object sender, EventArgs e)
        {
            GameState = Game_State.GAMING;
            Sides = true;

            // 设置菜单栏按键状态
            退出游戏ToolStripMenuItem.Enabled = false;       // 实则为开始游戏
            重新开始游戏ToolStripMenuItem.Enabled = true;
            结束本局游戏ToolStripMenuItem.Enabled = true;
            悔棋ToolStripMenuItem.Enabled = true;           // 实则为结束游戏

            ChessBoard.Invalidate();
        }

        // 重新开始游戏按钮的点击事件
        private void BT_RestartGame_Click(object sender, EventArgs e)
        {
            Interaction.Beep();
            if (MessageBox.Show("确定要重新开始游戏吗？", "提示", 
                MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) 
                == DialogResult.OK)
            {
                InitializeGame();
                BT_StartGame_Click(sender, e);
            }
        }

        // 退出游戏按钮的点击事件
        private void BT_QuitGame_Click(object sender, EventArgs e)
        {
            Interaction.Beep();
            if (MessageBox.Show("确定要退出游戏吗？", "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)
                == DialogResult.OK)
            {
                this.Dispose();
            }
        }
         */
        #endregion

        private void ChessBoard_MouseDown(object sender, MouseEventArgs e)
        {
            // 首先确保在游戏中且游戏模式不是人工智能对战
            if (GameState == Game_State.GAMING && GameMode != Game_Mode.AVA)
            {
                // （采用棋子状态类型表示）当前要判断的玩家
                Chess_State JudgePlayer = Chess_State.NONE;

                // 获取鼠标点击对应的棋盘坐标位置
                int PlacementX = e.X / MainSize.ChessBoardGap;
                int PlacementY = e.Y / MainSize.ChessBoardGap;

                // 判断单次点击的有效性
                try
                {
                    // 如果点击的位置已经存在棋子，则直接返回不做处理
                    if (BoardCells[PlacementX, PlacementY] != Chess_State.NONE)
                    {
                        return;
                    }

                    // 如果当前是黑色棋子
                    if (Sides)
                    {
                        BoardCells[PlacementX, PlacementY] = Chess_State.BLACK;
                        JudgePlayer = Chess_State.BLACK;
                    }

                    // 如果当前是白色棋子
                    else
                    {
                        BoardCells[PlacementX, PlacementY] = Chess_State.WHITE;
                        JudgePlayer = Chess_State.WHITE;
                    }

                    //全局棋盘重绘
                    Chess.RefocusDrawChessBoard(ChessBoard, ChessBoard.BackgroundImage);

                    // 播放落子音效
                    isChessDrop = true;

                    // 将本次着子记录添加到悔棋列表
                    RetractList.Add(new Retract(PlacementX, PlacementY));

                    // 如果之前没有棋子，重置悔棋按钮的可见性
                    if (pictureBox6.Visible == false)
                    {
                        pictureBox6.Visible = true;
                    }

                    // 判断当前玩家是否胜利
                    if (GameControl.ChessJudgement(BoardCells, JudgePlayer))
                    {
                        if (JudgePlayer == Chess_State.BLACK)
                        {
                            // 重置当前游戏状态，并且调用胜利方窗体
                            GameState = Game_State.WAITING;
                            WinnerForm WForm = new WinnerForm(1, this.Location.X, this.Location.Y);
                            WForm.ShowDialog();
                            if (WForm.DialogResult == DialogResult.OK)
                            {
                                InitializeGame();
                                WForm = null;
                            }
                        }
                        else
                        {
                            // 重置当前游戏状态，并且调用胜利方窗体
                            GameState = Game_State.WAITING;
                            WinnerForm WForm = new WinnerForm(2, this.Location.X, this.Location.Y);
                            WForm.ShowDialog();
                            if (WForm.DialogResult == DialogResult.OK)
                            {
                                InitializeGame();
                                WForm = null;
                            }
                        }
                    }

                    // 如果棋盘已经下满，则判定和棋
                    else if (RetractList.Count == BoardSize * BoardSize)
                    {
                        // 重置当前游戏状态，并且调用胜利方窗体
                        GameState = Game_State.WAITING;
                        WinnerForm WForm = new WinnerForm(3, this.Location.X, this.Location.Y);
                        WForm.ShowDialog();
                        if (WForm.DialogResult == DialogResult.OK)
                        {
                            InitializeGame();
                            WForm = null;
                        }
                    }

                    else
                    {
                        Sides = !Sides;
                        setSidePicture(Sides);
                        if(GameMode == Game_Mode.AI)
                        {
                            ArtificialIntelligence.Pairint r = new ArtificialIntelligence.Pairint();
                            r = ArtificialIntelligence.Result();
                            BoardCells[r.x, r.y] = Chess_State.WHITE;
                            JudgePlayer = Chess_State.WHITE;

                            // 播放落子音效
                            isChessDrop = true;

                            // 将本次着子记录添加到悔棋列表
                            RetractList.Add(new Retract(r.x, r.y));

                            // 重置悔棋按钮的可见性
                            if (pictureBox6.Visible == false)
                            {
                                pictureBox6.Visible = true;
                            }

                            // 用于AI落子后的棋子重绘
                            Chess.RefocusDrawChessBoard(ChessBoard, ChessBoard.BackgroundImage);

                            #region 判断AI下完棋子后游戏是否结束
                            if (GameControl.ChessJudgement(BoardCells, JudgePlayer))
                            {
                                // 重置当前游戏状态，并且调用胜利方窗体
                                GameState = Game_State.WAITING;
                                WinnerForm WForm = new WinnerForm(2, this.Location.X, this.Location.Y);
                                WForm.ShowDialog();
                                if (WForm.DialogResult == DialogResult.OK)
                                {
                                    InitializeGame();
                                    WForm = null;
                                }
                            }
                            else if (RetractList.Count == BoardSize * BoardSize)
                            {
                                // 重置当前游戏状态，并且调用胜利方窗体
                                GameState = Game_State.WAITING;
                                WinnerForm WForm = new WinnerForm(3, this.Location.X, this.Location.Y);
                                WForm.ShowDialog();
                                if (WForm.DialogResult == DialogResult.OK)
                                {
                                    InitializeGame();
                                    WForm = null;
                                }
                            }

                            else
                            {
                                Sides = !Sides;
                                setSidePicture(Sides);
                            }
                            #endregion
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        #region 已弃用：获取控件列表
        private void GetLabelList()
        {
            RowsList.Add(Rows01);
            RowsList.Add(Rows02);
            RowsList.Add(Rows03);
            RowsList.Add(Rows04);
            RowsList.Add(Rows05);
            RowsList.Add(Rows06);
            RowsList.Add(Rows07);
            RowsList.Add(Rows08);
            RowsList.Add(Rows09);
            RowsList.Add(Rows10);
            RowsList.Add(Rows11);
            RowsList.Add(Rows12);
            RowsList.Add(Rows13);
            RowsList.Add(Rows14);
            RowsList.Add(Rows15);
            ColumnsList.Add(Cols01);
            ColumnsList.Add(Cols02);
            ColumnsList.Add(Cols03);
            ColumnsList.Add(Cols04);
            ColumnsList.Add(Cols05);
            ColumnsList.Add(Cols06);
            ColumnsList.Add(Cols07);
            ColumnsList.Add(Cols08);
            ColumnsList.Add(Cols09);
            ColumnsList.Add(Cols10);
            ColumnsList.Add(Cols11);
            ColumnsList.Add(Cols12);
            ColumnsList.Add(Cols13);
            ColumnsList.Add(Cols14);
            ColumnsList.Add(Cols15);
        }
        #endregion

        // 记录上一次的精确鼠标位置，以判断下一次是否发生了点位偏移判断
        public static int LastAccurateX = 0;
        public static int LastAccurateY = 0;

        // 已彻底优化：鼠标移动时可绘制着子点位
        private void ChessBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if(GameState == Game_State.WAITING || GameMode == Game_Mode.AVA)
            {
                return;
            }

            int AccurateX = e.X / MainSize.ChessBoardGap * MainSize.ChessBoardGap + 1;
            int AccurateY = e.Y / MainSize.ChessBoardGap * MainSize.ChessBoardGap + 1;

            // 如果发生了点位偏移再刷新
            if(AccurateX != LastAccurateX || AccurateY != LastAccurateY)
            {
                // 先创建一个位图，在上面绘制完成以后再一次性绘制到画面上
                #region 着子动态点位的绘制
                Bitmap Map = new Bitmap(600, 600);
                Graphics MapG = Graphics.FromImage(Map);
                MapG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Pen Pencil = new Pen(Color.White, 2);

                Bitmap ResizedMap = ResizeBitmap(ChessBoard.BackgroundImage, 1);
                MapG.DrawImage(ResizedMap, new Point(0, 0));
                // 在位图的图像类型上绘制背景、可着子点位与所有的棋子
                MapG.DrawEllipse(Pencil, AccurateX, AccurateY, 37, 37);
                Chess.RefocusDrawChess(MapG, BoardCells);

                LastAccurateX = AccurateX;
                LastAccurateY = AccurateY;

                //ChessBoard.Refresh();  //不能频繁刷新，直接采取覆盖即可
                Graphics g = ChessBoard.CreateGraphics();
                g.DrawImage(Map, new Point(0, 0));

                // 销毁临时控件
                Map.Dispose();
                MapG.Dispose();
                ResizedMap.Dispose();
                g.Dispose();
                #endregion
                // 通知环境回收
                GC.Collect();
            }
        }

        // 将位图缩放到原先的一定比率，用于自适应
        public static Bitmap ResizeBitmap(Image bit, double Ratio)
        {
            Bitmap destBitmap = new Bitmap(Convert.ToInt32(bit.Width * Ratio), Convert.ToInt32(bit.Height * Ratio));
            Graphics g = Graphics.FromImage(destBitmap);
            g.Clear(Color.Transparent);          
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(bit, new Rectangle(0, 0, destBitmap.Width, destBitmap.Height), 0, 0, bit.Width, bit.Height, GraphicsUnit.Pixel);
            g.Dispose();
            return destBitmap;
        }

        // 用于存储游戏音乐文件夹下的音乐文件
        public static List<FileInfo> MusicList = new List<FileInfo>();
        public static void GetMusicDirectories()
        {
            DirectoryInfo dir = new DirectoryInfo(".\\Music");
            FileInfo[] LocalList = dir.GetFiles("*.mp3");

            // 新建一个日志文档存储用户的音乐文件列表
            System.IO.StreamWriter MusicLog = new System.IO.StreamWriter(".\\Logs\\MusicList.log", false);
            foreach(FileInfo Files in LocalList)
            {
                MusicLog.Write(Files.ToString() + "\n");
                MusicList.Add(Files);
            }
            MusicLog.Close();
            MusicLog.Dispose();
        }


        // 记录上一次音乐开始时的播放时间
        public static int MusicStartHour = 0;
        public static int MusicStartMin = 0;
        public static int MusicStartSec = 0;

        // 初始化计时器，用于控制音频自动播放
        public static System.Timers.Timer MusicTimer = new System.Timers.Timer();

        // 是否第一颗黑子落下
        public static bool isFirstBlackDrop = false;

        private void InitTimer()
        {
            MusicTimer.Enabled = true;
            MusicTimer.Interval = 1000;
            MusicTimer.Start();
            MusicTimer.Elapsed += new System.Timers.ElapsedEventHandler(MusicTimer_Elapsed);
        }

        // 计时器函数，每1秒触发一次
        private void MusicTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 获取已播放时长
            int PlayTime = (e.SignalTime.Hour - MusicStartHour) * 3600 + (e.SignalTime.Minute - MusicStartMin) * 60 + (e.SignalTime.Second - MusicStartSec);
            Console.WriteLine(PlayTime);

            // 如果播放时长已经等于歌曲时长（+1是歌曲间的间隔），就播放下一首音乐
            if (PlayTime == MusicTime + 1)
            {
                Console.WriteLine("Next Music\n");
                PlayMusic();
            }

            if (GameState == Game_State.GAMING)
            {
                // 获取游戏时长
                int GameTime = (e.SignalTime.Hour - GameStartHour) * 3600 + (e.SignalTime.Minute - GameStartMin) * 60 + (e.SignalTime.Second - GameStartSec);
                int GameTimeMin = GameTime / 60;
                int GameTimeSec = GameTime % 60;
                string TimeLabel = string.Format("{0}:{1}", GameTimeMin.ToString("00"), GameTimeSec.ToString("00"));
                setGameTime(TimeLabel);
            }
        }

        private void setGameTime(string TimeLabel)
        {
            
            Action<string> actionDelegate = (TimeLabel) => { this.label1.Text = TimeLabel.ToString(); };
            this.label1.Invoke(actionDelegate, TimeLabel);
        }

        // 记录上一次播放（或正在播放）的音乐在音乐列表中的下标和音乐时长
        public static int LastPlayed = -1;
        public static int MusicTime = int.MaxValue;
        public static void PlayMusic()
        {
            // 检测游戏音乐目录下是否存在音乐
            if(MusicList.Count() == 0)
            {
                // 直接返回，此时计时器将永远不会再执行音乐播放器
                return;
            }
            else
            {
                // 生成一个随机数，并挑选出对应的随机音乐
                int NowPlay = (new Random()).Next() % MusicList.Count();

                // 如果随机挑选的音乐在先前已经播放过，则挑选下一首
                if(NowPlay == LastPlayed)
                {
                    NowPlay = (NowPlay + 1) % MusicList.Count();
                }
                LastPlayed = NowPlay;

                // 获取歌曲的绝对路径
                string SongPath = MusicList[NowPlay].ToString();
                string dirName = Path.GetDirectoryName(SongPath);

                //获得歌曲名称
                string SongName = Path.GetFileName(SongPath);
                FileInfo MusicInfo = new FileInfo(SongPath);
                ShellClass sh = new ShellClass();
                Folder dir = sh.NameSpace(dirName);
                FolderItem item = dir.ParseName(SongName);

                //获取歌曲时间
                string SongTime = Regex.Match(dir.GetDetailsOf(item, -1), "\\d:\\d{2}:\\d{2}").Value;
                string[] TimeList = SongTime.Split(":");
                MusicTime = Convert.ToInt32(TimeList[2]) + 60 * Convert.ToInt32(TimeList[1]) + 3600 * Convert.ToInt32(TimeList[0]);

                // 开始播放音乐
                MediaController.Mp3Player mp3Play = new MediaController.Mp3Player();
                mp3Play.FileName = SongPath;
                string[] NowTimeList = DateTime.Now.ToLongTimeString().ToString().Split(":");

                // 记录歌曲的开始时间
                MusicStartHour = Convert.ToInt32(NowTimeList[0]);
                MusicStartMin = Convert.ToInt32(NowTimeList[1]);
                MusicStartSec = Convert.ToInt32(NowTimeList[2]);

                // 控制台返回播放歌曲的名称和开始时间
                Console.WriteLine(SongName);
                Console.WriteLine(MusicStartHour.ToString() + ":" + MusicStartMin.ToString() + ":" + MusicStartSec.ToString());

                mp3Play.play();
            }
        }

        private void 下一首音乐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayMusic();
        }

        #region 已弃用：系统菜单栏的点击事件
        /* 已弃用
        // 开始游戏点击事件
        private void 退出游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameState = Game_State.GAMING;
            Sides = true;

            // 设置菜单栏按键状态
            退出游戏ToolStripMenuItem.Enabled = false;       // 实则为开始游戏
            重新开始游戏ToolStripMenuItem.Enabled = true;
            结束本局游戏ToolStripMenuItem.Enabled = true;
            悔棋ToolStripMenuItem.Enabled = true;           // 实则为结束游戏

            ChessBoard.Invalidate();
        }

        // 结束游戏点击事件
        private void 悔棋ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Interaction.Beep();
            if (MessageBox.Show("确定要退出游戏吗？", "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)
                == DialogResult.OK)
            {
                this.Dispose();
            }
        }

        // 重新开始游戏点击事件
        private void 重新开始游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Interaction.Beep();
            if (MessageBox.Show("确定要重新开始游戏吗？", "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)
                == DialogResult.OK)
            {
                InitializeGame();
                BT_StartGame_Click(sender, e);
            }
        }

        // 结束本局游戏点击事件
        private void 结束本局游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Interaction.Beep();
            if (MessageBox.Show("确定要结束本局游戏吗？", "提示",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2)
                == DialogResult.OK)
            { 
                InitializeGame();
                ChessBoard.Invalidate();
            }
        }
        */
        #endregion

        #region 为无边框窗口构建移动性能以及设置自定义按钮的属性
        //窗体是否移动
        bool formMove = false;

        //记录窗体的位置 
        Point formPoint;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            formPoint = new Point();
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X;
                yOffset = -e.Y;
                formPoint = new Point(xOffset, yOffset);
                formMove = true;//开始移动  
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMove == true)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(formPoint.X, formPoint.Y);
                Location = mousePos;
            }
            else
            {
                // 先将控件恢复到默认形态
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                if(ChessBoards.isChessBoardDark == false)
                {
                    pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.LightMode00;
                }
                else
                {
                    pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.DarkMode00;
                }

                if (ChessBoards.isChessBoardDark == false)
                {
                    pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton0;
                }
                else
                {
                    pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton0Dark;
                }

                if (560 <= e.X && e.X <= 588 && 19 <= e.Y && e.Y <= 47)
                {
                    if (pictureBox1.Visible == true)
                    {
                        
                    }
                    else
                    {
                        pictureBox1.Visible = true;
                    }
                }
                else if(484 <= e.X && e.X <= 512 && 18 <= e.Y && e.Y <= 46)
                {
                    if (pictureBox2.Visible == true)
                    {

                    }
                    else
                    {
                        pictureBox2.Visible = true;
                    }
                }
                else if(427 <= e.X && e.X <= 455 && 19 <= e.Y && e.Y <= 47)
                {
                    if (ChessBoards.isChessBoardDark == false)
                    {
                        pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.LightMode01;
                    }
                    else
                    {
                        pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.DarkMode01;
                    }
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键    
            {
                formMove = false;//停止移动        
            }
        }

        #endregion

        #region 关闭按钮的鼠标事件
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(ChessBoards.isChessBoardDark == false)
            {
                pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.CloseButton2;
            }
            else
            {
                pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.CloseButton2Dark;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        // 关闭按钮的鼠标悬浮提示
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            // 设置伴随的对象
            toolTip1.SetToolTip(pictureBox1, "退出游戏");
        }
        #endregion

        #region 最小化按钮的鼠标事件

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MinimizeButton1;
            }
            else
            {
                pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MinimizeButton1Dark;
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MinimizeButton2;
            }
            else
            {
                pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MinimizeButton2Dark;
            }
            WindowState = FormWindowState.Minimized;
        } 

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            // 设置伴随的对象
            toolTip1.SetToolTip(pictureBox2, "最小化窗口");
        }
        #endregion

        #region 色彩模式按钮的鼠标事件

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            toolTip1.ToolTipTitle = "色彩模式";
            // 设置伴随的对象
            if(ChessBoards.isChessBoardDark == false)
            {
                toolTip1.SetToolTip(pictureBox3, "当前为明亮模式，按下“色彩模式”按钮可以切换到黑暗模式");
                pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.LightMode01;
            }
            else
            {
                toolTip1.SetToolTip(pictureBox3, "当前为黑暗模式，按下“色彩模式”按钮可以切换到明亮模式");
                pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.DarkMode01;
            } 
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.LightMode02;
            }
            else
            {
                pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.DarkMode02;
            }
        }

        private void pictureBox3_Mouseup(object sender, MouseEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                this.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MainFormDark;
                pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.DarkMode01;
                pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.CloseButton1Dark;
                pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MinimizeButton2Dark;
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton0Dark;
                ChessBoards.isChessBoardDark = true;
                ChessBoard.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Background02;
            }
            else
            {
                this.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MainForm;
                pictureBox3.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.LightMode01;
                pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.CloseButton1;
                pictureBox2.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.MinimizeButton2;
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton0;
                ChessBoards.isChessBoardDark = false;
                ChessBoard.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Background01;
                Chess.RefocusDrawChess(ChessBoard, BoardCells);
            }

            Bitmap Map = new Bitmap(600, 600);
            Graphics MapG = Graphics.FromImage(Map);

            // 强制锁定为自己的1倍，防止分辨率缩放后底图尺寸错误
            Bitmap ResizedMap = ResizeBitmap(ChessBoard.BackgroundImage, 1);
            MapG.DrawImage(ResizedMap, new Point(0, 0));
            Chess.RefocusDrawChess(MapG, BoardCells);

            ChessBoard.Refresh();
            Graphics g = ChessBoard.CreateGraphics();
            g.DrawImage(Map, new Point(0, 0));

            Map.Dispose();
            MapG.Dispose();
            ResizedMap.Dispose();
        }
        #endregion

        #region 开发者信息按钮的鼠标事件
        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            // 设置伴随的对象
            toolTip1.SetToolTip(pictureBox4, "开发者信息");
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton1;
            }
            else
            {
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton1Dark;
            }
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton2;
            }
            else
            {
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton2Dark;
            }
        }

        // 单例模式建立开发者信息窗口
        private static DeveloperInfoForm developerInfoForm = new DeveloperInfoForm(0, 0);
        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton1;
            }
            else
            {
                pictureBox4.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.InfoButton1Dark;
            }
            Console.WriteLine("进入“开发者信息”界面");

            // 确保只有一个开发者信息窗口在运行
            developerInfoForm.Dispose();

            // 传入主窗口目前的位置以便新窗口定位到主窗口中心
            developerInfoForm = new DeveloperInfoForm(this.Location.X, this.Location.Y);
            developerInfoForm.Show();
        }
        #endregion

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox5.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.GameStart02;
        }

        // 记录游戏开始时间
        public static int GameStartHour = 0;
        public static int GameStartMin = 0;
        public static int GameStartSec = 0;

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            GameState = Game_State.GAMING;
            Sides = true;

            // 设置菜单栏按键状态
            // 退出游戏ToolStripMenuItem.Enabled = false;       // 实则为开始游戏
            // 重新开始游戏ToolStripMenuItem.Enabled = true;
            // 结束本局游戏ToolStripMenuItem.Enabled = true;
            悔棋ToolStripMenuItem.Enabled = true;           // 实则为结束游戏
            ChessBoard.Invalidate();

            // 隐藏开始游戏按钮
            pictureBox5.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.GameStart01;
            pictureBox5.Visible = false;
            pictureBox7.Visible = true;

            // 如果是人工智能对决则特殊处理
            if (GameMode == Game_Mode.AVA)
            {
                pictureBox6.Visible = false;
                pictureBox7.Visible = false;
                isFirstBlackDrop = false;
            }

            // 获取游戏开始时间
            string[] NowTimeList = DateTime.Now.ToLongTimeString().ToString().Split(":");
            GameStartHour = Convert.ToInt32(NowTimeList[0]);
            GameStartMin = Convert.ToInt32(NowTimeList[1]);
            GameStartSec = Convert.ToInt32(NowTimeList[2]);

            // 显示对战时间
            label1.Visible = true;
            label2.Visible = true;

            // 将棋盘上的光标改为十字
            ChessBoard.Cursor = Cursors.Cross;

            // 绘制玩家指示图形
            setSidePicture(Sides);
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox6.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Retract02;
        }

        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox6.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.Retract01;
            
            // 由于在悔棋列表为空时不显示悔棋按钮，进入点击事件时已经保证一定有可撤回的记录
            string NowPlayer = "";

            // 判断悔棋方，此时的悔棋方与Sides变量记录的执子方相反
            if (Sides)
            {
                NowPlayer = "白方";
            }
            else
            {
                NowPlayer = "黑方";
            }

            // 玩家对战模式下一次撤回上一位玩家的一颗子
            if(GameMode == Game_Mode.PVP)
            {
                // 记录最后一枚着子的位置
                Retract LastPosition = RetractList[RetractList.Count - 1];

                // 横坐标改为字母
                char Coord_X = (char)(LastPosition.X + 65);

                // 纵坐标从1开始计数
                int Coord_Y = LastPosition.Y + 1;

                string RetractMsg = "您是" + NowPlayer + "，确定要撤回上一步落在[" + Coord_X + ", " + Coord_Y + "]处的棋子吗";
                RetractForm retractForm = new RetractForm(RetractMsg, this.Location.X, this.Location.Y);
                retractForm.ShowDialog();

                // 弹出悔棋确认提示框
                if (retractForm.DialogResult == DialogResult.OK)
                {
                    // 去除该子点位的着子信息
                    BoardCells[LastPosition.X, LastPosition.Y] = Chess_State.NONE;

                    // 去除列表中的最后一条着子记录
                    RetractList.RemoveAt(RetractList.Count - 1);

                    // 将执子方重新交换
                    Sides = !Sides;
                    setSidePicture(Sides);

                    if (RetractList.Count == 0)
                    {
                        pictureBox6.Visible = false;
                    }

                    #region 采用与绘制可着子点位策略相同的方法重绘棋盘
                    Chess.RefocusDrawChessBoard(ChessBoard, ChessBoard.BackgroundImage);
                    retractForm = null;
                    #endregion
                }
            }
            
            // 人机对战模式下一次悔棋撤回包括电脑在内的两颗子
            else if(GameMode == Game_Mode.AI)
            {
                // 记录最后一枚着子的位置
                Retract LastPosition = RetractList[RetractList.Count - 2];

                // 横坐标改为字母
                char Coord_X = (char)(LastPosition.X + 65);

                // 纵坐标从1开始计数
                int Coord_Y = LastPosition.Y + 1;

                string RetractMsg = "您是黑方，确定要撤回上一步落在[" + Coord_X + ", " + Coord_Y + "]处的棋子吗";
                RetractForm retractForm = new RetractForm(RetractMsg, this.Location.X, this.Location.Y);
                retractForm.ShowDialog();

                // 弹出悔棋确认提示框
                if (retractForm.DialogResult == DialogResult.OK)
                {
                    // 去除包括人工智能在内的两颗子
                    BoardCells[RetractList[RetractList.Count - 1].X, RetractList[RetractList.Count - 1].Y] = Chess_State.NONE;
                    BoardCells[RetractList[RetractList.Count - 2].X, RetractList[RetractList.Count - 2].Y] = Chess_State.NONE;

                    // 去除列表中的最后两条记录
                    RetractList.RemoveAt(RetractList.Count - 1);
                    RetractList.RemoveAt(RetractList.Count - 1);

                    if (RetractList.Count == 0)
                    {
                        pictureBox6.Visible = false;
                    }

                    #region 采用与绘制可着子点位策略相同的方法重绘棋盘
                    Chess.RefocusDrawChessBoard(ChessBoard, ChessBoard.BackgroundImage);
                    retractForm = null;
                    #endregion
                }
            }
        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            toolTip1.ToolTipTitle = "悔棋";

            // 设置伴随的对象
            toolTip1.SetToolTip(pictureBox6, "观棋不语真君子，举棋不悔大丈夫");
        }

        #region 窗体渐现渐隐
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05;
            if(this.Opacity >= 1)
            {
                timer1.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;
            if(this.Opacity <= 0)
            {
                timer2.Enabled = false;
                Application.Exit();
                Application.ExitThread();
                this.Dispose();
                System.Environment.Exit(0);
            }
        }

        #endregion

        private void pictureBox7_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            toolTip1.ToolTipTitle = "认输";

            // 设置伴随的对象
            toolTip1.SetToolTip(pictureBox7, "山重水复疑无路，柳暗花明又一村");
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox7.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.GiveUp02;
        }

        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox7.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.GiveUp01;

            GiveUpForm giveUpForm = new GiveUpForm(Sides, this.Location.X, this.Location.Y);
            giveUpForm.ShowDialog();
            if(giveUpForm.DialogResult == DialogResult.OK)
            {
                if (Sides)
                {
                    GameState = Game_State.WAITING;
                    WinnerForm WForm = new WinnerForm(2, this.Location.X, this.Location.Y);
                    WForm.ShowDialog();
                    if (WForm.DialogResult == DialogResult.OK)
                    {
                        InitializeGame();
                        WForm = null;
                    }
                }
                else
                {
                    GameState = Game_State.WAITING;
                    WinnerForm WForm = new WinnerForm(1, this.Location.X, this.Location.Y);
                    WForm.ShowDialog();
                    if (WForm.DialogResult == DialogResult.OK)
                    {
                        InitializeGame();
                        WForm = null;
                    }
                }
            }
        }

        private void setSidePicture(bool Sides)
        {
            if(Sides)
            {
                label3.Visible = true;
                label4.Visible = false;
            }
            else
            {
                label3.Visible = false;
                label4.Visible = true;
            }
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            toolTip1.ToolTipTitle = "回合";

            // 设置伴随的对象
            toolTip1.SetToolTip(label3, "当前是黑方的回合");
        }

        private void label4_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // 设置显示样式
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(254, 240, 202);
            toolTip1.ForeColor = Color.Black;
            toolTip1.ShowAlways = true;
            toolTip1.ToolTipTitle = "回合";

            // 设置伴随的对象
            toolTip1.SetToolTip(label4, "当前是白方的回合");
        }

        #region 独立线程播放落子音效
        // 创建一个棋盘落子音效独立线程
        Thread ChessBoardSound_t = new Thread(new ThreadStart(ThreadMethod));

        // 判断是否有来自ChessBoard点击事件的落子信息
        public static bool isChessDrop = false;

        internal static DeveloperInfoForm DeveloperInfoForm { get => developerInfoForm; set => developerInfoForm = value; }

        // 线程方法
        public static void ThreadMethod()
        {
            while(true)
            {
                if (isChessDrop == true)
                {
                    // 新建一个用于播放落子音效的播放器
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.ChessDrop);
                    player.Play();
                    // 手动销毁对象
                    player = null;
                    isChessDrop = false;
                }
                else
                {
                    // 防止线程过度调用
                    Thread.Sleep(100);
                }
            }
        }
        #endregion

        // 让每次调用的控件都维持在原始大小，用于自适应
        private void Form1_Resize(object sender, EventArgs e)
        {
            float[] scale = (float[])Tag;
            int i = 2;

            foreach (Control ctrl in this.Controls)
            {
                ctrl.Left = (int)(Size.Width * scale[i++]);
                ctrl.Top = (int)(Size.Height * scale[i++]);
                ctrl.Width = (int)(Size.Width / (float)scale[0] * ((Size)ctrl.Tag).Width);
                ctrl.Height = (int)(Size.Height / (float)scale[1] * ((Size)ctrl.Tag).Height);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        // 为右键菜单栏关闭窗口添加提示
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChessBoards.isChessBoardDark == false)
            {
                pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.CloseButton1;
            }
            else
            {
                pictureBox1.BackgroundImage = global::求是五子棋___Gomoku_at_ZJU.Properties.Resources.CloseButton1Dark;
            }

            QuitForm QForm = new QuitForm(this.Location.X, this.Location.Y);
            Interaction.Beep();
            QForm.ShowDialog();

            if (QForm.DialogResult == DialogResult.OK)
            {
                QForm = null;
                timer2.Start();
            }
            else
            {
                e.Cancel = true;
            }
        }

        // 人工智能对战专用计时器，与主窗体共用线程
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (GameMode == Game_Mode.AVA && GameState == Game_State.GAMING)
            {
                #region 单独处理黑方的第一颗棋子
                if (isFirstBlackDrop == false)
                {
                    // 先由黑方的人工智能在棋盘中央的一个5×5的小区域内随意落子
                    Random random = new Random();
                    int FirstX = random.Next() % 5 + 5;
                    random = new Random();
                    int FirstY = (random.Next() + 3) % 5 + 5;

                    BoardCells[FirstX, FirstY] = Chess_State.BLACK;

                    RetractList.Add(new Retract(FirstX, FirstY));

                    // 全局棋子重绘
                    Chess.RefocusDrawChessBoard(ChessBoard, ChessBoard.BackgroundImage);

                    isChessDrop = true;
                    Sides = !Sides;
                    isFirstBlackDrop = true;
                }
                #endregion

                #region 处理除第一颗落子以外的AI落子
                else
                {
                    // 建立一个新的整数对类型储存后续的点位
                    ArtificialIntelligence.Pairint u = new ArtificialIntelligence.Pairint();

                    // 当前正在接受判断的玩家
                    Chess_State JudgePlayer = Chess_State.NONE;

                    // 如果是黑子
                    if (Sides)
                    {
                        u = ArtificialIntelligenceII.Result();
                        BoardCells[u.x, u.y] = Chess_State.BLACK;
                        JudgePlayer = Chess_State.BLACK;
                        RetractList.Add(new Retract(u.x, u.y));
                    }

                    // 如果是白子
                    else
                    {
                        u = ArtificialIntelligence.Result();
                        BoardCells[u.x, u.y] = Chess_State.WHITE;
                        JudgePlayer = Chess_State.WHITE;
                        RetractList.Add(new Retract(u.x, u.y));
                    }

                    // 全局棋子重绘
                    Chess.RefocusDrawChessBoard(ChessBoard, ChessBoard.BackgroundImage);
                    
                    isChessDrop = true;

                    if (GameControl.ChessJudgement(BoardCells, JudgePlayer))
                    {
                        if (JudgePlayer == Chess_State.BLACK)
                        {
                            // MessageBox.Show("黑子玩家胜利！", "游戏结束");
                            GameState = Game_State.WAITING;
                            WinnerForm WForm = new WinnerForm(1, this.Location.X, this.Location.Y);
                            WForm.ShowDialog();
                            if (WForm.DialogResult == DialogResult.OK)
                            {
                                InitializeGame();
                                WForm = null;
                            }
                        }
                        else
                        {
                            // MessageBox.Show("白子玩家胜利！", "游戏结束");
                            GameState = Game_State.WAITING;
                            WinnerForm WForm = new WinnerForm(2, this.Location.X, this.Location.Y);
                            WForm.ShowDialog();
                            if (WForm.DialogResult == DialogResult.OK)
                            {
                                InitializeGame();
                                WForm = null;
                            }
                        }
                    }

                    // 如果棋盘已经下满，则判定和棋
                    else if (RetractList.Count == BoardSize * BoardSize)
                    {
                        // MessageBox.Show("和棋！莫为输赢扰清梦，一局残棋寄深情。", "游戏结束");
                        GameState = Game_State.WAITING;
                        WinnerForm WForm = new WinnerForm(3, this.Location.X, this.Location.Y);
                        WForm.ShowDialog();
                        if (WForm.DialogResult == DialogResult.OK)
                        {
                            InitializeGame();
                            WForm = null;
                        }
                    }

                    else
                    {
                        Sides = !Sides;
                        setSidePicture(Sides);
                    }
                }
                #endregion
            }
        }
    }
}