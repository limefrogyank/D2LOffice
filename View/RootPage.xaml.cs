using D2LOffice.ViewModel;

namespace D2LOffice.View;

public partial class RootPage : ReactiveUI.Maui.ReactiveContentPage<RootViewModel>
{
	public RootPage()
	{
		InitializeComponent();
		
	}
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new SettingsView());
    }
}