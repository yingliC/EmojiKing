using EmojiKing.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EmojiKing.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FaceStickersPage : Page
    {
        ObservableCollection<StickerData> stickerList=new ObservableCollection<StickerData>();
        public FaceStickersPage()
        {
            this.InitializeComponent();
            this.DataContext = StickerData.GetStickerData();
            stickerList.Add(new StickerData() { imagePath = "ms-appx:///Assets/Stickers/EyeStickers/sticker1.jpg" ,imageName="1001"});
            //stickerList.Add(new StickerData() { imagePath = "ms-appx:///Assets/Stickers/EyeStickers/sticker1.jpg" });
            //stickerList.Add(new StickerData() { imagePath = "ms-appx:///Assets/Stickers/EyeStickers/sticker1.jpg" });
            //stickerList.Add(new StickerData() { imagePath = "ms-appx:///Assets/Stickers/EyeStickers/sticker1.jpg" });
            //stickerList.Add(new StickerData() { imagePath = "ms-appx:///Assets/Stickers/EyeStickers/sticker1.jpg" });
            //stickerList.Add(new StickerData() { imagePath = "ms-appx:///Assets/Stickers/EyeStickers/sticker1.jpg" });
        }
    }
}
