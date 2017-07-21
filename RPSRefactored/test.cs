using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    class test : GameData
    {
        // Fields
        private static test _instance;
        private static object syncLock = new object();

        // Constructors
        private test(int pnum, int cnum)
            : base(pnum, cnum)
        {

        }

        // Properties

        // Methods
        // Singleton
        public static test Instance(int pnum, int cnum)
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new test(pnum, cnum);
                    }
                }
            }
            return _instance;
        }

        public static void InstanceReset()
        {
            _instance = null;
        }
        //////////////////////// 大メソッド ////////////////////////
        public override void Main()
        {
            Console.WriteLine("テストなう");
            Console.ReadLine();
        }

        public override string[] ReportCurrentScore()
        {
            string[] inter = new string[Settings.PMAX + Settings.CMAX + 1];
            for (int i = 0; i < inter.Length; i++)
            {
                if (i < Pnum)
                    inter[i] = player_list[i].Score.ToString();
                else if (i < Settings.PMAX)
                    inter[i] = string.Empty;
                else if (i >= Settings.PMAX && i < Settings.PMAX + Cnum)
                    inter[i] = computer_list[i - Settings.PMAX].Score.ToString();
                else if (i < Settings.PMAX + Settings.CMAX)
                    inter[i] = string.Empty;
                else
                    inter[i] = TotalCount.ToString();
            }
            ConsoleIO.Result(inter);
            return inter;
        }
    }
}
