using EmojiKing.Control;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using LiveCharts.Uwp;
using System;
using System.Collections.Generic;
using LiveCharts;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace EmojiKing.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EmotionDataPage : Page
    {
        public EmotionDataPage(EmotionData data)
        {
            this.InitializeComponent();
            Happiness.Values = new ChartValues<double>(new double[] { Math.Round(100*data.Happiness,2) });
            Surprise.Values = new ChartValues<double>(new double[] {  Math.Round(100 * data.Surprise, 2) });
            Contempt.Values = new ChartValues<double>(new double[] { Math.Round(100 * data.Contempt, 2) });
            Anger.Values = new ChartValues<double>(new double[] { Math.Round(100 * data.Anger, 2) });
            Disgust.Values = new ChartValues<double>(new double[] { Math.Round(100 * data.Disgust, 2) });
            Fear.Values = new ChartValues<double>(new double[] { Math.Round(100 * data.Fear, 2) });
            Neutral.Values = new ChartValues<double>(new double[] { Math.Round(100 * data.Neutral, 2) });
            Sadness.Values = new ChartValues<double>(new double[] { Math.Round(100 * data.Sadness, 2) });
        }
    }
}
