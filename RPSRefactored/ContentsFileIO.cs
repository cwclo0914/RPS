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
        public static List<string> Read()
        {
            List<string> list = new List<string>();

            // ファイルチェック
            if (File.Exists(@"Data\rates.csv") == false)
            {
                return list;
            }

            // 読み込み
            try
            {
                using (StreamReader reader = new StreamReader(@"Data\rates.csv", Encoding.Default))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                return list;
            }
            catch
            {
                Console.WriteLine("読込エラー");
                return list;
            }
        }

        // CSVに書き込む
        public static void Write(List<string> list)
        {
            // ファイルチェック
            if (Directory.Exists("Data") == false)
            {
                Directory.CreateDirectory("Data");
            }

            // 書き込む
            try
            {
                using (StreamWriter writer = new StreamWriter(@"Data\rates.csv", false, Encoding.Default))
                {
                    foreach (string s in list)
                    {
                        writer.Write(s);
                    }
                }
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
