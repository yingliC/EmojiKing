using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace EmojiKing.Control
{
    class StickerData
    {
        public string imageName { get; set; }
        public static List<StickerData> GetStickerData()
        {
            return new List<StickerData>()
            {
                new StickerData(){imageName="Assets/Stickers/HeadStickers/sticker1.jpg"}
            };
        }
        public string imagePath { get; set; }
    }
}
