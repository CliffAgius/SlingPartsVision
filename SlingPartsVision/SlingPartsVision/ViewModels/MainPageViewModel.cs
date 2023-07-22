using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using SlingPartsVision.Models;
using SlingPartsVision.Services;
using ZXing;

namespace SlingPartsVision.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        string tagID;

        [ObservableProperty]
        Tag tag;

        [ObservableProperty]
        Result[] barcodeResults;

        TrainingService TrainingService;

        public MainPageViewModel(TrainingService trainingService)
        {
            TrainingService = trainingService;
        }

        partial void OnBarcodeResultsChanged(Result[] value)
        {
            TagID = BarcodeResults[0].Text;
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

                        if (Tag is null)
                        {
                            IList<Tag> Tags = await TrainingService.TrainingAPI.GetTagsAsync(Globals.ProjectID);

                            Tag = Tags.FirstOrDefault(x => x.Name == TagID);
                            if (Tag is null)
                            {
                                Tag = await TrainingService.TrainingAPI.CreateTagAsync(Globals.ProjectID, TagID);
                            }
                        }

                        using (var sourceStream = await photo.OpenReadAsync())
                        {
                            TrainingService.TrainingAPI.CreateImagesFromData(
                                Globals.ProjectID,
                                sourceStream,
                                new List<Guid>() { Tag.Id });
                        }
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
