using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// 様々なゲームが生成できるゲームプール（予定）
    /// ゲームに応じてゲーム内の人数を作成するので、Entityの情報を保持する
    /// </summary>
    internal class Game
    {
        // Fields

        // Constructors
        public Game(int pnum, int cnum)
        {
            Pnum = pnum;
            Cnum = cnum;
            TotalCount = 0;
            IsRedo = false;
            EntityCreate();
        }

        // Properties
        public Entity[] p { get; set; }
        public Entity[] c { get; set; }

        public int Pnum { get; set; }
        public int Cnum { get; set; }
        public int TotalCount { get; set; }
        public bool IsRedo { get; set; }

        // Methods
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
