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
            // Commons
            string continueflg = string.Empty;
            string redoflg = string.Empty;
            int roundcount = 0;
            int[] choicecount = new int[3];
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
                string initialise = string.Empty;
                for (int i = 0; i < pmax + cmax; i++)
                {
                    initialise += ",";
                }

                List<string> empty = new List<string> { initialise + Environment.NewLine };
                ContentsFileIO.Write(empty);
                continueflg = "N";

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
                        string initialise = string.Empty;
                        for (int i = 0; i < pmax + cmax; i++)
                        {
                            initialise += ",";
                        }

                        string dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                        System.IO.File.Move(@"Data\rates.csv", @"Data\rates_" + dt + ".csv"); // バックアップを取る
                        List<string> empty = new List<string> { initialise + Environment.NewLine };
                        ContentsFileIO.Write(empty);

                        rates = ContentsFileIO.Read();
                        r = rates[0].Split(',');

                        Console.WriteLine();
                    }

                    Array.Copy(r, output, r.Length); // 書き込みに使用するコピーを作成する

                    // 空白セーブ（Rounds = 0）を読み込んだ場合
                    if (r[r.Length - 1] == string.Empty)
                    {
                        continueflg = "N";
                    }
                    else
                    {
                        // 画面の表示
                        Console.WriteLine("前回結果：");

                        // 総ラウンド数
                        Console.WriteLine("総ラウンド数：{0}", r[r.Length - 1]);

                        for (int i = 0; i < pmax + cmax; i++)
                        {
                            // Player
                            if (i < pmax)
                            {
                                if (r[i] != string.Empty)
                                {
                                    Console.WriteLine("プレイヤー{0}：{1}/{2} ({3}%)", i + 1, r[i], r[r.Length - 1], Math.Round(Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1]) * 100));
                                }
                            }

                            // CPU
                            else
                            {
                                if (r[i] != string.Empty)
                                {
                                    Console.WriteLine("コンピューター{0}：{1}/{2} ({3}%)", i - pmax + 1, r[i], r[r.Length - 1], Math.Round(Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1]) * 100));
                                }
                            }
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
            // 続きから
            if (continueflg == "Y" || continueflg == "y")
            {
                if (rates[0].IndexOf(',') != -1)
                {
                    string[] r = rates[0].Split(',');
                    for (int i = 0; i < pmax + cmax; i++)
                    {
                        if (r[i] != string.Empty)
                        {
                            if (i < pmax)
                            {
                                pnum++;
                            }
                            else
                            {
                                cnum++;
                            }
                        }
                    }
                }
            }
            else // 初めから
            {
                // 前回のデータを消す
                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = "0";
                }

                // Player
                Console.WriteLine("プレイヤーは何人ですか？");
                do
                {
                    try
                    {
                        Console.Write("1～{0}の数字を入力してください。＞", pmax);
                        pnum = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.Write("入力が正しくありません。");
                    }
                } while (pnum < 1 || pnum > pmax);

                Console.WriteLine();

                // CPU
                Console.WriteLine("コンピューターは何人ですか？");
                do
                {
                    try
                    {
                        Console.Write("1～{0}の数字を入力してください。＞", cmax);
                        cnum = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.Write("入力が正しくありません。");
                    }
                } while (cnum < 1 || cnum > cmax);
            }

            // インスタンス生成
            Player[] p = new Player[pnum];
            Computer[] c = new Computer[cnum];
            for (int i = 0; i < pnum; i++)
            {
                p[i] = new Player();
            }

            for (int i = 0; i < cnum; i++)
            {
                c[i] = new Computer();
            }

            Console.WriteLine("\n　★　☆　★　じゃんけんへようこそ　★　☆　★　\n");

            // "N"を入力するまで続く
            do
            {
                // 勝負が決まるまで続く
                do
                {
                    // 初期値に戻す
                    for (int i = 0; i < pnum; i++)
                    {
                        p[i].Reset();
                    }

                    for (int i = 0; i < cnum; i++)
                    {
                        c[i].Reset();
                    }

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

                    for (int i = 0; i < pnum; i++)
                    {
                        // 0:グー、1:チョキ、2:パー
                        switch (p[i].Choice)
                        {
                            case 0:
                                Console.WriteLine("プレイヤー{0}がグーを出しました。", i + 1);
                                break;
                            case 1:
                                Console.WriteLine("プレイヤー{0}がチョキを出しました。", i + 1);
                                break;
                            case 2:
                                Console.WriteLine("プレイヤー{0}がパーを出しました。", i + 1);
                                break;
                        }
                    }

                    // コンピューターの出力
                    for (int i = 0; i < cnum; i++)
                    {
                        c[i].Shuffle(random);

                        // 0:グー、1:チョキ、2:パー
                        switch (c[i].Choice)
                        {
                            case 0:
                                Console.WriteLine("コンピューター{0}がグーを出しました。", i + 1);
                                break;
                            case 1:
                                Console.WriteLine("コンピューター{0}がチョキを出しました。", i + 1);
                                break;
                            case 2:
                                Console.WriteLine("コンピューター{0}がパーを出しました。", i + 1);
                                break;
                        }

                        System.Threading.Thread.Sleep(15);
                    }

                    Console.WriteLine();

                    // グーチョキパーを数える
                    for (int i = 0; i < choicecount.Length; i++)
                    {
                        choicecount[i] = 0;
                    }

                    // Player
                    for (int i = 0; i < pnum; i++)
                    {
                        switch (p[i].Choice)
                        {
                            case 0:
                                choicecount[0]++;
                                break;
                            case 1:
                                choicecount[1]++;
                                break;
                            case 2:
                                choicecount[2]++;
                                break;
                        }
                    }

                    // CPU
                    for (int i = 0; i < cnum; i++)
                    {
                        switch (c[i].Choice)
                        {
                            case 0:
                                choicecount[0]++;
                                break;
                            case 1:
                                choicecount[1]++;
                                break;
                            case 2:
                                choicecount[2]++;
                                break;
                        }
                    }

                    // あいこ判定
                    zerocount = 0;

                    for (int i = 0; i < choicecount.Length; i++)
                    {
                        if (choicecount[i] == 0)
                        {
                            zerocount++;
                        }
                    }

                    if (zerocount == 0 || zerocount == choicecount.Length - 1)
                    {
                        Console.WriteLine("あいこです。もう一度勝負します。\n");
                    }
                } while (zerocount == 0 || zerocount == choicecount.Length - 1); // 勝負が決まるまで続く（全種類出しても一種類だけ出してもあいこ）

                // パーが0でない→グーvsチョキでない
                if (choicecount[choicecount.Length - 1] != 0)
                {
                    for (int j = 0; j < choicecount.Length; j++)
                    {
                        // j = 0: チョキvsパー、j = 1: グーvsパー
                        if (choicecount[j] == 0)
                        {
                            for (int i = 0; i < pnum; i++)
                            {
                                if (p[i].Choice == j + 1)
                                {
                                    // (0)グーなしであれば(1)チョキが勝つ、(1)チョキなしであれば(2)パーが勝つ
                                    Console.Write("プレイヤー{0}、", i + 1);
                                    p[i].Score++;
                                }
                            }

                            for (int i = 0; i < cnum; i++)
                            {
                                if (c[i].Choice == j + 1)
                                {
                                    Console.Write("コンピューター{0}、", i + 1);
                                    c[i].Score++;
                                }
                            }

                            Console.WriteLine("の勝ちです。\n");
                        }
                    }
                }
                else // グーvsチョキ（両端のケース）
                {
                    for (int i = 0; i < pnum; i++)
                    {
                        if (p[i].Choice == 0)
                        {
                            Console.Write("プレイヤー{0}、", i + 1);
                            p[i].Score++;
                        }
                    }

                    for (int i = 0; i < cnum; i++)
                    {
                        if (c[i].Choice == 0)
                        {
                            Console.Write("コンピューター{0}、", i + 1);
                            c[i].Score++;
                        }
                    }

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
                {
                    finalscore[i] = Convert.ToInt32(output[i]) + p[i].Score;
                }
                else if (i >= pmax && i < pmax + cnum)
                {
                    finalscore[i] = Convert.ToInt32(output[i]) + c[i - pmax].Score;
                }
                else if (i == finalscore.Length - 1)
                {
                    finalscore[i] = Convert.ToInt32(output[i]) + roundcount;
                }
            }

            // 結果報告
            Console.WriteLine("総ラウンド数：{0}", finalscore[finalscore.Length - 1]);

            for (int i = 0; i < pnum; i++)
            {
                Console.WriteLine("プレイヤー{0}：{1}/{2} ({3}%)", i + 1, finalscore[i], finalscore[finalscore.Length - 1], Math.Round(Convert.ToDouble(finalscore[i]) / Convert.ToDouble(finalscore[finalscore.Length - 1]) * 100));
            }

            for (int i = 0; i < cnum; i++)
            {
                Console.WriteLine("コンピューター{0}：{1}/{2} ({3}%)", i + 1, finalscore[i + pmax], finalscore[finalscore.Length - 1], Math.Round(Convert.ToDouble(finalscore[i + pmax]) / Convert.ToDouble(finalscore[finalscore.Length - 1]) * 100));
            }

            // 書き込み準備
            for (int i = 0; i < output.Length; i++)
            {
                // Player
                if (i < pmax)
                {
                    if (i < pnum)
                    {
                        output[i] = finalscore[i].ToString();
                    }
                    else
                    {
                        output[i] = string.Empty;
                    }
                }
                // CPU
                else if (i < pmax + cmax)
                {
                    if (i - pmax < cnum)
                    {
                        output[i] = finalscore[i].ToString();
                    }
                    else
                    {
                        output[i] = string.Empty;
                    }
                }
                // 総ラウンド数
                else
                {
                    output[i] = finalscore[i].ToString();
                }
            }

            // 総ラウンド数(+1)を含めるStringの生成
            string s = string.Empty;

            for (int i = 0; i < (pmax + cmax) + 1; i++)
            {
                if (i == 0)
                {
                    s = output[i];
                }
                else
                {
                    s += ',' + output[i];
                }
            }

            rates.Clear();
            rates.Add(s + Environment.NewLine);
            ContentsFileIO.Write(rates);

            Console.WriteLine("終了します。お疲れ様でした。");
            Console.Read();
        }
    }
}
