namespace bluelotus360.com.mauiBlazor
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //new Thread(new ParameterizedThreadStart(Synchronizer.StartSynchronization)).Start(cancellationTokenSource.Token);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            cancellationTokenSource.Cancel();
            base.OnDisappearing();
        }

        //for display full screen size when app is opened
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
#if WINDOWS
                var window = App.Current.Windows.FirstOrDefault().Handler.PlatformView as Microsoft.UI.Xaml.Window;
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                 Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                 Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
              //  appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);
              (appWindow.Presenter as Microsoft.UI.Windowing.OverlappedPresenter).Maximize();
           // this line can maximize the window
#endif
        }

    }
}