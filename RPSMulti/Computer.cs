﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS
{
    internal class Computer : Player
    {
        // Fields

        // Constructor
        public Computer()
        {
        }

        // Methods
        public void Shuffle(Random rand)
        {
            this.Choice = rand.Next(0, 3);
        }
    }
}
