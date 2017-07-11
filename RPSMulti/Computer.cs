using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSMulti
{
    internal class Computer : Player
    {
        // Fields

        // Constructor
        public Computer()
        {
        }

        // Methods
        public override void Shuffle(Random rand)
        {
            this.Choice = rand.Next(0, 3);
        }
    }
}
