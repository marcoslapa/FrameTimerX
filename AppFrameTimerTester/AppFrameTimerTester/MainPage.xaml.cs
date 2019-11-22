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

        private void FrtTimer_Started(object sender, FrameTimerEventArgs args)
        {
            Debug.WriteLine("@@@@@@@@@@@@@@@@@@ Simple Started Event raised!!!!! @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Counter:{args.Counter} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Hour:{args.Hour} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Minute:{args.Minute} @@@@@@@@@@@@@@@@@@@");
            Debug.WriteLine($"@@@@@@@@@@@@@@@@@@ Starting Second:{args.Second} @@@@@@@@@@@@@@@@@@@");

        }

        private void btnStartStop_Clicked(object sender, EventArgs e)
        {
            Button bt = sender as Button;

            // Identifies the acion and frametimer by the button clicked
            TimerAction action = bt.Text == "Start" ? TimerAction.Start : (bt.Text == "Stop" ? TimerAction.Stop : TimerAction.Resume);
            bt.Text = bt.Text == "Start" ? "Stop" : (bt.Text == "Stop" ? "Resume" : "Stop");
            FrameTimer fr = (bt == btnStartStopFTX1 ? frtTimer1 :
                (bt == btnStartStopFTX2 ? frtTimer2 :
                (bt == btnStartStopFTX3 ? frtTimer3 : 
                (bt == btnStartStopFTX4 ? frtTimer4 : frtTimer5))));
            
            // Call the apropriated method from the frametimer 
            if (action == TimerAction.Start)
                fr.Start();
            else if (action == TimerAction.Stop)
                fr.Stop();
            else 
                fr.Resume();
        }
    }

    enum TimerAction { Start, Stop, Resume };
}
