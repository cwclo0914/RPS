using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    internal class RPS : Game
    {
        // Constructors
        public RPS(int pnum, int cnum)
            :base(pnum,cnum)
        {
            Totalcount = 0;
        }

        // Properties
        public int Totalcount { get; set; }

        // Methods
        //////////////////////// 大メソッド ////////////////////////
        // じゃんけんメイン　→　ラウンド数を出力
        public void RPSMain()
        {
            Random random = new Random();

            int zerocount = 0;
            int[] choicecount = new int[3] { 0, 0, 0 };

            Console.WriteLine("\n　★　☆　★　じゃんけんへようこそ　★　☆　★　\n");
            char redoflg = '0';

            // "N"を入力するまで続く
            do
            {
                redoflg = '0';
                // 勝負が決まるまで続く
                do
                {
                    // 初期値に戻す
                    for (int i = 0; i < Pnum; i++)
                        p[i].Reset();
                    for (int i = 0; i < Cnum; i++)
                        c[i].Reset();

                    for (int i = 0; i < Pnum; i++) // 正しく入力するまで続く
                    {
                        do
                        {
                            try
                            {
                                Console.Write("プレイヤー{0}、入力してください。0:グー、1:チョキ、2:パー＞", i + 1);
                                p[i].Choice = int.Parse(Console.ReadLine());
                            }
                            catch (Exception) { }
                        } while (!(p[i].Choice >= 0 && p[i].Choice <= 2)); // 正しく入力するまで続く
                    }

                    Console.WriteLine();

                    // コンピューターのシャッフル
                    for (int i = 0; i < Cnum; i++)
                    {
                        c[i].Shuffle(random);
                        System.Threading.Thread.Sleep(15);
                    }

                    // グーチョキパーを数える準備
                    for (int i = 0; i < choicecount.Length; i++)
                        choicecount[i] = 0;

                    // 結果の出力
                    AnnouncementAndCount(Pnum, p, ref choicecount);
                    AnnouncementAndCount(Cnum, c, ref choicecount);

                    Console.WriteLine();

                    // あいこ判定
                    zerocount = 0;

                    for (int i = 0; i < choicecount.Length; i++)
                        if (choicecount[i] == 0)
                            zerocount++;

                    if (zerocount != 1) // グーチョキパーどれか一つが0じゃないとあいこ
                        Console.WriteLine("あいこです。もう一度勝負します。\n");

                } while (zerocount != 1); // 勝負が決まるまで続く

                // 勝負判定
                if (choicecount[choicecount.Length - 1] == 0) // パーが0　→　グーvsチョキ、(0)グーが勝つ
                {
                    for (int i = 0; i < Pnum; i++)
                        p[i].Scoring(0, i);
                    for (int i = 0; i < Cnum; i++)
                        c[i].Scoring(0, i);
                    Console.WriteLine("の勝ちです。\n");
                }
                else  // グーが0 or チョキが0 → チョキvsパー、(0+1 = 1)チョキが勝つ; グーvsパー、(1+1 = 2)パーが勝つ
                {
                    for (int j = 0; j < choicecount.Length - 1; j++)
                    {
                        if (choicecount[j] == 0)
                        {
                            for (int i = 0; i < Pnum; i++)
                                p[i].Scoring(j + 1, i);
                            for (int i = 0; i < Cnum; i++)
                                c[i].Scoring(j + 1, i);
                        }
                    }
                    Console.WriteLine("の勝ちです。\n");
                }

                Totalcount++; // 1ラウンドの終わりにカウント

                redoflg = ConsoleIO.YesNoQ("もう一度勝負しますか？（Y/N）＞", redoflg);
            } while (redoflg == 'Y' || redoflg == 'y'); // "N"を入力するまで続く
        }

        // ラウンドごとの報告、カウント（0:グー、1:チョキ、2:パー）
        private void AnnouncementAndCount(int max, Entity[] entity, ref int[] choicecount)
        {
            for (int i = 0; i < max; i++)
            {
                switch (entity[i].Choice)
                {
                    case 0:
                        Console.WriteLine("{0}{1}がグーを出しました。", entity[i].Name, i + 1);
                        choicecount[0]++;
                        break;
                    case 1:
                        Console.WriteLine("{0}{1}がチョキを出しました。", entity[i].Name, i + 1);
                        choicecount[1]++;
                        break;
                    case 2:
                        Console.WriteLine("{0}{1}がパーを出しました。", entity[i].Name, i + 1);
                        choicecount[2]++;
                        break;
                }
            }
        }
    }
}
