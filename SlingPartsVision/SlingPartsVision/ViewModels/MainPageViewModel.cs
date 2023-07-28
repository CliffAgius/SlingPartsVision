using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            Vibration.Vibrate();
        }

        [RelayCommand]
        public async Task Confirm()
        {
            if (string.IsNullOrEmpty(TagID))
            {
                await Shell.Current.DisplayAlert("Error!", "You need to either scan a barcode or manually enter a value into the entry box...", "OK");
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(CameraPage)}?Barcode={TagID}");
        }
    }
}
