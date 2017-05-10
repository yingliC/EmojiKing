using EmojiKing.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace EmojiKing.Utlity
{
    public class DoWhat
    {
        /// <summary>
        /// 保存Image控件里的图片到本地
        /// </summary>
        /// <param name="image">要保存的图片</param>
        public async void SaveImage(Image image)
        {
            if (image == null) return;
            if (image.Source == null) return;
            var bitmap = new RenderTargetBitmap();
            var fsp = new FileSavePicker();
            fsp.SuggestedFileName = "emoji";
            fsp.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fsp.FileTypeChoices.Add("图片", new List<string>() { ".png", ".jpg", ".jpeg", ".bmp" });
            StorageFile file = await fsp.PickSaveFileAsync();

            if (file == null) return;
            await bitmap.RenderAsync(image);

            var buffer = await bitmap.GetPixelsAsync();

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encod = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                encod.SetPixelData(BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)bitmap.PixelWidth,
                    (uint)bitmap.PixelHeight,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    buffer.ToArray()
                   );
                await encod.FlushAsync();
            }
        }

        /// <summary>
        /// 打开摄像头，获取采集到的图片
        /// </summary>
        /// <returns>StorageFile</returns>
        public async Task<StorageFile>  GetImageFromCamera()
        {
            CameraCaptureUI photoCapture = new CameraCaptureUI();
            photoCapture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;
            photoCapture.PhotoSettings.CroppedSizeInPixels = new Size(200, 150);
            StorageFile photo = await photoCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);
            return photo;
        }

        /// <summary>
        /// 从相册中获取图片
        /// </summary>
        /// <returns>StorageFile</returns>
        public async Task<StorageFile> GetImageFromAlbum()
        {
            FileOpenPicker fop = new FileOpenPicker();
            fop.ViewMode = PickerViewMode.Thumbnail;    //缩略图
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".png");
            fop.FileTypeFilter.Add(".bmp");
            StorageFile photo = await fop.PickSingleFileAsync();
            return photo;
        }

        public static void ShowNotify(string msg)
        {
            NotifyPopup notifyPopup = new NotifyPopup(msg);
            notifyPopup.Show();
        }
    }
}
