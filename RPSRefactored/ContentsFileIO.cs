using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSRefactored
{
    public static class ContentsFileIO
    {
        // セーブデータの選択
        public static string SelectFile(string choice)
        {
            switch(choice.ToLower())
            {
                case "rps": return @"Data\rates";
                case "test": return @"Data\test";
                default: return null;
            }
        }

        // CSVから読み込む
        public static string Read(string choice)
        {
            // ファイルチェック
            if (File.Exists(SelectFile(choice) + ".csv") == false)
                return string.Empty;

            // 読み込み
            try
            {
                using (StreamReader reader = new StreamReader(SelectFile(choice) + ".csv", Encoding.Default))
                    return reader.ReadLine();
            }
            catch
            {
                Console.WriteLine("読込エラー");
                return string.Empty;
            }
        }

        // CSVに書き込む
        public static void Write(string choice, string s)
        {
            // ファイルチェック
            if (Directory.Exists("Data") == false)
                Directory.CreateDirectory("Data");

            // 書き込む
            try
            {
                using (StreamWriter writer = new StreamWriter(SelectFile(choice) + ".csv", false, Encoding.Default))
                        writer.Write(s);
            }
            catch
            {
                Console.WriteLine("書込エラー：csvファイルを閉じてください");
                Console.Read();
            }
        }

        // バックアップを取る
        public static void BackUp(string choice)
        {
            try
            {
                string dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                System.IO.File.Move((SelectFile(choice) + ".csv"), (SelectFile(choice) + dt  + ".csv"));
            }
            catch
            {
                Console.WriteLine("書込エラー：csvファイルを閉じてください");
                Console.Read();
            }
        }
    }
}
