using System;
using System.Collections.Generic;
using System.Text;

namespace FrameTimerX
{
    public abstract class TimerStrategyAb
    {
        public abstract void Resume(FrameTimer ft);

        public abstract void Start(FrameTimer ft);

        public abstract void ResetClockOrCounter(FrameTimer ft);

        public abstract void InitTimer(FrameTimer ft);
    }
}
