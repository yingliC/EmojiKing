using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace EmojiKing.Utlity
{
    public class CognitiveService
    {
        private string key_face = "33a1073f52b94e07882eab6f66cdb33a";
        private string key_emotion = "0f583131728d430faa769ea1ed5e7e7a";
        FaceAttributeType[] faceAttributes=new  FaceAttributeType[] {
                                FaceAttributeType.Age,
                                FaceAttributeType.Gender,
                                FaceAttributeType.Smile,
                                FaceAttributeType.FacialHair,
                                FaceAttributeType.HeadPose,
                                FaceAttributeType.Glasses
         };
        VisualFeature[] visualFeatures=new VisualFeature[]{
                VisualFeature.Tags,
                VisualFeature.Description,
                VisualFeature.Adult,
                VisualFeature.Categories,
                VisualFeature.Color,
                VisualFeature.Faces,
                VisualFeature.ImageType
        };

        /// <summary>
        /// 获取图片中的脸部信息
        /// </summary>
        /// <param name="photo">Storage 类型的文件（图片）</param>
        /// <returns>脸部数组</returns>
        public async Task<Face[]> GetFaces(StorageFile photo)
        {
            var stream = await photo.OpenAsync(FileAccessMode.Read);
            var stream_send = stream.CloneStream();
            var image = new BitmapImage();
            image.SetSource(stream);

            FaceServiceClient faceClient = new FaceServiceClient(key_face);
            var face_task = faceClient.DetectAsync(stream_send.AsStream(), true, true, faceAttributes);
            return await face_task;
        }

        /// <summary>
        /// 获取链接图片中的人脸？
        /// </summary>
        /// <param name="uri">链接地址</param>
        /// <returns>表情包</returns>
        public async Task<Face[]> GetFacesFromUri(string uri)
        {
            FaceServiceClient f_client = new FaceServiceClient(key_face);
            var faces_task = f_client.DetectAsync(uri, true, true, faceAttributes);
            return await faces_task;
        }

        /// <summary>
        /// 获取图片中的表情
        /// </summary>
        /// <param name="photo">Storage 类型的文件（图片）</param>
        /// <returns>表情宝</returns>
        public async Task<Emotion[]> GetEmotions(StorageFile photo)
        {
            var stream = await photo.OpenAsync(FileAccessMode.Read);
            var stream_send = stream.CloneStream();
            var image = new BitmapImage();
            image.SetSource(stream);
            EmotionServiceClient emotionClient = new EmotionServiceClient(key_emotion);
            var emotion_task = emotionClient.RecognizeAsync(stream_send.AsStream());
            return await emotion_task;
        }

        /// <summary>
        /// 获取链接图片中的表情
        /// </summary>
        /// <param name="uri">链接地址</param>
        /// <returns>表情包</returns>
        public async Task<Emotion[]> GetEmotionsFromUri(string uri)
        {
            var faceAttributes = GetWhat.GetFaceAttirbutes();
            EmotionServiceClient e_client = new EmotionServiceClient(key_emotion);
            var emotion_task = e_client.RecognizeAsync(uri);
            return await emotion_task;
        }

    }
}
