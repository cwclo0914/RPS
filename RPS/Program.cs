﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool first = true;
            string redoflg = string.Empty;
            int judge = 0;

            // 人数定義
            Player p1 = new Player();
            Computer c1 = new Computer();

            // "N"を入力するまで続く
            do
            {
                // 初期値に戻す
                p1.Reset();
                c1.Reset();

                do // 勝負が決まるまで続く
                {
                    do // 正しく入力するまで続く
                    {
                        try
                        {
                            if (first == true)
                            {
                                Console.Write("じゃんけんへようこそ。\n入力してください。");
                            }
                            else
                            {
                                Console.Write("入力してください。");
                            }

                            Console.Write("0:グー、1:チョキ、2:パー＞");

                            p1.Choice = int.Parse(Console.ReadLine());
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        first = false;
                    } while (!(p1.Choice >= 0 && p1.Choice <= 2)); // 正しく入力するまで続く

                    // コンピューターの出力
                    c1.Shuffle();

                    if (c1.Choice == 0)
                        Console.WriteLine("コンピューターがグーを出しました。");
                    else if (c1.Choice == 1)
                        Console.WriteLine("コンピューターがチョキを出しました。");
                    else if (c1.Choice == 2)
                        Console.WriteLine("コンピューターがパーを出しました。");

                    // あいこ判定
                    if (p1.Choice == c1.Choice)
                    {
                        Console.WriteLine("あいこです。もう一度勝負します。\n");
                    }

                } while (p1.Choice == c1.Choice); // 勝負が決まるまで続く

                judge = Judgement(p1.Choice, c1.Choice);

                // 勝ち「2」、負け「1」
                if (judge == 2)
                {
                    Console.WriteLine("あなたの勝ちです。おめでとうございます。\n");
                    p1.Score++;
                    do
                    {
                        Console.Write("もう一度勝負しますか？（Y/N）＞");
                        redoflg = Console.ReadLine();
                    } while (!(redoflg == "Y" || redoflg == "y" || redoflg == "N" || redoflg == "n")); // "Y/N"以外を弾く
                }
                else if (judge == 1)
                {
                    Console.WriteLine("コンピューターの勝ちです。残念です。\n");
                    c1.Score++;
                    do
                    {
                        Console.Write("もう一度勝負しますか？（Y/N）＞");
                        redoflg = Console.ReadLine();
                    } while (!(redoflg == "Y" || redoflg == "y" || redoflg == "N" || redoflg == "n"));
                }
            } while (redoflg == "Y" || redoflg == "y"); // "N"を入力するまで続く

            Console.WriteLine("あなた: {0}　コンピューター: {1}", p1.Score, c1.Score);
            if (p1.Score == c1.Score)
                Console.WriteLine("あいこです。");
            else if (p1.Score > c1.Score)
                Console.WriteLine("あなたの勝ちです。");
            else if (p1.Score < c1.Score)
                Console.WriteLine("コンピューターの勝ちです。");

            Console.WriteLine("終了します。お疲れ様でした。");
            Console.Read();
        }

        static int Judgement(int choice1, int choice2) // choice1: PC、choice2: CPU
        {
            int diff = choice1 - choice2; // 勝ち「2 or -1」、負け「-2 or 1」
            diff += 3; // 勝ち「5 or 2」、負け「1 or 4」
            diff = diff % 3; // 勝ち「2」、負け「1」

            return diff;
        }
    }
}
