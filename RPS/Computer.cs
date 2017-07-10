using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    class Computer : Player
    {
        //Fields
        private Random random = new Random();

        //Constructor
        public Computer()
        {
        }

        //Methods
        public void Shuffle()
        {
            Choice = random.Next() % 3;
        }

    }
}
