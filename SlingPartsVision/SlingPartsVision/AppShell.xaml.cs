using SlingPartsVision.Services;
using SlingPartsVision.Views;

namespace SlingPartsVision;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("CameraPage", typeof(CameraPage));

		new TrainingService();
    }
}
