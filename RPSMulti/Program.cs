using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSMulti
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Common
            string continueflg = string.Empty;
            string redoflg = string.Empty;
            int roundcount = 0;
            int[] choicecount = new int[3] { 0, 0, 0 };
            int zerocount = 0;
            const int pmax = 3;
            const int cmax = 5;

            int pnum = 0;
            int cnum = 0;

            List<string> rates = new List<string>(); // 入力用
            string[] output = new string[pmax + cmax + 1]; // 出力用

            Random random = new Random();

            // 前回勝率の表示
            if (File.Exists(@"Data\rates.csv") == false)
            {
                Console.WriteLine("新しいセーブデータを作成します。");

                SaveInitialise(pmax + cmax);

                Console.WriteLine();
            }
            else
            {
                rates = ContentsFileIO.Read();

                if (rates[0].IndexOf(',') != -1)
                {
                    string[] r = rates[0].Split(',');

                    // 最大人数変更後のセーブデータ初期化
                    if (r.Length != pmax + cmax + 1)
                    {
                        Console.WriteLine("セーブデータが使用できません。\n現在のセーブデータをバックアップし、初期化します。");
                        ContentsFileIO.BackUp();

                        SaveInitialise(pmax + cmax);

                        rates = ContentsFileIO.Read();
                        r = rates[0].Split(',');

                        Console.WriteLine();
                    }

                    Array.Copy(r, output, r.Length); // 書き込みに使用するコピーを作成する

                    // 空白セーブ（Rounds = 0）を読み込んだ場合
                    if (r[r.Length - 1] == string.Empty)
                        continueflg = "N";
                    else
                    {
                        // 画面の表示
                        Console.WriteLine("前回結果：");

                        // 総ラウンド数
                        Console.WriteLine("総ラウンド数：{0}", r[r.Length - 1]);

                        for (int i = 0; i < pmax + cmax; i++)
                        {
                            if (r[i] != string.Empty)
                                // Player
                                if (i < pmax)
                                    Console.WriteLine("プレイヤー{0}：{1}/{2} ({3}%)", i + 1, r[i], r[r.Length - 1], Math.Round(Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1]) * 100, 1));
                                // CPU
                                else
                                    Console.WriteLine("コンピューター{0}：{1}/{2} ({3}%)", i - pmax + 1, r[i], r[r.Length - 1], Math.Round(Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1]) * 100, 1));
                        }

                        Console.WriteLine();
                    }
                }
            }

            while (!(continueflg == "Y" || continueflg == "y" || continueflg == "N" || continueflg == "n"))
            {
                Console.WriteLine("続きから始めますか？");
                Console.Write("Y/Nを入力してください。＞");
                continueflg = Console.ReadLine();
                Console.WriteLine();
            }

            // 人数定義
            if (continueflg == "Y" || continueflg == "y") // 続きから
            {
                if (rates[0].IndexOf(',') != -1)
                {
                    string[] r = rates[0].Split(',');
                    for (int i = 0; i < pmax + cmax; i++)
                    {
                        if (r[i] != string.Empty)
                        {
                            if (i < pmax)
                                pnum++;
                            else
                                cnum++;
                        }
                    }
                }
            }
            else // 初めから
            {
                // 前回のデータを消す
                for (int i = 0; i < output.Length; i++)
                    output[i] = "0";

                // 人数確認
                NumberConfirmation("プレイヤー", pmax, ref pnum); // Player
                NumberConfirmation("コンピューター", cmax, ref cnum); // CPU
            }

            // インスタンス生成
            Player[] p = new Player[pnum];
            Player[] c = new Computer[cnum];

            for (int i = 0; i < pnum; i++)
                p[i] = new Player();

            for (int i = 0; i < cnum; i++)
                c[i] = new Computer();

            Console.WriteLine("\n　★　☆　★　じゃんけんへようこそ　★　☆　★　\n");

            // "N"を入力するまで続く
            do
            {
                // 勝負が決まるまで続く
                do
                {
                    // 初期値に戻す
                    for (int i = 0; i < pnum; i++)
                        p[i].Reset();

                    for (int i = 0; i < cnum; i++)
                        c[i].Reset();

                    for (int i = 0; i < pnum; i++)
                    {
                        // 正しく入力するまで続く
                        do
                        {
                            try
                            {
                                Console.Write("プレイヤー{0}、入力してください。0:グー、1:チョキ、2:パー＞", i + 1);
                                p[i].Choice = int.Parse(Console.ReadLine());
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        } while (!(p[i].Choice >= 0 && p[i].Choice <= 2)); // 正しく入力するまで続く
                    }

                    Console.WriteLine();

                    // コンピューターのシャッフル
                    for (int i = 0; i < cnum; i++)
                    {
                        c[i].Shuffle(random);
                        System.Threading.Thread.Sleep(15);
                    }

                    // グーチョキパーを数える準備
                    for (int i = 0; i < choicecount.Length; i++)
                        choicecount[i] = 0;

                    // 結果の出力
                    AnnouncementAndCount("プレイヤー", pnum, p, ref choicecount);
                    AnnouncementAndCount("コンピューター", cnum, c, ref choicecount);

                    Console.WriteLine();

                    // あいこ判定
                    zerocount = 0;

                    for (int i = 0; i < choicecount.Length; i++)
                    {
                        if (choicecount[i] == 0)
                            zerocount++;
                    }

                    if (zerocount == 0 || zerocount == choicecount.Length - 1) // choicecount.Length - 1 = 2(パー)
                        Console.WriteLine("あいこです。もう一度勝負します。\n");
                } while (zerocount == 0 || zerocount == choicecount.Length - 1); // 勝負が決まるまで続く（全種類出しても一種類だけ出してもあいこ）


                if (choicecount[choicecount.Length - 1] != 0) // パーが存在する → グーvsチョキでない
                {
                    for (int j = 0; j < choicecount.Length - 1; j++) // j = 0: チョキvsパー、j = 1: グーvsパー
                    {
                        // (0)グーなしであれば+1の(1)チョキが勝つ、(1)チョキなしであれば+1の(2)パーが勝つ
                        if (choicecount[j] == 0)
                        {
                            Judgement("プレイヤー", pnum, j + 1, ref p);
                            Judgement("コンピューター", cnum, j + 1, ref c);

                            Console.WriteLine("の勝ちです。\n");
                        }
                    }
                }
                else // グーvsチョキ（両端のケース）、(0)グーが勝つ
                {
                    Judgement("プレイヤー", pnum, 0, ref p);
                    Judgement("コンピューター", cnum, 0, ref c);

                    Console.WriteLine("の勝ちです。\n");
                }

                roundcount++; // 1ラウンドの終わりにカウント

                do
                {
                    Console.Write("もう一度勝負しますか？（Y/N）＞");
                    redoflg = Console.ReadLine();
                    Console.WriteLine();
                } while (!(redoflg == "Y" || redoflg == "y" || redoflg == "N" || redoflg == "n")); // "Y/N"以外を弾く

            } while (redoflg == "Y" || redoflg == "y"); // "N"を入力するまで続く

            // 続きからであれば点数を足していく
            int[] finalscore = new int[(pmax + cmax) + 1];

            for (int i = 0; i < finalscore.Length; i++)
            {
                if (i < pnum)
                    finalscore[i] = Convert.ToInt32(output[i]) + p[i].Score;
                else if (i >= pmax && i < pmax + cnum)
                    finalscore[i] = Convert.ToInt32(output[i]) + c[i - pmax].Score;
                else if (i == finalscore.Length - 1)
                    finalscore[i] = Convert.ToInt32(output[i]) + roundcount;
            }

            // 結果報告
            Console.WriteLine("総ラウンド数：{0}", finalscore[finalscore.Length - 1]);

            for (int i = 0; i < pnum; i++)
                Console.WriteLine("プレイヤー{0}：{1}/{2} ({3}%)", i + 1, finalscore[i], finalscore[finalscore.Length - 1], Math.Round(Convert.ToDouble(finalscore[i]) / Convert.ToDouble(finalscore[finalscore.Length - 1]) * 100, 1));

            for (int i = 0; i < cnum; i++)
                Console.WriteLine("コンピューター{0}：{1}/{2} ({3}%)", i + 1, finalscore[i + pmax], finalscore[finalscore.Length - 1], Math.Round(Convert.ToDouble(finalscore[i + pmax]) / Convert.ToDouble(finalscore[finalscore.Length - 1]) * 100, 1));

            // 書き込み準備
            for (int i = 0; i < output.Length; i++)
            {
                // Player
                if (i < pmax)
                {
                    if (i < pnum)
                        output[i] = finalscore[i].ToString();
                    else
                        output[i] = string.Empty;
                }
                // CPU
                else if (i < pmax + cmax)
                {
                    if (i - pmax < cnum)
                        output[i] = finalscore[i].ToString();
                    else
                        output[i] = string.Empty;
                }
                // 総ラウンド数
                else
                    output[i] = finalscore[i].ToString();
            }

            // 総ラウンド数(+1)を含めるStringの生成
            string s = string.Empty;

            for (int i = 0; i < (pmax + cmax) + 1; i++)
            {
                if (i == 0)
                    s = output[i];
                else
                    s += ',' + output[i];
            }

            rates.Clear();
            rates.Add(s + Environment.NewLine);
            ContentsFileIO.Write(rates);

            Console.WriteLine("終了します。お疲れ様でした。");
            Console.Read();
        }

        // セーブデータ初期化
        private static void SaveInitialise(int max)
        {
            string initialise = string.Empty;
            for (int i = 0; i < max; i++)
                initialise += ",";

            List<string> empty = new List<string> { initialise + Environment.NewLine };
            ContentsFileIO.Write(empty);
        }

        // 人数確認
        private static void NumberConfirmation(string name, int max, ref int num)
        {
            Console.WriteLine("{0}は何人ですか？", name);

            do
            {
                try
                {
                    Console.Write("1～{0}の数字を入力してください。＞", max);
                    num = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Write("入力が正しくありません。");
                }
            } while (num < 1 || num > max);

            Console.WriteLine();
        }

        // 0:グー、1:チョキ、2:パー
        private static void AnnouncementAndCount(string name, int max, Player[] entity, ref int[] choicecount)
        {
            for (int i = 0; i < max; i++)
            {
                switch (entity[i].Choice)
                {
                    case 0:
                        Console.WriteLine("{0}{1}がグーを出しました。", name, i + 1);
                        choicecount[0]++;
                        break;
                    case 1:
                        Console.WriteLine("{0}{1}がチョキを出しました。", name, i + 1);
                        choicecount[1]++;
                        break;
                    case 2:
                        Console.WriteLine("{0}{1}がパーを出しました。", name, i + 1);
                        choicecount[2]++;
                        break;
                }
            }
        }

        // スコア増加の判定（Scoreがプロパティであるためreturnによって回避）
        private static void Judgement(string name, int max, int winningchoice, ref Player[] entity)
        {
            for (int i = 0; i < max; i++)
            {
                if (entity[i].Choice == winningchoice)
                {
                    Console.Write("{0}{1}、", name, i + 1);
                    entity[i].Score++;
                }
            }
        }
    }
}
