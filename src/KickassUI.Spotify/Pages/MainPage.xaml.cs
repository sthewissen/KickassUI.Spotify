using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace KickassUI.Spotify.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Hide the navbar on Android
            if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
