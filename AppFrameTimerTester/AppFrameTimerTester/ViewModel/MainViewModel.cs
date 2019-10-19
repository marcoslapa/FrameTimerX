using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace AppFrameTimerTester.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            StopCommand = new Command(StopTimer);
            StartCommand = new Command(StartTimer);
            ResumeCommand = new Command(ResumeTimer);
            WarningColor1 = Color.Gold;
            Velocity = 500;
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

        private int vel;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Velocity {
            get { return vel; } 
            set { 
                vel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Velocity"));
            } 
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
