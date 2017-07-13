using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSRefactored
{
    abstract class Entity
    {
        // Fields
        private int choice; // 0:グー、1:チョキ、2:パー

        // Constructors
        public Entity()
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

        public int Score { get; set; }

        public abstract string Name { get; set; }

        // Methods
        public abstract void Reset();

        public abstract void Shuffle(Random rand);

        public abstract void Scoring(int winningchoice, int i);
    }
}
