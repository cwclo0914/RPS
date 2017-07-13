using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    /// <summary>
    /// CPUの設定
    /// </summary>
    internal class Computer : Entity
    {
        // Constructor
        public Computer()
        {
            this.Name = "コンピューター";
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
            this.Choice = rand.Next(0, 3);
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
