using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SlingPartsVision.Services;
using SlingPartsVision.Views;
using ZXing;

namespace SlingPartsVision.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        string tagID;

        [ObservableProperty]
        Result[] barcodeResults;

        partial void OnBarcodeResultsChanged(Result[] value)
        {
            TagID = BarcodeResults[0].Text;
        }

        [RelayCommand]
        public async Task Confirm()
        {
            await Shell.Current.GoToAsync($"{nameof(CameraPage)}?Barcode={TagID}");
        }
    }
}
