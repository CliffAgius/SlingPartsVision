using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using SlingPartsVision.Models;
using SlingPartsVision.Services;
using System.Web;

namespace SlingPartsVision.ViewModels
{
    public partial class CameraPageViewModel : BaseViewModel, IQueryAttributable
    {

        [ObservableProperty]
        bool takeSnapShot = true;

        [ObservableProperty]
        Stream snapShotStream;

        [ObservableProperty]
        int tagImageCount = 0;

        [ObservableProperty]
        string tagName = "";

        [ObservableProperty]
        string stroke = "Gray";

        bool TakeNextPhoto = false;

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var code = HttpUtility.UrlDecode((string)query["Barcode"]);
            await TrainingService.CheckTags(code);

            TagName = TrainingService.Tag.Name;            
        }

        async partial void OnSnapShotStreamChanged(Stream value)
        {
            try
            {
                if (SnapShotStream != null && TakeNextPhoto)
                {
                    TrainingService.TrainingAPI.CreateImagesFromData(
                        Globals.ProjectID,
                        SnapShotStream,
                        new List<Guid>() { TrainingService.Tag.Id });

                    await TrainingService.UpdateTag();
                }
                TakeNextPhoto = false;
                TagImageCount = TrainingService.Tag.ImageCount;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error - {ex.Message}", "OK");
            }
        }

        partial void OnTagImageCountChanged(int value)
        {
            if (value >= 30)
            {
                Stroke = "Green";
            }
        }

        [RelayCommand]
        public void TakePhoto()
        {
            TakeSnapShot = false;
            TakeSnapShot = true;
            TakeNextPhoto = true;
        }
    }
}
