using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class ChessTimer
    {
        public float increment { get; set; }
        public int p1Time { get; set; }
        public int p2Time { get; set; }

        public ChessTimer() //Standard
        {
            increment = 0;
            p1Time = 599;
            p2Time = 599;
        }

        public ChessTimer(float increment, int p1Time, int p2Time)
        {
            this.increment = increment;
            this.p1Time = p1Time;
            this.p2Time = p2Time;
        }

        public string TimeString(float time)
        {
            string min;
            string sec;
            min = ((int)(time / 60)).ToString();
            sec = ((int)(time % 60)).ToString();
            if ((time / 60) < 10) //min
            {
                min = "0" + min;
            }

            if ((time % 60) < 10) //sec
            {
                sec = "0" + sec;
            }

            return min + ":" + sec;
        }
    }
}
