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
        }

        private void ButtonSR_Clicked(object sender, EventArgs e)
        {
            if (btnStopResume.Text == "Stop")
            {
                btnStopResume.Text = "Resume";
                frtTimer1.Stop();
            }
            else
            {
                btnStopResume.Text = "Stop";
                frtTimer1.Resume();
            }
        }

        private void frtTimer1_Started(object sender, FrameTimerX.FrameTimerEventArgs evt)
        {
            Debug.WriteLine("##########################################################");
            Debug.WriteLine("################### Timer Started! #######################");
            Debug.WriteLine("##########################################################");
        }

        private void ButtonSTART_Clicked(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                btnStart.Text = "Stop";
                frtTimer3.Start();
            }
            else
            {
                frtTimer3.Stop();
                btnStart.IsEnabled = false;
            }
        }
    }
}
