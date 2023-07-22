using CommunityToolkit.Mvvm.ComponentModel;

namespace SlingPartsVision.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        bool IsNotBusy => !isBusy;
    }
}
