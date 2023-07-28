using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using SlingPartsVision.Models;

namespace SlingPartsVision.Services
{
    public class TrainingService
    {
        public static CustomVisionTrainingClient TrainingAPI;
        public static List<ImageFileCreateEntry> imageFileCreateEntries = new List<ImageFileCreateEntry>();

        public static Tag Tag { get; set; } = new Tag();

        public TrainingService()
        {
            TrainingAPI = AuthenticateTraining(Globals.TrainingEndpoint, Globals.TrainingKey);
        }

        private CustomVisionTrainingClient AuthenticateTraining(string endpoint, string trainingKey)
        {
            // Create the API
            CustomVisionTrainingClient API = new CustomVisionTrainingClient(new ApiKeyServiceClientCredentials(trainingKey))
            {
                Endpoint = endpoint,
            };

            return API;
        }

        public static async Task CheckTags(string TagID)
        {
            if (Tag.Id == Guid.Empty || Tag is null)
            {
                IList<Tag> Tags = await TrainingAPI.GetTagsAsync(Globals.ProjectID);

                Tag = Tags.FirstOrDefault(x => x.Name == TagID);
                if (Tag is null)
                {
                    Tag = await TrainingAPI.CreateTagAsync(Globals.ProjectID, TagID);
                }
            }
        }
    }
}
