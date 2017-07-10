using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string redoflg = string.Empty;
            int[] count = new int[3];
            int zerocount = 0;

            int pnum = 0;
            int cnum = 0;

            // 人数定義
            // Player
            Console.WriteLine("プレーヤーは何人ですか？");
            do
            {
                try
                {
                    Console.Write("1～3の数字を入力してください。＞");
                    pnum = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (pnum <= 0 || pnum >= 4);

            // CPU
            Console.WriteLine("\nコンピューターは何人ですか？");
            do
            {
                try
                {
                    Console.Write("1～3の数字を入力してください。＞");
                    cnum = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (cnum <= 0 || cnum >= 4);

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
                // 初期値に戻す
                for (int i = 0; i < pnum; i++)
                {
                    p[i].Reset();
                }

                for (int i = 0; i < cnum; i++)
                {
                    c[i].Reset();
                }

                // 勝負が決まるまで続く
                do
                {
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
                    for (int i = 0; i < count.Length; i++)
                    {
                        count[i] = 0;
                    }

                    // Player
                    for (int i = 0; i < pnum; i++)
                    {
                        switch (p[i].Choice)
                        {
                            case 0:
                                count[0]++;
                                break;
                            case 1:
                                count[1]++;
                                break;
                            case 2:
                                count[2]++;
                                break;
                        }
                    }

                    // CPU
                    for (int i = 0; i < cnum; i++)
                    {
                        switch (c[i].Choice)
                        {
                            case 0:
                                count[0]++;
                                break;
                            case 1:
                                count[1]++;
                                break;
                            case 2:
                                count[2]++;
                                break;
                        }
                    }

                    // あいこ判定
                    zerocount = 0;

                    for (int i = 0; i < count.Length; i++)
                    {
                        if (count[i] == 0)
                        {
                            zerocount++;
                        }
                    }

                    if (zerocount == 0 || zerocount == count.Length - 1)
                    {
                        Console.WriteLine("あいこです。もう一度勝負します。\n");
                    }
                } while (zerocount == 0 || zerocount == count.Length - 1); // 勝負が決まるまで続く（全種類出しても一種類だけ出してもあいこ）

                // パーが0でない→グーvsチョキでない
                if (count[count.Length - 1] != 0)
                {
                    for (int j = 0; j < count.Length; j++)
                    {
                        // j = 0: チョキvsパー、j = 1: グーvsパー
                        if (count[j] == 0)
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

                do
                {
                    Console.Write("もう一度勝負しますか？（Y/N）＞");
                    redoflg = Console.ReadLine();
                    Console.WriteLine();
                } while (!(redoflg == "Y" || redoflg == "y" || redoflg == "N" || redoflg == "n")); // "Y/N"以外を弾く

            } while (redoflg == "Y" || redoflg == "y"); // "N"を入力するまで続く

            for (int i = 0; i < pnum; i++)
            {
                Console.Write("プレーヤー{0}: {1}\n", i + 1, p[i].Score);
            }

            for (int i = 0; i < cnum; i++)
            {
                Console.Write("コンピューター{0}: {1}\n", i + 1, c[i].Score);
            }

            Console.WriteLine("終了します。お疲れ様でした。");
            Console.Read();
        }
    }
}
