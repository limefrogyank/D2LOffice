using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Service
{
    internal class UIService
    {
        private BehaviorSubject<int> windowWidth = new BehaviorSubject<int>(-1);
        private IDisposable _widthValues;

        public IObservable<int> OnWindowWidth => windowWidth.AsObservable();

        public UIService()
        {



        }

        //public void Initialize()
        //{
        //    var widthValues = Observable.FromEvent<EventHandler<DisplayInfoChangedEventArgs>, DisplayInfoChangedEventArgs>(
        //        handler =>
        //        {
        //            EventHandler<DisplayInfoChangedEventArgs> nativehandler = (sender, e) => handler(e);
        //            return nativehandler;
        //        },
        //        add => DeviceDisplay.Current.MainDisplayInfoChanged += add,
        //        remove => DeviceDisplay.Current.MainDisplayInfoChanged -= remove,
        //        RxApp.MainThreadScheduler)
        //        .Select(x =>
        //        {
        //            Debug.WriteLine($"Width: {x}");
        //            return (int)x.DisplayInfo.Width;
        //        });
        //    windowWidthMulti.OnNext(widthValues);
        //}


//        public void Initialize(Page page)
//        {
//#if WINDOWS

//            page.HandlerChanged += Host_HandlerChanged;

//            if (_widthValues != null)
//            {
//                _widthValues.Dispose();
//            }
//            //windowWidth.OnNext((int)page.Width);
//            //var appView = (Microsoft.Maui.Controls.Application.Current.Handler.PlatformView);

//            //OnWindowWidth = 
//            //    Observable.FromEvent<EventHandler, EventArgs>(
//            //        handler =>
//            //        {
//            //            EventHandler nativeHandler = (sender, e) => handler(e);
//            //            return nativeHandler;
//            //        },
//            //        add => appView..SizeChanged += add,
//            //        remove => page.SizeChanged -= remove,
//            //        RxApp.MainThreadScheduler)
//            //        .Select(x =>
//            //        {
//            //            Debug.WriteLine(page.Width);
//            //            //windowWidth.OnNext((int)(page.Width));
//            //            return (int)(page.Width);
//            //        })
//            //    ;
//            //windowWidthMulti.OnNext(widthValues);
//#endif
//        }

#if WINDOWS
        //private void Host_HandlerChanged(object sender, EventArgs e)
        //{
        //    var element = (IElement)sender;

        //    (element.Handler.PlatformView as Microsoft.UI.Xaml.Controls.Control).SizeChanged += App_SizeChanged;
        //}

        //private void App_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        //{
        //    windowWidth.OnNext((int)e.NewSize.Width);
        //}

        //private void AppWindow_Changed(Microsoft.UI.Windowing.AppWindow sender, Microsoft.UI.Windowing.AppWindowChangedEventArgs args)
        //{
        //    Debug.WriteLine("changed: " + sender.Size.Width);
        //}
#endif
    }
}
