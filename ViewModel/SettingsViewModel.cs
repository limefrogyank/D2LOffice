using D2LOffice.Service;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.ViewModel
{
    public partial class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "settings";

        public IScreen HostScreen { get; set; }

        public List<string> ServiceItems { get; set; } = new List<string> { "D2L", "Canvas", "Blackboard" };

        [Reactive] private string _selectedService = string.Empty;
        [Reactive] private int _selectedServiceIndex= 0;

        [Reactive] private string _domain = string.Empty;
        [Reactive] private string _username = string.Empty;
        [Reactive] private string _password = string.Empty;

        public ReactiveCommand<Unit,Unit> ForceLogoutCommand { get; set; }

        public SettingsViewModel()
        {
            var hostscreen = Locator.Current.GetService<IScreen>();
            if (hostscreen == null)
            {
                throw new InvalidOperationException("HostScreen is null");
            }
            HostScreen = hostscreen;

            var d2lService = Locator.Current.GetService<D2LService>();
            if (d2lService == null)
            {
                throw new InvalidOperationException("d2lService is null");
            }

            var localSettings = Preferences.Default;
            if (localSettings == null)
                throw new Exception("Local settings was null.");

            _selectedService = localSettings!.Get<string>("selectedService", "D2L");
            _domain = localSettings!.Get<string>("domain", "https://online.pcc.edu");
            _username = localSettings!.Get<string>("username", "");
            _password = localSettings!.Get<string>("password", "");

            this.WhenAnyValue(x => x.SelectedService, x => x.Domain, x => x.Username, x => x.Password).Subscribe(val =>
            {
                localSettings!.Set("selectedService", val.Item1);
                localSettings!.Set("domain", val.Item2);
                localSettings!.Set("username", val.Item3);
                localSettings!.Set("password", val.Item4);
            });

            ForceLogoutCommand = ReactiveCommand.Create(() =>
            {
                d2lService.Logout();
            });
        }
    }
}
