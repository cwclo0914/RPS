using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{

    internal class GameMaker
    {
        public GameData ChooseGame(bool newgame, string choice, int pnum, int cnum)
        {
            GameData game = null;

            if (choice.ToLower() == "rps")
            {
                if (newgame) RPS.InstanceReset();
                game = RPS.Instance(pnum, cnum);
            }
            else if (choice.ToLower() == "test")
            {
                Console.WriteLine("ステージ１：テスト成功");
                if (newgame) test.InstanceReset();
                game = test.Instance(pnum, cnum);
            }

            return game;
        }
    }
}
