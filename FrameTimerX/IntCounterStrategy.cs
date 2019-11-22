using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FrameTimerX
{
    public class IntCounterStrategy : TimerStrategyAb
    {
        public override void ResetClockOrCounter(FrameTimer ft)
        {
            ft._innerCount = ft.StartingCounter;
        }

        public override void Resume(FrameTimer ft)
        {

            if (ft.AllowNegativeValues)
            {
                ft.timerStopped = false;
            }
            else
            {
                if (ft._innerCount > 0) ft.timerStopped = false;
            }

            if (!ft.timerStopped) ft.RaiseResumedEvent();

            //if (ft._innerCount == 0)
            //{
            //    if (ft.AllowNegativeValues)
            //        ft.timerStopped = false;
            //}
            //else
            //{
            //    ft.timerStopped = false;
            //}

            //if (ft.OnResume != null && !ft.timerStopped)
            //{
            //    ft.TimerResumed(new FrameTimerEventArgs
            //    {
            //        Counter = ft._innerCount
            //    });
            //}

        }

        public override void Start(FrameTimer ft)
        {
            // Starts only once
            if (!ft.alreadyStarted)
            {
                ft._defaultColor = ft.BackgroundColor;

                // Raise the Started event!
                ft.RaiseStartedEvent();
                //FrameTimerEventArgs args;
                //args = new FrameTimerEventArgs { Counter = ft.StartingCounter };
                //if(ft.OnStart!=null) ft.TimerStarted(args);
                
                Device.StartTimer(ft.GetVelocity(), () => {
                    // Verify if it has to continue increasing or decreasing the values
                    if (!ft.timerStopped)
                    {
                        if (ft.IsCountDown)
                            ft._innerCount--;
                        else
                            ft._innerCount++;

                        // Check if is time to change the TextColor for NegativeTextColor
                        if (!ft.negativeValueReached 
                                && ft._innerCount < 0 
                                && ft.ClockFontNegaviteTextColor != Color.Default)
                        {
                            ft.negativeValueReached = true;
                            ft._innerLabel.TextColor = ft.ClockFontNegaviteTextColor;
                        }
                    }

                    // If IsCountDown and IsNegativeEnabled == false, stops automatically when reach 0.
                    if (ft.IsCountDown && !ft.AllowNegativeValues && ft._innerCount == 0)
                    {
                        ft.timerStopped = true;
                        ft.RaiseStoppedEvent();
                        //if (ft.OnStop != null)
                        //    ft.TimerStopped(new FrameTimerEventArgs { Counter = 0 });
                    }

                    if (!ft.timerStopped)
                    {
                        // Verify if is the moment for Warning...
                        if (ft.IsWarningTime(ft._innerCount))
                        {
                            // Fires the OnStartWarning event
                            if (!ft._warningStarted)
                            {
                                // First time Warning!
                                ft._warningStarted = true;
                                ft.RaiseWarningStartedEvent();

                                //if (ft.OnStartWarning != null)
                                //    ft.TimerWarningStarted(new FrameTimerEventArgs
                                //    {
                                //        Counter = ft.StartWarningCount
                                //    });
                            }

                            ft.ChangeWarningBackgourndColor();
                        }
                    }

                    ft.alreadyStarted = true;

                    ft._innerLabel.Text = ft.GetTimerString();

                    return true;
                });
            }
        }
    }
}
