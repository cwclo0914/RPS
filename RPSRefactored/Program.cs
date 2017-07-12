using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSRefactored
{
    internal class Program
    {
        // 最大人数設定
        const int pmax = 3; // Player
        const int cmax = 3; // CPU

        private static void Main(string[] args)
        {
            int pnum = 0;
            int cnum = 0;
            int totalcount = 0;
            string[] buffer = new string[pmax + cmax + 1]; // ブッファ

            Player[] p, c;

            char continueflg = StartUp(out buffer);
            EntityCreate(continueflg, buffer, ref pnum, ref cnum, out p, out c);
            totalcount = RPSMain(pnum, cnum, ref p, ref c);
            End(pnum, cnum, p, c, buffer, totalcount);

            Console.Read();
        }

        //////////////////////// 大メソッド ////////////////////////
        // 起動処理（ファイルロード）→　継続フラグを出力
        private static char StartUp(out string[] buffer)
        {
            char continueflg = '0';
            List<string> rates = new List<string>();
            buffer = new string[pmax + cmax + 1];

            // 前回勝率の表示
            if (File.Exists(@"Data\rates.csv") == false)
            {
                Console.WriteLine("新しいセーブデータを作成します。");
                SaveInitialise(pmax + cmax);
                Console.WriteLine();
                continueflg = 'N';
            }
            else
            {
                rates = ContentsFileIO.Read();

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

                Array.Copy(r, buffer, r.Length); // Returnで使用するコピーを作成する

                // 空白セーブ（Rounds = 0）を読み込んだ場合
                if (r[r.Length - 1] == string.Empty)
                    continueflg = 'N';
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

            while (!(continueflg == 'Y' || continueflg == 'y' || continueflg == 'N' || continueflg == 'n'))
            {
                Console.Write("続きから始めますか？（Y/N）＞");
                char.TryParse(Console.ReadLine(), out continueflg);
                Console.WriteLine();
            }
            return continueflg;
        }

        // 人数定義（インスタンス生成）
        private static void EntityCreate(char continueflg, string[] buffer, ref int pnum, ref int cnum, out Player[] p, out Player[] c)
        {
            if (continueflg == 'Y' || continueflg == 'y') // 続きから
            {
                for (int i = 0; i < pmax + cmax; i++)
                {
                    if (buffer[i] != string.Empty)
                    {
                        if (i < pmax)
                            pnum++;
                        else
                            cnum++;
                    }
                }
            }
            else // 初めから
            {
                // 前回のデータを消す
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = "0";

                // 人数確認
                pnum = NumberConfirmation("プレイヤー", pmax); // Player
                cnum = NumberConfirmation("コンピューター", cmax); // CPU
            }

            // インスタンス生成
            p = new Player[pnum];
            c = new Computer[cnum];

            for (int i = 0; i < pnum; i++)
                p[i] = new Player();

            for (int i = 0; i < cnum; i++)
                c[i] = new Computer();
        }

        // じゃんけんメイン　→　ラウンド数を出力
        private static int RPSMain(int pnum, int cnum, ref Player[] p, ref Player[] c)
        {
            Random random = new Random();

            int zerocount = 0;
            int[] choicecount = new int[3] { 0, 0, 0 };
            int tc = 0;

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
                    for (int i = 0; i < pnum; i++)
                        p[i].Reset();
                    for (int i = 0; i < cnum; i++)
                        c[i].Reset();

                    for (int i = 0; i < pnum; i++) // 正しく入力するまで続く
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
                        if (choicecount[i] == 0)
                            zerocount++;

                    if (zerocount != 1) // グーチョキパーどれか一つが0じゃないとあいこ
                        Console.WriteLine("あいこです。もう一度勝負します。\n");

                } while (zerocount != 1); // 勝負が決まるまで続く

                // 勝負判定
                if (choicecount[choicecount.Length - 1] == 0) // パーが0　→　グーvsチョキ、(0)グーが勝つ
                {
                    for (int i = 0; i < pnum; i++)
                        p[i].Judge(0, i);
                    for (int i = 0; i < cnum; i++)
                        c[i].Judge(0, i);
                    Console.WriteLine("の勝ちです。\n");
                }
                else  // グーが0 or チョキが0 → チョキvsパー、(0+1 = 1)チョキが勝つ; グーvsパー、(1+1 = 2)パーが勝つ
                {
                    for (int j = 0; j < choicecount.Length - 1; j++)
                    {
                        if (choicecount[j] == 0)
                        {
                            for (int i = 0; i < pnum; i++)
                                p[i].Judge(j + 1, i);
                            for (int i = 0; i < cnum; i++)
                                c[i].Judge(j + 1, i);
                        }
                    }
                    Console.WriteLine("の勝ちです。\n");
                }

                tc++; // 1ラウンドの終わりにカウント

                while (!(redoflg == 'Y' || redoflg == 'y' || redoflg == 'N' || redoflg == 'n')) // "Y/N"以外を弾く
                {
                    Console.Write("もう一度勝負しますか？（Y/N）＞");
                    char.TryParse(Console.ReadLine(), out redoflg);
                    Console.WriteLine();
                }
            } while (redoflg == 'Y' || redoflg == 'y'); // "N"を入力するまで続く

            return tc;
        }

        // 終了処理（結果報告、ファイルセーブ）
        private static void End(int pnum, int cnum, Player[] p, Player[] c, string[] buffer, int tc)
        {
            List<string> rates = new List<string>() { null };

            // 点数を足していく
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i < pnum)
                    buffer[i] = (Convert.ToInt32(buffer[i]) + p[i].Score).ToString();
                else if (i >= pmax && i < pmax + cnum)
                    buffer[i] = (Convert.ToInt32(buffer[i]) + c[i - pmax].Score).ToString();
                else if (i == buffer.Length - 1)
                    buffer[i] = (Convert.ToInt32(buffer[i]) + tc).ToString();
            }

            // 結果報告
            Console.WriteLine("総ラウンド数：{0}", buffer[buffer.Length - 1]);

            for (int i = 0; i < pnum; i++)
                Console.WriteLine("プレイヤー{0}：{1}/{2} ({3}%)", i + 1, buffer[i], buffer[buffer.Length - 1], Math.Round(Convert.ToDouble(buffer[i]) / Convert.ToDouble(buffer[buffer.Length - 1]) * 100, 1));

            for (int i = 0; i < cnum; i++)
                Console.WriteLine("コンピューター{0}：{1}/{2} ({3}%)", i + 1, buffer[i + pmax], buffer[buffer.Length - 1], Math.Round(Convert.ToDouble(buffer[i + pmax]) / Convert.ToDouble(buffer[buffer.Length - 1]) * 100, 1));

            // 不参加ならbufferを空白にする
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i < pmax && i >= pnum) // Player
                    buffer[i] = string.Empty;
                else if (i < pmax + cmax && i >= pmax + cnum) // CPU
                    buffer[i] = string.Empty;
            }

            // 総ラウンド数(+1)を含めるStringの生成
            string s = string.Empty;

            for (int i = 0; i < (pmax + cmax) + 1; i++)
            {
                if (i == 0)
                    s = buffer[i];
                else
                    s += ',' + buffer[i];
            }

            rates.Add(s + Environment.NewLine);
            ContentsFileIO.Write(rates);

            Console.WriteLine("終了します。お疲れ様でした。");
        }

        //////////////////////// 小メソッド////////////////////////
        // セーブデータ初期化
        private static void SaveInitialise(int max)
        {
            string initialise = string.Empty;
            for (int i = 0; i < max; i++)
                initialise += ",";

            List<string> empty = new List<string> { initialise + Environment.NewLine };
            ContentsFileIO.Write(empty);
        }

        // 人数確認　→　人数を出力
        private static int NumberConfirmation(string name, int max)
        {
            int num = 0;

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

            return num;
        }

        // ラウンドごとの報告、カウント（0:グー、1:チョキ、2:パー）
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
    }
}
