using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSRefactored
{
    internal static class ContentsFileIO
    {
        // CSVから読み込む
        public static string Read()
        {
            // ファイルチェック
            if (File.Exists(@"Data\rates.csv") == false)
                return string.Empty;

            // 読み込み
            try
            {
                using (StreamReader reader = new StreamReader(@"Data\rates.csv", Encoding.Default))
                    return reader.ReadLine();
            }
            catch
            {
                Console.WriteLine("読込エラー");
                return string.Empty;
            }
        }

        // CSVに書き込む
        public static void Write(string s)
        {
            // ファイルチェック
            if (Directory.Exists("Data") == false)
                Directory.CreateDirectory("Data");

            // 書き込む
            try
            {
                using (StreamWriter writer = new StreamWriter(@"Data\rates.csv", false, Encoding.Default))
                        writer.Write(s);
            }
            catch
            {
                Console.WriteLine("書込エラー");
            }
        }

        // バックアップを取る
        public static void BackUp()
        {
            string dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            System.IO.File.Move(@"Data\rates.csv", @"Data\rates_" + dt + ".csv");
        }
    }
}
