using D2LOffice.Models;
using D2LOffice.Service;
using D2LOffice.ViewModel;
using Newtonsoft.Json;
using ReactiveUI.Maui;
using Splat;
using System.Diagnostics;
using System.Reactive.Linq;

namespace D2LOffice.View;

//public class WebViewViewBase : ReactivePage<WebViewViewModel> { }

public partial class WebViewView : ReactiveContentPage<WebViewViewModel>
{
    private readonly D2LService _d2lService;
    private IDispatcherTimer _timer;

    public string Domain { get; private set; } = "";

    public WebViewView()
    {
        InitializeComponent();
        var d2lService = Locator.Current.GetService<D2LService>();
        if (d2lService == null)
        {
            throw new InvalidOperationException("D2LService is null");
        }
        this._d2lService = d2lService;

        //var localSettings = Preferences.Default;
        //if (localSettings == null)
        //    throw new Exception("Local settings was null.");
        //Domain = localSettings!.Get<string>("domain", "https://online.pcc.edu");
    }



    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        await Observable.Timer(TimeSpan.FromSeconds(2));
        
        try
        {

            if (ViewModel == null)
                return;

            var webview = (WebView)sender;
            if (ViewModel.SelectedService == "D2L")
            {
                var result = await webview.EvaluateJavaScriptAsync("localStorage.getItem('D2L.Fetch.Tokens')");
                if (result == null)
                {
                    // check if login screen (Specific to pcc.edu maybe.  Need a better way to create this logic.)
                    var result2 = await webview.EvaluateJavaScriptAsync($"function trylogin() {{ var login = document.querySelector('input#username'); if (login) {{ login.value='{ViewModel.Username}'; return true; }} else {{ return false;}} }}");
                    var result3 = await webview.EvaluateJavaScriptAsync("trylogin()");
                    var result4 = await webview.EvaluateJavaScriptAsync($"function trypassword() {{ var pass = document.querySelector('input#password'); if (pass) {{ pass.value='{ViewModel.Password}'; return true; }} else {{ return false;}} }}");
                    var result5 = await webview.EvaluateJavaScriptAsync("trypassword()");
                    if (result5 == "true")
                    {
                        await webview.EvaluateJavaScriptAsync("document.querySelector('button[type=submit]').click()");
                    }
                    return;
                }
                result = result.Replace(@"\""", @"""");
                result = result.Replace(@"\\""", @"""");
                if (result.StartsWith('"'))
                    result = result.Substring(1);
                if (result.EndsWith('\\'))
                    result = result.Substring(0, result.Length - 1);
                if (result.EndsWith('"'))
                    result = result.Substring(0, result.Length - 1);
                Debug.WriteLine(result);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    Debug.WriteLine("Token: " + result);
                    var wrapper = JsonConvert.DeserializeObject<SimpleAuthWrapper>(result);
                    if (wrapper != null && wrapper.Wrapper != null)
                    {
                        var authToken = wrapper.Wrapper;
                        Debug.WriteLine($"Got a token: expires: {authToken.ExpiresAt}");
                        await _d2lService.LoadTokenAsync(authToken);

                        //if (!_refreshTimerStarted)
                        //{
                        //    //await SetupTokenTimeAsync();
                        //}
                    }
                    else
                    {
                        Debug.WriteLine("Wrapper is null");
                    }
                }
                else
                {
                    Debug.WriteLine("Token empty or null");
                    //                    await wv.InvokeScriptAsync("eval", new string[] { @"document.addEventListener('DOMContentLoaded', (event) => {
                    //    window.external.notify('DOMchange');
                    //});" });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            // maybe refresh webview once
        }
    }

    private void webView_Navigating(object sender, WebNavigatingEventArgs e)
    {

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        webView.Source = "https://www.google.com";
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new SettingsView());
    }
}