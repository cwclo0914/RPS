using System;
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
            RPSHost host = new RPSHost();
            host.Main();

            Console.Read();
        }
    }
}
