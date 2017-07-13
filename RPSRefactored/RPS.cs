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
    internal class RPS : Game
    {
        // Fields
        Random random = new Random();

        // Constructors
        public RPS(int pnum, int cnum)
            : base(pnum, cnum)
        {
            Totalcount = 0;
            IsRedo = false;
            choicecount = new int[3] { 0, 0, 0 };
        }

        // Properties
        public int Totalcount { get; set; }
        public int[] choicecount { get; set; }
        public bool IsRedo { get; set; }

        // Methods
        //////////////////////// 大メソッド ////////////////////////
        // じゃんけんメイン　→　ラウンド数を出力
        public void RPSMain()
        {
            Console.WriteLine("\n　★　☆　★　じゃんけんへようこそ　★　☆　★　\n");

            do // "N"を入力するまで続く
            {
                do // 勝負が決まるまで続く
                {
                    Reset();
                    PlayerInput();
                    ComputerShuffle();
                    AnnouncementAndCount(p, Pnum);
                    AnnouncementAndCount(c, Cnum);

                } while (IsDraw()); 

                Judgement();
                IsRedo = ConsoleIO.YesNoQ("もう一度勝負しますか？（Y/N）＞");

            } while (IsRedo);
        }

        //////////////////////// 小メソッド ////////////////////////
        // リセット
        private void Reset()
        {
            IsRedo = false;
            for (int i = 0; i < Pnum; i++)
                p[i].Reset();
            for (int i = 0; i < Cnum; i++)
                c[i].Reset();
            for (int i = 0; i < choicecount.Length; i++)
                choicecount[i] = 0;
        }

        // プレイヤー入力
        private void PlayerInput()
        {
            for (int i = 0; i < Pnum; i++)
            {
                do
                {
                    Console.Write("プレイヤー{0}、入力してください。0:グー、1:チョキ、2:パー＞", i + 1);
                    p[i].Choice = ConsoleIO.ParseOrDefault(Console.ReadLine());
                } while (!(p[i].Choice >= 0 && p[i].Choice <= 2)); // 正しく入力するまで続く
            }
            Console.WriteLine();
        }

        // コンピューターシャッフル
        private void ComputerShuffle()
        {
            for (int i = 0; i < Cnum; i++)
            {
                c[i].Shuffle(random);
                System.Threading.Thread.Sleep(15);
            }
        }

        // ラウンドごとの報告、カウント（0:グー、1:チョキ、2:パー）
        private void AnnouncementAndCount(Entity[] e, int num)
        {
            for (int i = 0; i < num; i++)
            {
                switch (e[i].Choice)
                {
                    case 0:
                        Console.WriteLine("{0}{1}がグーを出しました。", e[i].Name, i + 1);
                        choicecount[0]++;
                        break;
                    case 1:
                        Console.WriteLine("{0}{1}がチョキを出しました。", e[i].Name, i + 1);
                        choicecount[1]++;
                        break;
                    case 2:
                        Console.WriteLine("{0}{1}がパーを出しました。", e[i].Name, i + 1);
                        choicecount[2]++;
                        break;
                }
            }
        }

        // 引き分け判定
        private bool IsDraw()
        {
            Console.WriteLine();
            int zerocount = 0;
            for (int i = 0; i < choicecount.Length; i++)
                if (choicecount[i] == 0)
                    zerocount++;

            if (zerocount != 1) // グーチョキパーどれか一つが0じゃないとあいこ
            {
                Console.WriteLine("あいこです。もう一度勝負します。\n");
                return true;
            }
            else return false;
        }

        // 勝負判定
        private void Judgement()
        {
            if (choicecount[choicecount.Length - 1] == 0) // パーが0　→　グーvsチョキ、(0)グーが勝つ
            {
                ScoringJudgement(0);
                Console.WriteLine("の勝ちです。\n");
            }
            else  // グーが0 or チョキが0 → チョキvsパー、(0+1 = 1)チョキが勝つ; グーvsパー、(1+1 = 2)パーが勝つ
            {
                for (int j = 0; j < choicecount.Length - 1; j++)
                    if (choicecount[j] == 0)
                        ScoringJudgement(j + 1);
                Console.WriteLine("の勝ちです。\n");
            }

            Totalcount++; // 勝負がついたのでカウント
        }

        // 点数の引き上げ
        private void ScoringJudgement(int winningchoice)
        {
            for (int i = 0; i < Pnum; i++)
                p[i].Scoring(winningchoice, i);
            for (int i = 0; i < Cnum; i++)
                c[i].Scoring(winningchoice, i);
        }
    }
}
