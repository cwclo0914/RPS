using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSMulti
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string continueflg = string.Empty;
            string redoflg = string.Empty;
            int roundcount = 0;
            int[] choicecount = new int[3];
            int zerocount = 0;
            const int maxplayer = 3;

            int pnum = 0;
            int cnum = 0;

            string[] output = new string[(maxplayer * 2) + 1];

            // 前回勝率の読込
            List<string> rates = ContentsFileIO.Read();

            if (rates[0].IndexOf(',') != -1)
            {
                string[] r = rates[0].Split(',');
                Array.Copy(r, output, r.Length); // 書き込みに使用するコピーを作成する

                // 書き込む
                Console.WriteLine("前回結果：");

                // 総ラウンド数
                Console.WriteLine("総ラウンド数：{0}", r[r.Length - 1]);
                if (r[r.Length - 1] == string.Empty)
                {
                    continueflg = "N";
                }

                // Player
                for (int i = 0; i < maxplayer; i++)
                {
                    if (r[i] != string.Empty)
                    {
                        Console.WriteLine("プレーヤー{0}：{1}", i, (Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1])) * 100);
                    }
                    else
                    {
                        Console.WriteLine("プレーヤー{0}：参加せず", i);
                    }
                }

                // CPU
                for (int i = maxplayer; i < maxplayer * 2; i++)
                {
                    if (r[i] != string.Empty)
                    {
                        Console.WriteLine("コンピューター{0}：{1}", i - maxplayer + 1, (Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1])) * 100);
                    }
                    else
                    {
                        Console.WriteLine("コンピューター{0}：参加せず", i - maxplayer + 1);
                    }
                }
            }

            Console.WriteLine();

            while (!(continueflg == "Y" || continueflg == "y" || continueflg == "N" || continueflg == "n"))
            {
                Console.WriteLine("続きから始めますか？");
                Console.Write("Y/Nを入力してください。");
                continueflg = Console.ReadLine();
            }

            // 人数定義
            // 続きから
            if (continueflg == "Y" || continueflg == "y")
            {
                if (rates[0].IndexOf(',') != -1)
                {
                    string[] r = rates[0].Split(',');
                    for (int i = 0; i < maxplayer; i++)
                    {
                        if (r[i] != string.Empty)
                        {
                            pnum++;
                        }
                    }
                    for (int i = maxplayer; i < maxplayer * 2; i++)
                    {
                        if (r[i] != string.Empty)
                        {
                            cnum++;
                        }
                    }
                }
            }
            // 初めから
            else
            {
                // 前回のデータを消す
                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = string.Empty;
                }

                // Player
                Console.WriteLine("プレーヤーは何人ですか？");
                do
                {
                    try
                    {
                        Console.Write("1～{0}の数字を入力してください。＞", maxplayer);
                        pnum = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                } while (pnum < 1 || pnum > maxplayer);

                // CPU
                Console.WriteLine("\nコンピューターは何人ですか？");
                do
                {
                    try
                    {
                        Console.Write("1～{0}の数字を入力してください。＞", maxplayer);
                        cnum = int.Parse(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                } while (cnum < 1 || cnum > maxplayer);
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

            Console.WriteLine("\nじゃんけんへようこそ。\n");

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
                                Console.Write("プレーヤー{0}、入力してください。0:グー、1:チョキ、2:パー＞", i + 1);
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
                                Console.WriteLine("プレーヤー{0}がグーを出しました。", i + 1);
                                break;
                            case 1:
                                Console.WriteLine("プレーヤー{0}がチョキを出しました。", i + 1);
                                break;
                            case 2:
                                Console.WriteLine("プレーヤー{0}がパーを出しました。", i + 1);
                                break;
                        }
                    }

                    Random random = new Random();

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

                        System.Threading.Thread.Sleep(100);
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
                                    Console.Write("プレーヤー{0}、", i + 1);
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
                // グーvsチョキ（両端のケース）
                else
                {
                    for (int i = 0; i < pnum; i++)
                    {
                        if (p[i].Choice == 0)
                        {
                            Console.Write("プレーヤー{0}、", i + 1);
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

            // （暫定処置）続きからであれば点数を足していく
            int[] finalscore = new int[(maxplayer * 2) + 1];

            if (continueflg == "Y" || continueflg == "y")
            {
                for (int i = 0; i < finalscore.Length; i++)
                {
                    if (i < pnum)
                    {
                        finalscore[i] = Convert.ToInt32(output[i]) + p[i].Score;
                    }
                    else if (i >= maxplayer && i < maxplayer + cnum)
                    {
                        finalscore[i] = Convert.ToInt32(output[i]) + c[i - maxplayer].Score;
                    }
                    else if (i == maxplayer * 2)
                    {
                        finalscore[i] = Convert.ToInt32(output[i]) + roundcount;
                    }
                }
            }
            else
            {
                for (int i = 0; i < finalscore.Length; i++)
                {
                    if (i < pnum)
                    {
                        finalscore[i] = p[i].Score;
                    }
                    else if (i >= maxplayer && i < maxplayer + cnum)
                    {
                        finalscore[i] = c[i - maxplayer].Score;
                    }
                    else if (i == maxplayer * 2)
                    {
                        finalscore[i] = roundcount;
                    }
                }
            }

            // 結果報告
            Console.WriteLine("総ラウンド数：{0}", finalscore[finalscore.Length - 1]);

            for (int i = 0; i < pnum; i++)
            {
                Console.Write("プレーヤー{0}：{1}\n", i + 1, finalscore[i]);
            }

            for (int i = 0; i < cnum; i++)
            {
                Console.Write("コンピューター{0}：{1}\n", i + 1, finalscore[i + maxplayer]);
            }

            // 書き込み準備
            // Player
            for (int i = 0; i < maxplayer; i++)
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
            for (int i = maxplayer; i < maxplayer * 2; i++)
            {
                if (i - maxplayer < cnum)
                {
                    output[i] = finalscore[i].ToString();
                }
                else
                {
                    output[i] = string.Empty;
                }
            }

            // 総ラウンド数
            output[output.Length - 1] = finalscore[finalscore.Length - 1].ToString();

            // 総ラウンド数(+1)を含めるStringの生成
            string s = string.Empty;

            for (int i = 0; i < (maxplayer * 2) + 1; i++)
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
