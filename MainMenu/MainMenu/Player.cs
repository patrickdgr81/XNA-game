using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Main
{
    class Player
    {
        static int arrows;
        static int gold;
        static int turns;
        public int Arrows
        {
            get { return arrows; }
            set { arrows = value; }
        }
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }
        public int Turns
        {
            get { return turns; }
            set { turns = value; }
        }
        public int getScore()
        {
            return (100 - turns + gold + (10 * arrows));
        }

        public Player()
        {
            arrows = 3;
            gold = 0;
            turns = 0;
        }


    }
}
