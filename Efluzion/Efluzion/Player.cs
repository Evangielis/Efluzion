using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efluzion
{
    public class Player
    {
        public String Name { get; private set; }
        public int HighScore { get; private set; }

        public Player(String Name)
        {
            this.Name = Name;
            this.HighScore = 0;
        }

        public bool SubmitScore(int Score)
        {
            if (Score > HighScore)
            {
                HighScore = Score;
                return true;
            }
            else return false;
        }
    }
}
