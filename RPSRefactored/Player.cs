using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// Playerの設定
    /// </summary>
    internal class Player : Entity
    {
        // Constructor
        public Player()
        {
            this.Name = "プレイヤー";
        }

        // Properties
        public override string Name { get; set; }

        // Methods
        public override void Reset()
        {
            this.Choice = -1;
        }

        public override void Shuffle(Random rand)
        {
            throw new NotImplementedException();
        }

        public override void Scoring(int winningchoice, int i)
        {
            if (this.Choice == winningchoice)
            {
                Console.Write("{0}{1}、", this.Name, i + 1);
                this.Score++;
            }
        }
    }
}
