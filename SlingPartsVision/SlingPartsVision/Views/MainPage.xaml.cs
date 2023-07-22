using SlingPartsVision.ViewModels;

namespace SlingPartsVision.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (CameraView.Cameras.Count > 0)
        {
            CameraView.Camera = CameraView.Cameras.First();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await CameraView.StopCameraAsync(); // Needed for some Android devices, you need to stop and then start...
                await CameraView.StartCameraAsync();
            });
        }
    }
}

