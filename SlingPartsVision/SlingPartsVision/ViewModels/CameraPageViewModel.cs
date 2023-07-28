using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using SlingPartsVision.Models;
using SlingPartsVision.Services;
using System.Web;

namespace SlingPartsVision.ViewModels
{
    public partial class CameraPageViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        ImageSource imageSource = null;

        [ObservableProperty]
        string? tagImageCount;

        [ObservableProperty]
        string? tagName;

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var code = HttpUtility.UrlDecode((string)query["Barcode"]);
            await TrainingService.CheckTags(code);

            TagName = $"Barcode value - {TrainingService.Tag.Name}";
            TagImageCount = $"Images with TagID - {TrainingService.Tag.ImageCount.ToString()}";
        }

        [RelayCommand]
        public async Task TakePhoto()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // save the file into local storage
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using (var sourceStream = await photo.OpenReadAsync())
                        {
                            TrainingService.TrainingAPI.CreateImagesFromData(
                                Globals.ProjectID,
                                sourceStream,
                                new List<Guid>() { TrainingService.Tag.Id });
                        }

                        TagImageCount = $"Images with TagID - {TrainingService.Tag.ImageCount.ToString()}";
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error - {ex.Message}", "OK");
            }
        }
    }
}
