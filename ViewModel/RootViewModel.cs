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
    public class RootViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment => "root";

        public IScreen HostScreen { get; set; }

        public ReactiveCommand<Unit, Unit>? NavigateToSettingsCommand { get; }

        public RootViewModel()
        {
            var hostscreen = Locator.Current.GetService<IScreen>();
            if (hostscreen == null)
            {
                throw new InvalidOperationException("HostScreen is null");
            }
            HostScreen = hostscreen;

            NavigateToSettingsCommand = ReactiveCommand.Create(() =>
            {
                HostScreen.Router.Navigate.Execute(Locator.Current.GetService<SettingsViewModel>()!).Subscribe();
            });
        }
    }
}
