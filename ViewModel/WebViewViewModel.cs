using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.ViewModel
{
    public class WebViewViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "WebViewViewModel";

        public IScreen HostScreen { get; protected set; }

        public string SelectedService { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ReactiveCommand<Unit, Unit>? NavigateToSettingsCommand { get; }
        public WebViewViewModel()
        {
            var hostscreen = Locator.Current.GetService<IScreen>();
            if (hostscreen == null)
            {
                throw new InvalidOperationException("HostScreen is null");
            }
            HostScreen = hostscreen;

            var localSettings = Preferences.Default;
            if (localSettings == null)
                throw new Exception("Local settings was null.");

            SelectedService = localSettings!.Get<string>("selectedService", "D2L");
            Domain = localSettings!.Get<string>("domain", "https://online.pcc.edu");
            Username = localSettings!.Get<string>("username", "");
            Password = localSettings!.Get<string>("password", "");

            NavigateToSettingsCommand = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.Navigate.Execute(Locator.Current.GetService<SettingsViewModel>()!).Subscribe();
            });
        }
    }
}
