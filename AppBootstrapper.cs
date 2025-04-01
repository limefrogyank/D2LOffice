
using D2LOffice.Service;
using D2LOffice.View;
using D2LOffice.ViewModel;
using ReactiveUI;
using ReactiveUI.Maui;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;


namespace D2LOffice
{
    public static class AppBootstrapper
    {
        
        private class AppBootstrapScreen : ReactiveObject, IScreen
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AppBootstrapScreen"/> class.
            /// </summary>
            public AppBootstrapScreen(RoutingState router)
            {
                Router = router;
            }

            /// <summary>
            /// Gets or sets the router which is used to navigate between views.
            /// </summary>
            public RoutingState Router { get; protected set; }
        }

        public static MauiAppBuilder UseAppBootstrapper(this MauiAppBuilder builder)
        {
            var router = new RoutingState();
            var screen = new AppBootstrapScreen(router);
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDI4MzEyQDMxMzkyZTMxMmUzMFBsbTZPTWk4V083U3FuOGVkZDEwRVVoakk1R1ZTQktSNVkyNHM2Q3FzOEk9");
            //Locator.CurrentMutable.Register(() => new ActivationForViewFetcher(), typeof(IActivationForViewFetcher));
            Locator.CurrentMutable.RegisterConstant(screen, typeof(IScreen));

            var d2lService = new D2LService();
            var settingsService = new SettingsService();

            Locator.CurrentMutable.Register(() => d2lService, typeof(D2LService));
            Locator.CurrentMutable.RegisterConstant(new UIService(), typeof(UIService));
            Locator.CurrentMutable.RegisterConstant(settingsService);

            Locator.CurrentMutable.Register<SettingsViewModel>(()=>new SettingsViewModel());

            //Locator.CurrentMutable.Register(() => new CanvasView(), typeof(IViewFor<CanvasViewModel>));  // display pdf (or other) with inkcanvas to write on
            //Locator.CurrentMutable.Register(() => new UserSubmissionView(), typeof(IViewFor<EntityDropboxViewModel>));  // individual student submission(s)
            //Locator.CurrentMutable.Register(() => new SubmissionGraderView(), typeof(IViewFor<GradingSubmissionsViewModel>));  //a shell for navigating the submissions of each student.
            //Locator.CurrentMutable.Register(() => new SubmissionsView(), typeof(IViewFor<SubmissionsViewModel>));  // shows all students who have a/some submission(s)
            //Locator.CurrentMutable.Register(() => new GradeDropboxesView(), typeof(IViewFor<GradeDropboxesViewModel>)); // assignments view
            //Locator.CurrentMutable.Register(() => new CourseView(), typeof(IViewFor<CourseViewModel>)); // just a shell to go from course to assignments
            //Locator.CurrentMutable.Register(() => new EnrollmentsView(), typeof(IViewFor<EnrollmentsViewModel>)); // enrollments

            Locator.CurrentMutable.Register(() => new WebViewView(), typeof(IViewFor<WebViewViewModel>)); // webview for logging in
            Locator.CurrentMutable.Register(() => new RootPage(), typeof(IViewFor<RootViewModel>)); // root page
            Locator.CurrentMutable.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>)); // settings page

            d2lService.HasAccess.ObserveOn(RxApp.MainThreadScheduler).Subscribe(async x =>
            {
                if (x)
                {
                    if (router.NavigationStack.Count == 0)
                    {
                        router
                            .NavigateAndReset
                            .Execute(new RootViewModel())
                            .Subscribe();
                    }
                    else
                    {
                        router
                            .NavigateAndReset
                            .Execute(new RootViewModel())
                            .Subscribe();
                        router
                            .Navigate
                            .Execute(new RootViewModel())
                            .Subscribe();
                    }
                }
                else
                {
                    if (router.NavigationStack.Count == 0)
                    {
                        router
                       .NavigateAndReset
                       .Execute(new WebViewViewModel())
                       .Subscribe();
                    }
                    else
                    {
                        router
                            .NavigateAndReset
                            .Execute(new WebViewViewModel())
                            .Subscribe();
                        router
                            .Navigate
                            .Execute(new WebViewViewModel())
                            .Subscribe();
                    }
                }
            });

            return builder;
        }

        public static ReactiveUI.Maui.RoutedViewHost CreateMainPage()
        {
            // NB: This returns the opening page that the platform-specific
            // boilerplate code will look for. It will know to find us because
            // we've registered our AppBootstrappScreen.
            var host = new ReactiveUI.Maui.RoutedViewHost();
            //host.Router = router;

            return host;
        }

    }
}
