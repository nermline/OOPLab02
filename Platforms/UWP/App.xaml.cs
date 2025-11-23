using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;

namespace Lab02.UWP;

sealed partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // Subscribe to the UWP close-request preview which can be cancelled
        SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnCloseRequested;

        // ... existing launch initialization (create Window, set Content, Activate, etc.)
    }

    private async void OnCloseRequested(object? sender, SystemNavigationCloseRequestedPreviewEventArgs e)
    {
        // Get a deferral so we can await a UI dialog before the app closes
        var deferral = e.GetDeferral();

        var dialog = new MessageDialog("Are you sure you want to exit the application?", "Confirm Exit");
        dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
        dialog.Commands.Add(new UICommand("No") { Id = 1 });
        dialog.DefaultCommandIndex = 0;
        dialog.CancelCommandIndex = 1;

        var cmd = await dialog.ShowAsync();

        if ((int)cmd.Id == 1)
        {
            // Cancel close
            e.Handled = true;
        }

        deferral.Complete();
    }
}