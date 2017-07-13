using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    class Game
    {
        // 最大人数設定
        const int pmax = 3; // Player
        const int cmax = 3; // CPU

        // Constructors
        public Game(int pnum, int cnum)
        {
            Pnum = pnum;
            Cnum = cnum;
        }

        // Properties
        public Entity[] p { get; set; }
        public Entity[] c { get; set; }

        public int Pnum { get; set; }
        public int Cnum { get; set; }
        

        // Methods
        //////////////////////// 大メソッド ////////////////////////
        // インスタンス生成

        public void EntityCreate()
        {
            p = new Entity[Pnum];
            c = new Entity[Cnum];

            for (int i = 0; i < Pnum; i++)
                p[i] = new Player();

            for (int i = 0; i < Cnum; i++)
                c[i] = new Computer();
        }
    }
}
