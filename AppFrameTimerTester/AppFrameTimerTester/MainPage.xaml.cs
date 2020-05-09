using FrameTimerX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppFrameTimerTester
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            frtTimer2.Started += FrtTimer_Started;            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void FrtTimer_Started(object sender, FrameTimerEventArgs args)
        {
            Debug.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine("@@@@@@@@@@@@@@@@@@ Simple Started Event raised!!!!! @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Counter:{args.Counter} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Hour:{args.Hour} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Minute:{args.Minute} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Second:{args.Second} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        }

        private void btnStartPause_Clicked(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            // Identifies the acion and frametimer by the button clicked
            TimerAction action = bt.Text == "Start" ? TimerAction.Start : (bt.Text == "Pause" ? TimerAction.Pause : TimerAction.Resume);
            bt.Text = bt.Text == "Start" ? "Pause" : (bt.Text == "Pause" ? "Resume" : "Pause");
            FrameTimer fr = (bt == btnStartStopFTX1 ? frtTimer1 :
                (bt == btnStartStopFTX2 ? frtTimer2 :
                (bt == btnStartStopFTX3 ? frtTimer3 : 
                (bt == btnStartStopFTX4 ? frtTimer4 : frtTimer5))));
            
            // Call the apropriated method from the frametimer 
            if (action == TimerAction.Start)
                fr.Start();
            else if (action == TimerAction.Pause)
                fr.Pause();
            else 
                fr.Resume();
        }

        private void btnOtherPage_Clicked(object sender, EventArgs e)
        {
            OtherPage newPage = new OtherPage();
            newPage.Finished += (param) =>
            {
                if (param == 0)
                {
                    frtTimer6.StartingMinute = 1;
                    frtTimer6.StartingSecond = 30;
                    frtTimer6.IsEnabled = true;
                    frtTimer6.Start();
                }
                else
                {
                    frtTimer6.StartingMinute = 2;
                    frtTimer6.StartingSecond = 15;
                    frtTimer6.IsEnabled = true;
                    frtTimer6.Start();
                }
                return 0;
            };
            Navigation.PushAsync(newPage);
         }
    }

    enum TimerAction { Start, Stop, Pause, Resume };
}
