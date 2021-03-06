﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// ユーザーとGameをつなげるホスト、前回結果を保持する
    /// </summary>
    internal class Host
    {
        // Fields
        private string gameChoice;
        private GameMaker gameMaker;

        // Constructors
        public Host()
        {
            Buffer = new string[Settings.PMAX + Settings.CMAX + 1];
            IsContinue = true;
            gameChoice = string.Empty;
            gameMaker = new GameMaker();
        }

        // Properties
        public GameData currentGame { get; set; }
        public string[] Buffer { get; set; }
        public bool IsContinue { get; set; }

        // Methods
        ////////////////////// メインメソッド //////////////////////
        public void Main()
        {
            this.WhatGame();
            this.StartUp();
            currentGame.Main();
            this.End();
        }

        // Methods
        private void WhatGame()
        {
            while (gameChoice.ToLower() != "rps" && gameChoice.ToLower() != "test")
            {
                Console.Write("何を遊びたいですか？\n（RPSしかないです）＞");
                gameChoice = Console.ReadLine();
            }
        }

        //////////////////////// 起動処理 ////////////////////////

        private void StartUp()
        {
            // 前回勝率の表示
            if (ContentsFileIO.Read(gameChoice) == string.Empty)
                NewFile();
            else
            {
                ReadOldFile();
                UseOldFile();
            }

            if (!IsContinue || !ConsoleIO.YesNoQ("続きから始めますか？（Y/N）＞"))
                NewGame();
        }

        // 小メソッド
        // セーブファイルを作成
        private void NewFile()
        {
            Console.WriteLine("新しいセーブデータを作成します。\n");
            WriteFile(true);
            IsContinue = false;
        }

        // セーブファイルを読み込む
        private void ReadOldFile()
        {
            do
            {
                Buffer = ContentsFileIO.Read(gameChoice).Split(',');

                if (Buffer.Length != Settings.PMAX + Settings.CMAX + 1) // 最大人数変更後のセーブデータ初期化
                {
                    Console.WriteLine("セーブデータが使用できません。\n現在のセーブデータをバックアップし、初期化します。\n");
                    ContentsFileIO.BackUp(gameChoice);
                    WriteFile(true);
                }
            } while (Buffer.Length != Settings.PMAX + Settings.CMAX + 1);
        }

        // セーブファイルを使用
        private void UseOldFile()
        {
            if (Buffer[Buffer.Length - 1] == string.Empty) // 空白セーブ（Rounds = 0）を読み込んだ場合
                IsContinue = false;
            else
            {
                Continue();
                Console.WriteLine("前回結果：");
                currentGame.ReportCurrentScore();
            }
        }

        // 前回のデータでインスタンス生成
        private void Continue()
        {
            int pnum = 0, cnum = 0;
            for (int i = 0; i < Settings.PMAX + Settings.CMAX; i++) // Bufferをスキャンして前回の人数を数える
            {
                if (Buffer[i] != string.Empty)
                {
                    if (i < Settings.PMAX)
                        pnum++;
                    else
                        cnum++;
                }
            }

            currentGame = gameMaker.ChooseGame(false, gameChoice, pnum, cnum);
            ConvertScore();
        }

        // 読み込んだ前回結果をEntityに反映する
        private void ConvertScore()
        {
            foreach (Player player in currentGame.player_list)
                player.Score = int.Parse(Buffer[currentGame.player_list.IndexOf(player)]);
            foreach (Computer computer in currentGame.computer_list)
                computer.Score = int.Parse(Buffer[currentGame.computer_list.IndexOf(computer) + Settings.PMAX]);
            currentGame.TotalCount = int.Parse(Buffer[Buffer.Length - 1]);
        }

        // 初めからの場合
        private void NewGame()
        {
            int pnum = 0, cnum = 0;

            // 人数確認
            pnum = NumberConfirmation("プレイヤー", Settings.PMAX); // Player
            cnum = NumberConfirmation("コンピューター", Settings.CMAX); // CPU

            currentGame = gameMaker.ChooseGame(true, gameChoice, pnum, cnum);
        }

        // 初めからの人数確認
        private int NumberConfirmation(string name, int max)
        {
            int num = 0;
            Console.WriteLine("{0}は何人ですか？", name);

            do
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
            Array.Copy(currentGame.ReportCurrentScore(), Buffer, Buffer.Length); // 最後に点数を報告し、情報をHostに戻す
            WriteFile(false);
            Console.WriteLine("終了します。お疲れ様でした。");
        }

        // Stringの生成と書込
        private void WriteFile(bool init)
        {
            string s = string.Empty;
            for (int i = 0; i < Settings.PMAX + Settings.CMAX; i++)
            {
                if (init)
                    s += ',';
                else
                {
                    s += Buffer[i] + ',';
                    if (i == Settings.PMAX + Settings.CMAX - 1) s += Buffer[i + 1];
                }
            }
            ContentsFileIO.Write(gameChoice, s);
        }
    }
}
