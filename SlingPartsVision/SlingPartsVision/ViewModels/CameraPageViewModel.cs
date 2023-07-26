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
        string TagID = null;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TagID = HttpUtility.UrlDecode((string)query["Barcode"]);
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

                        if (TrainingService.Tag is null)
                        {
                            IList<Tag> Tags = await TrainingService.TrainingAPI.GetTagsAsync(Globals.ProjectID);

                            TrainingService.Tag = Tags.FirstOrDefault(x => x.Name == TagID);
                            if (TrainingService.Tag is null)
                            {
                                TrainingService.Tag = await TrainingService.TrainingAPI.CreateTagAsync(Globals.ProjectID, TagID);
                            }
                        }

                        using (var sourceStream = await photo.OpenReadAsync())
                        {
                            TrainingService.TrainingAPI.CreateImagesFromData(
                                Globals.ProjectID,
                                sourceStream,
                                new List<Guid>() { TrainingService.Tag.Id });
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
