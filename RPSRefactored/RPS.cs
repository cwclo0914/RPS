using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// じゃんけんゲームの本体
    /// </summary>
    internal class RPS : GameData
    {
        // Fields
        private static RPS _instance;
        Random random = new Random();
        private static object syncLock = new object();

        // Constructors
        private RPS(int pnum, int cnum)
            : base(pnum, cnum)
        {
            ChoiceCount = new int[3] { 0, 0, 0 };
        }

        // Properties
        public int[] ChoiceCount { get; set; }

        // Methods
        // Singleton
        public static RPS Instance(int pnum, int cnum)
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new RPS(pnum, cnum);
                    }
                }
            }
            return _instance;
        }

        public static void InstanceReset()
        {
            _instance = null;
        }
        //////////////////////// 大メソッド ////////////////////////
        /// <summary>
        /// じゃんけんメイン　→　ラウンド数を出力
        /// </summary>
        public override void Main()
        {
            Console.WriteLine("　★　☆　★　じゃんけんへようこそ　★　☆　★　\n");

            do // "N"を入力するまで続く
            {
                do // 勝負が決まるまで続く
                {
                    Reset();
                    PlayerInput();
                    ComputerShuffle();
                    AnnouncementAndCount(player_list);
                    AnnouncementAndCount(computer_list);
                    Console.WriteLine();
                } while (IsDraw());
                Judge();
            } while (ConsoleIO.YesNoQ("もう一度勝負しますか？（Y/N）＞"));
        }

        //////////////////////// 小メソッド ////////////////////////
        // リセット
        private void Reset()
        {
            foreach (Player player in player_list)
                player.Reset();
            foreach (Computer computer in computer_list)
                computer.Reset();
            for (int i = 0; i < ChoiceCount.Length; i++)
                ChoiceCount[i] = 0;
        }

        // プレイヤー入力
        private void PlayerInput()
        {
            foreach (Player player in player_list)
            {
                do
                {
                    Console.Write("プレイヤー{0}、入力してください。0:グー、1:チョキ、2:パー；9:現状報告＞", player_list.IndexOf(player) + 1);
                    int temp = -1;
                    try
                    {
                        temp = int.Parse(Console.ReadLine());
                    }
                    catch (Exception) { }

                    if (temp == 9)
                        ReportCurrentScore();
                    else
                        player.Choice = temp;

                } while (!(player.Choice >= 0 && player.Choice <= 2)); // 正しく入力するまで続く
            }
            Console.WriteLine();
        }

        // コンピューターシャッフル
        private void ComputerShuffle()
        {
            foreach (Computer computer in computer_list)
            {
                computer.Shuffle(random);
                System.Threading.Thread.Sleep(15);
            }
        }

        /// <summary>
        /// ラウンドごとの報告とカウント（0:グー、1:チョキ、2:パー）
        /// </summary>
        /// <param name="list"></param>
        private void AnnouncementAndCount(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                switch (entity.Choice)
                {
                    case 0:
                        Console.WriteLine("{0}{1}がグーを出しました。", entity.Name, list.IndexOf(entity) + 1);
                        ChoiceCount[0]++;
                        break;
                    case 1:
                        Console.WriteLine("{0}{1}がチョキを出しました。", entity.Name, list.IndexOf(entity) + 1);
                        ChoiceCount[1]++;
                        break;
                    case 2:
                        Console.WriteLine("{0}{1}がパーを出しました。", entity.Name, list.IndexOf(entity) + 1);
                        ChoiceCount[2]++;
                        break;
                }
            }
        }

        /// <summary>
        /// 引き分け判定
        /// </summary>
        /// <returns>true: 引き分け false: 勝負がつく</returns>
        private bool IsDraw()
        {
            int zerocount = 0;
            for (int i = 0; i < ChoiceCount.Length; i++)
                if (ChoiceCount[i] == 0)
                    zerocount++;

            if (zerocount != 1) // グーチョキパーどれか一つだけが0じゃないとあいこ
            {
                Console.WriteLine("あいこです。もう一度勝負します。\n");
                return true;
            }
            else return false;
        }

        // 勝負判定
        private void Judge()
        {
            if (ChoiceCount[0] == 0) // グーが0  → チョキvsパー、(1)チョキが勝つ
                ScoringJudgement(1);
            else if (ChoiceCount[1] == 0) // チョキが0　→　グーvsパー、(2)パーが勝つ
                ScoringJudgement(2);
            else if (ChoiceCount[2] == 0) // パーが0　→　グーvsチョキ、(0)グーが勝つ
                ScoringJudgement(0);

            Console.WriteLine("の勝ちです。\n");
            TotalCount++; // 勝負がついたのでカウント
        }

        // 各Entityの勝敗を決める
        private void ScoringJudgement(int winningchoice)
        {
            foreach (Player player in player_list)
                player.Scoring(winningchoice, player_list.IndexOf(player));
            foreach (Computer computer in computer_list)
                computer.Scoring(winningchoice, computer_list.IndexOf(computer));
        }

        // 現在の点数を報告し、必要であれば返せる
        public override string[] ReportCurrentScore()
        {
            string[] inter = new string[Settings.PMAX + Settings.CMAX + 1];
            for (int i = 0; i < inter.Length; i++)
            {
                if (i < Pnum)
                    inter[i] = player_list[i].Score.ToString();
                else if (i < Settings.PMAX)
                    inter[i] = string.Empty;
                else if (i >= Settings.PMAX && i < Settings.PMAX + Cnum)
                    inter[i] = computer_list[i - Settings.PMAX].Score.ToString();
                else if (i < Settings.PMAX + Settings.CMAX)
                    inter[i] = string.Empty;
                else
                    inter[i] = TotalCount.ToString();
            }
            ConsoleIO.Result(inter);
            return inter;
        }
    }
}
