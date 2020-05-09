using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FrameTimerX
{
    public class HourMinSecStrategy : TimerStrategyAb
    {
        public override void InitTimer(FrameTimer ft)
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

        public override void ResetClockOrCounter(FrameTimer ft)
        {
            InitTimer(ft);
        }

        public override void Resume(FrameTimer ft)
        {
            ft.timerStopped = false;
            ft.RaiseResumedEvent();
        }

        public override void Start(FrameTimer ft)
        {
            // Starts the Device Timer only once
            if (!ft.alreadyStarted)
            {
                ft._defaultColor = ft.BackgroundColor;

                ft.RaiseStartedEvent();

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
                        ft.RaiseStoppedEvent();
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
                                ft.RaiseWarningStartedEvent();
                            }

                            ft.ChangeWarningBackgroundColor();
                        }
                    }

                    ft.alreadyStarted = true;

                    ft._innerLabel.Text = ft.GetTimerString();

                    return true;
                });
            }
            else
            {
                // Allows to resume a timer started before  
                Resume(ft);
            }
        }
    }
}
