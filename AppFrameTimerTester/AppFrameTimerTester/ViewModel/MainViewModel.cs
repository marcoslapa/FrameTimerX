using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppFrameTimerTester.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            WarningColor1 = Color.BlueViolet;
            Velocity = 0.5;
            StopCommand = new Command(StopTimer);
            StartCommand = new Command(StartTimer);
            ResumeCommand = new Command(ResumeTimer);
        }

        private Color wc1;
        public Color WarningColor1 {
            get {
                return wc1;
            }
            set {
                wc1 = value;
            }
        }

        private double vel;

        public double Velocity {
            get { return vel; } 
            set { vel = value; } 
        }

        public Command StopCommand { get; }

        public Command StartCommand { get; }

        public Command ResumeCommand { get; }

        public void StopTimer()
        {
            Debug.WriteLine("########### Timer Stopped! ###########");
        }

        public void StartTimer()
        {
            Debug.WriteLine("########### Timer Started! ###########");
        }

        public void ResumeTimer()
        {
            Debug.WriteLine("########### Comming back! ###########");
        }
    }
}
