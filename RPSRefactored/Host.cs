using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSRefactored
{
    class Host
    {
        // Fields
        // 最大人数
        const int pmax = 3; // Player
        const int cmax = 3; // CPU

        int pnum;
        int cnum;

        // Constructors
        public Host()
        {
            buffer = new string[pmax + cmax + 1];
            pnum = 0;
            cnum = 0;
        }

        // Properties
        public RPS rps { get; set; }

        public string[] buffer { get; set; }

        public int gamechoice { get; set; }
        public char continueflg { get; set; }

        // Methods
        ////////////////////////// メイン //////////////////////////
        public void Main()
        {
            this.StartUp();
            this.NumberDef();

            rps = new RPS(pnum, cnum);

            rps.EntityCreate();
            rps.RPSMain();

            this.End();
        }

        //////////////////////// 大メソッド ////////////////////////
        // 起動処理（ファイルロード）→　継続フラグを出力
        private char StartUp()
        {
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

                buffer = rates[0].Split(',');

                // 最大人数変更後のセーブデータ初期化
                if (buffer.Length != pmax + cmax + 1)
                {
                    Console.WriteLine("セーブデータが使用できません。\n現在のセーブデータをバックアップし、初期化します。");
                    ContentsFileIO.BackUp();

                    SaveInitialise(pmax + cmax);

                    rates = ContentsFileIO.Read();
                    buffer = rates[0].Split(',');

                    Console.WriteLine();
                }

                // 空白セーブ（Rounds = 0）を読み込んだ場合
                if (buffer[buffer.Length - 1] == string.Empty)
                    continueflg = 'N';
                else
                {
                    // 画面の表示
                    Console.WriteLine("前回結果：");
                    ConsoleIO.Result(buffer, pmax, cmax);
                }
            }

            continueflg = ConsoleIO.YesNoQ("続きから始めますか？（Y/N）＞", continueflg);
            return continueflg;
        }

        // セーブデータ初期化
        private void SaveInitialise(int max)
        {
            string initialise = string.Empty;
            for (int i = 0; i < max; i++)
                initialise += ",";

            List<string> empty = new List<string> { initialise + Environment.NewLine };
            ContentsFileIO.Write(empty);
        }


        //////////////////////// 大メソッド ////////////////////////
        // 人数定義（インスタンス生成）
        private void NumberDef()
        {
            if (continueflg == 'Y' || continueflg == 'y') // 続きから
            {
                for (int i = 0; i < pmax + cmax; i++) // bufferをスキャンして前回の人数を数える
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
        }

        // 人数確認　→　人数を出力
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


        //////////////////////// 大メソッド ////////////////////////
        // 終了処理（結果報告、ファイルセーブ）
        private void End()
        {
            List<string> rates = new List<string>() { null };

            // 点数を足していく
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i < pnum)
                    buffer[i] = (Convert.ToInt32(buffer[i]) + rps.p[i].Score).ToString();
                else if (i >= pmax && i < pmax + cnum)
                    buffer[i] = (Convert.ToInt32(buffer[i]) + rps.c[i - pmax].Score).ToString();
                else if (i == buffer.Length - 1)
                    buffer[i] = (Convert.ToInt32(buffer[i]) + rps.Totalcount).ToString();
            }

            // 不参加ならbufferを空白にする
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i < pmax && i >= pnum) // Player
                    buffer[i] = string.Empty;
                else if (i < pmax + cmax && i >= pmax + cnum) // CPU
                    buffer[i] = string.Empty;
            }

            // 結果報告
            ConsoleIO.Result(buffer, pmax, cmax);

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
    }
}
