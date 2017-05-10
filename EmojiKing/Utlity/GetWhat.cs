using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace EmojiKing.Utlity
{
    public class GetWhat
    {
        public static FaceAttributeType[] GetFaceAttirbutes()
        {
            return new FaceAttributeType[] {
                                FaceAttributeType.Age,
                                FaceAttributeType.Gender,
                                FaceAttributeType.Smile,
                                FaceAttributeType.FacialHair,
                                FaceAttributeType.HeadPose,
                                FaceAttributeType.Glasses
                                };
        }
        public static VisualFeature[] GetVisualFeatureService()
        {
            return new VisualFeature[]{
                VisualFeature.Tags,
                VisualFeature.Description,
                VisualFeature.Adult,
                VisualFeature.Categories,
                VisualFeature.Color,
                VisualFeature.Faces,
                VisualFeature.ImageType };
        }
        
    }
}
