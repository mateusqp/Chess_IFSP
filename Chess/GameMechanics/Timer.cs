using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameMechanics
{
    public class Timer
    {
        float increment;
        float p1Time;
        float p2Time;

        public Timer() //Standard
        {
            increment = 0;
            p1Time = 0;
            p2Time = 0;
        }

        public Timer(float increment, float p1Time, float p2Time)
        {
            this.increment = increment;
            this.p1Time = p1Time;
            this.p2Time = p2Time;
        }
    }
}
