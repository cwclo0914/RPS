using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// ゲームの中で共通する便利なIO Methods
    /// </summary>
    internal static class ConsoleIO
    {
        // Methods
        // "Y/N"答えるまで出させないループ
        public static bool YesNoQ(string cout)
        {
            char cin = '0';

            while (!(cin == 'Y' || cin == 'y' || cin == 'N' || cin == 'n'))
            {
                Console.Write("{0}", cout);
                char.TryParse(Console.ReadLine(), out cin);
                Console.WriteLine();
            }
            if (cin == 'Y' || cin == 'y') return true;
            else return false;
        }

        // 結果発表
        public static void Result(string[] r)
        {
            Console.WriteLine("総ラウンド数：{0}", r[r.Length - 1]);
            for (int i = 0; i < Settings.PMAX + Settings.CMAX ; i++)
            {
                if (r[i] != string.Empty)
                {
                    if (i < Settings.PMAX) // Player
                        Console.WriteLine("プレイヤー{0}：{1}/{2} ({3}%)", i + 1, r[i], r[r.Length - 1], Math.Round(Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1]) * 100, 1));
                    else //CPU
                        Console.WriteLine("コンピューター{0}：{1}/{2} ({3}%)", i - Settings.PMAX + 1, r[i], r[r.Length - 1], Math.Round(Convert.ToDouble(r[i]) / Convert.ToDouble(r[r.Length - 1]) * 100, 1));
                }
            }
            Console.WriteLine();
        }
    }
}
