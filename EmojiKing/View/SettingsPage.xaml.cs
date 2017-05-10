using EmojiKing.Manage;
using System;
using System.Collections.Generic;
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
    public sealed partial class SettingsPage : Page
    {
        bool isLoaded = false;
        public SettingsPage()
        {
            this.InitializeComponent();
            isLoaded = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;
            int selectIndex = (sender as ComboBox).SelectedIndex;
            Settings.ThemeSettingSet(selectIndex);

            if (selectIndex == 0)
            {
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Light;
            }
            else
            {
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Dark;
            }
        }
    }
}
