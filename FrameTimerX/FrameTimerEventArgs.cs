using System;
using System.Collections.Generic;
using System.Text;

namespace FrameTimerX
{
    public class FrameTimerEventArgs : EventArgs
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Counter { get; set; }

        public FrameTimerEventArgs() : base()
        {
        }
    }
}
