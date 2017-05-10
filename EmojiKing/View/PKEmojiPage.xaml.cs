using EmojiKing.Utlity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EmojiKing.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PKEmojiPage : Page
    {
        public PKEmojiPage()
        {
            this.InitializeComponent();
        }
        StorageFile StorageCompareImage;
        StorageFile StorageBaseImage;
        DoWhat doWhat = new DoWhat();
        private async void CameraSnapButton_Click(object sender, RoutedEventArgs e)
        {
            StorageCompareImage = await doWhat.GetImageFromCamera();
            var stream = await StorageCompareImage.OpenAsync(FileAccessMode.Read);
            var image = new BitmapImage();
            image.SetSource(stream);
            CompareImage.Source = image;
        }

        private async void OpenAlbumButton_Click(object sender, RoutedEventArgs e)
        {
           
            StorageCompareImage = await doWhat.GetImageFromAlbum();
            var stream = await StorageCompareImage.OpenAsync(FileAccessMode.Read);
            var image = new BitmapImage();
            image.SetSource(stream);
            CompareImage.Source = image;
        }

        private async void CompareImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BaseImage.IsTapEnabled = false;
            try
            {
                StorageBaseImage = await doWhat.GetImageFromAlbum();
                var stream = await StorageBaseImage.OpenAsync(FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);
                BaseImage.Source = image;
            }
            catch
            {
                DoWhat.ShowNotify("图片获取失败");
            }
            BaseImage.IsTapEnabled = true;
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            doWhat.SaveImage(CompareImage);
        }
    }
}
