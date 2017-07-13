﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSRefactored
{
    /// <summary>
    /// ユーザーとGameをつなげるホスト
    /// </summary>
    internal class Host
    {
        // Fields
        // 最大人数
        const int pmax = 3; // Player
        const int cmax = 5; // CPU

        private int pnum;
        private int cnum;

        // Constructors
        public Host()
        {
            Buffer = new string[pmax + cmax + 1];
            pnum = 0;
            cnum = 0;
            IsContinue = true;
        }

        // Properties
        public RPS rps { get; set; }
        public string[] Buffer { get; set; }
        public int GameChoice { get; set; }
        public bool IsContinue { get; set; }

        // Methods
        ////////////////////// メインメソッド //////////////////////
        public void Main()
        {
            this.StartUp();
            this.NumberDef();

            rps = new RPS(pnum, cnum);

            rps.EntityCreate();
            rps.RPSMain();
            this.End();
        }

        //////////////////////// 起動処理 ////////////////////////
        private void StartUp()
        {
            // 前回勝率の表示
            if (File.Exists(@"Data\rates.csv") == false)
                NewFile();
            else
            {
                ReadOldFile();
                DisplayOldData();
            }

            if (IsContinue == true)
                IsContinue = ConsoleIO.YesNoQ("続きから始めますか？（Y/N）＞");
        }

        // 小メソッド
        // セーブファイル作成
        private void NewFile()
        {
            Console.WriteLine("新しいセーブデータを作成します。\n");
            InitialiseSave(pmax + cmax);
            IsContinue = false;
        }

        // セーブファイルを読み込む
        private void ReadOldFile()
        {
            do
            {
                List<string> rates = new List<string>() { null };
                rates = ContentsFileIO.Read();
                Buffer = rates[0].Split(',');

                if (Buffer.Length != pmax + cmax + 1) // 最大人数変更後のセーブデータ初期化
                {
                    Console.WriteLine("セーブデータが使用できません。\n現在のセーブデータをバックアップし、初期化します。\n");
                    ContentsFileIO.BackUp();
                    InitialiseSave(pmax + cmax);
                }
            } while (Buffer.Length != pmax + cmax + 1);
        }

        // 前回結果の表示
        private void DisplayOldData()
        {
            if (Buffer[Buffer.Length - 1] == string.Empty) // 空白セーブ（Rounds = 0）を読み込んだ場合
                IsContinue = false;
            else
            {
                Console.WriteLine("前回結果：");
                ConsoleIO.Result(Buffer, pmax, cmax);
            }
        }

        // セーブデータ初期化
        private void InitialiseSave(int max)
        {
            string initialise = string.Empty;
            for (int i = 0; i < max; i++)
                initialise += ",";

            List<string> empty = new List<string> { initialise + Environment.NewLine };
            ContentsFileIO.Write(empty);
        }

        //////////////////////// 人数定義 ////////////////////////
        private void NumberDef()
        {
            if (IsContinue)
                Continue();
            else
                NewGame();
        }

        // 小メソッド
        // 続きから
        private void Continue()
        {
            for (int i = 0; i < pmax + cmax; i++) // Bufferをスキャンして前回の人数を数える
            {
                if (Buffer[i] != string.Empty)
                {
                    if (i < pmax)
                        pnum++;
                    else
                        cnum++;
                }
            }
        }

        // 初めから
        private void NewGame()
        {
            // 前回のデータを消す
            for (int i = 0; i < Buffer.Length; i++)
                Buffer[i] = "0";

            // 人数確認
            pnum = NumberConfirmation("プレイヤー", pmax); // Player
            cnum = NumberConfirmation("コンピューター", cmax); // CPU
        }

        // 人数確認、出力
        private int NumberConfirmation(string name, int max)
        {
            int num = 0;
            Console.WriteLine("{0}は何人ですか？", name);

            do // PC
            {
                Console.Write("1～{0}の数字を入力してください。＞", max);
                int.TryParse(Console.ReadLine(), out num);
            } while (num < 1 || num > max);

            Console.WriteLine();
            return num;
        }


        //////////////////////// 終了処理 ////////////////////////
        private void End()
        {
            AddScore();
            BlankNonPart();
            ConsoleIO.Result(Buffer, pmax, cmax);
            WriteFile();

            Console.WriteLine("終了します。お疲れ様でした。");
        }

        // 小メソッド
        // 点数を足していく
        private void AddScore()
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (i < pnum)
                    Buffer[i] = (Convert.ToInt32(Buffer[i]) + rps.p[i].Score).ToString();
                else if (i >= pmax && i < pmax + cnum)
                    Buffer[i] = (Convert.ToInt32(Buffer[i]) + rps.c[i - pmax].Score).ToString();
                else if (i == Buffer.Length - 1)
                    Buffer[i] = (Convert.ToInt32(Buffer[i]) + rps.Totalcount).ToString();
            }
        }

        // 不参加なら空白にする
        private void BlankNonPart()
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (i < pmax && i >= pnum) // Player
                    Buffer[i] = string.Empty;
                else if (i < pmax + cmax && i >= pmax + cnum) // CPU
                    Buffer[i] = string.Empty;
            }
        }

        // 総ラウンド数(+1)を含めるStringの生成
        private void WriteFile()
        {
            List<string> rates = new List<string>() { null };

            string s = string.Empty;

            for (int i = 0; i < (pmax + cmax) + 1; i++)
            {
                if (i == 0)
                    s = Buffer[i];
                else
                    s += ',' + Buffer[i];
            }

            rates.Add(s + Environment.NewLine);
            ContentsFileIO.Write(rates);
        }
    }
}
