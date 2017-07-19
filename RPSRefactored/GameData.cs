using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// ゲーム内の情報（ラウンド数とEntity Data）を保存する
    /// </summary>
    abstract internal class GameData
    {
        // Fields

        // Constructors
        public GameData(int pnum, int cnum)
        {
            Pnum = pnum;
            Cnum = cnum;
            TotalCount = 0;

            p_list = new List<Entity>();
            c_list = new List<Entity>();

            EntityCreate();
        }

        // Properties
        public List<Entity> p_list { get; set; }
        public List<Entity> c_list { get; set; }

        public int Pnum { get; set; }
        public int Cnum { get; set; }
        public int TotalCount { get; set; }

        // Methods
        // インスタンス生成
        public void EntityCreate()
        {
            for (int i = 0; i < Pnum; i++)
                p_list.Add(new Player());

            for (int i = 0; i < Cnum; i++)
                c_list.Add(new Computer());
        }

        // Abstract Methods
        public abstract void RPSMain();

        public abstract string[] ReportCurrentScore();
    }
}
