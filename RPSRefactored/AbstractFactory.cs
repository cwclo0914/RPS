using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    abstract class AbstractFactory
    {
        public abstract GameData ChooseGame(string choice, int pnum, int cnum);
    }

    internal class GameFactory : AbstractFactory
    {
        public override GameData ChooseGame(string choice, int pnum, int cnum)
        {
            GameData game = null;

            if (choice.ToLower() == "rps") { game = RPS.Instance(pnum, cnum); }
            else if (choice.ToLower() == "test")
            {
                Console.WriteLine("ステージ１：テスト成功");
                game = test.Instance(pnum, cnum);
            }

            return game;
        }
    }
}
