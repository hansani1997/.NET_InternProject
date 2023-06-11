using bluelotus360.com.razorComponents.StateManagement;
using bluelotus360.Com.MauiSupports.Services.Detectors;
//using CoreFoundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.Diagnostics;
//using Windows.UI.ViewManagement;


namespace bluelotus360.com.mauiBlazor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        //protected override Window CreateWindow(IActivationState activationState)
        //{
        //    Window window = base.CreateWindow(activationState);
        //    window.Activated += Window_Activated;
        //    return window;
        //}

        //private async void Window_Activated(object sender, EventArgs e)
        //{
        //    const int DefaultWidth = 1024;
        //    const int DefaultHeight = 800;

        //    var window = sender as Window;

        //    //change window size
        //    window.Width = DefaultWidth;
        //    window.Height = DefaultHeight;

        //    await window.Dispatcher.DispatchAsync(() => { });

        //    var disp = DeviceDisplay.Current.MainDisplayInfo;

        //    //move to screen center
        //    window.X = (disp.Width / disp.Density - window.Width) / 2;
        //    window.Y = (disp.Height / disp.Density + window.Height) / 2;

        //}


        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }
		protected override void OnResume()
		{
			base.OnResume();
		}


        //protected override void OnCreate(CFBundle savedInstanceState) 
        //{ 
        //    base.OnCreate (savedInstanceState);
        //}

    }
}