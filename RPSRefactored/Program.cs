﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPSRefactored
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Host host = new Host();
            host.Main();

            Console.Read();
        }
    }
}
