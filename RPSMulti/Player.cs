using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSMulti
{
    internal class Player
    {
        // Fields
        private int choice; // 0:グー、1:チョキ、2:パー

        // Constructor
        public Player()
        {
            this.Choice = -1;
            this.Score = 0;
        }

        // Properties
        public int Choice // 0:グー、1:チョキ、2:パー
        {
            get
            {
                return this.choice;
            }

            set
            {
                if (value >= 0 && value <= 2)
                {
                    this.choice = value;
                }
                else
                {
                    this.choice = -1;
                }
            }
        }

        public int Score { get; set; } // 後期フェーズ用

        // Methods
        public void Reset()
        {
            this.Choice = -1;
        }
    }
}
