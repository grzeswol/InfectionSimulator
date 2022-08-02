using InfectionSimulator.Views;
using System.Timers;
using Xamarin.Forms;

namespace InfectionSimulator
{
    public partial class App : Application
    {
        public static Timer AppTimer;

        public App()
        {
            InitializeComponent();
            AppTimer = new Timer();
            AppTimer.Interval = 1000;
            MainPage = new BoardPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}