using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    internal class Computer : Player
    {
        // Fields

        // Constructor
        public Computer()
        {
            this.Name = "コンピューター";
        }

        // Properties
        public new string Name { get; set; }

        // Methods
        public override void Shuffle(Random rand)
        {
            this.Choice = rand.Next(0, 3);
        }

        public override void Judge(int winningchoice, int i)
        {
            if (this.Choice == winningchoice)
            {
                Console.Write("{0}{1}、", Name, i + 1);
                this.Score++;
            }
        }
    }
}
