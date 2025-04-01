namespace D2LOffice
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(AppBootstrapper.CreateMainPage());
            //return new Window(new MainPage()) { Title = "D2LOffice" };
        }
    }
}
