using EmojiKing.Utlity;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using EmojiKing.Control;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EmojiKing.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EmotionPage : Page
    {
        ObservableCollection<FaceData> faceDatas = new ObservableCollection<FaceData>();
        DoWhat doWhat = new DoWhat();
        CognitiveService cognitiveService = new CognitiveService();
        public EmotionPage()
        {
            this.InitializeComponent();
        }

        Size size_image;  //当前图片实际size
       //AnalysisResult analysisResult;  //当前分析结果
        Microsoft.ProjectOxford.Face.Contract.Face[] faces=null;
        Microsoft.ProjectOxford.Emotion.Contract.Emotion[] emotions=null;

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuItem = sender as MenuFlyoutItem;
            switch (menuItem.Tag.ToString())
            {
                case "FromCamera": {
                        MainCanvas.Children.Clear();
                        StorageFile photo = await doWhat.GetImageFromCamera();
                        if (photo != null)
                        {
                            var stream = await photo.OpenAsync(FileAccessMode.Read);
                            var image = new BitmapImage();
                            image.SetSource(stream);
                            MainImage.Source = image;
                            size_image = new Size(image.PixelWidth, image.PixelHeight);
                            
                            //开始请求认知服务
                            LoadingRing.IsActive = true;
                            try
                            {
                                faces = await cognitiveService.GetFaces(photo);
                                emotions = await cognitiveService.GetEmotions(photo);
                            }
                            catch
                            {
                                DoWhat.ShowNotify("获取服务失败，请检查网络状况");
                            }

                            if (faces != null)
                                DisplayFaces(faces);
                            if (emotions != null)
                                DisplayEmotions(emotions);

                            LoadingRing.IsActive = false;
                        }
                        break; }
                case "FromAlbum": {
                        StorageFile file = await doWhat.GetImageFromAlbum();
                        if (file != null)
                        {
                            var stream = await file.OpenAsync(FileAccessMode.Read);
                            var image = new BitmapImage();
                            image.SetSource(stream);
                            MainImage.Source = image;
                            size_image = new Size(image.PixelWidth, image.PixelWidth);

                            LoadingRing.IsActive = true;
                            try
                            {
                                faces = await cognitiveService.GetFaces(file);
                                emotions = await cognitiveService.GetEmotions(file);
                            }
                            catch
                            {
                                DoWhat.ShowNotify("获取服务失败，请检查网络状况");
                            }

                            if (faces != null)
                                DisplayFaces_Me(faces);
                            if (emotions != null)
                                DisplayEmotions(emotions);
                            LoadingRing.IsActive = false;
                        }
                        break; }
                case "FromURL": {
                        var content = Clipboard.GetContent();
                        if (content != null && content.Contains(StandardDataFormats.Text))
                        {
                            var url = await content.GetTextAsync();
                            imageLocation.Text = url;
                            MainImage.Source = new BitmapImage(new Uri(imageLocation.Text));
                            LoadingRing.IsActive = true;
                        }
                        break; }
                default:
                    break;  
            }
        }

        private void DisplayEmotions(Microsoft.ProjectOxford.Emotion.Contract.Emotion[] emotions)
        {
            
        }

        private void DisplayFaces(Microsoft.ProjectOxford.Face.Contract.Face[] faces,bool init=true)
        {
            if (faces == null) return;
            MainCanvas.Children.Clear();
            var offset_h = 0.0; var offset_w = 0.0;
            var p = 0.0;
            var d = MainCanvas.ActualHeight / MainCanvas.ActualWidth;
            var d2 = size_image.Height / size_image.Width;
            if (d < d2)
            {
                offset_h = 0;
                offset_w = (MainCanvas.ActualWidth - MainCanvas.ActualHeight / d2) / 2;
                p = MainCanvas.ActualHeight / size_image.Height;
            }
            else
            {
                offset_w = 0;
                offset_h = (MainCanvas.ActualHeight - MainCanvas.ActualWidth / d2) / 2;
                p = MainCanvas.ActualWidth / size_image.Width;
            }

            if (faces != null)
            {
                int count = 1;
                //将face矩形显示到界面（如果有）
                foreach (var face in faces)
                {
                    Windows.UI.Xaml.Shapes.Rectangle rect = new Windows.UI.Xaml.Shapes.Rectangle();
                    rect.Width = face.FaceRectangle.Width * p;
                    rect.Height = face.FaceRectangle.Height * p;
                    Canvas.SetLeft(rect, face.FaceRectangle.Left * p + offset_w);
                    Canvas.SetTop(rect, face.FaceRectangle.Top * p + offset_h);
                    rect.Stroke = new SolidColorBrush(Colors.Orange);
                    rect.StrokeThickness = 3;

                    MainCanvas.Children.Add(rect);

                    TextBlock txt = new TextBlock();
                    txt.Foreground = new SolidColorBrush(Colors.Orange);
                    txt.Text = "#" + count;
                    Canvas.SetLeft(txt, face.FaceRectangle.Left * p + offset_w);
                    Canvas.SetTop(txt, face.FaceRectangle.Top * p + offset_h - 20);
                    MainCanvas.Children.Add(txt);
                    count++;
                }
            }
            if (!init)
                return;
            faceDatas.Clear();
            faceDatas.Add(new FaceData() { Age = "年龄", Gender = "性别", Glasses = "眼镜", Index = "序号", Smile = "笑容" });
            int index = 1;
            foreach (var face in faces)
            {
                faceDatas.Add(
                    new FaceData() {Index="#"+ index++,
                        Age = Math.Round(face.FaceAttributes.Age, 2).ToString() ,
                        Gender =face.FaceAttributes.Gender,
                        Glasses = face.FaceAttributes.Glasses.ToString(),
                        Smile=face.FaceAttributes.Smile.ToString()
                    });
                
            }
        }

        private void DisplayFaces_Me(Microsoft.ProjectOxford.Face.Contract.Face[] faces,bool init=true)
        {
            if (faces == null) return;
            MainCanvas.Children.Clear();
            double offset_canvas_x = 0.0f;
            double offset_canvas_y = 0.0f;
            double canvas_w = MainCanvas.ActualWidth;
            double canvas_h = MainCanvas.ActualHeight;
            double image_w = MainImage.ActualWidth;
            double image_h = MainImage.ActualHeight;

            offset_canvas_x = (canvas_w - image_w) / 2;
            offset_canvas_y = (canvas_h - image_h) / 2;

            double p = MainImage.ActualWidth / size_image.Width;
            if (faces != null)
            {
                int index = 1;
                foreach (var face in faces)
                {
                    //计算矩形的宽和高
                    Rectangle rect = new Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Orange);
                    rect.StrokeThickness = 3;

                    rect.Width = face.FaceRectangle.Width*p;
                    rect.Height = face.FaceRectangle.Height*p;

                    double offset_image_x = face.FaceRectangle.Left * p;
                    double offset_image_y = face.FaceRectangle.Top * p;

                    Canvas.SetLeft(rect, offset_canvas_x+offset_image_x);
                    Canvas.SetTop(rect, offset_canvas_y+offset_image_y);
                    MainCanvas.Children.Add(rect);
                }
            }

            if (!init)
                return;
            faceDatas.Clear();
            faceDatas.Add(new FaceData() { Age = "年龄", Gender = "性别", Glasses = "眼镜", Index = "序号", Smile = "笑容" });
            int indext = 1;
            foreach (var face in faces)
            {
                faceDatas.Add(
                    new FaceData()
                    {
                        Index = "#" + indext++,
                        Age = Math.Round(face.FaceAttributes.Age, 2).ToString(),
                        Gender = face.FaceAttributes.Gender,
                        Glasses = face.FaceAttributes.Glasses.ToString(),
                        Smile = face.FaceAttributes.Smile.ToString()
                    });

            }

        }

        private void MainCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (emotions == null) return;
            double canvas_w = MainCanvas.ActualWidth;
            double canvas_h = MainCanvas.ActualHeight;
            double image_w = MainImage.ActualWidth;
            double image_h = MainImage.ActualHeight;

            double p = MainImage.ActualWidth / size_image.Width;

            double offset_canvas_x = (canvas_w - image_w) / 2;
            double offset_canvas_y = (canvas_h - image_h) / 2;

            int index = 0;  //为了上传对应的face数据
            foreach (var emotion in emotions)
            {
                index++;
                double offset_image_x = emotion.FaceRectangle.Left * p;
                double offset_image_y = emotion.FaceRectangle.Top * p;

                Rect rect = new Rect();
                //矩形的宽高
                rect.Width = emotion.FaceRectangle.Width * p + offset_image_x;
                rect.Height = emotion.FaceRectangle.Height * p + offset_image_y;
                //矩形的起始位置=canvas上的偏移量+image上的偏移量
                rect.X = offset_canvas_x;
                rect.Y = offset_canvas_y;
                Point point = e.GetPosition(MainCanvas);
                if (rect.Contains(point))
                {
                    EmotionData data = new EmotionData()
                    {
                        Anger = emotion.Scores.Anger,
                        Contempt = emotion.Scores.Contempt,
                        Disgust = emotion.Scores.Disgust,
                        Fear = emotion.Scores.Fear,
                        Happiness = emotion.Scores.Happiness,
                        Neutral = emotion.Scores.Neutral,
                        Sadness = emotion.Scores.Sadness,
                        Surprise = emotion.Scores.Surprise
                    };
                    if (index > faceDatas.Count)
                        break;
                    EmotionDataPage edc = new EmotionDataPage(data,faceDatas[index]);
                    edc.Width = MainCanvas.ActualWidth * 2 / 5;
                    edc.Height = MainCanvas.ActualHeight / 2;
                    EmotionPop.Child = edc;
                    EmotionPop.VerticalOffset = point.Y;
                    EmotionPop.HorizontalOffset = MainCanvas.ActualWidth / 8;
                    EmotionPop.IsOpen = true;
                    break;
                }
            }
            

            //if (emotions != null)
            //{
            //    var offset_h = 0.0; var offset_w = 0.0;
            //    var p = 0.0;
            //    var d = MainCanvas.ActualHeight / MainCanvas.ActualWidth;
            //    var d2 = size_image.Height / size_image.Width;
            //    if (d < d2)
            //    {
            //        offset_h = 0;
            //        offset_w = (MainCanvas.ActualWidth - MainCanvas.ActualHeight / d2) / 2;
            //        p = MainCanvas.ActualHeight / size_image.Height;
            //    }
            //    else
            //    {
            //        offset_w = 0;
            //        offset_h = (MainCanvas.ActualHeight - MainCanvas.ActualWidth / d2) / 2;
            //        p = MainCanvas.ActualWidth / size_image.Width;
            //    }
            //    foreach (var emotion in emotions)
            //    {
            //        Rect rect = new Rect();
            //        rect.Width = emotion.FaceRectangle.Width * p;
            //        rect.Height = emotion.FaceRectangle.Height * p;

            //        rect.X = emotion.FaceRectangle.Left * p + offset_w;
            //        rect.Y = emotion.FaceRectangle.Top * p + offset_h;

            //        Point point = e.GetPosition(MainCanvas);
            //        if (rect.Contains(point))
            //        {
            //            EmotionData data = new EmotionData()
            //            {
            //                Anger = emotion.Scores.Anger,
            //                Contempt = emotion.Scores.Contempt,
            //                Disgust = emotion.Scores.Disgust,
            //                Fear = emotion.Scores.Fear,
            //                Happiness = emotion.Scores.Happiness,
            //                Neutral = emotion.Scores.Neutral,
            //                Sadness = emotion.Scores.Sadness,
            //                Surprise = emotion.Scores.Surprise
            //            };
            //            EmotionDataPage edc = new EmotionDataPage(data);
            //            edc.Width = MainCanvas.ActualWidth * 2 / 5;
            //            edc.Height = MainCanvas.ActualHeight / 2;
            //            EmotionPop.Child = edc;
            //            EmotionPop.VerticalOffset = point.Y;
            //            EmotionPop.HorizontalOffset = MainCanvas.ActualWidth / 8;
            //            EmotionPop.IsOpen = true;

            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 以URL的形式打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            size_image = new Size((MainImage.Source as BitmapImage).PixelWidth, (MainImage.Source as BitmapImage).PixelHeight);
            
            faces = await cognitiveService.GetFacesFromUri(imageLocation.Text);
            emotions = await cognitiveService.GetEmotionsFromUri(imageLocation.Text);

            if (faces != null)
                DisplayFaces(faces);
            if (emotions != null)
                DisplayEmotions(emotions);
            LoadingRing.IsActive = false;
        }

        private void MainImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (faces == null) return;
            //DisplayFaces(faces,false);
            DisplayFaces_Me(faces);
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void ImageSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainImage.Source == null) return;
            BitmapImage bt = MainImage.Source as BitmapImage;
            doWhat.SaveImage(MainImage);
        }
    }
}
