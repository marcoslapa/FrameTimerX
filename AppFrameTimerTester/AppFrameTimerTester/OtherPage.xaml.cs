using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFrameTimerTester
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherPage : ContentPage
    {
        public delegate int PopDelegate(int index);
        public event PopDelegate Finished;
        public OtherPage()
        {
            InitializeComponent();
        }

        private async void SetTo1_30_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            Finished(0);
        }

        private async void SetTo2_15_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            Finished(1);
        }
    }
}