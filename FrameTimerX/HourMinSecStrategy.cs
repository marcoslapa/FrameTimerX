using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FrameTimerX
{
    public class HourMinSecStrategy : TimerStrategyAb
    {
        public override void ResetClockOrCounter(FrameTimer ft)
        {
            ft._innerTime = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                ft.StartingHour,
                ft.StartingMinute,
                ft.StartingSecond
            );
        }

        public override void Resume(FrameTimer ft)
        {
            ft.timerStopped = false;
            if (ft.OnResume != null)
            {
                ft.Resumed(new FrameTimerEventArgs
                {
                    Hour = ft._innerTime.Hour,
                    Minute = ft._innerTime.Minute,
                    Second = ft._innerTime.Second
                });
            }

        }

        public override void Start(FrameTimer ft)
        {
            // Starts only once
            if (!ft.alreadyStarted)
            {
                ft._defaultColor = ft.BackgroundColor;

                // Raise the Started event!
                FrameTimerEventArgs args = new FrameTimerEventArgs { 
                    Hour = ft.StartingHour, 
                    Minute = ft.StartingMinute, 
                    Second = ft.StartingSecond 
                };

                // Raises the Started event
                if(ft.OnStart != null)
                {                    
                    ft.Started(args);
                }

                Device.StartTimer(ft.GetVelocity(), () => {

                    // Verify if it has to continue increasing or decreasing the values
                    if (!ft.timerStopped)
                    {
                        ft._innerTime = ft.IsCountDown ? ft._innerTime.AddSeconds(-1) : ft._innerTime.AddSeconds(1);
                    }

                    int timeInSeconds = (ft._innerTime.Hour * 360 + ft._innerTime.Minute * 60 + ft._innerTime.Second);
                    //Debug.WriteLine("timeInSeconds:" + timeInSeconds + " / WarningTime:" + WarningTime);

                    // Verify Automatically stopping...
                    // If IsCountDown and IsNegativeEnabled == false, stops automatically when reach 00:00:00.
                    if (ft.IsCountDown && !ft.AllowNegativeValues
                        && ft._innerTime.Hour == 0
                        && ft._innerTime.Minute == 0
                        && ft._innerTime.Second == 0)
                    {
                        ft.timerStopped = true;
                        if (ft.OnStop != null)
                            ft.Stopped(new FrameTimerEventArgs { Hour = 0, Minute = 0, Second = 0 });
                    }

                    if (!ft.timerStopped)
                    {
                        // Verify if it is time to warning
                        if (ft.IsWarningTime(timeInSeconds))
                        {
                            if (!ft._warningStarted)
                            {
                                // First time Warning!
                                ft._warningStarted = true;

                                if (ft.OnStartWarning != null)
                                    ft.WarningStarted(new FrameTimerEventArgs
                                    {
                                        Hour = ft._innerTime.Hour,
                                        Minute = ft._innerTime.Minute,
                                        Second = ft._innerTime.Second
                                    });
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
